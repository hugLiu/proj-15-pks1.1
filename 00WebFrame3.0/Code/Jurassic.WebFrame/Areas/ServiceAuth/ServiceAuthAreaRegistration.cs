using System.Web.Mvc;

namespace Jurassic.WebFrame.Areas.ServiceAuth
{
    public class ServiceAuthAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ServiceAuth";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ServiceAuth_default",
                "ServiceAuth/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
