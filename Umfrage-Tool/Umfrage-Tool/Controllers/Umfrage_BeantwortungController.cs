using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umfrage_Tool.Models;

namespace Umfrage_Tool.Controllers
{
    public class Umfrage_BeantwortungController : Controller
    {

        FragebogenModel Umfrage = new FragebogenModel()
        {
            Name = "Test-Umfrage",
            FragenListe = new List<FragenModel>()
            {
                new FragenModel()
                {
                    ID = 0,
                    Text ="Wie heißen Sie? Ist ja eine eher kurze Frage. Die Fragenanzeige setzt auch einen Zeilenumbruch, wenn der Satz zu lang ist. Wenn es jetz' aber ein Wort gibt das zu lang für eine Zeile ist, wird das Überschüssige versteckt.",
                    Typ ="Freitext"
                },
                new FragenModel()
                {
                    ID = 5,
                    Text ="Wie heißen Sie? RREEEEEEEEEEEE REEEEEEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEEEEEEEEEEEE",
                    Typ ="Freitext"
                },
                new FragenModel()
                {
                    ID = 2,
                    Text ="Wie heißen Sie? Ist ja eine eher kurze Frage. Die Fragenanzeige setzt auch einen Zeilenumbruch, wenn der Satz zu lang ist. Wenn es jetz' aber ein Wort gibt das zu lang für eine Zeile ist, wird das Überschüssige versteckt.",
                    Typ ="Freitext"
                },
                new FragenModel()
                {
                    ID = 3,
                    Text ="Wie heißen Sie? Ist ja eine eher kurze Frage. Die Fragenanzeige setzt auch einen Zeilenumbruch, wenn der Satz zu lang ist. Wenn es jetz' aber ein Wort gibt das zu lang für eine Zeile ist, wird das Überschüssige versteckt.",
                    Typ ="Freitext"
                },
                new FragenModel()
                {
                    ID = 4,
                    Text ="Wie heißen Sie? Ist ja eine eher kurze Frage. Die Fragenanzeige setzt auch einen Zeilenumbruch, wenn der Satz zu lang ist. Wenn es jetz' aber ein Wort gibt das zu lang für eine Zeile ist, wird das Überschüssige versteckt.",
                    Typ ="Freitext"
                },
                new FragenModel()
                {
                    ID = 1,
                    Text ="Wie heißen Sie? Ist ja eine eher kurze Frage. Die Fragenanzeige setzt auch einen Zeilenumbruch, wenn der Satz zu lang ist. Wenn es jetz' aber ein Wort gibt das zu lang für eine Zeile ist, wird das Überschüssige versteckt.",
                    Typ ="Freitext"
                }
            }
        };

        // GET: Umfrage_Beantwortung
        public ActionResult Index()
        {
            Umfrage.FragenListe = Umfrage.FragenListe.OrderBy(m => m.ID).ToList();
            return View(Umfrage);
        }

        public ActionResult FragenObjekt(FragenModel Frage)
        {
            switch (Frage.Typ)
            {
                case "Freitext":
                    return PartialView("Freitext", Frage);
                default:
                    return PartialView("Freitext", Frage);
            }
        }

        public PartialViewResult Freitext(FragenModel Frage)
        {
            return PartialView(Frage);
        }
    }
}