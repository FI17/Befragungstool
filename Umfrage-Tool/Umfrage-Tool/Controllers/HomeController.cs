﻿using Domain;
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

        private List<SurveyViewModel> Liste_erstellter_Umfragen()
        {
            var umfrage_Liste = db.Surveys;


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
        public ActionResult StatusWechseln(Guid umfrageID)
        {
            var umfrage = db.Surveys.First(f => f.ID == umfrageID);
            if (umfrage.states != Survey.States.Beendet)
            {
                umfrage.states++;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}