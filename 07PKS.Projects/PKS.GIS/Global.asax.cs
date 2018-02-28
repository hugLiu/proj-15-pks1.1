using System.Web.Routing;
using PKS.Web;

namespace PKS.GIS
{
    /// <summary>GIS Web MVC应用程序</summary>
    public class GisMvcApplication : PKSMvcBaseApplication
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
