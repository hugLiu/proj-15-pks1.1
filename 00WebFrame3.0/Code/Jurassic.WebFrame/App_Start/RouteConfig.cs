using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Jurassic.WebFrame
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Globalization_App", // 带区域性质的路由
               "{culture}/{controller}/{action}/{id}",
               new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
               new { culture = "^[a-zA-Z]{2}(-[a-zA-Z]{2,6})?$", controller = "^\\w+\\d*$" }//参数约束
               //namespaces
              );

            routes.MapRoute(
                "Default_App", // 默认路由
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
                //namespaces
            );
        }
    }
}