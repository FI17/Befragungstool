using Domain;
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
        ModelToSurveyTransformer modeltransformer = new ModelToSurveyTransformer();
        SurveyToModelTransformer surveytransformer = new SurveyToModelTransformer();

        public ActionResult Index()
        {
            var Survey_List = db.Surveys;

            ICollection<SurveyViewModel> umfrage3 = new List<SurveyViewModel>();

            umfrage3 = surveytransformer.ListTransform(Survey_List.ToList());

            umfrage3 = umfrage3.OrderByDescending(m => m.creationTime).ToList();

            return View(umfrage3);
        }

        public ActionResult Loeschfunktion(Guid arg)
        {
            List<Answering> Zu_loeschende_Antworten = db.Answerings.Where(i => i.session.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Antwort in Zu_loeschende_Antworten)
            {
                db.Answerings.Remove(Zu_loeschende_Antwort);
            }

            List<Session> Zu_loeschende_Sessions = db.Sessions.Where(i => i.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Session in Zu_loeschende_Sessions)
            {
                db.Sessions.Remove(Zu_loeschende_Session);
            }

            List<Answer> Zu_loeschende_Beantwortungen = db.Answers.Where(i => i.question.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Beantwortung in Zu_loeschende_Beantwortungen)
            {
                db.Answers.Remove(Zu_loeschende_Beantwortung);
            }

            List<Question> Zu_loeschende_Fragen = db.Questions.Where(i => i.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Frage in Zu_loeschende_Fragen)
            {
                db.Questions.Remove(Zu_loeschende_Frage);
            }

            Survey Zu_loeschende_Umfrage = db.Surveys.FirstOrDefault(i => i.ID == arg);
            db.Surveys.Remove(Zu_loeschende_Umfrage);
            
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}