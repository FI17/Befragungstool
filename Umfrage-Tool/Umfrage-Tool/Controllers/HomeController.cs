using Domain;
using Domain.Acces;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Umfrage_Tool.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Roles = "Ersteller, Admin")]

    public class HomeController : Controller
    {
        DatabaseContent db = new DatabaseContent();
        SurveyToModelTransformer umfrage_zu_Model_Transformer = new SurveyToModelTransformer();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        



        public HomeController()
        {

        }

        public HomeController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
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

        public LoginViewModel username()
        {
            var model = new LoginViewModel();
            model.Email = User.Identity.Name;

            return model;
        }

        public ActionResult Index()
        {
            return View(username());
        }



        public PartialViewResult Erstellte_Umfragen_Normaler_Nutzer()
        {
            return PartialView(Liste_erstellter_Umfragen());
        }

        public PartialViewResult Erstellte_Umfragen_Admin()
        {
            return PartialView(Liste_erstellter_Umfragen());
        }

        public PartialViewResult Ersteller_ändern()
        {
            var benutzerListe = UserManager.Users.ToList();
            return PartialView(benutzerListe);
        }

        public ActionResult Ändere_Ersteller_in_Datenbank(string Umfrage, string Ersteller)
        {
            Guid umfrageID = new Guid(Umfrage);
            Guid erstellerID = new Guid(Ersteller);
            Survey umfrage = db.Surveys.First(z=>z.ID==umfrageID);
            umfrage.Creator = erstellerID;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private List<SurveyViewModel> Liste_erstellter_Umfragen()
        {


            var a = User.Identity.Name;
            var sUserID = UserManager.Users.First(d => d.Email == a).Id;
            var userID = new Guid(sUserID);
            var umfrage_Liste = db.Surveys.ToList();
            if (a != "Admin@FI17.de")
            {
                umfrage_Liste = db.Surveys.Where(i => i.Creator == userID).ToList();
            }

            ICollection<SurveyViewModel> umfrage_View_Model_Liste = new List<SurveyViewModel>();

            umfrage_View_Model_Liste = umfrage_zu_Model_Transformer.ListTransform(umfrage_Liste.ToList());

            umfrage_View_Model_Liste = umfrage_View_Model_Liste.OrderByDescending(m => m.creationTime).ToList();

            foreach(var umfrage in umfrage_View_Model_Liste)
            {
                var CreatorIDText = Convert.ToString(umfrage.Creator);
                umfrage.CreatorName = UserManager.Users.First(j => j.Id == CreatorIDText).Email;
            }
            return umfrage_View_Model_Liste.ToList();
        }

        public ActionResult Umfrage_loeschen(Guid arg)
        {
            var umfrage = db.Surveys.First(d => d.ID == arg);

            if (!BenutzerDarfDas(umfrage.Creator))
            {
                return RedirectToAction("Index", "Home");
                //TODO: Redirect to Custom Seite (Keine Berechtigung) 
            }
            List<GivenAnswer> zu_loeschende_Antworten_Liste = db.GivenAnswers.Where(i => i.session.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Antwort in zu_loeschende_Antworten_Liste)
            {
                db.GivenAnswers.Remove(zu_loeschende_Antwort);
            }

            List<Session> zu_loeschende_Sessions_Liste = db.Sessions.Where(i => i.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Session in zu_loeschende_Sessions_Liste)
            {
                db.Sessions.Remove(zu_loeschende_Session);
            }

            List<Choice> zu_loeschende_Beantwortungen_Liste = db.Choices.Where(i => i.question.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Beantwortung in zu_loeschende_Beantwortungen_Liste)
            {
                db.Choices.Remove(zu_loeschende_Beantwortung);
            }

            List<Chapter> zu_loeschende_Kapitel_Liste = db.Chapters.Where(i => i.survey.ID == arg).ToList();
            foreach (var zu_loeschendes_Kapitel in zu_loeschende_Kapitel_Liste)
            {
                db.Chapters.Remove(zu_loeschendes_Kapitel);
            }

            List<Question> zu_loeschende_Fragen_Liste = db.Questions.Where(i => i.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Frage in zu_loeschende_Fragen_Liste)
            {
                db.Questions.Remove(zu_loeschende_Frage);
            }

            Survey zu_loeschende_Umfrage = db.Surveys.FirstOrDefault(i => i.ID == arg);
            db.Surveys.Remove(zu_loeschende_Umfrage);
            
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private bool BenutzerDarfDas(Guid umfrageCreator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == umfrageCreator.ToString()).Email;
            var a = User.Identity.Name;
            var DarfErDasWirklich = a == benutzerText;
            if (User.Identity.Name == "Admin@FI17.de")
            {
                //TODO: Name an richtigen Benutzer anpassen
                DarfErDasWirklich = true;
            }

            return DarfErDasWirklich;
        }

        public ActionResult StatusWechseln(Guid umfrageID)
        {
            var umfrage = db.Surveys.First(f => f.ID == umfrageID);

            if (!BenutzerDarfDas(umfrage.Creator))
            {
                return RedirectToAction("Index", "Home");
                //TODO: Redirect to Custom Seite (Keine Berechtigung) 
            }

            if (umfrage.states == Survey.States.InBearbeitung)
            {
                return RedirectToAction("Umfrage_freigeben", "Home", new { arg=umfrageID });
            }

            if (umfrage.states != Survey.States.Beendet)
            {
                umfrage.states++;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Umfrage_freigeben(Guid arg)
        {
            var umfrage = db.Surveys.First(d => d.ID == arg);

            if (!BenutzerDarfDas(umfrage.Creator))
            {
                return RedirectToAction("Index", "Home");
                //TODO: Redirect to Custom Seite (Keine Berechtigung) 
            }

            var umfrageViewModel = umfrage_zu_Model_Transformer.Transform(umfrage);

            return View(umfrageViewModel);
        }

        [HttpPost]
        public ActionResult Umfrage_freigeben(Guid arg, string subject, DateTime? Enddatum = null)
        {
            DateTime nutzEnddatum = DateTime.MaxValue;
            if (Enddatum.HasValue)
            {
                nutzEnddatum = Enddatum.Value;
            }

            var umfrage = db.Surveys.First(d => d.ID == arg);
            umfrage.releaseTime = DateTime.Now;
            
            switch (subject)
            {
                case "Umfrage veröffentlichen":
                    umfrage.endTime = nutzEnddatum;
                    break;
                case "Umfrage ohne festes Enddatum veröffentlichen":
                    umfrage.endTime = DateTime.MaxValue;
                    break;
            }

            umfrage.states++;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}