using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PKS.Core;
using PKS.Web;

namespace PKS.WebAPI.Filter
{
    /// <summary>GTAPI方法参数验证器</summary>
    public class PKSParameterValidationAttribute : ActionFilterAttribute
    {
        /// <summary>Occurs before the action method is invoked.</summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionArguments = actionContext.ActionArguments;
            if (actionArguments.Count == 0) return;
            if (actionContext.ActionDescriptor.SupportedHttpMethods.Count > 1) return;
            foreach (var parameterDescriptor in actionContext.ActionDescriptor.GetParameters())
            {
                if (!typeof(IParameterValidation).IsAssignableFrom(parameterDescriptor.ParameterType)) continue;
                object parameterValue;
                if (!actionArguments.TryGetValue(parameterDescriptor.ParameterName, out parameterValue) || parameterValue == null)
                {
                    ExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺少请求参数！", $"请求参数{parameterDescriptor.ParameterName}不允许为null！");
                }
            }
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                var httpError = new HttpError(modelState, true);
                var message = BuildErrorDetails(httpError.ModelState);
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺少请求参数！", message);
            }
        }
        /// <summary>Occurs after the action method is invoked.</summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
        }
        /// <summary>生成错误明细</summary>
        private string BuildErrorDetails(HttpError modelStateError)
        {
            var details = modelStateError.Values.Cast<string[]>().SelectMany(e => e).ToArray();
            return string.Join(Environment.NewLine, details);

        }
    }
}
