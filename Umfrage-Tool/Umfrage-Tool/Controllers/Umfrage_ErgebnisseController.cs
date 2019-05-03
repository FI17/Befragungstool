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
        SurveyToModelQuestionTransformer umfrage_zu_View_Tranformer_mit_Fragen = new SurveyToModelQuestionTransformer();
        SessionToModelTransformer session_zu_View_Transformer = new SessionToModelTransformer();
        AnsweringToModelAllTransformer beantwortung_zu_View_Transformer = new AnsweringToModelAllTransformer();
        QuestionToModelTransformer fragen_zu_View_Transformer = new QuestionToModelTransformer();

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

        public Umfrage_ErgebnisseController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Ergebnisse(Guid arg)
        {
            Guid Umfrage_ID = arg;
            Session["Vorherige_Umfrage"] = Umfrage_ID.ToString();
            ICollection<SessionViewModel> Session_Liste = new List<SessionViewModel>();
            Survey ausgewaehlte_Umfrage = db.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.givenAnswer
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == Umfrage_ID);
            Session_Liste = session_zu_View_Transformer.ListTransform(ausgewaehlte_Umfrage.sessions).ToList();
            Session_Liste = Session_Liste.OrderByDescending(m => m.creationDate).ToList();
            Session_Liste.First().surveyviewModel = 
                umfrage_zu_View_Tranformer_mit_Fragen.Transform(ausgewaehlte_Umfrage);

            if (!BenutzerDarfDas(ausgewaehlte_Umfrage.Creator) || ausgewaehlte_Umfrage.states != Survey.States.Beendet)
            {
                return RedirectToAction("StatusUmfrageAuswertung", "Fehlermeldungen");
            }

            return View(Session_Liste);
        }

        public ActionResult Antworten(Guid arg)
        {
            Guid Session_ID = arg;
            ICollection<GivenAnswerViewModel> Beantwortung_Liste = new List<GivenAnswerViewModel>();
            Session ausgewaehlte_Session = db.Sessions
                .Include(a => a.givenAnswer
                .Select(c => c.question)
                .Select(g => g.survey))
                .Include(a => a.givenAnswer
                .Select(c => c.question)
                .Select(g => g.choice))
                .FirstOrDefault(b => b.ID == Session_ID);

            Beantwortung_Liste = beantwortung_zu_View_Transformer.ListTransform(ausgewaehlte_Session.givenAnswer).ToList();
            Beantwortung_Liste = Beantwortung_Liste.OrderBy(m => m.questionViewModel.position).ToList();

            if (!BenutzerDarfDas(ausgewaehlte_Session.survey.Creator) || ausgewaehlte_Session.survey.states != Survey.States.Beendet)
            {
                return RedirectToAction("StatusUmfrageAuswertung", "Fehlermeldungen");
            }

            return View(Beantwortung_Liste.ToList());
        }

        public PartialViewResult Freitext_Einzel(GivenAnswerViewModel Beantwortung)
        {
            return PartialView(Beantwortung);
        }

        public PartialViewResult Skala_Einzel(GivenAnswerViewModel Beantwortung)
        {
            return PartialView(Beantwortung);
        }

        public PartialViewResult MultipleOne_Einzel(GivenAnswerViewModel Beantwortung)
        {
            return PartialView(Beantwortung);
        }

        public PartialViewResult MultipleMore_Einzel(GivenAnswerViewModel Beantwortung)
        {
            return PartialView(Beantwortung);
        }

        public PartialViewResult MultipleOneMitSonstiges_Einzel(GivenAnswerViewModel Beantwortung)
        {
            return PartialView(Beantwortung);
        }

        public PartialViewResult MultipleMoreMitSonstiges_Einzel(GivenAnswerViewModel Beantwortung)
        {
            return PartialView(Beantwortung);
        }

        public ActionResult Fragen_Ergebnisse(Guid arg)
        {
            Guid umfrage_ID = arg;
            ICollection<QuestionViewModel> fragen_Liste = new List<QuestionViewModel>();
            Survey ausgewaehlte_Umfrage = db.Surveys
                .Include(a => a.questions
                .Select(c => c.givenAnswer.Select(k => k.session)))
                .Include(s => s.questions
                .Select(t => t.choice))
                .FirstOrDefault(b => b.ID == umfrage_ID);
            fragen_Liste = fragen_zu_View_Transformer.ListTransform(ausgewaehlte_Umfrage.questions);
            fragen_Liste = fragen_Liste.OrderBy(u => u.position).ToList();

            fragen_Liste.First().surveyViewModel =
                umfrage_zu_View_Tranformer_mit_Fragen.Transform(ausgewaehlte_Umfrage);

            if (!BenutzerDarfDas(fragen_Liste.First().surveyViewModel.Creator) || fragen_Liste.First().surveyViewModel.states != Survey.States.Beendet)
            {
                return RedirectToAction("StatusUmfrageAuswertung", "Fehlermeldungen");
            }

            return View(fragen_Liste.ToList());
        }

        private bool BenutzerDarfDas(Guid creator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == creator.ToString()).Email;
            var a = User.Identity.Name;
            var DarfErDasWirklich = a == benutzerText;
            if (User.Identity.Name == "Admin@FI17.de")
            {
                //TODO: Name an richtigen Benutzer anpassen
                DarfErDasWirklich = true;
            }

            return DarfErDasWirklich;
        }

        public PartialViewResult Panel_fuer_Frage_in_kumulierter_Auswertung(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult Freitext_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult MultipleOne_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult Skalenfrage_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult MultipleOneMitSonstiges_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult MultipleMore_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult MultipleMoreMitSonstiges_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
    }
}
