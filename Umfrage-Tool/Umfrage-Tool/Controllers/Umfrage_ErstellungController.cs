using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Acces;
using System.Data.Entity;
using Domain;

namespace Umfrage_Tool.Controllers
{
    public class Umfrage_ErstellungController : Controller
    {
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();
        SurveyToModelTransformer modeltransformer = new SurveyToModelTransformer();
        private DatabaseContent db = new DatabaseContent();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SurveyViewModel umfrage)
        {
            var surveyData = surveytransformer.Transform(umfrage);
            db.Surveys.Add(surveyData);
            db.SaveChanges();
            var umfrage4 = modeltransformer.Transform(surveyData);
            return RedirectToAction("FrageErstellung", new { arg = umfrage4.ID });
        }

        public ActionResult FrageErstellung()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult FrageErstellung(QuestionViewModel frage, string subject, Guid arg)
        {
            var questionData = questiontransformer.Transform(frage);
            Survey S = db.Surveys.Include(b => b.questions).First(f => f.ID == arg);
            questionData.survey = S;
            questionData.position = S.questions.Count;
            db.Questions.Add(questionData);
            db.SaveChanges();
            if (subject == "Speichern und Ende")
            {                
                return RedirectToAction("Index");
            }            
            return RedirectToAction("FrageErstellung", new { arg = arg });
        }
    }
}