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
    [Authorize(Roles = "Betreuer, Admin")]

    public class Auswertung_EinzelController : Controller
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


        public Auswertung_EinzelController() { }

        public Auswertung_EinzelController(ApplicationUserManager userManager)
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

        private bool BenutzerDarfDas(Guid creator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == creator.ToString()).Email;
            var a = User.Identity.Name;
            var darfErDasWirklich = a == benutzerText || User.Identity.Name == "Admin@FI17.de";

            return darfErDasWirklich;
        }
    }
}
