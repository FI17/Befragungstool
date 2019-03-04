using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Acces;
using System.Data.Entity;
using Domain;
using System.Web.UI;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Users = "Admin@FI17.de")]
    public class Umfrage_ErstellungController : Controller
    {
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();
        ModelToAnswerTransformer answerTransformer = new ModelToAnswerTransformer();
        SurveyToModelTransformer modeltransformer = new SurveyToModelTransformer();
        QuestionToModelTransformer modelquestionformer = new QuestionToModelTransformer();
        private DatabaseContent db = new DatabaseContent();

        public ActionResult Index()
        {
            Session["UmfrageID"] = "";
            Session["Fertig"] = "FALSE";
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

        public ActionResult FrageErstellung(Guid arg)
        {
            Session["Antwortlaenge"] = -1;
            var a = new QuestionViewModel();
            a.answers = new List<AnswerViewModel>();
            return View(a);
        }

        [HttpPost]
        public ActionResult FrageErstellung(QuestionViewModel model, string subject, Guid arg)
        {
            if (subject == "Ende")
            {
                return RedirectToAction("Index", "Home");
            }
            var questionData = questiontransformer.Transform(model);
            Survey S = db.Surveys.Include(b => b.questions).First(f => f.ID == arg);
            questionData.survey = S;
            questionData.position = S.questions.Count;
            db.Questions.Add(questionData);
            
            db.SaveChanges();
            if (subject == "Speichern und Ende")
            {
                Session["UmfrageID"] = arg.ToString();
                Session["Fertig"] = "TRUE";
                return RedirectToAction("FrageErstellung", new { arg = arg });
            }
            Session["UmfrageID"] = "";
            Session["Fertig"] = "FALSE";
            return RedirectToAction("FrageErstellung", new { arg = arg });
        }

        public ActionResult FragenTyp(QuestionViewModel frage)
        {
            switch (frage.typ.ToString())
            {
                case "Freitext":
                    return PartialView("Freitext_Vorschau", frage);
                case "MultipleChoice":
                    return PartialView("MultipleChoice_Vorschau", frage);
                case "SkalenFrage":
                    return PartialView("SkalenFragen_Vorschau", frage);
                default:
                    return PartialView("Freitext_Vorschau", frage);
            }
        }

        public PartialViewResult Freitext()
        {
            return PartialView();
        }

        public PartialViewResult MultipleChoice()
        {
            return PartialView();
        }

        public PartialViewResult Plus_Antwort(QuestionViewModel a)
        {
            Session["Antwortlaenge"] = Convert.ToInt32(Session["Antwortlaenge"]) + 1;
            for (int i = 0; i < Convert.ToInt32(Session["Antwortlaenge"]); i++)
            {
                a.answers.Add(new AnswerViewModel());
            }
            return PartialView(a);
        }

        public PartialViewResult FrageHinzufügen()
        {
            return PartialView();
        }

        public PartialViewResult Vorschau(Guid arg)
        {
            var questionModel = new List<QuestionViewModel>();
            List<Question> questionList = db.Questions
                .Where(i => i.survey.ID == arg)
                .Include(a=>a.answers)
                .ToList();
            questionModel = modelquestionformer.ListTransform(questionList).ToList();
            questionModel = questionModel.OrderBy(e=>e.position).ToList();
            questionModel.Reverse();
            return PartialView(questionModel);
        }
        public ActionResult Vorschau_Fragentyp(QuestionViewModel frage)
        {
            switch (frage.typ.ToString())
            {
                case "Freitext":
                    return PartialView("Freitext_Vorschau", frage);
                case "MultipleOne":
                    return PartialView("MultipleChoice_Vorschau", frage);
                case "Skalenfrage":
                    return PartialView("SkalenFragen_Vorschau", frage);
                default:
                    return PartialView("Freitext_Vorschau", frage);
            }
        }


        public PartialViewResult Freitext_Vorschau(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }

        public PartialViewResult MultipleChoice_Vorschau(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }

        public PartialViewResult SkalenFragen_Vorschau(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }

        public ActionResult Vorschauloeschfunktion(Guid arg)
        {
            
            List<Answering> Zu_loeschende_Beantwortungen = db.Answerings.Where(i => i.question.ID == arg).ToList();
            foreach (var item in Zu_loeschende_Beantwortungen)
            {
                db.Answerings.Remove(item);
            }
            List<Answer> Zu_loeschende_Antworten = db.Answers.Where(i => i.question.ID == arg).ToList();
            foreach (var item in Zu_loeschende_Antworten)
            { 
                db.Answers.Remove(item);
            }
            Question Zu_loeschende_Frage = db.Questions.Include(r=>r.survey).FirstOrDefault(i => i.ID == arg);
            Survey Mutter_Umfrage = Zu_loeschende_Frage.survey;
            db.Questions.Remove(Zu_loeschende_Frage);
            db.SaveChanges();
            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = Mutter_Umfrage.ID });
        }

    }
}