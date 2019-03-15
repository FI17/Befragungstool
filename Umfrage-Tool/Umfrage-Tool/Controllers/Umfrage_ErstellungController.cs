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
            var a = new SurveyViewModel();
            a = modeltransformer.Transform(db.Surveys.Include(e => e.questions).FirstOrDefault(d => d.ID == arg));
            a.questionViewModels = a.questionViewModels.OrderBy(d => d.position).ToList();
            for (int i = 0; i < a.questionViewModels.Count(); i++)
            {
                Guid ID = a.questionViewModels.ToList()[i].ID;
                Question Frage = db.Questions.Include(e => e.answers).FirstOrDefault(d => d.ID == ID);
                a.questionViewModels.ToList()[i] = modelquestionformer.Transform(Frage);
            }
            return View(a);
        }

        [HttpPost]
        public ActionResult FrageErstellung(SurveyViewModel model, string subject, Guid arg)
        {
            switch (subject)
            {
                case "Ende":
                    return RedirectToAction("Index", "Home");
                case "Speichern und weitere Frage":

                    //if (model.questionViewModels.ToList().Last().text == null)
                    //{
                    //    //ViewBag.Javascript = "<script language='javascript' type='text/javascript'>alert(\"Der Fragetext darf nicht leer sein!\");</script>";

                    //    //return RedirectToAction("FrageErstellung", new { arg = arg });

                    //    string JavaScript = "<script>";
                    //    JavaScript += "alert(\"Der Fragetext darf nicht leer sein!\");";
                    //    JavaScript += "location.reload();";
                    //    JavaScript += "";
                    //    JavaScript += "</script>";
                    //    return Content(JavaScript);
                    //}

                    //Code Oben soll in Zukunft leere Fragentexte vermeiden

                    Neue_Frage_speichern(model, arg);
                    break;
                case "Aktualisieren":
                    Fragen_aktualisieren(model, arg);
                    break;
                case "Speichern und Ende":
                    Session["UmfrageID"] = arg.ToString();
                    Session["Fertig"] = "TRUE";
                    return RedirectToAction("FrageErstellung", new { arg = arg });
            }

            Session["UmfrageID"] = "";
            Session["Fertig"] = "FALSE";
            return RedirectToAction("FrageErstellung", new { arg = arg });
        }

        void Neue_Frage_speichern(SurveyViewModel model, Guid arg)
        {
            var surveyData = surveytransformer.Transform(model);
            Survey Umfrage_aus_DB_vor_neue_Frage = db.Surveys.Include(b => b.questions).First(f => f.ID == arg);
            List<List<Answer>> Antworten = new List<List<Answer>>();
            List<List<Answering>> Beantwortungen = new List<List<Answering>>();

            Question Neuste_Frage = questiontransformer.Transform(model.questionViewModels.ToList().Last());

            

            Neuste_Frage.position = Umfrage_aus_DB_vor_neue_Frage.questions.Count();
            Neuste_Frage.survey = Umfrage_aus_DB_vor_neue_Frage;
            db.Questions.Add(Neuste_Frage);

            if (Neuste_Frage.typ != Question.choices.Freitext)
            {
                foreach (var item in Neuste_Frage.answers)
                {
                    db.Answers.Add(item);
                }
            }
            db.SaveChanges();

            List<Question> Zu_loeschende_Fragen = db.Questions.Where(i => i.survey.ID == arg).Include(d => d.answers).ToList();



            for (int i = 0; i < Zu_loeschende_Fragen.Count(); i++)
            {
                Question Q = Zu_loeschende_Fragen[i];
                List<Answer> Zu_loeschende_Antworten = db.Answers.Where(d => d.question.ID == Q.ID).ToList();
                List<Answering> Zu_loeschende_Beantwortungen = db.Answerings.Where(d => d.question.ID == Q.ID).ToList();

                Antworten.Add(new List<Answer>());
                Beantwortungen.Add(new List<Answering>());
                foreach (var item in Zu_loeschende_Antworten)
                {
                    Antworten.Last().Add(item);
                    db.Answers.Remove(item);
                }
                foreach (var item in Zu_loeschende_Beantwortungen)
                {
                    Beantwortungen.Last().Add(item);
                    db.Answerings.Remove(item);
                }
                db.Questions.Remove(Q);
            }
            db.SaveChanges();



            for (int i = 0; i < model.questionViewModels.Count(); i++)
            {
                Question Neuere_Frage = questiontransformer.Transform(model.questionViewModels.ToList()[i]);
                if (Neuere_Frage.answers != null)
                {
                    for (int r = 0; r < Neuere_Frage.answers.Count(); r++)
                    {
                        Neuere_Frage.answers.ToList()[r].position = r;
                        Neuere_Frage.answers.ToList()[r].question = Neuere_Frage;
                    }
                }

                Neuere_Frage.survey = Umfrage_aus_DB_vor_neue_Frage;
                Neuere_Frage.position = i;
                db.Questions.Add(Neuere_Frage);
            }
            db.SaveChanges();
        }

        void Fragen_aktualisieren(SurveyViewModel model, Guid arg)
        {
            var surveyData = surveytransformer.Transform(model);
            Survey Umfrage_aus_DB_vor_neue_Frage = db.Surveys.Include(b => b.questions).First(f => f.ID == arg);
            List<List<Answer>> Antworten = new List<List<Answer>>();
            List<List<Answering>> Beantwortungen = new List<List<Answering>>();
            List<Question> Zu_loeschende_Fragen = db.Questions.Where(i => i.survey.ID == arg).Include(d => d.answers).ToList();
            
            for (int i = 0; i < Zu_loeschende_Fragen.Count(); i++)
            {
                Question Q = Zu_loeschende_Fragen[i];
                List<Answer> Zu_loeschende_Antworten = db.Answers.Where(d => d.question.ID == Q.ID).ToList();
                List<Answering> Zu_loeschende_Beantwortungen = db.Answerings.Where(d => d.question.ID == Q.ID).ToList();

                Antworten.Add(new List<Answer>());
                Beantwortungen.Add(new List<Answering>());
                foreach (var item in Zu_loeschende_Antworten)
                {
                    Antworten.Last().Add(item);
                    db.Answers.Remove(item);
                }
                foreach (var item in Zu_loeschende_Beantwortungen)
                {
                    Beantwortungen.Last().Add(item);
                    db.Answerings.Remove(item);
                }
                db.Questions.Remove(Q);
            }
            db.SaveChanges();



            for (int i = 0; i < model.questionViewModels.Count()-1; i++)
            {
                Question Neuere_Frage = questiontransformer.Transform(model.questionViewModels.ToList()[i]);
                if (Neuere_Frage.answers != null)
                {
                    for (int r = 0; r < Neuere_Frage.answers.Count(); r++)
                    {
                        Neuere_Frage.answers.ToList()[r].position = r;
                        Neuere_Frage.answers.ToList()[r].question = Neuere_Frage;
                    }
                }

                Neuere_Frage.survey = Umfrage_aus_DB_vor_neue_Frage;
                Neuere_Frage.position = i;
                db.Questions.Add(Neuere_Frage);
            }
            db.SaveChanges();
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
            Guid arg = new Guid(Session["UmfrageID"].ToString());
            Survey Umfrage = db.Surveys.Include(e => e.questions).FirstOrDefault(d => d.ID == arg);
            Session["Antwortlaenge"] = Convert.ToInt32(Session["Antwortlaenge"]) + 1;

            var a = new SurveyViewModel();
            a = modeltransformer.Transform(Umfrage);
            if (a.questionViewModels.Count() == 0)
            {
                a.questionViewModels = new List<QuestionViewModel>();
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
                .Include(a => a.answers)
                .ToList();
            questionModel = modelquestionformer.ListTransform(questionList).ToList();
            questionModel = questionModel.OrderBy(e => e.position).ToList();
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
            Question Zu_loeschende_Frage = db.Questions.Include(r => r.survey).FirstOrDefault(i => i.ID == arg);
            Survey Mutter_Umfrage = Zu_loeschende_Frage.survey;
            db.Questions.Remove(Zu_loeschende_Frage);
            db.SaveChanges();
            var umfrage = db.Surveys.Include(r => r.questions).First(d => d.ID == Mutter_Umfrage.ID);
            var zahler = 0;
            foreach (var item in umfrage.questions.OrderBy(z => z.position))
            {
                item.position = zahler;
                zahler++;
            }
            db.SaveChanges();
            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = Mutter_Umfrage.ID });
        }


        public ActionResult Position_nach_oben(Guid arg)
        {
            Question Frage_Ursprung = db.Questions.Include(r => r.survey).FirstOrDefault(i => i.ID == arg);
            Survey Mutter_Umfrage = Frage_Ursprung.survey;
            Question Frage_Runter = db.Questions.Where(i => i.survey.ID == Mutter_Umfrage.ID).FirstOrDefault(t => t.position == Frage_Ursprung.position + 1);

            List<Answer> Antworten_von_Ursprung = db.Answers.Where(i => i.question.ID == Frage_Ursprung.ID).ToList();
            List<Answering> Beantwortungen_von_Ursprung = db.Answerings.Where(i => i.question.ID == Frage_Ursprung.ID).ToList();
            List<Answer> Antworten_von_Austausch = db.Answers.Where(i => i.question.ID == Frage_Runter.ID).ToList();
            List<Answering> Beantwortungen_von_Austausch = db.Answerings.Where(i => i.question.ID == Frage_Runter.ID).ToList();

            foreach (var item in Antworten_von_Ursprung)
            {
                db.Answers.Remove(item);
            }
            foreach (var item in Beantwortungen_von_Ursprung)
            {
                db.Answerings.Remove(item);
            }
            foreach (var item in Antworten_von_Austausch)
            {
                db.Answers.Remove(item);
            }
            foreach (var item in Beantwortungen_von_Austausch)
            {
                db.Answerings.Remove(item);
            }

            db.Questions.Remove(Frage_Ursprung);
            db.Questions.Remove(Frage_Runter);
            db.SaveChanges();

            Frage_Runter.position = Frage_Ursprung.position;
            Frage_Ursprung.position++;

            Frage_Runter.survey = Mutter_Umfrage;
            Frage_Ursprung.survey = Mutter_Umfrage;

            foreach (var item in Antworten_von_Ursprung)
            {
                item.question = Frage_Ursprung;
                db.Answers.Add(item);
            }
            foreach (var item in Beantwortungen_von_Ursprung)
            {
                item.question = Frage_Ursprung;
                db.Answerings.Add(item);
            }
            foreach (var item in Antworten_von_Austausch)
            {
                item.question = Frage_Runter;
                db.Answers.Add(item);
            }
            foreach (var item in Beantwortungen_von_Austausch)
            {
                item.question = Frage_Runter;
                db.Answerings.Add(item);
            }
            db.Questions.Add(Frage_Ursprung);
            db.Questions.Add(Frage_Runter);
            db.SaveChanges();

            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = Mutter_Umfrage.ID });
        }
        public ActionResult Position_nach_unten(Guid arg)
        {
            Question Frage_Ursprung = db.Questions.Include(r => r.survey).FirstOrDefault(i => i.ID == arg);
            Survey Mutter_Umfrage = Frage_Ursprung.survey;
            Question Frage_Hoch = db.Questions.Where(i => i.survey.ID == Mutter_Umfrage.ID).FirstOrDefault(t => t.position == Frage_Ursprung.position - 1);

            List<Answer> Antworten_von_Ursprung = db.Answers.Where(i => i.question.ID == Frage_Ursprung.ID).ToList();
            List<Answering> Beantwortungen_von_Ursprung = db.Answerings.Where(i => i.question.ID == Frage_Ursprung.ID).ToList();
            List<Answer> Antworten_von_Austausch = db.Answers.Where(i => i.question.ID == Frage_Hoch.ID).ToList();
            List<Answering> Beantwortungen_von_Austausch = db.Answerings.Where(i => i.question.ID == Frage_Hoch.ID).ToList();

            foreach (var item in Antworten_von_Ursprung)
            {
                db.Answers.Remove(item);
            }
            foreach (var item in Beantwortungen_von_Ursprung)
            {
                db.Answerings.Remove(item);
            }
            foreach (var item in Antworten_von_Austausch)
            {
                db.Answers.Remove(item);
            }
            foreach (var item in Beantwortungen_von_Austausch)
            {
                db.Answerings.Remove(item);
            }

            db.Questions.Remove(Frage_Ursprung);
            db.Questions.Remove(Frage_Hoch);
            db.SaveChanges();

            Frage_Hoch.position = Frage_Ursprung.position;
            Frage_Ursprung.position--;

            Frage_Hoch.survey = Mutter_Umfrage;
            Frage_Ursprung.survey = Mutter_Umfrage;

            foreach (var item in Antworten_von_Ursprung)
            {
                item.question = Frage_Ursprung;
                db.Answers.Add(item);
            }
            foreach (var item in Beantwortungen_von_Ursprung)
            {
                item.question = Frage_Ursprung;
                db.Answerings.Add(item);
            }
            foreach (var item in Antworten_von_Austausch)
            {
                item.question = Frage_Hoch;
                db.Answers.Add(item);
            }
            foreach (var item in Beantwortungen_von_Austausch)
            {
                item.question = Frage_Hoch;
                db.Answerings.Add(item);
            }
            db.Questions.Add(Frage_Ursprung);
            db.Questions.Add(Frage_Hoch);
            db.SaveChanges();

            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = Mutter_Umfrage.ID });
        }

        public ActionResult Umfrage_Aktualisieren(Guid arg)
        {
            return RedirectToAction("FrageErstellung", "Umfrage_Erstellung", new { arg = arg });
        }
    }
}