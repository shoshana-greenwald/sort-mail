using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Timers;
using System.Diagnostics;
using Bll;

namespace webApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None; //.Object עושה עם קוד
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );




            //איתחול טיימר שיפעל כל 24 שעות
            const double intervalEveryDay = 24 * 60 * 60 * 1000; 
            Timer checkForTime = new Timer(intervalEveryDay);
            checkForTime.Elapsed += new ElapsedEventHandler(EveryDay);
            checkForTime.Enabled = true;

            //הפונקציה שתופעל כל 24 שעות ותמחק מיילים שעבר עליהם יותר משבוע
            void EveryDay(object sender, ElapsedEventArgs e)
            {
                Debug.WriteLine("A day has passed...");
                ClassDB.DeleteAfterWeek();
            }


            //חיבור לאאוטלוק
            OutlookConnecting outLook = new OutlookConnecting();
            outLook.StartConection();


        }
    }
}
