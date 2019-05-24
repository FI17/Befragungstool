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
            
            try
            {
                Session["Umfrage"] = Request.QueryString["arg"].ToString();
            }
            catch
            {
                return RedirectToAction("Fehlermeldung", "Fehlermeldungen", new { aufruf = "UmfrageBeantwortungExistiertNicht"});
            }

            if (Umfrage().questionViewModels.Count == 0 || Umfrage().states != Survey.States.Öffentlich)
            {
                return RedirectToAction("Fehlermeldung", "Fehlermeldungen", new { aufruf = "StatusUmfrageBeantwortung"});
            }
            SurveyViewModel Umfrage_View = Umfrage(); 
            Umfrage_View = Umfrage_Kontrollieren(Umfrage_View);
            return View(Umfrage_View); 
        }

        [HttpPost]
        public ActionResult Index(List<GivenAnswerViewModel> antworten)
        {
            Guid umfrage_ID = Umfrage().ID;
            Guid sitzungsID;
            Guid frageID;

            Session sitzungs_Daten;
            SessionViewModel sitzung = new SessionViewModel();
            sitzung.surveyviewModel = Umfrage();
            sitzung.ID = Guid.NewGuid();
            sitzungs_Daten = model_zu_Sitzung_Transformer.Transform(sitzung);
            sitzungs_Daten.survey = db.Surveys.First(se => se.ID == umfrage_ID);
            db.Sessions.Add(sitzungs_Daten);
            db.SaveChanges();
           
            foreach (var beantwortung in antworten)
            {
                frageID = Umfrage().questionViewModels.ToList()[Convert.ToInt32(beantwortung.questionViewModel.position)].ID;
                sitzungsID = sitzungs_Daten.ID;
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
            return RedirectToAction("Fehlermeldung", "Fehlermeldungen", new { aufruf = "Umfrage_beendet" });
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
        public PartialViewResult MultipleOneMitSonstiges(QuestionViewModel Frage)
        {
            Session["FragenIndex"] = Convert.ToInt32(Session["FragenIndex"]) + 1;
            return PartialView(Frage);
        }

        public PartialViewResult MultipleMoreMitSonstiges(QuestionViewModel Frage)
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
                .Include(b => b.questions
                    .Select(p => p.chapter))
                .Include(b => b.chapters
                    .Select(p => p.questions))
                .FirstOrDefault();
            Umfrage_View = umfrage_zu_Model_Transformer.Transform(Umfrage_DB);
            Umfrage_View.questionViewModels = Umfrage_View.questionViewModels.OrderBy(m => m.position).ToList();

            return Umfrage_View;
        }

        public SurveyViewModel Umfrage_Kontrollieren(SurveyViewModel Umfrage_View)
        {
            ChapterViewModel testKapitel = new ChapterViewModel();
            testKapitel.ID = Guid.NewGuid();
            testKapitel.position = 0;
            testKapitel.text = "SfWA/DFcqYls7ZHjnK7JUODE057RVnr66GxTcxX05b2kwdoHHtTlVQ+CyH4oMm4khThHr+HHpFhuvk2+3LkfJOSt67vIGbCknaw3haS1oqZ2t9sEbPYDrEOE7UUibu9d";
            testKapitel.questionViewModels = Umfrage_View.questionViewModels;

            var fragenOhneKapitel = Umfrage_View.questionViewModels.Where(z => z.chapterViewModel == null);
            testKapitel.questionViewModels = fragenOhneKapitel.ToList();
            if (fragenOhneKapitel.Count() != 0)
            {
                Umfrage_View.chapterViewModels.Add(testKapitel);
                foreach (var frage in fragenOhneKapitel)
                {
                    frage.chapterViewModel = testKapitel;
                }
            }
            return Umfrage_View;
        }
    }
}