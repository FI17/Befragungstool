﻿using Domain;
using Domain.Acces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Users = "Admin@FI17.de")]
    public class HomeController : Controller
    {
        DatabaseContent db = new DatabaseContent();
        SurveyToModelTransformer umfrage_zu_Model_Transformer = new SurveyToModelTransformer();

        public ActionResult Index()
        {
            var umfrage_Liste = db.Surveys;

            ICollection<SurveyViewModel> umfrage_View_Model_Liste = new List<SurveyViewModel>();

            umfrage_View_Model_Liste = umfrage_zu_Model_Transformer.ListTransform(umfrage_Liste.ToList());

            umfrage_View_Model_Liste = umfrage_View_Model_Liste.OrderByDescending(m => m.creationTime).ToList();

            return View(umfrage_View_Model_Liste);
        }

        public ActionResult Umfrage_loeschen(Guid arg)
        {
            List<Answering> zu_loeschende_Antworten_Liste = db.Answerings.Where(i => i.session.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Antwort in zu_loeschende_Antworten_Liste)
            {
                db.Answerings.Remove(zu_loeschende_Antwort);
            }

            List<Session> zu_loeschende_Sessions_Liste = db.Sessions.Where(i => i.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Session in zu_loeschende_Sessions_Liste)
            {
                db.Sessions.Remove(zu_loeschende_Session);
            }

            List<Answer> zu_loeschende_Beantwortungen_Liste = db.Answers.Where(i => i.question.survey.ID == arg).ToList();
            foreach (var zu_loeschende_Beantwortung in zu_loeschende_Beantwortungen_Liste)
            {
                db.Answers.Remove(zu_loeschende_Beantwortung);
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
    }
}