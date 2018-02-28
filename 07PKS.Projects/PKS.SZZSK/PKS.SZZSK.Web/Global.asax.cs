using PKS.SZZSK.Web.Config;
using PKS.Web;
using System.Web.Routing;

namespace PKS.SZZSK.Web
{
    public class MvcApplication : PKSMvcBaseApplication
    {
        protected override void Application_Start()
        {
            base.Application_Start();
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ConfigService.Run(this);
        }
    }
}
