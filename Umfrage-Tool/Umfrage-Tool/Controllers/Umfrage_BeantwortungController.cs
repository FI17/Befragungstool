using Domain;
using Domain.Acces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

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
            Session["FragenIndex"] = -1;
            Session sitzungs_Daten;
            try
            {
                Session["Umfrage"] = Request.QueryString["arg"].ToString();
                SessionViewModel sitzung = new SessionViewModel();
                sitzung.surveyviewModel = Umfrage();
                sitzungs_Daten = model_zu_Sitzung_Transformer.Transform(sitzung);
                Guid umfrage_ID = Umfrage().ID;
                sitzungs_Daten.survey = db.Surveys.First(se => se.ID == umfrage_ID);
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

            return View(Umfrage());
        }

        [HttpPost]
        public ActionResult Index(List<GivenAnswerViewModel> antworten)
        {
            Guid sitzungsID;
            Guid frageID;
            foreach (var beantwortung in antworten)
            {
                frageID = Umfrage().questionViewModels.ToList()[Convert.ToInt32(beantwortung.questionViewModel.position)].ID;
                sitzungsID = new Guid(Session["Session"].ToString());

                beantwortung.questionViewModel = new QuestionViewModel();

                var dbBeantwortung = model_zu_Beantwortung_Transformer.Transform(beantwortung);

                if (beantwortung.arrayText != null)
                {
                    foreach (var item in beantwortung.arrayText)
                    {

                        db.GivenAnswers.Add(new GivenAnswer()
                        {
                            text = item,
                            question = db.Questions.First(s => s.ID == frageID),
                            session = db.Sessions.First(se => se.ID == sitzungsID)
                        });
                    }
                }


                dbBeantwortung.question = db.Questions.First(s => s.ID == frageID);
                dbBeantwortung.session = db.Sessions.First(se => se.ID == sitzungsID);
                if (dbBeantwortung.text != null && dbBeantwortung.text != "")
                {
                    db.GivenAnswers.Add(dbBeantwortung);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Umfrage_beendet", "Umfrage_Beantwortung");
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
            Session["FragenIndex"] = Convert.ToInt32(Session["FragenIndex"]) + 1;
            return PartialView(Frage);
        }
        public PartialViewResult MultipleOne(QuestionViewModel Frage)
        {
            Session["FragenIndex"] = Convert.ToInt32(Session["FragenIndex"]) + 1;
            return PartialView(Frage);
        }
        public PartialViewResult Skalenfrage(QuestionViewModel Frage)
        {
            Session["FragenIndex"] = Convert.ToInt32(Session["FragenIndex"]) + 1;
            return PartialView(Frage);
        }
        public PartialViewResult MultipleMore(QuestionViewModel Frage)
        {
            Session["FragenIndex"] = Convert.ToInt32(Session["FragenIndex"]) + 1;
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
                       .Select(p => p.choice))
                       .FirstOrDefault();
            Umfrage_View = umfrage_zu_Model_Transformer.Transform(Umfrage_DB);
            Umfrage_View.questionViewModels = Umfrage_View.questionViewModels.OrderBy(m => m.position).ToList();

            return Umfrage_View;
        }
    }
}