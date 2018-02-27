using Jurassic.AppCenter.Logs;
using Jurassic.CommonModels.ServerAuth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Jurassic.WebApi.Providers
{
    /// <summary>
    /// 服务授权过滤
    /// </summary>
    public class WebApiFilterAuthorize : AuthorizationFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //获取请求的控制器完整命名
            string controllerFullName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            //获取请求名称
            string actionName = actionContext.ActionDescriptor.ActionName;

            var logInfo2 = new JLogInfo
            {
                LogType = JLogType.Info.ToString(),
                Message = "获取请求:控制器名称:" + controllerFullName + ";请求名称:" + actionName + "",
                ActionName = controllerFullName,
                ModuleName = actionName
            };
            LogHelper.Write(logInfo2);

            #region 获取服务的自定义属性(如果自定义数据忽略该请求的验证直接通过
            Collection<ApiAuthAttribute> anonymousAction = actionContext.ActionDescriptor.GetCustomAttributes<ApiAuthAttribute>();
            if (anonymousAction.Any())
            {
                if (anonymousAction[0].IsIgnoreAuth)
                {
                    return;
                }
            }
            #endregion 

            //获取正在请求的服务对象信息
            List<ServiceInfo> serviceList = ApiManager.mServiceInfoManager.GetServiceInfo(actionName, controllerFullName);
            if (!serviceList.Any())
            { 
                //如果未授权返回错误提示信息
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            ServiceInfo serviceInfo = serviceList[0];

            #region 根据服务提供的授权方式与对应客户id的授权方式进行判断该请求是否允许访问
            /*
             * 根据所访问的服务对象来确定该服务采用的何种验证方式
             * 0 有权限的客户组(NeedAuth)  :必须通过安全验证,与客户组id授权验证
             * 1 所有授权客户端(AllUsers)  :只需要通过安全验证
             * 2 所有人(EveryOne)          :不需要验证直接访问
             * 4 禁止所有人(Forbidden)     :不允许任何访问
             */
            switch (serviceInfo.AuthWay)
            {
                case "0":
                    if (!this.IsAuthenticated)
                    {
                        //如果未授权返回错误提示信息
                        HandleUnauthorizedRequest(actionContext);
                    }
                    bool isService = ApiManager.mServerAuthManager.GetAuthService(System.Web.HttpContext.Current.User.Identity.Name, actionName, controllerFullName);
                    if (!isService)
                    {
                        //如果未授权返回错误提示信息
                        HandleUnauthorizedRequest(actionContext);
                    }
                    break;
                case "1":
                    if (!this.IsAuthenticated)
                    {
                        //如果未授权返回错误提示信息
                        HandleUnauthorizedRequest(actionContext);
                    }
                    break;
                case "2":
                    break;
                case "4":
                    HandleUnauthorizedRequest(actionContext);
                    return;
                default:
                    HandleUnauthorizedRequest(actionContext);
                    return;
            }

            #endregion
             
        }
         
        /// <summary>
        /// 处理未授权的请求
        /// </summary>
        /// <param name="actionContext"></param>
        private void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.ReasonPhrase = "Not Authenticated";
            //actionContext.Response.Headers.Add("WWW-Authenticate", "Basic");

            //返回具体信息集合的方式获取执行结果
            //var parameters = new Dictionary<string, string>();
            //parameters.Add("resMsg", "返回消息");
            //parameters.Add("resStatus", HttpStatusCode.Unauthorized.ToString());
            //actionContext.Response.Content = new FormUrlEncodedContent(parameters);
            //actionContext.Response.Content = new ObjectContent<Dictionary<string, string>>(parameters, new JsonMediaTypeFormatter());

        }

        /// <summary>
        /// 是否通过安全令牌的授权验证
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                //通过安全令牌的验证结果来确定是否属于授权请求 
                if (!Thread.CurrentPrincipal.Identity.IsAuthenticated || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return false;
                }
                return true;
            }

        }




    }


}