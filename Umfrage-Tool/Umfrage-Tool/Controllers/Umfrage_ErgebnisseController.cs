using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Acces;
using System.Data.Entity;

namespace Umfrage_Tool.Controllers
{
    public class Umfrage_ErgebnisseController : Controller
    {
        DatabaseContent data = new DatabaseContent();
        SurveyToModelQuestionTransformer modelTransformer = new SurveyToModelQuestionTransformer();
        SessionToModelTransformer sessionTransformer = new SessionToModelTransformer();
        // GET: Umfrage_Ergebnisse
        //public ActionResult Index()
        //{
        //    return View();
        //}

//        [HttpGet]
        public ActionResult Index()
        {
            var models = new List<SurveyViewModel>();
            foreach (Survey survey in data.Surveys.Include(d=> d.sessions))
            {
                models.Add(modelTransformer.Transform(survey));
            }
            return View(models);
        }
        public ActionResult Ergebnisse(Guid? umfrageID)
        {
            umfrageID = new Guid("150603ac-a13d-4574-9f69-a52049403c90");
            var sessionModels = new List<SessionViewModel>();
            Survey sessions = data.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.answerings
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == umfrageID);
            foreach(var session in sessions.sessions)
            {
                sessionModels.Add(sessionTransformer.Transform(session));
            }
            return View(sessionModels);
        }
        public ActionResult Antworten()
        {
            return View();
        }
    }
}