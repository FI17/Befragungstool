using Domain;
using Domain.Acces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Umfrage_Tool.Controllers
{
    public class Umfrage_BeantwortungController : Controller
    {
        private DatabaseContent db = new DatabaseContent();
        ModelToAnsweringTransformer answertransformer = new ModelToAnsweringTransformer();
        ModelToSessionTransformer sessiontransformer = new ModelToSessionTransformer();
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        // GET: Umfrage_Beantwortung
        public ActionResult Index()
        {
            Session sessionData;
            try
            {
                Session["Umfrage"] = Request.QueryString["arg"].ToString();            
                SessionViewModel session = new SessionViewModel();
                session.surveyviewModel = Umfrage();
                sessionData = sessiontransformer.Transform(session);
                Guid survey_ID = Umfrage().ID;
                sessionData.survey = db.Surveys.First(se => se.ID == survey_ID);
            }
            catch
            {
                return RedirectToAction("Umfrage_fehlgeschlagen", "Umfrage_Beantwortung");
            }

            if (Umfrage().questionViewModels.Count == 0)
            {
                return RedirectToAction("Umfrage_fehlgeschlagen", "Umfrage_Beantwortung");
            }

            db.Sessions.Add(sessionData);
            db.SaveChanges();
            Session["Session"] = sessionData.ID;
            Session["Aktuelle_Frage"] = 0;

            return View(Umfrage());
        }

        public ActionResult FragenObjekt(QuestionViewModel Frage)
        {
            switch (Frage.typ)
            {
                case 0://Freitext
                    return PartialView("Freitext", Frage);
                default:
                    return PartialView("Freitext", Frage);
            }
        }

        [HttpPost]
        public ActionResult Index(string antworttext, string subject)
        {
            AnsweringViewModel antwort = new AnsweringViewModel();
            antwort.text = antworttext;

            antwort.questionViewModel = new QuestionViewModel();
            var answerData = answertransformer.Transform(antwort);

            Guid frage_ID = Umfrage().questionViewModels.ToList()[Convert.ToInt32(Session["Aktuelle_Frage"])].ID;
            answerData.question = db.Questions.First(s => s.ID == frage_ID);
            Guid session_ID = new Guid(Session["Session"].ToString());
            answerData.session = db.Sessions.First(se => se.ID == session_ID);
            
            db.Answerings.Add(answerData);
            db.SaveChanges();

            if (subject == "Fertigstellen")
            {
                return RedirectToAction("Umfrage_beendet", "Umfrage_Beantwortung");
            }
            else
            {
                Session["Aktuelle_Frage"] = Convert.ToInt32(Session["Aktuelle_Frage"]) + 1;
                return View(Umfrage());
            }

        }
        public ActionResult Umfrage_beendet()
        {
            return View();
        }

        public ActionResult Umfrage_fehlgeschlagen()
        {
            return View();
        }

        public PartialViewResult Freitext(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }

        public SurveyViewModel Umfrage()
        {
            SurveyToModelTransformer surveytransformer = new SurveyToModelTransformer();
            Survey Umfrage_DB = new Survey();
            SurveyViewModel Umfrage_View = new SurveyViewModel();

            Guid Gesuchte_Umfragen_ID = new Guid(Session["Umfrage"].ToString());
            Umfrage_DB = db.Surveys
                       .Where(b => b.ID == Gesuchte_Umfragen_ID)
                       .Include(b => b.questions)
                       .FirstOrDefault();
            Umfrage_View = surveytransformer.Transform(Umfrage_DB);
            Umfrage_View.questionViewModels = Umfrage_View.questionViewModels.OrderBy(m => m.position).ToList();

            return Umfrage_View;
        }
    }
}