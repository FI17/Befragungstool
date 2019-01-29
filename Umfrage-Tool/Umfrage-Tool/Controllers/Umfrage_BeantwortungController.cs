using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umfrage_Tool.Models;
using Domain;
using Domain.Acces;
using System.Data.Entity;


namespace Umfrage_Tool.Controllers
{
    public class Umfrage_BeantwortungController : Controller
    {
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();        
        
        

        // GET: Umfrage_Beantwortung
        public ActionResult Index()
        {
            TempData["Umfrage"] = Diese_Umfrage();
            TempData["Aktuelle_Frage"] = 0;
            return View(Diese_Umfrage());
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

        public PartialViewResult Freitext(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }

        //[HttpPost]
        //public ActionResult Index(string subject, Guid arg, string Wichtig)
        //{
           
        //    ////var questionData = questiontransformer.Transform(frage);
        //    ////Survey S = db.Surveys.Include(b => b.questions).First(f => f.ID == arg);
        //    ////questionData.survey = S;
        //    ////questionData.position = S.questions.Count;
        //    ////db.Questions.Add(questionData);
        //    ////db.SaveChanges();

        //    //if (subject == "Fertigstellen")
        //    //{
        //    //    RedirectToAction("Index", "Home");
        //    //}
        //    //return new EmptyResult();
        //}

        SurveyViewModel Diese_Umfrage()
        {
            DatabaseContent db = new DatabaseContent();
            SurveyToModelTransformer I = new SurveyToModelTransformer();
            Survey U = new Survey();
            SurveyViewModel Umfrage = new SurveyViewModel();
            Guid Gesuchte_Umfragen_ID = new Guid(Request.QueryString["arg"].ToString());
            U = db.Surveys
                       .Where(b => b.ID == Gesuchte_Umfragen_ID)
                       .Include(b => b.questions)
                       .FirstOrDefault();
            Umfrage = I.Transform(U);
            Umfrage.questionViewModels = Umfrage.questionViewModels.OrderBy(m => m.position).ToList();
            return Umfrage;
        }
    }
}