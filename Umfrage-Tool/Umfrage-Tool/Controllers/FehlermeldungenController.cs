using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Umfrage_Tool.Controllers
{
    public class FehlermeldungenController : Controller
    {
        // GET: Fehlermeldungen
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatusUmfrageBeendet()
        {
            return View();
        }

        public ActionResult StatusUmfrageLäuft()
        {
            return View();
        }

        public ActionResult StatusUmfrageUnveröffentlicht()
        {
            return View();
        }
    }
}