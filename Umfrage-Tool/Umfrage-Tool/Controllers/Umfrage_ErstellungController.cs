using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Acces;
using Microsoft.AspNet.Identity.Owin;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Roles = "Ersteller, Admin")]
    public class Umfrage_ErstellungController : Controller
    {
        private readonly ModelToSurveyTransformer surveyTransformer = new ModelToSurveyTransformer();
        private readonly ModelToQuestionTransformer questionTransformer = new ModelToQuestionTransformer();
        private readonly SurveyToModelTransformer modelTransformer = new SurveyToModelTransformer();
        private readonly QuestionToModelTransformer modelQuestionFormer = new QuestionToModelTransformer();
        private readonly DatabaseContent db = new DatabaseContent();

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
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private ApplicationSignInManager SignInManager
        {
            set { }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SurveyViewModel umfrage)
        {
            var a = User.Identity.Name;
            var userId = UserManager.Users.First(d => d.Email == a).Id;

            if (userId == null) return View("Error");

            umfrage.Creator = new Guid(userId);
            var umfrageKernDaten = surveyTransformer.Transform(umfrage);
            db.Surveys.Add(umfrageKernDaten);
            db.SaveChanges();
            umfrage = modelTransformer.Transform(umfrageKernDaten);
            return RedirectToAction("FrageErstellung", new {arg = umfrage.ID});
        }

        public ActionResult FrageErstellung(Guid arg)
        {
            Session["AnzahlAntworten"] = -1;
            var zuTransformierendeUmfrage = db.Surveys
                .Include(e => e.questions
                    .Select(b => b.choice))
                .FirstOrDefault(d => d.ID == arg);
            var umfrage = modelTransformer.Transform(zuTransformierendeUmfrage);
            umfrage.questionViewModels = umfrage.questionViewModels
                .OrderBy(d => d.position)
                .ToList();
            if (!BenutzerDarfDas(umfrage.Creator) || umfrage.states != Survey.States.InBearbeitung)
                return RedirectToAction("StatusUmfrageBearbeitung", "Fehlermeldungen");

            return View(umfrage);
        }

        private bool BenutzerDarfDas(Guid umfrageCreator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == umfrageCreator.ToString()).Email;
            var a = User.Identity.Name;
            var darfErDasWirklich = a == benutzerText || User.Identity.Name == "Admin@FI17.de";
            return darfErDasWirklich;
        }

        #region Vorschau / Bearbeiten

        public PartialViewResult Vorschau(Guid arg)
        {
            var fragenListe = db.Questions
                .Where(i => i.survey.ID == arg)
                .Include(a => a.choice)
                .ToList();
            var fragenModelle = modelQuestionFormer.ListTransform(fragenListe).ToList();
            fragenModelle = fragenModelle.OrderBy(e => e.position).ToList();
            return PartialView(fragenModelle);
        }

        public void Position_nach_oben(Guid frageID)
        {
            var frageDieHochSoll = db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == frageID);
            var mutterUmfrage = frageDieHochSoll?.survey;
            var frageDarüber = db.Questions
                .Where(i => i.survey.ID == mutterUmfrage.ID)
                .FirstOrDefault(t => t.position == frageDieHochSoll.position - 1);

            if (frageDarüber != null) frageDarüber.position++;
            if (frageDieHochSoll != null) frageDieHochSoll.position--;
            db.SaveChanges();
        }

        public void Position_nach_unten(Guid frageID)
        {
            var frageDieRunterSoll = db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == frageID);
            var mutterUmfrage = frageDieRunterSoll?.survey;
            var frageDarunter = db.Questions
                .Where(i => i.survey.ID == mutterUmfrage.ID)
                .FirstOrDefault(t => t.position == frageDieRunterSoll.position + 1);

            if (frageDarunter != null) frageDarunter.position--;
            if (frageDieRunterSoll != null) frageDieRunterSoll.position++;
            db.SaveChanges();
        }

        public void FrageLöschen(Guid frageID)
        {
            var beantwortungenDerFrage = db.GivenAnswers
                .Where(i => i.question.ID == frageID)
                .ToList();
            foreach (var item in beantwortungenDerFrage) db.GivenAnswers.Remove(item);

            var antwortmöglichkeitenDerFrage = db.Choices
                .Where(i => i.question.ID == frageID)
                .ToList();
            foreach (var item in antwortmöglichkeitenDerFrage) db.Choices.Remove(item);

            var zuLöschendeFrage = db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == frageID);
            var mutterUmfrage = zuLöschendeFrage?.survey;
            if (zuLöschendeFrage != null) db.Questions.Remove(zuLöschendeFrage);
            db.SaveChanges();
            var umfrage = db.Surveys.Include(r => r.questions).First(d => d.ID == mutterUmfrage.ID);
            var zahler = 0;
            foreach (var item in umfrage.questions.OrderBy(z => z.position))
            {
                item.position = zahler;
                zahler++;
            }

            db.SaveChanges();
        }

        public void AntwortLöschen(Guid antwortID)
        {
            var zuLöschendeAntwort = db.Choices
                .Include(q => q.question)
                .First(i => i.ID == antwortID);

            var frage = db.Questions.Include(s => s.survey).FirstOrDefault(q => q.ID == zuLöschendeAntwort.question.ID);
            db.Choices.Remove(zuLöschendeAntwort);
            foreach (var antwort in db.Choices)
            {
                if (antwort.question == frage && antwort.position > zuLöschendeAntwort.position)
                {
                    antwort.position--;
                }
            }
            db.SaveChanges();
        }

        private void Fragen_aktualisieren(SurveyViewModel umfrageView, Guid arg)
        {
            var umfrageAusDb = db.Surveys
                .Include(b => b.questions
                    .Select(c => c.choice))
                .First(f => f.ID == arg);
            umfrageAusDb.name = umfrageView.name;
            foreach (var frage in umfrageAusDb.questions)
            {
                var frageAktuellerPosition = umfrageView.questionViewModels.First(f => f.position == frage.position);
                frage.text = frageAktuellerPosition.text;
                frage.scaleLength = frageAktuellerPosition.scaleLength;
                if (frage.choice == null) continue;
                for (var i = 0; i < frage.choice.Count; i++)
                {
                    frage.choice.ToList()[i].text = frageAktuellerPosition.choices.ToList()[i].text;
                    frage.choice.ToList()[i].question = frage;
                    frage.choice.ToList()[i].position = i;
                }
            }
            db.SaveChanges();
        }

        #endregion

        [HttpPost]
        public ActionResult FrageErstellung(SurveyViewModel model, string subject, Guid arg)
        {
            Guid frageID = new Guid();
            if (subject.Contains("/"))
            {
                var id = subject.Substring(subject.IndexOf('/')+1);
                frageID = new Guid(id);
                subject = subject.Substring(0,subject.IndexOf('/'));
            }

            Fragen_aktualisieren(model, arg);
            switch (subject)
            {
                case "Frage hinzufügen":
                    Neue_Frage_speichern(model, arg);
                    break;
                case "Bearbeitung beenden":
                    return RedirectToAction("Index", "Home");
                case "Plus_Antwort":
                    Plus_Antwort(frageID);
                    break;
                case "Antwort_loeschen":
                    AntwortLöschen(frageID);
                    break;
                case "Frage_loeschen":
                    FrageLöschen(frageID);
                    break;
                case "Position_nach_oben":
                    Position_nach_oben(frageID);
                    break;
                case "Position_nach_unten":
                    Position_nach_unten(frageID);
                    break;
            }

            return RedirectToAction("FrageErstellung", new {arg});
        }

        private void Neue_Frage_speichern(SurveyViewModel model, Guid arg)
        {
            var neueFrage = questionTransformer.Transform(
                model.questionViewModels
                    .ToList()
                    .Last());
            var umfrageAusDbVorNeueFrage = db.Surveys
                .Include(b => b.questions)
                .First(f => f.ID == arg);
            neueFrage.survey = umfrageAusDbVorNeueFrage;
            neueFrage.position = umfrageAusDbVorNeueFrage.questions.Count;

            if (neueFrage.type == Question.choices.Skalenfrage)
            {    for (var i = 0; i < neueFrage.choice.Count; i++)
                {
                    neueFrage.choice.ToList()[i].position = i;
                    db.Choices.Add(neueFrage.choice.ToList()[i]);
                }
            }
            else
            {
                neueFrage.choice.Clear();
            }
            db.Questions.Add(neueFrage);
            db.SaveChanges();
        }
        
        public void Plus_Antwort(Guid frageID)
        {
            var frage = db.Questions
                .Include(y => y.choice)
                .Include(s => s.survey)
                .First(f => f.ID == frageID);
            var neueAntwort = new Choice {question = frage, position = frage.choice.Count};
            db.Choices.Add(neueAntwort);
            db.SaveChanges();
        }

        public PartialViewResult Skalenfragen_Erstellung()
        {
            return PartialView();
        }

        public PartialViewResult Multiple_Choice_Fragen_Erstellung()
        {
            return PartialView();
        }


        public PartialViewResult Freitext(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleMoreMitSonstiges(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleMore(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleOneMitSonstiges(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleOne(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult Skalenfrage(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
    }
}