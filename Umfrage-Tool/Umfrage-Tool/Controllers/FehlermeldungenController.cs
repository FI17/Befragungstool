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
        public ActionResult FehlerMeldung(string aufruf)
        {
            Message nachricht = new Message();
            switch (aufruf)
            {
                case "StatusUmfrageBearbeitung":
                    nachricht = StatusUmfrageBearbeitung();
                    break;
                case "StatusUmfrageBeantwortung":
                    nachricht = StatusUmfrageBeantwortung();
                    break;
                case "StatusUmfrageAuswertung":
                    nachricht = StatusUmfrageAuswertung();
                    break;
                case "AuswertungKeineAntworten":
                    nachricht = AuswertungKeineAntworten();
                    break;
                case "NutzerHinzugefügt":
                    nachricht = NutzerHinzugefügt();
                    break;
                case "Umfrage_beendet":
                    nachricht = Umfrage_beendet();
                    break;
                case "UmfrageBeantwortungExistiertNicht":
                    nachricht = UmfrageBeantwortungExistiertNicht();
                    break;
                default:
                    nachricht = new Message()
                    {
                        siteTitle = "Unbekannter Fehler",
                        mainMessage = "Unbekannter Fehler",
                        additionalInformation = "Dieser Fehler ist noch nicht aufgetreten",
                        useLayout = true,
                        allowReturn = true,
                    };
                    break;
            }

            return View(nachricht);
        }


        public Message StatusUmfrageBearbeitung()
        {
            return new Message()
            {
                siteTitle = "Fehler",
                mainMessage = "Die Umfrage kann nicht bearbeitet werden!",
                additionalInformation = "Die Umfrage wurde schon veröffentlicht.",
                useLayout = false,
                allowReturn = true,
            };
        }

        public Message StatusUmfrageBeantwortung()
        {
            return new Message()
            {
                siteTitle = "Fehler",
                mainMessage = "Die Umfrage kann nicht beantwortet werden!",
                additionalInformation = "Die Umfrage wurde schon beendet.",
                useLayout = false,
                allowReturn = false,
            };
        }

        public Message StatusUmfrageAuswertung()
        {
            return new Message()
            {
                siteTitle = "Fehler",
                mainMessage = "Die Umfrage kann nicht ausgewertet werden!",
                additionalInformation = "Die Umfrage läuft noch.",
                useLayout = false,
                allowReturn = false,
            };
        }

        public Message AuswertungKeineAntworten()
        {
            return new Message()
            {
                siteTitle = "Fehler",
                mainMessage = "Die Umfrage kann nicht ausgewertet werden!",
                additionalInformation = "Die Umfrage wurde noch nicht beantwortet.",
                useLayout = false,
                allowReturn = true,
            };
        }

        public Message NutzerHinzugefügt()
        {
            return new Message()
            {
                siteTitle = "Nutzer hinzugefügt",
                mainMessage = "Erfolg",
                additionalInformation = "Sie haben einen neuen Nutzer hinzugefügt.",
                useLayout = true,
                allowReturn = true,
            };
        }

        public Message Umfrage_beendet()
        {
            return new Message()
            {
                siteTitle = "Umfrage beendet",
                mainMessage = "Vielen Dank für Ihre Teilnahme!",
                additionalInformation = "",
                useLayout = false,
                allowReturn = false,
            };
        }

        public Message UmfrageBeantwortungExistiertNicht()
        {
            return new Message()
            {
                siteTitle = "Umfrage nicht gefunden",
                mainMessage = "Die Umfrage konnte nicht gefunden werden!",
                additionalInformation = "Bitte überprüfen Sie die Richtigkeit Ihres Links.",
                useLayout = false,
                allowReturn = false,
            };
        }

        
    }
}