using System.Web.Mvc;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using PKS.Web.Controllers;

namespace PKS.Portal.Controllers
{
    /// <summary>重定向控制器</summary>
    [AllowAnonymous]
    public class RedirectController : PKSBaseController
    {
        /// <summary>显示主题</summary>
        public ActionResult ShowTopic(string dataId)
        {
            var redirectUrl = string.Empty;
            if (dataId.IsNullOrEmpty())
            {
                redirectUrl = "/Index.aspx";
            }
            else
            {
                redirectUrl = $"/ShowTopic-{dataId}.aspx";
            }
            return Redirect(redirectUrl);
        }
    }
}