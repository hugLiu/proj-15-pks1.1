using PKS.Web;
using PKS.Web.MVC;
using System.Web.Mvc;
using System.Web.Routing;

namespace PKS.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //混合认证
            WindowsLoginHandler.AddMixedAuthRoute(routes);

            routes.MapRoute(
                name: PKSMvcConfig.DefaultRouteName,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Redirect", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
