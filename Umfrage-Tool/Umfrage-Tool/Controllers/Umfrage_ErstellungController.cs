using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Acces;
using System.Data.Entity;
using Domain;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Umfrage_Tool.Models;

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

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public Umfrage_ErstellungController()
        {

        }

        public Umfrage_ErstellungController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SurveyViewModel umfrage)
        {
            var a = User.Identity.Name;
            var id = SignInManager.GetVerifiedUserIdAsync();
            var userId = UserManager.Users.First(d=>d.Email == a).Id;
            //var userId = UserManager.FindByEmail("a").Id;

            if (userId == null)
            {
                return View("Error");
            }

            umfrage.Creator = new Guid(userId);
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
                .Select(b => b.choice))
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
                .Include(a => a.choice)
                .ToList();
            fragenModelle = modelquestionformer.ListTransform(fragenListe).ToList();
            fragenModelle = fragenModelle.OrderBy(e => e.position).ToList();
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
            List<GivenAnswer> beantwortungenDerFrage = db.GivenAnswers
                .Where(i => i.question.ID == arg)
                .ToList();
            foreach (var item in beantwortungenDerFrage)
            {
                db.GivenAnswers.Remove(item);
            }

            List<Choice> antwortmoeglichkeitenDerFrage = db.Choices
                .Where(i => i.question.ID == arg)
                .ToList();
            foreach (var item in antwortmoeglichkeitenDerFrage)
            {
                db.Choices.Remove(item);
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
        public ActionResult Antwort_loeschen(Guid arg)
        {
            Choice Antwort = db.Choices
                .Include(q => q.question)
                .First(i => i.ID == arg);

            Question Frage = db.Questions.Include(s => s.survey).FirstOrDefault(q => q.ID == Antwort.question.ID);
            Guid Umfrage_ID = db.Surveys.FirstOrDefault(u => u.ID == Frage.survey.ID).ID;
            db.Choices.Remove(Antwort);
            db.SaveChanges();
            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = Umfrage_ID });
        }
        void Fragen_aktualisieren(SurveyViewModel umfrageView, Guid arg)
        {
            Survey umfrageAusDb = db.Surveys
                .Include(b => b.questions
                .Select(c => c.choice))
                .First(f => f.ID == arg);
            umfrageAusDb.name = umfrageView.name;
            foreach (var frage in umfrageAusDb.questions)
            {
                var frageAktuellerPosition = umfrageView.questionViewModels.First(f => f.position == frage.position);
                frage.text = frageAktuellerPosition.text;
                frage.scaleLength = frageAktuellerPosition.scaleLength;
                if (frage.choice != null)
                {
                    for (int i = 0; i < frage.choice.Count(); i++)
                    {
                        frage.choice.ToList()[i].text = frageAktuellerPosition.choices.ToList()[i].text;
                        frage.choice.ToList()[i].question = frage;
                        frage.choice.ToList()[i].position = i;

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

            if (neue_Frage.type != Question.choices.Freitext)
            {
                for (int i = 0; i < neue_Frage.choice.Count(); i++)
                {
                    neue_Frage.choice.ToList()[i].position = i;
                    db.Choices.Add(neue_Frage.choice.ToList()[i]);
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

        public PartialViewResult Skalenfragen_Erstellung()
        {
            return PartialView();
        }

        public PartialViewResult Multiple_Choice_Fragen_Erstellung()
        {
            return PartialView();
        }
    }
}
