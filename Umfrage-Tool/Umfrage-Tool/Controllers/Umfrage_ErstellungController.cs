﻿using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Acces;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.ImageAssemblyAnalysis.LogModel;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Roles = "Betreuer, Admin")]
    public class Umfrage_ErstellungController : Controller
    {
        private readonly ModelToSurveyTransformer _surveyTransformer = new ModelToSurveyTransformer();
        private readonly ModelToQuestionTransformer _questionTransformer = new ModelToQuestionTransformer();
        private readonly SurveyToModelTransformer _modelTransformer = new SurveyToModelTransformer();
        private readonly QuestionToModelTransformer _modelQuestionFormer = new QuestionToModelTransformer();
        private readonly ModelToChapterTransformer _modelChapterTransformer = new ModelToChapterTransformer();
        private readonly DatabaseContent _db = new DatabaseContent();

        private ApplicationUserManager _userManager;

        public Umfrage_ErstellungController()
        {
        }

        public Umfrage_ErstellungController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private ApplicationSignInManager SignInManager
        {
            set { }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SurveyViewModel umfrage)
        {
            var a = User.Identity.Name;
            var userId = UserManager.Users.First(d => d.Email == a).Id;

            if (userId == null) return View("Error");

            umfrage.Creator = new Guid(userId);
            var umfrageKernDaten = _surveyTransformer.Transform(umfrage);
            umfrageKernDaten.releaseTime = DateTime.MaxValue;
            umfrageKernDaten.endTime = DateTime.MaxValue;

            _db.Surveys.Add(umfrageKernDaten);
            _db.SaveChanges();
            umfrage = _modelTransformer.Transform(umfrageKernDaten);
            return RedirectToAction("FrageErstellung", new { arg = umfrage.ID });
        }

        public ActionResult FrageErstellung(Guid arg)
        {
            Session["AnzahlAntworten"] = -1;
            var zuTransformierendeUmfrage = _db.Surveys
                .Include(e => e.questions
                    .Select(b => b.choice))
                .FirstOrDefault(d => d.ID == arg);
            var umfrage = _modelTransformer.Transform(zuTransformierendeUmfrage);
            umfrage.questionViewModels = umfrage.questionViewModels
                .OrderBy(d => d.position)
                .ToList();
            if (!BenutzerDarfDas(umfrage.Creator) || umfrage.states != Survey.States.InBearbeitung)
                return RedirectToAction("Fehlermeldung", "Fehlermeldungen", new { aufruf = "StatusUmfrageBearbeitung" });

            return View(umfrage);
        }

        private bool BenutzerDarfDas(Guid umfrageCreator)
        {
            var benutzerText = UserManager.Users.First(f => f.Id == umfrageCreator.ToString()).Email;
            var a = User.Identity.Name;
            var darfErDasWirklich = a == benutzerText || User.Identity.Name == "Admin@FI17.de";
            return darfErDasWirklich;
        }

        #region Vorschau / Bearbeiten

        public PartialViewResult Vorschau(Guid arg)
        {
            var umfrage = _db.Surveys
                .Include(k => k.questions
                    .Select(t => t.choice))
                .Include(c => c.chapters
                    .Select(q => q.questions
                        .Select(g => g.choice)))
                .First(g => g.ID == arg);
            var umfrageModele = _modelTransformer.Transform(umfrage);
            umfrageModele.questionViewModels.OrderBy(e => e.position).ToList();

            return PartialView(umfrageModele);
        }

        public void Position_nach_oben(Guid frageId)
        {
            var frageDieHochSoll = _db.Questions
                .Include(r => r.survey)
                .Include(z => z.chapter)
                .FirstOrDefault(i => i.ID == frageId);

            if (frageDieHochSoll.chapter != null)
            {
                var Mutterkapitel = _db.Chapters
                    .Include(q => q.questions)
                    .FirstOrDefault(i => i.ID == frageDieHochSoll.chapter.ID);
                if (Mutterkapitel.questions.OrderBy(f => f.position).First().ID == frageDieHochSoll.ID)
                {
                    FrageKapitelRauf(frageDieHochSoll.ID);
                    return;
                }

            }
            var mutterUmfrage = frageDieHochSoll?.survey;
            var frageDarüber = _db.Questions
                .Where(i => i.survey.ID == mutterUmfrage.ID)
                .FirstOrDefault(t => t.position == frageDieHochSoll.position - 1);

            if (frageDarüber != null) frageDarüber.position++;
            if (frageDieHochSoll != null) frageDieHochSoll.position--;
            _db.SaveChanges();
        }

        public void Position_nach_unten(Guid frageId)
        {
            var frageDieRunterSoll = _db.Questions
                .Include(r => r.survey)
                .Include(z => z.chapter)
                .FirstOrDefault(i => i.ID == frageId);

            if (frageDieRunterSoll.chapter != null)
            {
                var Mutterkapitel = _db.Chapters
                    .Include(q => q.questions)
                    .FirstOrDefault(i => i.ID == frageDieRunterSoll.chapter.ID);
                if (Mutterkapitel.questions.OrderBy(f => f.position).Last().ID == frageDieRunterSoll.ID)
                {
                    FrageKapitelRunter(frageDieRunterSoll.ID);
                    return;
                }

            }

            var mutterUmfrage = frageDieRunterSoll?.survey;
            var frageDarunter = _db.Questions
                .Where(i => i.survey.ID == mutterUmfrage.ID)
                .FirstOrDefault(t => t.position == frageDieRunterSoll.position + 1);


            if (frageDarunter != null) frageDarunter.position--;
            if (frageDieRunterSoll != null) frageDieRunterSoll.position++;
            _db.SaveChanges();
        }

        public void FrageLöschen(Guid frageId)
        {
            var beantwortungenDerFrage = _db.GivenAnswers
                .Where(i => i.question.ID == frageId)
                .ToList();
            foreach (var item in beantwortungenDerFrage) _db.GivenAnswers.Remove(item);

            var antwortmöglichkeitenDerFrage = _db.Choices
                .Where(i => i.question.ID == frageId)
                .ToList();
            foreach (var item in antwortmöglichkeitenDerFrage) _db.Choices.Remove(item);

            var zuLöschendeFrage = _db.Questions
                .Include(r => r.survey)
                .FirstOrDefault(i => i.ID == frageId);
            var mutterUmfrage = zuLöschendeFrage?.survey;
            if (zuLöschendeFrage != null) _db.Questions.Remove(zuLöschendeFrage);
            _db.SaveChanges();
            var umfrage = _db.Surveys.Include(r => r.questions).First(d => d.ID == mutterUmfrage.ID);
            var zahler = 0;
            foreach (var item in umfrage.questions.OrderBy(z => z.position))
            {
                item.position = zahler;
                zahler++;
            }

            _db.SaveChanges();
        }

        public void AntwortLöschen(int antwortPosition, Guid frageId)
        {
            var zuLöschendeAntwort = _db.Choices
                .Include(h => h.question)
                .Where(i => i.position == antwortPosition)
                .First(i => i.question.ID == frageId);
            var frage = _db.Questions.Include(s => s.survey)
                .FirstOrDefault(q => q.ID == zuLöschendeAntwort.question.ID);
            _db.Choices.Remove(zuLöschendeAntwort);
            foreach (var antwort in _db.Choices)
                if (antwort.question == frage && antwort.position > zuLöschendeAntwort.position)
                    antwort.position--;

            _db.SaveChanges();
        }

        private void Fragen_aktualisieren(SurveyViewModel umfrageView, Guid arg)
        {


            var umfrageAusDb = _db.Surveys
                .Include(b => b.questions
                    .Select(c => c.choice))
                .Include(g => g.chapters
                    .Select(f => f.questions
                        .Select(e => e.choice)))
                .First(f => f.ID == arg);
            umfrageAusDb.name = umfrageView.name;

            foreach (var frage in umfrageAusDb.questions)
            {
                var frageAktuellerPosition = umfrageView.questionViewModels.First(f => f.position == frage.position);
                frage.text = frageAktuellerPosition.text;
                frage.scaleLength = frageAktuellerPosition.scaleLength;
                if (frage.choice == null) continue;
                for (var i = 0; i < frage.choice.Count; i++)
                {
                    frage.choice.ToList()[i].text = frageAktuellerPosition.choices.ToList()[i].text;
                    frage.choice.ToList()[i].question = frage;
                    frage.choice.ToList()[i].position = i;
                }
            }

            var zähler = 0;
            foreach (var chapter in umfrageAusDb.chapters)
            {
                chapter.text = umfrageView.chapterViewModels.ToList()[chapter.position].text;
            }

            _db.SaveChanges();
        }

        #endregion

        [HttpPost]
        public ActionResult FrageErstellung(SurveyViewModel model, string subject, Guid arg)
        {
            var frageId = new Guid();
            var position = 0;
            if (subject.Contains("/"))
            {
                var zusatz = subject.Substring(subject.IndexOf('/') + 1);
                if (zusatz.Contains('#'))
                {
                    position = Convert.ToInt32(zusatz.Substring(0,zusatz.IndexOf('#')));
                    frageId = new Guid(zusatz.Substring(zusatz.IndexOf('#')+1));
                }
                else if (zusatz.Contains('-'))
                {
                    frageId = new Guid(zusatz);
                }
                subject = subject.Substring(0, subject.IndexOf('/'));
            }
            Fragen_aktualisieren(model, arg);
            switch (subject)
            {
                case "Frage hinzufügen":
                    Neue_Frage_speichern(model, arg);
                    break;
                case "Bearbeitung beenden":
                    return RedirectToAction("Index", "Home");
                case "Plus_Antwort":
                    Plus_Antwort(frageId);
                    break;
                case "AntwortLöschen":
                    AntwortLöschen(position, frageId);
                    break;
                case "FrageLöschen":
                    FrageLöschen(frageId);
                    break;
                case "Position_nach_oben":
                    Position_nach_oben(frageId);
                    break;
                case "Position_nach_unten":
                    Position_nach_unten(frageId);
                    break;
            }// TODO: SwitchCase Durch Formaction ersetzen!!!!!!!
            
            return RedirectToAction("FrageErstellung", new { arg });
        }

        private void Neue_Frage_speichern(SurveyViewModel model, Guid arg)
        {
            var neueFrage = _questionTransformer.Transform(
                model.questionViewModels
                    .ToList()
                    .Last());
            var umfrageAusDbVorNeueFrage = _db.Surveys
                .Include(b => b.questions)
                .Include(g => g.chapters
                    .Select(f => f.questions
                        .Select(e => e.choice)))
                .First(f => f.ID == arg);
            neueFrage.survey = umfrageAusDbVorNeueFrage;
            neueFrage.position = umfrageAusDbVorNeueFrage.questions.Count;
            if (umfrageAusDbVorNeueFrage.chapters.Count != 0)
            {
                neueFrage.chapter = umfrageAusDbVorNeueFrage.chapters.OrderBy(i => i.position).Last();
            }

            if (neueFrage.type == Question.choices.Skalenfrage)
            {
                neueFrage.choice.ToList()[0].text = "1";
                if (neueFrage.scaleLength % 2 == 0)
                {
                    neueFrage.choice.ToList()[1].text = neueFrage.scaleLength.ToString();
                }
                else
                {
                    int hälfte = neueFrage.scaleLength / 2;
                    hälfte++;
                    neueFrage.choice.ToList()[1].text = hälfte.ToString();
                    neueFrage.choice.ToList()[2].text = neueFrage.scaleLength.ToString();
                }
                for (var i = 0; i < neueFrage.choice.Count; i++)
                {
                    neueFrage.choice.ToList()[i].position = i;
                    _db.Choices.Add(neueFrage.choice.ToList()[i]);
                }

            }
            else
                neueFrage.choice.Clear();

            _db.Questions.Add(neueFrage);
            _db.SaveChanges();
        }

        public void Plus_Antwort(Guid frageId)
        {
            var frage = _db.Questions
                .Include(y => y.choice)
                .Include(s => s.survey)
                .First(f => f.ID == frageId);
            var neueAntwort = new Choice { question = frage, position = frage.choice.Count, ID = Guid.NewGuid()};
            _db.Choices.Add(neueAntwort);
            _db.SaveChanges();
        }



        public ActionResult NeuesKapitelHinzufügen(SurveyViewModel model, Guid arg)
        {
            Fragen_aktualisieren(model, arg);
            var umfrageID = new Guid(Session["UmfrageID"].ToString());
            var umfrage = _db.Surveys.Include(h => h.chapters).Include(t => t.questions).First(f => f.ID == umfrageID);
            var neuesKapitel = new Chapter() { text = "Kapitel 1", position = 0 };
            if (umfrage.chapters.Count == 0)
            {
                umfrage.chapters.Add(neuesKapitel);
                _db.SaveChanges();
                foreach (var question in umfrage.questions)
                {
                    question.chapter = neuesKapitel;
                }
                _db.SaveChanges();
                return RedirectToAction("FrageErstellung", new { arg = umfrageID });

            }
            else
            {
                neuesKapitel.position = umfrage.chapters.OrderBy(f => f.position).Last().position + 1;
                neuesKapitel.text =
                    "Kapitel " + Convert.ToString(umfrage.chapters.OrderBy(f => f.position).Last().position + 2);
                umfrage.chapters.Add(neuesKapitel);
                _db.SaveChanges();
                return RedirectToAction("FrageErstellung", new { arg = umfrageID });
            }

        }

        public ActionResult FrageZuKapitelHinzufügen(QuestionViewModel frageModel, ChapterViewModel kapitelModel, Guid arg)
        {
            var frage = _db.Questions.First(f => f.ID == frageModel.ID);
            var kapitel = _db.Chapters.First(c => c.ID == kapitelModel.ID);
            frage.chapter = kapitel;
            _db.SaveChanges();
            return RedirectToAction("FrageErstellung", new { arg });
        }

        public void FrageKapitelRunter(Guid FrageID)
        {
            var frage = _db.Questions
                .Include(t => t.chapter)
                .Include(k => k.survey)
                .FirstOrDefault(i => i.ID == FrageID);

            var umfrage = _db.Surveys.Include(c => c.chapters).First(g => g.ID == frage.survey.ID);
            var kapitelNeu = umfrage.chapters.First(g => g.position == frage.chapter.position + 1);
            frage.chapter = kapitelNeu;
            _db.SaveChanges();

        }

        public void FrageKapitelRauf(Guid FrageID)
        {
            var frage = _db.Questions
                .Include(t => t.chapter)
                .Include(k => k.survey)
                .FirstOrDefault(i => i.ID == FrageID);

            var umfrage = _db.Surveys
                .Include(c => c.chapters)
                .First(g => g.ID == frage.survey.ID);

            var kapitelNeu = umfrage.chapters
                .First(g => g.position == frage.chapter.position - 1);

            frage.chapter = kapitelNeu;
            _db.SaveChanges();

        }


        public ActionResult KapitelLöschen(SurveyViewModel model, string subject, Guid arg)
        {
            Fragen_aktualisieren(model, arg);
            var kapitelID = new Guid(subject);

            var kapitel = _db.Chapters
                .Include(k => k.questions)
                .Include(j => j.survey)
                .First(f => f.ID == kapitelID);

            var mutterUmfrage = _db.Surveys
                .Include(g => g.chapters)
                .Include(s => s.questions)
                .First(o => o.ID == kapitel.survey.ID);

            var neuesKapitel = new Chapter();

            if (kapitel.position == 0)
            {
                neuesKapitel = mutterUmfrage.chapters.Count == 1 ? null : mutterUmfrage.chapters.First(u => u.position == kapitel.position + 1);
            }
            else
            {
                neuesKapitel = mutterUmfrage.chapters.First(u => u.position == kapitel.position - 1);
            }

            foreach (var frage in kapitel.questions)
            {
                frage.chapter = neuesKapitel;
            }

            mutterUmfrage.chapters.Remove(kapitel);
            //_db.SaveChanges();

            //var umfrage = _db.Surveys
            //    .Include(g => g.chapters)
            //    .First(o => o.ID == kapitel.survey.ID);



            var zähler = 0;
            foreach (var _kapitel in mutterUmfrage.chapters.OrderBy(u => u.position))
            {
                _kapitel.position = zähler;
                zähler++;
            }

            _db.SaveChanges();


            return RedirectToAction("FrageErstellung", new { arg });

        }

        public ActionResult KapitelNachOben(SurveyViewModel model, string kapitelText, Guid arg)
        {
            Fragen_aktualisieren(model, arg);

            var kapitelID = new Guid(kapitelText);

            var kapitel = _db.Chapters.Include(s => s.survey).First(f => f.ID == kapitelID);
            var Umfrage = _db.Surveys.Include(c => c.chapters).First(i => i.ID == kapitel.survey.ID);
            var kapitelDarüber = Umfrage.chapters.First(f => f.position == kapitel.position - 1);

            kapitelDarüber.position++;
            kapitel.position--;
            _db.SaveChanges();

            FragenSortierer(Umfrage.ID);

            return RedirectToAction("FrageErstellung", new { arg });
        }

        public ActionResult KapitelNachUnten(SurveyViewModel model, string kapitelText, Guid arg)
        {
            Fragen_aktualisieren(model, arg);

            var kapitelID = new Guid(kapitelText);

            var kapitel = _db.Chapters.Include(s => s.survey).First(f => f.ID == kapitelID);
            var Umfrage = _db.Surveys.Include(c => c.chapters).First(i => i.ID == kapitel.survey.ID);
            var kapitelDarüber = Umfrage.chapters.First(f => f.position == kapitel.position + 1);

            kapitelDarüber.position--;
            kapitel.position++;
            _db.SaveChanges();

            FragenSortierer(Umfrage.ID);

            return RedirectToAction("FrageErstellung", new { arg });
        }

        public void FragenSortierer(Guid umfrageID)
        {
            var umfrage = _db.Surveys.Include(d => d.chapters.Select(g => g.questions)).First(u => u.ID == umfrageID);

            var zähler = 0;
            foreach (var kapitel in umfrage.chapters.OrderBy(x => x.position))
            {
                foreach (var frage in kapitel.questions.OrderBy(x => x.position))
                {
                    frage.position = zähler;
                    zähler++;
                }
            }

            _db.SaveChanges();
        }




        public PartialViewResult Skalenfragen_Erstellung()
        {
            return PartialView();
        }

        public PartialViewResult Multiple_Choice_Fragen_Erstellung()
        {
            return PartialView();
        }


        public PartialViewResult Freitext(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleMoreMitSonstiges(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleMore(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleOneMitSonstiges(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult MultipleOne(QuestionViewModel frage)
        {
            return PartialView(frage);
        }

        public PartialViewResult Skalenfrage(QuestionViewModel frage)
        {
            return PartialView(frage);
        }
    }
}