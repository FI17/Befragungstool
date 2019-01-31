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
    [Authorize(Users = "Admin@FI17.de")]
    public class Umfrage_ErgebnisseController : Controller
    {
        DatabaseContent data = new DatabaseContent();
        SurveyToModelQuestionTransformer modelTransformer = new SurveyToModelQuestionTransformer();
        SessionToModelTransformer sessionTransformer = new SessionToModelTransformer();
        AnsweringToModelAllTransformer answerTransformer = new AnsweringToModelAllTransformer();
        
        public ActionResult Index()
        {
            var models = new List<SurveyViewModel>();
            foreach (Survey survey in data.Surveys.Include(d => d.sessions))
            {
                models.Add(modelTransformer.Transform(survey));
            }
            models = models.OrderBy(m => m.creationTime).ToList();
            models.Reverse();
            return View(models);
        }
        public ActionResult Ergebnisse()
        {
            Guid umfrageID = new Guid(Request.Url.Segments.Last());//(Request.QueryString["arg"].ToString());
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
            sessionModels = sessionModels.OrderBy(m => m.creationDate).ToList();
            sessionModels.Reverse();
            return View(sessionModels);
        }

        public ActionResult Antworten()
        {
            Guid sessionID = new Guid(Request.Url.Segments.Last());
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
            answerModel = answerModel.OrderBy(m =>m.questionViewModel.position).ToList();
            return View(answerModel);
        }
    }
}