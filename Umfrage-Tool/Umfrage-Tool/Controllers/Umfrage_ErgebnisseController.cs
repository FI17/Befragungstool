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
        AnsweringToModelAllTransformer answerTransformer = new AnsweringToModelAllTransformer();
        // GET: Umfrage_Ergebnisse
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //        [HttpGet]
        public ActionResult Index()
        {
            var models = new List<SurveyViewModel>();
            foreach (Survey survey in data.Surveys.Include(d => d.sessions))
            {
                models.Add(modelTransformer.Transform(survey));
            }
            return View(models);
        }
        public ActionResult Ergebnisse(Guid? umfrageID)
        {
            umfrageID = new Guid(Request.Url.Segments.Last());
            var sessionModels = new List<SessionViewModel>();
            Survey sessions = data.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.answerings
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == umfrageID);
            foreach (var session in sessions.sessions)
            {
                sessionModels.Add(sessionTransformer.Transform(session));
            }
            return View(sessionModels);
        }

        //Request["parameter1"];
        public ActionResult Antworten(Guid? sessionID)
        {
            sessionID = new Guid(Request.Url.Segments.Last());
            //sessionID = new Guid("c17ceb41-f547-41e7-ac4a-eba72b743c24");
            var answerModel = new List<AnsweringViewModel>();
            Session sessions = data.Sessions
                .Include(a => a.answerings
                .Select(c => c.question)
                .Select(g => g.survey))
                .FirstOrDefault(b => b.ID == sessionID);
            foreach (var answering in sessions.answerings)
            {
                answerModel.Add(answerTransformer.Transform(answering));
            }
            return View(answerModel);
        }
    }
}


   