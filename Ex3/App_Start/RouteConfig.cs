using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("save", "save/{ip}/{port}/{refreshRate}/{timeout}/{fileName}",
             defaults: new { controller = "Main", action = "save" });

            routes.MapRoute("display", "display/{ip}/{port}/{refreshRate}",
           defaults: new { controller = "Main", action = "display", refreshRate = UrlParameter.Optional }
           );
        
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

         
        }
    }
}
