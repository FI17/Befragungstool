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

        public ActionResult StatusUmfrageBearbeitung()
        {
            return View();
        }

        public ActionResult StatusUmfrageBeantwortung()
        {
            return View();
        }

        public ActionResult StatusUmfrageAuswertung()
        {
            return View();
        }

        public ActionResult AuswertungKeineAntworten()
        {
            return View();
        }
    }
}