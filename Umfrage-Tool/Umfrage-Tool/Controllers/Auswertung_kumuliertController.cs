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

    public class Auswertung_kumuliertController : Controller
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


        public Auswertung_kumuliertController()  { }

        public Auswertung_kumuliertController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private bool BenutzerDarfDas(Guid creator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == creator.ToString()).Email;
            var a = User.Identity.Name;
            var darfErDasWirklich = a == benutzerText || User.Identity.Name == "Admin@FI17.de";

            return darfErDasWirklich;
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
            if (db.Surveys.First(s => s.ID == arg).sessions == null)
            {
                return RedirectToAction("AuswertungKeineAntworten", "Fehlermeldungen");
            }

            return View(fragenListe.ToList());
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
