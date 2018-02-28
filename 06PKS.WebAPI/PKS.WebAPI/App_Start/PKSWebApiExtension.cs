using PKS.Models;
using PKS.Utils;
using PKS.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace PKS.WebAPI.Controllers
{
    /// <summary>PKS Web API扩展</summary>
    public static class PKSWebApiExtension
    {
        /// <summary>获得当前子系统代码</summary>
        public static string GetSubSystemCode(this PKSApiController controller)
        {
            return controller.GetService<IPKSSubSystemConfig>().CurrentCode;
        }
        /// <summary>获得子系统URL字典</summary>
        public static Dictionary<string, string> GetSubSystemUrls(this PKSApiController controller)
        {
            return controller.GetService<IPKSSubSystemConfig>().Urls;
        }
        /// <summary>获得子系统URL</summary>
        public static string GetSubSystemUrl(this PKSApiController controller, string code)
        {
            return controller.GetSubSystemUrls()[code].TrimEnd('/');
        }
        /// <summary>获得Token</summary>
        public static string GetToken(this HttpRequestMessage request)
        {
            var token = request.Headers.Authorization?.Parameter;
            if (token.IsNullOrEmpty())
            {
                var ticket = request.GetTicketFromCookie();
                if (ticket != null) token = ticket.UserData;
            }
            // TODO: 用于调试
            if (token.IsNullOrEmpty())
            {
                token = PKSWebConsts.Token_Debug;
            }
            return token;
        }
        /// <summary>获得票据</summary>
        public static FormsAuthenticationTicket GetTicketFromCookie(this HttpRequestMessage request)
        {
            FormsAuthenticationTicket ticket = null;
            var httpCookies = request.Headers.GetCookies()?.SelectMany(e => e.Cookies).ToArray();
            if (!httpCookies.IsNullOrEmpty())
            {
                var authCookie = httpCookies.FirstOrDefault(e => e.Name == FormsAuthentication.FormsCookieName);
                if (authCookie != null)
                {
                    ticket = PKSWebExtension.ExtractTicketFromCookie(authCookie.Value);
                }
            }
            return ticket;
        }
    }
}