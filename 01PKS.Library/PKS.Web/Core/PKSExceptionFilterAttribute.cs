using System;
using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.Web.MVC.Filter
{
    /// <summary>异常处理拦截器</summary>
    public class PKSExceptionFilterAttribute : HandleErrorAttribute
    {
        /// <summary>构造函数</summary>
        public PKSExceptionFilterAttribute(ILog logger, IWebExceptionHandler handler)
        {
            this.Logger = logger;
            this.Handler = handler;
            this.ContentType = new MediaTypeHeaderValue(MimeTypes.Exception);
        }

        /// <summary>日志</summary>
        private ILog Logger { get; }

        /// <summary>异常处理器</summary>
        private IWebExceptionHandler Handler { get; }

        /// <summary>内容类型</summary>
        private MediaTypeHeaderValue ContentType { get; }

        /// <summary>Called when an exception occurs.</summary>
        /// <param name="filterContext">The action-filter context.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext" /> parameter is null.</exception>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.IsChildAction)
            {
                return;
            }
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            var exception = filterContext.Exception;
            this.Logger.Error(string.Empty, exception);
            if (filterContext.Result != null && !(filterContext.Result is EmptyResult))
            {
                filterContext.ExceptionHandled = true;
                return;
            }
            //if (new HttpException(null, exception).GetHttpCode() != (int)HttpStatusCode.InternalServerError)
            //{
            //    return;
            //}
            //if (!this.ExceptionType.IsInstanceOfType(exception))
            //{
            //    return;
            //}
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var exceptionModel = exception.ToModel();
                var webExceptionModel = Handler.Handle(exception, string.Empty, exceptionModel);
                filterContext.Result = new NewtonJsonResult()
                {
                    ContentType = MimeTypes.Exception,
                    Data = exceptionModel,
                    ResponseHandler = response =>
                    {
                        response.StatusCode = (int)webExceptionModel.StatusCode;
                        response.StatusDescription = webExceptionModel.ReasonPhrase;
                    }
                };
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                return;
            }
            //else
            //{
            //    //if (!filterContext.HttpContext.IsCustomErrorEnabled)
            //    //{
            //    //    return;
            //    //}
            //    string controllerName = (string)filterContext.RouteData.Values["controller"];
            //    string actionName = (string)filterContext.RouteData.Values["action"];
            //    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            //    var errorView = this.View;
            //    var httpCode = new HttpException(null, exception).GetHttpCode();
            //    filterContext.Result = new ViewResult
            //    {
            //        ViewName = this.View,
            //        MasterName = this.Master,
            //        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
            //        TempData = filterContext.Controller.TempData
            //    };
            //    filterContext.HttpContext.Response.Clear();
            //    filterContext.HttpContext.Response.StatusCode = httpCode;
            //}
            //filterContext.ExceptionHandled = true;
            //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}