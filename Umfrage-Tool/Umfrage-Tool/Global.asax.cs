using System;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain;
using Domain.Acces;

namespace Umfrage_Tool
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SetzeTimer();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void SetzeTimer()
        {
            var einTimer = new Timer {Interval = 120000};
            einTimer.Elapsed += SchließeBeendeteUmfragen;
            einTimer.AutoReset = true;
            einTimer.Enabled = true;
        }

        private static void SchließeBeendeteUmfragen(object sender, ElapsedEventArgs e)
        {
            var db = new DatabaseContent();
            foreach (var umfrage in db.Surveys)
                if (umfrage.states == Survey.States.Öffentlich && umfrage.endTime <= DateTime.Now)
                {
                    umfrage.states = Survey.States.Beendet;
                    umfrage.endTime = DateTime.Now;
                }

            db.SaveChanges();
        }
    }
}