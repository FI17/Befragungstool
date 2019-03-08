using System;
using System.Collections.Generic;
using System.Linq;
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
        QuestionToModelTransformer questionTransformer = new QuestionToModelTransformer();

        public ActionResult Index()
        {
            ICollection<Survey> transformableSurvey = data.Surveys.Include(d => d.sessions).ToList();
            ICollection<SurveyViewModel> models = new List<SurveyViewModel>();
            models = modelTransformer.ListTransform(transformableSurvey);
            models = models.OrderByDescending(m => m.creationTime).ToList();
            return View(models);
        }
        public ActionResult Ergebnisse()
        {
            Guid umfrageID = new Guid(Request.Url.Segments.Last());
            Session["Vorherige_Umfrage"] = umfrageID.ToString();
            ICollection<SessionViewModel> sessionModels = new List<SessionViewModel>();
            Survey surveys = data.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.answerings
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == umfrageID);

            sessionModels = sessionTransformer.ListTransform(surveys.sessions).ToList();

            sessionModels = sessionModels.OrderByDescending(m => m.creationDate).ToList();
            return View(sessionModels);
        }

        public ActionResult Antworten()
        {
            Guid sessionID = new Guid(Request.Url.Segments.Last());
            ICollection<AnsweringViewModel> answerModel = new List<AnsweringViewModel>();
            Session sessions = data.Sessions
                .Include(a => a.answerings
                .Select(c => c.question)
                .Select(g => g.survey))
                .Include(a => a.answerings
                .Select(c => c.question)
                .Select(g => g.answers))
                .FirstOrDefault(b => b.ID == sessionID);

            answerModel = answerTransformer.ListTransform(sessions.answerings).ToList();

            answerModel = answerModel.OrderBy(m => m.questionViewModel.position).ToList();
            return View(answerModel);
        }

        public ActionResult Fragen_Ergebnisse(Guid arg)
        {

            Guid SurveyID = arg;

            ICollection<QuestionViewModel> questionModel = new List<QuestionViewModel>();
            Survey surveys = data.Surveys
                .Include(a => a.questions
                .Select(c => c.answerings))
                .Include(s => s.questions
                .Select(t => t.answers))
                .FirstOrDefault(b => b.ID == SurveyID);

            questionModel = questionTransformer.ListTransform(surveys.questions);

            return View(questionModel);
        }
    }
}