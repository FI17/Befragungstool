using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Acces;
using System.Data.Entity;
using Domain;
using System.Web.UI;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Users = "Admin@FI17.de")]
    public class Umfrage_ErstellungController : Controller
    {
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();
        ModelToAnswerTransformer answerTransformer = new ModelToAnswerTransformer();
        SurveyToModelTransformer modeltransformer = new SurveyToModelTransformer();
        QuestionToModelTransformer modelquestionformer = new QuestionToModelTransformer();
        private DatabaseContent db = new DatabaseContent();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SurveyViewModel umfrage)
        {
            var umfrageKernDaten = surveytransformer.Transform(umfrage);
            db.Surveys.Add(umfrageKernDaten);
            db.SaveChanges();
            umfrage = modeltransformer.Transform(umfrageKernDaten);
            return RedirectToAction("FrageErstellung", new { arg = umfrage.ID });
        }

        public ActionResult FrageErstellung(Guid arg)
        {
            Session["AnzahlAntworten"] = -1;
            var zuTransformierendeUmfrage = db.Surveys
                .Include(e => e.questions
                .Select(b => b.answers))
                .FirstOrDefault(d => d.ID == arg);
            var umfrage = new SurveyViewModel();
            umfrage = modeltransformer.Transform(zuTransformierendeUmfrage);
            umfrage.questionViewModels = umfrage.questionViewModels
                .OrderBy(d => d.position)
                .ToList();
            return View(umfrage);
        }

        #region Vorschau / Bearbeiten

        public PartialViewResult Vorschau(Guid arg)
        {
            var fragenModelle = new List<QuestionViewModel>();
            List<Question> fragenListe = db.Questions
                .Where(i => i.survey.ID == arg)
                .Include(a => a.answers)
                .ToList();
            fragenModelle = modelquestionformer.ListTransform(fragenListe).ToList();
            fragenModelle = fragenModelle.OrderBy(e => e.position).ToList();
            //fragenModelle.Reverse();
            return PartialView(fragenModelle);
        }

        public ActionResult Position_nach_oben(Guid arg)
        {
            Question frageDieHochSoll = db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == arg);
            Survey mutterUmfrage = frageDieHochSoll.survey;
            Question frageDarueber = db.Questions
                .Where(i => i.survey.ID == mutterUmfrage.ID)
                .FirstOrDefault(t => t.position == frageDieHochSoll.position - 1);

            frageDarueber.position++;
            frageDieHochSoll.position--;
            db.SaveChanges();

            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = mutterUmfrage.ID });
        }

        public ActionResult Position_nach_unten(Guid arg)
        {
            Question frageDieRunterSoll = db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == arg);
            Survey mutterUmfrage = frageDieRunterSoll.survey;
            Question frageDarunter = db.Questions
                .Where(i => i.survey.ID == mutterUmfrage.ID)
                .FirstOrDefault(t => t.position == frageDieRunterSoll.position + 1);

            frageDarunter.position--;
            frageDieRunterSoll.position++;
            db.SaveChanges();

            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = mutterUmfrage.ID });
        }

        // TODO --> BUGFIX:
        // andere Fragen müssen Position nachrutschen,
        // damit keine Lücken entstehen
        public ActionResult FrageLoeschen(Guid arg)
        {
            List<Answering> beantwortungenDerFrage = db.Answerings
                .Where(i => i.question.ID == arg)
                .ToList();
            foreach (var item in beantwortungenDerFrage)
            {
                db.Answerings.Remove(item);
            }

            List<Answer> antwortmoeglichkeitenDerFrage = db.Answers
                .Where(i => i.question.ID == arg)
                .ToList();
            foreach (var item in antwortmoeglichkeitenDerFrage)
            {
                db.Answers.Remove(item);
            }

            Question zuLoeschendeFrage = db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == arg);
            Survey mutterUmfrage = zuLoeschendeFrage.survey;
            db.Questions.Remove(zuLoeschendeFrage);
            db.SaveChanges();
            var umfrage = db.Surveys.Include(r => r.questions).First(d => d.ID == mutterUmfrage.ID);
            var zahler = 0;
            foreach (var item in umfrage.questions.OrderBy(z => z.position))
            {
                item.position = zahler;
                zahler++;
            }
            db.SaveChanges();
            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = mutterUmfrage.ID });
        }

        void Fragen_aktualisieren(SurveyViewModel umfrageView, Guid arg)
        {
            Survey umfrageAusDb = db.Surveys
                .Include(b => b.questions
                .Select(c => c.answers))
                .First(f => f.ID == arg);
            foreach (var frage in umfrageAusDb.questions)
            {
                var frageAktuellerPosition = umfrageView.questionViewModels.First(f => f.position == frage.position);
                frage.text = frageAktuellerPosition.text;
                if (frage.answers != null)
                {
                    for (int i = 0; i < frage.answers.Count(); i++)
                    {
                        frage.answers.ToList()[i].text = frageAktuellerPosition.answers.ToList()[i].text;
                        frage.answers.ToList()[i].question = frage;
                        frage.answers.ToList()[i].position = i;
                    }
                }
            }
            db.SaveChanges();
        }

        #endregion

        [HttpPost]
        public ActionResult FrageErstellung(SurveyViewModel model, string subject, Guid arg)
        {
            switch (subject)
            {
                case "Aktualisieren":
                    Fragen_aktualisieren(model, arg);
                    break;
                case "Frage speichern":
                    Neue_Frage_speichern(model, arg);
                    break;
                case "Bearbeitung beenden":
                    Session["UmfrageID"] = arg.ToString();
                    Session["Fertig"] = "TRUE";
                    return RedirectToAction("FrageErstellung", new { arg = arg });
                case "Ende":
                    return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("FrageErstellung", new { arg = arg });
        }

        void Neue_Frage_speichern(SurveyViewModel model, Guid arg)
        {
            Question neue_Frage = questiontransformer.Transform(
                model.questionViewModels
                .ToList()
                .Last());
            Survey umfrage_aus_DB_vor_neue_Frage = db.Surveys
                .Include(b => b.questions)
                .First(f => f.ID == arg);
            neue_Frage.survey = umfrage_aus_DB_vor_neue_Frage;
            neue_Frage.position = umfrage_aus_DB_vor_neue_Frage.questions.Count();

            db.Questions.Add(neue_Frage);

            if (neue_Frage.typ != Question.choices.Freitext)
            {
                for (int i = 0; i < neue_Frage.answers.Count(); i++)
                {
                    neue_Frage.answers.ToList()[i].position = i;
                    db.Answers.Add(neue_Frage.answers.ToList()[i]);
                }
            }
            db.SaveChanges();
        }

        public PartialViewResult Plus_Antwort()
        {
            Guid arg = new Guid(Session["UmfrageID"].ToString());
            Survey umfrage = db.Surveys
                .Include(e => e.questions)
                .FirstOrDefault(d => d.ID == arg);
            Session["AnzahlAntworten"] = Convert.ToInt32(Session["AnzahlAntworten"]) + 1;

            var umfrageModell = new SurveyViewModel();
            umfrageModell = modeltransformer.Transform(umfrage);
            if (umfrageModell.questionViewModels.Count() == 0)
            {
                umfrageModell.questionViewModels = new List<QuestionViewModel>();
            }
            return PartialView(umfrageModell);
        }
    }
}