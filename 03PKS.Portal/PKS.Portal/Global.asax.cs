using PKS.Web;
using System.Web.Routing;

namespace PKS.Portal.Web
{
    /// <summary>Portal Web MVC应用程序</summary>
    public class PortalMvcApplication : PKSMvcBaseApplication
    {
        /// <summary>启动</summary>
        protected override void Application_Start()
        {
            base.Application_Start();
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
