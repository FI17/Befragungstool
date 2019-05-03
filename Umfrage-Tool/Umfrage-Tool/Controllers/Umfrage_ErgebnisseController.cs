using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain;
using Domain.Acces;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Roles = "Ersteller, Admin")]

    public class Umfrage_ErgebnisseController : Controller
    {

        DatabaseContent db = new DatabaseContent();
        SurveyToModelQuestionTransformer _umfrageZuViewTransformerMitFragen = new SurveyToModelQuestionTransformer();
        SessionToModelTransformer _sessionZuViewTransformer = new SessionToModelTransformer();
        AnsweringToModelAllTransformer _beantwortungZuViewTransformer = new AnsweringToModelAllTransformer();
        QuestionToModelTransformer _fragenZuViewTransformer = new QuestionToModelTransformer();

        private ApplicationUserManager _userManager;

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


        public Umfrage_ErgebnisseController()  { }

        public Umfrage_ErgebnisseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Ergebnisse(Guid arg)
        {
            Guid umfrageId = arg;
            Session["Vorherige_Umfrage"] = umfrageId.ToString();
            Survey ausgewählteUmfrage = db.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.givenAnswer
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == umfrageId);
            ICollection<SessionViewModel> sessionListe = _sessionZuViewTransformer.ListTransform(ausgewählteUmfrage?.sessions).ToList();
            sessionListe = sessionListe.OrderByDescending(m => m.creationDate).ToList();
            sessionListe.First().surveyviewModel = 
                _umfrageZuViewTransformerMitFragen.Transform(ausgewählteUmfrage);

            if (ausgewählteUmfrage == null || (!BenutzerDarfDas(ausgewählteUmfrage.Creator) || ausgewählteUmfrage.states != Survey.States.Beendet))
            {
                return RedirectToAction("StatusUmfrageAuswertung", "Fehlermeldungen");
            }

            return View(sessionListe);
        }

        public ActionResult Antworten(Guid arg)
        {
            Guid sessionId = arg;
            Session ausgewählteSession = db.Sessions
                .Include(a => a.givenAnswer
                .Select(c => c.question)
                .Select(g => g.survey))
                .Include(a => a.givenAnswer
                .Select(c => c.question)
                .Select(g => g.choice))
                .FirstOrDefault(b => b.ID == sessionId);

            ICollection<GivenAnswerViewModel> beantwortungListe = _beantwortungZuViewTransformer.ListTransform(ausgewählteSession?.givenAnswer).ToList();
            beantwortungListe = beantwortungListe.OrderBy(m => m.questionViewModel.position).ToList();

            if (ausgewählteSession == null || (!BenutzerDarfDas(ausgewählteSession.survey.Creator) || ausgewählteSession.survey.states != Survey.States.Beendet))
            {
                return RedirectToAction("StatusUmfrageAuswertung", "Fehlermeldungen");
            }

            return View(beantwortungListe.ToList());
        }

        public PartialViewResult Freitext_Einzel(GivenAnswerViewModel beantwortung)
        {
            return PartialView(beantwortung);
        }

        public PartialViewResult Skala_Einzel(GivenAnswerViewModel beantwortung)
        {
            return PartialView(beantwortung);
        }

        public PartialViewResult MultipleOne_Einzel(GivenAnswerViewModel beantwortung)
        {
            return PartialView(beantwortung);
        }

        public PartialViewResult MultipleMore_Einzel(GivenAnswerViewModel beantwortung)
        {
            return PartialView(beantwortung);
        }

        public PartialViewResult MultipleOneMitSonstiges_Einzel(GivenAnswerViewModel beantwortung)
        {
            return PartialView(beantwortung);
        }

        public PartialViewResult MultipleMoreMitSonstiges_Einzel(GivenAnswerViewModel beantwortung)
        {
            return PartialView(beantwortung);
        }

        public ActionResult Fragen_Ergebnisse(Guid arg)
        {
            Guid umfrageId = arg;
            Survey ausgewählteUmfrage = db.Surveys
                .Include(a => a.questions
                .Select(c => c.givenAnswer.Select(k => k.session)))
                .Include(s => s.questions
                .Select(t => t.choice))
                .FirstOrDefault(b => b.ID == umfrageId);
            var fragenListe = _fragenZuViewTransformer.ListTransform(ausgewählteUmfrage?.questions);
            fragenListe = fragenListe.OrderBy(u => u.position).ToList();

            fragenListe.First().surveyViewModel =
                _umfrageZuViewTransformerMitFragen.Transform(ausgewählteUmfrage);

            if (!BenutzerDarfDas(fragenListe.First().surveyViewModel.Creator) || fragenListe.First().surveyViewModel.states != Survey.States.Beendet)
            {
                return RedirectToAction("StatusUmfrageAuswertung", "Fehlermeldungen");
            }

            return View(fragenListe.ToList());
        }

        private bool BenutzerDarfDas(Guid creator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == creator.ToString()).Email;
            var a = User.Identity.Name;
            var darfErDasWirklich = a == benutzerText || User.Identity.Name == "Admin@FI17.de";

            return darfErDasWirklich;
        }

        public PartialViewResult Panel_fuer_Frage_in_kumulierter_Auswertung(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
        public PartialViewResult Freitext_Kumuliert(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
        public PartialViewResult MultipleOne_Kumuliert(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
        public PartialViewResult Skalenfrage_Kumuliert(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
        public PartialViewResult MultipleOneMitSonstiges_Kumuliert(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
        public PartialViewResult MultipleMore_Kumuliert(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
        public PartialViewResult MultipleMoreMitSonstiges_Kumuliert(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
    }
}
