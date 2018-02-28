using System.Web.Mvc;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace PKS.Portal.Controllers
{
    /// <summary>重定向控制器</summary>
    public class RedirectController : PortalBaseController
    {
        /// <summary>重定向</summary>
        public ActionResult Index()
        {
            var redirectUrl = this.HttpContext.GetRoleDefaultUrl(null, this.PKSUser);
            return Redirect(redirectUrl);
        }
    }
}