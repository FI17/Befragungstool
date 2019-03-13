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
        DatabaseContent Datenbank = new DatabaseContent();
        SurveyToModelQuestionTransformer Umfrage_zu_View_Tranformer_mit_Fragen = new SurveyToModelQuestionTransformer();
        SessionToModelTransformer Session_zu_View_Transformer = new SessionToModelTransformer();
        AnsweringToModelAllTransformer Beantwortung_zu_View_Transformer = new AnsweringToModelAllTransformer();
        QuestionToModelTransformer Fragen_zu_View_Transformer = new QuestionToModelTransformer();

        public ActionResult Index()
        {
            ICollection<Survey> Umfragen_aus_der_Datenbank_Liste = Datenbank.Surveys.Include(d => d.sessions).ToList();
            ICollection<SurveyViewModel> Umfragen_Liste = new List<SurveyViewModel>();
            Umfragen_Liste = Umfrage_zu_View_Tranformer_mit_Fragen.ListTransform(Umfragen_aus_der_Datenbank_Liste);
            Umfragen_Liste = Umfragen_Liste.OrderByDescending(m => m.creationTime).ToList();
            return View(Umfragen_Liste);
        }

        public ActionResult Ergebnisse(Guid arg)
        {
            Guid Umfrage_ID = arg;
            Session["Vorherige_Umfrage"] = Umfrage_ID.ToString();
            ICollection<SessionViewModel> Session_Liste = new List<SessionViewModel>();
            Survey ausgewaehlte_Umfrage = Datenbank.Surveys
                .Include(rt => rt.sessions
                .Select(r => r.answerings
                .Select(h => h.question)))
                .FirstOrDefault(t => t.ID == Umfrage_ID);
            Session_Liste = Session_zu_View_Transformer.ListTransform(ausgewaehlte_Umfrage.sessions).ToList();
            Session_Liste = Session_Liste.OrderByDescending(m => m.creationDate).ToList();
            return View(Session_Liste);
        }

        public ActionResult Antworten(Guid arg)
        {
            Guid Session_ID = arg;
            ICollection<AnsweringViewModel> Beantwortung_Liste = new List<AnsweringViewModel>();
            Session ausgewaehlte_Session = Datenbank.Sessions
                .Include(a => a.answerings
                .Select(c => c.question)
                .Select(g => g.survey))
                .Include(a => a.answerings
                .Select(c => c.question)
                .Select(g => g.answers))
                .FirstOrDefault(b => b.ID == Session_ID);
            Beantwortung_Liste = Beantwortung_zu_View_Transformer.ListTransform(ausgewaehlte_Session.answerings).ToList();
            Beantwortung_Liste = Beantwortung_Liste.OrderBy(m => m.questionViewModel.position).ToList();
            return View(Beantwortung_Liste);
        }

        public ActionResult Fragen_Ergebnisse(Guid arg)
        {
            Guid Umfrage_ID = arg;
            ICollection<QuestionViewModel> Fragen_Liste = new List<QuestionViewModel>();
            Survey ausgewaehlte_Umfrage = Datenbank.Surveys
                .Include(a => a.questions
                .Select(c => c.answerings))
                .Include(s => s.questions
                .Select(t => t.answers))
                .FirstOrDefault(b => b.ID == Umfrage_ID);
            Fragen_Liste = Fragen_zu_View_Transformer.ListTransform(ausgewaehlte_Umfrage.questions);
            Fragen_Liste = Fragen_Liste.OrderBy(u => u.position).ToList();
            return View(Fragen_Liste);
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