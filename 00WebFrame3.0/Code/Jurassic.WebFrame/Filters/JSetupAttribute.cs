using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebFrame
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 判断AppCenter是否已初始化，如已初始化则拒绝再次执行
    /// </summary>
    internal class JSetupAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (BaseController)filterContext.Controller;
            if (AppManager.Instance.NeedInit)
            {
                return;
            }
            else
            {
                //在随后出现的登录页面会出现这个提示
                controller.SetTips("warning", "需要登录", "Setup操作已经完成，现在需要登录进行管理。");
                filterContext.Result = controller.RedirectAction("Index", "AppManager");
            }
            controller.SaveTips();
        }
    }
}