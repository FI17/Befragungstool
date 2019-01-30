using Domain;
using Domain.Acces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Umfrage_Tool.Controllers
{
    [Authorize(Users = "Admin@FI17.de")]
    public class HomeController : Controller
    {
        DatabaseContent db = new DatabaseContent();
        ModelToSurveyTransformer modeltransformer = new ModelToSurveyTransformer();
        SurveyToModelTransformer surveytransformer = new SurveyToModelTransformer();

        public ActionResult Index(SurveyViewModel umfrage)
        {
            var Survey_List = db.Surveys;

            foreach (var survey in Survey_List)
            {
                umfrage.name = survey.name;
                umfrage.ID = survey.ID;
            }
            List<SurveyViewModel> umfrage3 = new List<SurveyViewModel>();
            foreach (var item in Survey_List)
            {
                umfrage3.Add(surveytransformer.Transform(item));
            }
            return View(umfrage3);
        }
    }
}