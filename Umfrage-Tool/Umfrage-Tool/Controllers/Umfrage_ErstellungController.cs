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

        public ActionResult FrageErstellung(SurveyViewModel umfrage)
        {
            umfrage.ID = new Guid("6e524888-b725-4625-b2f1-3ce90c7c9810");
            QuestionViewModel neueFrage = new QuestionViewModel();
            neueFrage.surveyViewModel = umfrage;
            return View(neueFrage);
        }

        [HttpPost]
        public ActionResult FrageErstellung(QuestionViewModel frage, string subject, string arg)
        {
            var questionData = questiontransformer.Transform(frage);
            Guid HI = new Guid(arg);
            Survey S = db.Surveys.Include(b => b.questions).First(f => f.ID == HI);
            questionData.survey = S;
            questionData.position = S.questions.Count;
            db.Questions.Add(questionData);
            db.SaveChanges();

            if (subject == "Speichern und Ende")
            {                
                return RedirectToAction("Index");
            }
            //string script = "document.getElementById('text').value = ''";
            //ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            return RedirectToAction("FrageErstellung", new { arg = arg });
        }
    }
}