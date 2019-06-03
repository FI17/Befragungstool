using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Acces;
using Microsoft.AspNet.Identity.Owin;
using Umfrage_Tool.Models;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Roles = "Betreuer, Admin")]

    public class HomeController : Controller
    {
        private readonly DatabaseContent _db = new DatabaseContent();
        private readonly SurveyToModelTransformer _umfrageZuModelTransformer = new SurveyToModelTransformer();

        private ApplicationUserManager _userManager;


        public HomeController()
        {
        }

        public HomeController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private ApplicationSignInManager SignInManager
        {
            set { }
        }

        public LoginViewModel Username()
        {
            var model = new LoginViewModel { Email = User.Identity.Name };

            return model;
        }

        public ActionResult Index()
        {
            return View(Username());
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

        public ActionResult Ändere_Ersteller_in_Datenbank(string umfrageIdString, string ersteller)
        {
            var umfrageId = new Guid(umfrageIdString);
            var erstellerId = new Guid(ersteller);
            var umfrage = _db.Surveys.First(z => z.ID == umfrageId);
            umfrage.Creator = erstellerId;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private List<SurveyViewModel> Liste_erstellter_Umfragen()
        {
            var userName = User.Identity.Name;
            var userIdString = UserManager.Users.First(d => d.Email == userName).Id;
            var userId = new Guid(userIdString);
            var umfrageListe = _db.Surveys.ToList();
            if (userName != "Admin@FI17.de") umfrageListe = _db.Surveys.Where(i => i.Creator == userId).ToList();

            var umfrageViewModelListe = _umfrageZuModelTransformer.ListTransform(umfrageListe.ToList());

            umfrageViewModelListe = umfrageViewModelListe.OrderByDescending(m => m.creationTime).ToList();

            foreach (var umfrage in umfrageViewModelListe)
            {
                var creatorIdText = Convert.ToString(umfrage.Creator);
                umfrage.CreatorName = UserManager.Users.First(j => j.Id == creatorIdText).Email;
            }

            return umfrageViewModelListe.ToList();
        }

        public ActionResult Umfrage_löschen(Guid arg)
        {
            var umfrage = _db.Surveys.First(d => d.ID == arg);

            if (!BenutzerDarfDas(umfrage.Creator))
                return RedirectToAction("Index", "Home");
            //TODO: Redirect to Custom Seite (Keine Berechtigung) 
            var zuLöschendeAntwortenListe = _db.GivenAnswers.Where(i => i.session.survey.ID == arg).ToList();
            foreach (var zuLöschendeAntwort in zuLöschendeAntwortenListe) _db.GivenAnswers.Remove(zuLöschendeAntwort);

            var zuLöschendeSessionsListe = _db.Sessions.Where(i => i.survey.ID == arg).ToList();
            foreach (var zuLöschendeSession in zuLöschendeSessionsListe) _db.Sessions.Remove(zuLöschendeSession);

            var zuLöschendeBeantwortungenListe = _db.Choices.Where(i => i.question.survey.ID == arg).ToList();
            foreach (var zuLöschendeBeantwortung in zuLöschendeBeantwortungenListe)
                _db.Choices.Remove(zuLöschendeBeantwortung);

            var zuLöschendeKapitelListe = _db.Chapters.Where(i => i.survey.ID == arg).ToList();
            foreach (var zuLöschendesKapitel in zuLöschendeKapitelListe) _db.Chapters.Remove(zuLöschendesKapitel);

            var zuLöschendeFragenListe = _db.Questions.Where(i => i.survey.ID == arg).ToList();
            foreach (var zuLöschendeFrage in zuLöschendeFragenListe) _db.Questions.Remove(zuLöschendeFrage);

            var zuLöschendeUmfrage = _db.Surveys.FirstOrDefault(i => i.ID == arg);
            if (zuLöschendeUmfrage != null) _db.Surveys.Remove(zuLöschendeUmfrage);

            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private bool BenutzerDarfDas(Guid umfrageCreator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == umfrageCreator.ToString()).Email;
            var a = User.Identity.Name;
            var darfErDasWirklich = a == benutzerText || User.Identity.Name == "Admin@FI17.de";

            return darfErDasWirklich;
        }

        public ActionResult StatusWechseln(Guid umfrageId)
        {
            var umfrage = _db.Surveys.First(f => f.ID == umfrageId);

            if (!BenutzerDarfDas(umfrage.Creator))
                return RedirectToAction("Index", "Home");
            //TODO: Redirect to custom Fehler-Seite (Keine Berechtigung) 

            switch (umfrage.states)
            {
                case Survey.States.InBearbeitung:
                    return RedirectToAction("Umfrage_freigeben", "Home", new { arg = umfrageId });
                case Survey.States.Öffentlich:
                    umfrage.states++;
                    umfrage.endTime = DateTime.Now;
                    _db.SaveChanges();
                    break;
                case Survey.States.Beendet:
                    break;
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Umfrage_freigeben(Guid arg)
        {
            var umfrage = _db.Surveys.First(d => d.ID == arg);

            if (!BenutzerDarfDas(umfrage.Creator))
                return RedirectToAction("Index", "Home");
            //TODO: Redirect to Custom Seite (Keine Berechtigung) 

            var umfrageViewModel = _umfrageZuModelTransformer.Transform(umfrage);

            return View(umfrageViewModel);
        }

        [HttpPost]
        public ActionResult Umfrage_freigeben(Guid arg, string subject, DateTime? enddatum = null)
        {
            var umfrage = _db.Surveys.First(d => d.ID == arg);
            umfrage.releaseTime = DateTime.Now;

            if (subject == "Umfrage veröffentlichen" && enddatum.HasValue)
            {
                umfrage.endTime = (DateTime)enddatum;
                var schließzeit = new TimeSpan(18, 0, 0);
                umfrage.endTime = umfrage.endTime.Date + schließzeit;
            }
            else
            {
                umfrage.endTime = DateTime.MaxValue;
            }

            umfrage.states++;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}