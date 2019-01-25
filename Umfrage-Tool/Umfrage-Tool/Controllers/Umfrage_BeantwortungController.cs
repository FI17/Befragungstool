using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umfrage_Tool.Models;
using Domain;
using Domain.Acces;
using System.Data.Entity;

namespace Umfrage_Tool.Controllers
{
    public class Umfrage_BeantwortungController : Controller
    {


        // GET: Umfrage_Beantwortung
        public ActionResult Index()
        {
            DatabaseContent db = new DatabaseContent();
            SurveyToModelTransformer I = new SurveyToModelTransformer();
            Survey U = new Survey();
            SurveyViewModel Umfrage = new SurveyViewModel();

            Guid Gesuchte_Umfragen_ID = db.Surveys.First().ID;
            U = db.Surveys
                       .Where(b => b.ID == Gesuchte_Umfragen_ID)
                       .Include(b => b.questions)
                       .FirstOrDefault();
            Umfrage = I.Transform(U);
            Umfrage.questionViewModels = Umfrage.questionViewModels.OrderBy(m => m.position).ToList();
            return View(Umfrage);
        }

        public ActionResult FragenObjekt(QuestionViewModel Frage)
        {
            switch (Frage.typ)
            {
                case 0://Freitext
                    return PartialView("Freitext", Frage);
                default:
                    return PartialView("Freitext", Frage);
            }
        }

        public PartialViewResult Freitext(QuestionViewModel Frage)
        {
            return PartialView(Frage);
        }
    }
}