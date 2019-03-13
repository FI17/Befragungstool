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
        ModelToAnsweringTransformer model_zu_Beantwortung_Transformer = new ModelToAnsweringTransformer();
        ModelToSessionTransformer model_zu_Sitzung_Transformer = new ModelToSessionTransformer();
        SurveyToModelTransformer umfrage_zu_Model_Transformer = new SurveyToModelTransformer();

        public ActionResult Index()
        {
            Session sitzungs_Daten;
            try
            {
                Session["Umfrage"] = Request.QueryString["arg"].ToString();
                SessionViewModel sitzung = new SessionViewModel();
                sitzung.surveyviewModel = Umfrage();
                sitzungs_Daten = model_zu_Sitzung_Transformer.Transform(sitzung);
                Guid survey_ID = Umfrage().ID;
                sitzungs_Daten.survey = db.Surveys.First(se => se.ID == survey_ID);
            }
            catch
            {
                return RedirectToAction("Umfrage_fehlgeschlagen", "Umfrage_Beantwortung");
            }

            if (Umfrage().questionViewModels.Count == 0)
            {
                return RedirectToAction("Umfrage_fehlgeschlagen", "Umfrage_Beantwortung");
            }

            db.Sessions.Add(sitzungs_Daten);
            db.SaveChanges();
            Session["Session"] = sitzungs_Daten.ID;
            Session["Aktuelle_Frage"] = 0;

            return View(Umfrage());
        }

        [HttpPost]
        public ActionResult Index(string antworttext, string Wert_Bestätigungsknopf)
        {
            AnsweringViewModel antwort = new AnsweringViewModel();
            antwort.text = antworttext;

            antwort.questionViewModel = new QuestionViewModel();
            var answerData = model_zu_Beantwortung_Transformer.Transform(antwort);

            Guid frage_ID = Umfrage().questionViewModels.ToList()[Convert.ToInt32(Session["Aktuelle_Frage"])].ID;
            answerData.question = db.Questions.First(s => s.ID == frage_ID);
            Guid session_ID = new Guid(Session["Session"].ToString());
            answerData.session = db.Sessions.First(se => se.ID == session_ID);

            db.Answerings.Add(answerData);
            db.SaveChanges();

            if (Wert_Bestätigungsknopf == "Fertigstellen")
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
        public PartialViewResult MultipleOne(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult Skalenfrage(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }

        public SurveyViewModel Umfrage()
        {
            Survey Umfrage_DB = new Survey();
            SurveyViewModel Umfrage_View = new SurveyViewModel();

            Guid Gesuchte_Umfragen_ID = new Guid(Session["Umfrage"].ToString());
            Umfrage_DB = db.Surveys
                       .Where(b => b.ID == Gesuchte_Umfragen_ID)
                       .Include(b => b.questions
                       .Select(p => p.answers))
                       .FirstOrDefault();
            Umfrage_View = umfrage_zu_Model_Transformer.Transform(Umfrage_DB);
            Umfrage_View.questionViewModels = Umfrage_View.questionViewModels.OrderBy(m => m.position).ToList();

            return Umfrage_View;
        }
    }
}