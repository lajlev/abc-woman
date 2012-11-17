using System.Web.Mvc;
using System.Web.Routing;

namespace FoosBall
{
    using System.Configuration;

    using FoosBall.Main;
    using FoosBall.Models;

    using MongoDB.Bson.Serialization;

    using SignalR;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

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
            AreaRegistration.RegisterAllAreas();

            // Persistent connection through SignalR:
            RouteTable.Routes.MapConnection<EventConnection>("Events", "Events/{*operation}");

            // Initialize App configuration
            AppConfig.InitalizeConfig();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

    }
}