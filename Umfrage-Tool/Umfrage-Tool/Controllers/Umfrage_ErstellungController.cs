using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Acces;

namespace Umfrage_Tool.Controllers
{
    public class Umfrage_ErstellungController : Controller
    {
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();

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
            return RedirectToAction("FrageErstellung");
        }


        public ActionResult FrageErstellung()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FrageErstellung(QuestionViewModel questionModel)
        {
            var question = questiontransformer.Transform(questionModel);
            db.Questions.Add(question);
            db.SaveChanges();
            return View();
        }
    }
}