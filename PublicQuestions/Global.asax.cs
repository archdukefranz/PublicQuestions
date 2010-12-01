using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client.Document;
using System.Configuration;
using Raven.Client;

namespace PublicQuestions
{

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string RavenSessionKey = "RavenMVC.Session";
        private static DocumentStore documentStore;

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {

            //Create a DocumentStore in Application_Start
            //DocumentStore should be created once per application and stored as a singleton.
            documentStore = new DocumentStore { Url =  ConfigurationManager.AppSettings["dataURL"] };
            documentStore.Initialize();            
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }

        public MvcApplication()
        {

            //Create a DocumentSession on BeginRequest   
            //create a document session for every unit of work
            BeginRequest += (sender, args) =>
                HttpContext.Current.Items[RavenSessionKey] = documentStore.OpenSession();

            //Destroy the DocumentSession on EndRequest
            EndRequest += (o, eventArgs) =>
            {
                var disposable = HttpContext.Current.Items[RavenSessionKey] as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            };
        }

        //Getting the current DocumentSession
        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey]; }
        }
    }
}