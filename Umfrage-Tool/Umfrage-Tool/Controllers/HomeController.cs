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
        SurveyToModelTransformer umfrage_zu_Model_Transformer = new SurveyToModelTransformer();

        public ActionResult Index()
        {
            var umfrage_Liste = db.Surveys;

            ICollection<SurveyViewModel> umfrage_View_Model_Liste = new List<SurveyViewModel>();

            umfrage_View_Model_Liste = umfrage_zu_Model_Transformer.ListTransform(umfrage_Liste.ToList());

            umfrage_View_Model_Liste = umfrage_View_Model_Liste.OrderByDescending(m => m.creationTime).ToList();

            return View(umfrage_View_Model_Liste);
        }

        public ActionResult Loeschfunktion(Guid arg)
        {
            List<Answering> Zu_loeschende_Antworten_Liste = db.Answerings.Where(i => i.session.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Antwort in Zu_loeschende_Antworten_Liste)
            {
                db.Answerings.Remove(Zu_loeschende_Antwort);
            }

            List<Session> Zu_loeschende_Sessions_Liste = db.Sessions.Where(i => i.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Session in Zu_loeschende_Sessions_Liste)
            {
                db.Sessions.Remove(Zu_loeschende_Session);
            }

            List<Answer> Zu_loeschende_Beantwortungen_Liste = db.Answers.Where(i => i.question.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Beantwortung in Zu_loeschende_Beantwortungen_Liste)
            {
                db.Answers.Remove(Zu_loeschende_Beantwortung);
            }

            List<Question> Zu_loeschende_Fragen_Liste = db.Questions.Where(i => i.survey.ID == arg).ToList();
            foreach (var Zu_loeschende_Frage in Zu_loeschende_Fragen_Liste)
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