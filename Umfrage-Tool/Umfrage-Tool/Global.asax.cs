using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Domain.Acces;
using Domain;

namespace Umfrage_Tool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SetzeTimer();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }

        private void SetzeTimer()
        {
            var einTimer = new System.Timers.Timer();
            einTimer.Interval = 108000000;
            einTimer.Elapsed += SchließeBeendeteUmfragen;
            einTimer.AutoReset = true;
            einTimer.Enabled = true;
        }

        private void SchließeBeendeteUmfragen(object sender, ElapsedEventArgs e)
        {
            DatabaseContent db = new DatabaseContent();
            foreach (var umfrage in db.Surveys)
            {
                if (umfrage.states == Survey.States.Öffentlich && umfrage.endTime <= DateTime.Now)
                {
                    umfrage.states = Survey.States.Beendet;
                }
            }

            db.SaveChanges();
        }
    }
}
