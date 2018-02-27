using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.Logging;
using Newtonsoft.Json;
using NLog;
using PKS.Base;
using PKS.Core;
using PKS.Services;
using PKS.Utils;
using System.Web.Security;
using System.Diagnostics;

namespace PKS.Web
{
    /// <summary>Http扩展</summary>
    public static class PKSWebExtension
    {
        /// <summary>授权头</summary>
        public static string Header_Authorization { get; } = "Authorization";
        /// <summary>授权头方案</summary>
        public static string AuthorizationSchema { get; } = "Bearer";
        /// <summary>设置文件内容头</summary>
        public static void UseContentDisposition(this HttpContent httpContent, string fileName, bool setContentType = true)
        {
            fileName = Path.GetFileName(fileName);
            httpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = @"""attachment""",
                FileName = $@"""{fileName}"""
            };
            if (setContentType)
            {
                var mimeType = fileName.GetMediaTypeFromFile();
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            }
        }
        /// <summary>解析Forms认证Cookies</summary>
        public static FormsAuthenticationTicket ExtractTicketFromCookie(this HttpCookieCollection cookies)
        {
            var httpCookie = cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie == null) return null;
            return ExtractTicketFromCookie(httpCookie.Value);
        }
        /// <summary>解析Forms认证Cookies</summary>
        public static FormsAuthenticationTicket ExtractTicketFromCookie(string cookieValue)
        {
            try
            {
                if (cookieValue.IsNullOrEmpty()) return null;
                var ticket = FormsAuthentication.Decrypt(cookieValue);
                if (ticket == null) return null;
                if (ticket.Expired) return null;
                return ticket;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }
        /// <summary>设置缓存控制,默认为30天</summary>
        public static void UseMaxAgeCacheControl(this HttpResponseHeaders headers, int age = 30 * 24 * 60 * 60)
        {
            if (headers.CacheControl == null)
            {
                headers.CacheControl = new CacheControlHeaderValue();
            }
            headers.CacheControl.Public = true;
            headers.CacheControl.MaxAge = TimeSpan.FromSeconds(age);
        }
        /// <summary>载入上传的第一个文件流</summary>
        public static async Task ReadFileStreamFirstOrDefault(this HttpContent httpContent, IFileUploadRequest request, string rootPath)
        {
            if (!httpContent.IsMimeMultipartContent()) return;
            var provider = await httpContent.ReadAsMultipartAsync(new MultipartFileStreamProvider(rootPath));
            var fileData = provider.FileData.FirstOrDefault();
            if (fileData == null) return;
            request.FileName = fileData.Headers.ContentDisposition.FileName;
            request.ContentType = fileData.Headers.ContentType.ToString();
            request.ServerFile = fileData.LocalFileName;
        }
        /// <summary>处理应用程序错误</summary>
        public static void HandleApplicationError(this HttpApplication application)
        {
            var lastError = application.Server.GetLastError();
            if (lastError == null) return;
            var logger = PKS.Core.Bootstrapper.Get<ILog>();
            logger.Error("Application_Error", lastError);
            try
            {
                var httpError = lastError as HttpException;
                if (httpError != null)
                {
                    //ASP.NET的400与404错误不记录日志，并都以自定义404页面响应
                    var httpCode = (HttpStatusCode)httpError.GetHttpCode();
                    if (httpCode == HttpStatusCode.BadRequest || httpCode == HttpStatusCode.NotFound)
                    {
                        application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }
                }
                //对于路径错误不记录日志，并都以自定义404页面响应
                if (lastError.TargetSite.ReflectedType == typeof(Path))
                {
                    application.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }
                if (httpError == null) httpError = new HttpException("", lastError);
                application.Response.StatusCode = httpError.GetHttpCode();
            }
            finally
            {
                //application.Server.ClearError();
            }
        }
        /// <summary>记录应用程序退出事件</summary>
        public static void LogApplicationEnd(this HttpApplication application)
        {
            if (Bootstrapper.Kernel == null) return;
            var message = "Application_End";
            var content = GetApplicationEndMessage();
            if (content.Length > 0)
            {
                Bootstrapper.Log(CacheManager.Core.Logging.LogLevel.Information, message, content);
            }
            else
            {
                Bootstrapper.Get<ILog>().Info(message);
            }
        }
        /// <summary>获得WEB应用退出信息</summary>
        private static string GetApplicationEndMessage()
        {
            var runtime = ReflectUtil.GetFieldInfo(typeof(HttpRuntime), "_theRuntime", true)?.GetValue(null);
            if (runtime == null) return string.Empty;
            var shutDownMessage = ReflectUtil.GetFieldValue(runtime, "_shutDownMessage").As<string>() ?? string.Empty;
            var shutDownStack = ReflectUtil.GetFieldValue(runtime, "_shutDownStack").As<string>() ?? string.Empty;
            return $"{shutDownMessage}{shutDownStack}";
        }
    }
}
