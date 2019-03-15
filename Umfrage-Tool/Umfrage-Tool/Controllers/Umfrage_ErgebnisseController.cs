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
        DatabaseContent db = new DatabaseContent();
        SurveyToModelQuestionTransformer umfrage_zu_View_Tranformer_mit_Fragen = new SurveyToModelQuestionTransformer();
        SessionToModelTransformer session_zu_View_Transformer = new SessionToModelTransformer();
        AnsweringToModelAllTransformer beantwortung_zu_View_Transformer = new AnsweringToModelAllTransformer();
        QuestionToModelTransformer fragen_zu_View_Transformer = new QuestionToModelTransformer();

        public ActionResult Index()
        {
            ICollection<Survey> umfragen_aus_der_Datenbank_Liste = db.Surveys.Include(d => d.sessions).ToList();
            ICollection<SurveyViewModel> umfragen_Liste = new List<SurveyViewModel>();
            umfragen_Liste = umfrage_zu_View_Tranformer_mit_Fragen.ListTransform(umfragen_aus_der_Datenbank_Liste);
            umfragen_Liste = umfragen_Liste.OrderByDescending(m => m.creationTime).ToList();
            return View(umfragen_Liste);
        }

        public ActionResult Ergebnisse(Guid arg)
        {
            Guid Umfrage_ID = arg;
            Session["Vorherige_Umfrage"] = Umfrage_ID.ToString();
            ICollection<SessionViewModel> Session_Liste = new List<SessionViewModel>();
            Survey ausgewaehlte_Umfrage = db.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.givenAnswer
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == Umfrage_ID);
            Session_Liste = session_zu_View_Transformer.ListTransform(ausgewaehlte_Umfrage.sessions).ToList();
            Session_Liste = Session_Liste.OrderByDescending(m => m.creationDate).ToList();
            return View(Session_Liste);
        }

        public ActionResult Antworten(Guid arg)
        {
            Guid Session_ID = arg;
            ICollection<GivenAnswerViewModel> Beantwortung_Liste = new List<GivenAnswerViewModel>();
            Session ausgewaehlte_Session = db.Sessions
                .Include(a => a.givenAnswer
                .Select(c => c.question)
                .Select(g => g.survey))
                .Include(a => a.givenAnswer
                .Select(c => c.question)
                .Select(g => g.choice))
                .FirstOrDefault(b => b.ID == Session_ID);
            Beantwortung_Liste = beantwortung_zu_View_Transformer.ListTransform(ausgewaehlte_Session.givenAnswer).ToList();
            Beantwortung_Liste = Beantwortung_Liste.OrderBy(m => m.questionViewModel.position).ToList();
            return View(Beantwortung_Liste);
        }

        public ActionResult Fragen_Ergebnisse(Guid arg)
        {
            Guid umfrage_ID = arg;
            ICollection<QuestionViewModel> fragen_Liste = new List<QuestionViewModel>();
            Survey ausgewaehlte_Umfrage = db.Surveys
                .Include(a => a.questions
                .Select(c => c.givenAnswer))
                .Include(s => s.questions
                .Select(t => t.choice))
                .FirstOrDefault(b => b.ID == umfrage_ID);
            fragen_Liste = fragen_zu_View_Transformer.ListTransform(ausgewaehlte_Umfrage.questions);
            fragen_Liste = fragen_Liste.OrderBy(u => u.position).ToList();
            return View(fragen_Liste);
        }
        public PartialViewResult Panel_fuer_Frage_in_kumulierter_Auswertung(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult Freitext_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult MultipleOne_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
        public PartialViewResult Skalenfrage_Kumuliert(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
    }
}