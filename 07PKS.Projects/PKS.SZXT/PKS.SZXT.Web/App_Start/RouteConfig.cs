using PKS.Web.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PKS.SZXT.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: PKSMvcConfig.DefaultRouteName,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ExplorationOverview", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
