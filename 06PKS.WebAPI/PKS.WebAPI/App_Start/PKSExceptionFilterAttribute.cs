using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Common.Logging;
using Newtonsoft.Json;
using PKS.Core;
using PKS.Utils;
using PKS.Web;
using System.Web.Http;

namespace PKS.WebAPI.Filter
{
    /// <summary>异常处理拦截器</summary>
    public class PKSExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionLogger
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

        /// <summary>处理异常</summary>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var ex = actionExecutedContext.Exception;
            CheckResponseReason(ex);
            var exceptionModel = ex.ToModel();
            var controller = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var webExceptionModel = Handler.Handle(ex, controller, exceptionModel);
            var response = actionExecutedContext.Request.CreateResponse(webExceptionModel.StatusCode, exceptionModel);
            if (!webExceptionModel.ReasonPhrase.IsNullOrEmpty())
            {
                response.ReasonPhrase = webExceptionModel.ReasonPhrase;
            }
            response.Content.Headers.ContentType = this.ContentType;
            actionExecutedContext.Response = response;
        }
        /// <summary>检查应答原因</summary>
        private void CheckResponseReason(Exception ex)
        {
            var rex = ex.As<HttpResponseException>();
            if (rex != null)
            {
                rex.SetResponseReason(rex.Response);
                return;
            }
        }
        /// <summary>记录异常</summary>
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var actionArguments = context.ExceptionContext?.ActionContext?.ActionArguments;
            if (!actionArguments.IsNullOrEmpty())
            {
                foreach (var argument in actionArguments)
                {
                    context.Exception.Data["ApiActionArgument_" + argument.Key] = GetActionArgumentValue(argument.Value);
                }
            }
            this.Logger.Error(string.Empty, context.Exception);
            return Utility.CompletedTask;
        }

        /// <summary>获得请求参数值</summary>
        private string GetActionArgumentValue(object value)
        {
            if (value == null) return "[null]";
            if (value.GetType().GetTypeCode() != TypeCode.Object) return value.ToString();
            return value.ToJson(Formatting.None);
        }
    }
}