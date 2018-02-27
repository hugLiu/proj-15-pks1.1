using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Jurassic.AppCenter;
using System.Web.Routing;
using Jurassic.Com.Tools;
using System.Threading;
using Jurassic.AppCenter.Resources;
using System.Text.RegularExpressions;
using Jurassic.WebFrame.Controllers;

namespace Jurassic.WebFrame
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 对AuthorizeAttribute的扩展,并且已自动注册为Mvc的全局筛选器。<br/>
    /// 使用此特性就无需显式声明对应的角色或用户，由AppCenter自动判定权限
    /// 子类继承基类时，此特性不能被继承
    /// </summary>
    public class JGlobalAuthAttribute : AuthorizeAttribute
    {
        AppFunction mFunction;
        bool mIsParamMatched = true;

        private static Func<object> mTimeoutData =
            () => new ReturnValueWithTips
            {
                Type = "error",
                Title = ResHelper.GetStr("Login+Timeout"),
                Message = ResHelper.GetStr("Login+Timeout"),
                ReturnValue = new { Url = System.Web.Security.FormsAuthentication.LoginUrl }
            };

        private static Func<object> mUnauthorizedData =
            () => new ReturnValueWithTips
            {
                Type = "error",
                Title = ResHelper.GetStr("Access Denied"),
                Message = ResHelper.GetStr("Access Denied"),
                ReturnValue = "AccessDenied"
            };

        /// <summary>
        /// 在登录超时时所返回的数据
        /// </summary>
        public static Func<object> UnauthorizedData
        {
            get
            {
                return mUnauthorizedData;
            }
            set
            {
                mUnauthorizedData = value;
            }
        }

        public static Func<object> TimeoutData
        {
            get
            {
                return mTimeoutData;
            }
            set
            {
                mTimeoutData = value;
            }
        }

        /// <summary>
        /// 对权限的判断
        /// </summary>
        /// <param name="httpContext">http请求上下文</param>
        /// <returns>是否有权限</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //string url = httpContext.Request.Url.PathAndQuery;
            //string method = httpContext.Request.HttpMethod;

            //没有记录在案的功能默认登录后都可用
            if (mFunction == null) return true;

            if (mFunction.AuthType == JAuthType.EveryOne) return true;

            if (mFunction.AuthType == JAuthType.AllUsers
                && httpContext.User.Identity.IsAuthenticated) return true;

            if (mFunction.AuthType == JAuthType.Forbidden) return false;
            if (!mIsParamMatched) return false;

            return httpContext.User.Identity.HasRightId(mFunction.Id);
        }

        /// <summary>
        /// 重写非授权访问时的处理逻辑
        /// </summary>
        /// <param name="filterContext">筛选器上下文</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var isAuthed = filterContext.HttpContext.User.Identity.IsAuthenticated;
            var controller = filterContext.Controller as BaseController;
            if (!isAuthed)
            {
              
                var model = controller.GetLoginModel();
                if (model.AutoLogin)
                //标识在浏览器仍然开启的请况下由于超时引起的重新登录
                // 此时不应该再次自动登录，而需要用户手动登录
                {
                    model.RememberMe = false;
                    controller.SetLoginModel(model);
                }
                else if (!model.Password.IsEmpty())
                // 在浏览器重新打开的情况下，重新登录时，如果已勾选了记住密码，则标识为自动登录
                //在进入登录页时会自动根据已记住的密码进行登录
                {
                    model.RememberMe = true;
                    controller.SetLoginModel(model);
                }
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //在ajax的请求中，返回一个Json数据 以便于ajax模块中
                filterContext.Result = new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = filterContext.HttpContext.User.Identity.IsAuthenticated ? UnauthorizedData() : TimeoutData(),
                    ContentType = "application/json"
                };
            }
            else if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("AccessDenied");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        /// <summary>
        /// 重写此方法以获取当前的控制器和视图，检查是否有AllowAnonymous特性。
        /// </summary>
        /// <param name="filterContext">筛选器上下文</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            var method = CommOp.ToEnum<WebMethod>(filterContext.HttpContext.Request.HttpMethod.ToUpper());
            var macthFunctions = AppManager.Instance.FunctionManager.GetAll()
               .Where(f => !f.ActionName.IsEmpty() && !f.ControllerName.IsEmpty()
                     && actionName.Equals(f.ActionName, StringComparison.OrdinalIgnoreCase)
                     && controllerName.StartsWith(f.ControllerName, StringComparison.OrdinalIgnoreCase)
                     && f.Method == method);

            mFunction = null;
            mIsParamMatched = true;

            foreach (var func in macthFunctions.OrderByDescending(func => func.Parameters.Count))
            {
                bool match = true;
                foreach (var ap in func.Parameters)
                {
                    var pattern = ap.ValuePattern ?? "";

                    var str = filterContext.RequestContext.GetParamsValue(ap.Name);

                    if (!Regex.IsMatch(str, "^" + pattern + "$"))
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    mFunction = func;
                    break;
                }
            }

            BaseController controller = filterContext.Controller as BaseController;

            if (controller != null)
            {
                controller.Function = mFunction;
            }

            base.OnAuthorization(filterContext);
        }
    }
}
