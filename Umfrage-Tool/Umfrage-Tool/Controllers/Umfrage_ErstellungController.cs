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
    [Authorize(Users ="Admin@FI17.de")]
    public class Umfrage_ErstellungController : Controller
    {
        ModelToSurveyTransformer surveytransformer = new ModelToSurveyTransformer();
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();
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
            return View();
        }

        [HttpPost]
        public ActionResult FrageErstellung(QuestionViewModel frage, string subject, Guid arg)
        {
            if (subject == "Ende")
            {
                return RedirectToAction("Index", "Home");
            }
            var questionData = questiontransformer.Transform(frage);
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

        public PartialViewResult Plus_Antwort()
        {
            return PartialView();
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

    }
}