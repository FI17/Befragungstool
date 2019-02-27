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

        public ActionResult Index()
        {
            var Survey_List = db.Surveys;
             
            ICollection<SurveyViewModel> umfrage3 = new List<SurveyViewModel>();
            
                umfrage3 = surveytransformer.ListTransform(Survey_List.ToList());
            
            umfrage3 = umfrage3.OrderByDescending(m => m.creationTime).ToList();
            return View(umfrage3);
        }
    }
}