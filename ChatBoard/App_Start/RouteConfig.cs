using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChatBoard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Specific to general flow :)
            // Thanks http://stackoverflow.com/questions/5903996/defining-custom-url-routes-in-asp-net-mvc

            routes.MapRoute(
                name: "Create",
                url: "Create",
                defaults: new { controller = "Post", action = "Create"}
             );

            routes.MapRoute(
                name: "Details",
                url: "Post/{id}",
                defaults: new { controller = "Post", action = "Details" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
