using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace signedup
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Add a specific route for AssignUserTask with taskId as parameter
            //routes.MapRoute(
            //    name: "AssignUserTask",
            //    url: "Admin/AssignUserTask/{taskId}",
            //    defaults: new { controller = "Admin", action = "AssignUserTask", taskId = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "AdminHome",
                url: "Admin/AdminHome",
                defaults: new { controller = "Admin", action = "AdminHome" }
       );

            routes.MapRoute(
                name: "AssignUserTask",
                url: "Admin/AssignUserTask/{taskId}",
                defaults: new { controller = "Admin", action = "AssignUserTask", taskId = UrlParameter.Optional }
       );


            // Ensure default route is registered last
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
