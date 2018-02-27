using System;
using System.Configuration;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace PKS.Core.Logging
{
    /// <summary>子系统布局渲染器</summary>
    [LayoutRenderer("pks-subsystem")]
    public class PKSSubSystemLayoutRenderer : LayoutRenderer
    {
        /// <summary>构造函数</summary>
        public PKSSubSystemLayoutRenderer()
        {
            //Lazy_Code = new Lazy<string>(() => Bootstrapper.Get<IPKSSubSystemConfig>().CurrentCode);
            this.Code = PKSWebConsts.GetSubSystemCode();
        }

        ///// <summary>系统代码</summary>
        //private Lazy<string> Lazy_Code { get; }
        /// <summary>系统代码</summary>
        private string Code { get; }
        /// <summary>渲染</summary>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            //builder.Append(Lazy_Code.Value);
            builder.Append(this.Code);
        }
    }

    /// <summary>Web用户布局渲染器</summary>
    [LayoutRenderer("pks-principal")]
    public class PKSPrincipalLayoutRenderer : AspNetLayoutRendererBase
    {
        /// <summary>构造函数</summary>
        private HttpRequestBase TryGetRequest(HttpContextBase context)
        {
            try
            {
                return context.Request;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>渲染</summary>
        /// <remarks>SessionID:${aspnet-sessionid}${newline}Token:${aspnet-session:Variable=PKS.AuthenticationToken} - ${aspnet-header-authorization}${newline}Principal:${pks-principal}</remarks>
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var context = HttpContextAccessor.HttpContext;
            if (context == null) return;
            var request = TryGetRequest(context);
            try
            {
                var session = context.Session;
                string token = null;
                if (session != null)
                {
                    if (!session.SessionID.IsNullOrEmpty())
                    {
                        builder.Append("SessionID:").AppendLine(session.SessionID);
                    }
                    token = session[PKSWebConsts.Session_Authentication].As<string>();
                }
                if (token.IsNullOrEmpty() && request != null)
                {
                    token = request.Headers[PKSWebExtension.Header_Authorization];
                }
                if (token != null)
                {
                    builder.Append("Token:").AppendLine(token);
                }
                var user = context.User.As<IPKSPrincipal>();
                if (user == null)
                {
                    user = context.Items[PKSWebConsts.HttpContext_Principal].As<IPKSPrincipal>();
                }
                if (user == null)
                {
                    if (context.User == null) return;
                    builder.Append("Principal:");
                    builder.Append(context.User.Identity.Name);
                }
                else
                {
                    builder.Append("Principal:");
                    builder.Append(user.Identity.Name);
                }
            }
            catch
            {
            }
        }
    }

    /// <summary>Web请求布局渲染器</summary>
    [LayoutRenderer("pks-request")]
    public class PKSRequestLayoutRenderer : AspNetLayoutRendererBase
    {
        /// <summary>构造函数</summary>
        private HttpRequestBase TryGetRequest(HttpContextBase context)
        {
            try
            {
                return context.Request;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>渲染</summary>
        /// <remarks>${aspnet-request-method} ${aspnet-request-url:IncludeHost=true:IncludePort=true:IncludeQueryString=true}${newline}UserAgent:${aspnet-request-useragent}${newline}Referrer:${aspnet-request-referrer}</remarks>
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var context = HttpContextAccessor.HttpContext;
            if (context == null) return;
            var request = TryGetRequest(context);
            if (request == null) return;
            try
            {
                builder.Append(request.HttpMethod).Append(" ");
                builder.Append(request.Url).AppendLine();
                if (!request.UserAgent.IsNullOrEmpty())
                {
                    builder.Append("UserAgent:").AppendLine(request.UserAgent);
                }
                if (request.UrlReferrer != null)
                {
                    builder.Append("Referrer:").Append(request.UrlReferrer);
                }
            }
            catch
            {
            }
        }
    }

    /// <summary>Web请求授权头布局渲染器</summary>
    [LayoutRenderer("pks-exception-source")]
    public class PKSExceptionSourceLayoutRenderer : LayoutRenderer
    {
        /// <summary>渲染</summary>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var ex = logEvent.Exception;
            if (ex != null) builder.Append(ex.Source);
        }
    }

    /// <summary>消息布局渲染器</summary>
    [LayoutRenderer("pks-message")]
    public class PKSMessageLayoutRenderer : LayoutRenderer
    {
        /// <summary>渲染</summary>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.FormattedMessage);
            if (logEvent.Exception != null) builder.Append(logEvent.Exception.Message);
        }
    }
}