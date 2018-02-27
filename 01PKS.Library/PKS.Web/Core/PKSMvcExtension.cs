using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.Web.MVC;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PKS.Web
{
    /// <summary>PKS Web MVC扩展</summary>
    public static class PKSMvcExtension
    {
        /// <summary>获得注入服务</summary>
        private static TService GetService<TService>()
        {
            return (TService)DependencyResolver.Current.GetService(typeof(TService));
        }
        /// <summary>授权配置</summary>
        public static PKSAuthenticationConfig AuthenticationConfig { get; } = new PKSAuthenticationConfig();
        /// <summary>获得HttpContextWrapper</summary>
        public static HttpContextBase GetHttpContextWrapper(this HttpContext context)
        {
            return new HttpContextWrapper(context);
        }
        /// <summary>设置控制器上下文</summary>
        public static void SetControllerContext(this ControllerContext context)
        {
            context.HttpContext.Items[PKSWebConsts.Session_MvcControllerContext] = context;
        }
        /// <summary>获得控制器上下文</summary>
        public static ControllerContext GetControllerContext(this HttpContextBase context)
        {
            return context.Items[PKSWebConsts.Session_MvcControllerContext].As<ControllerContext>();
        }
        /// <summary>获得当前子系统代码</summary>
        public static string GetSubSystemCode(this HttpContextBase context)
        {
            return GetService<IPKSSubSystemConfig>().CurrentCode;
        }
        /// <summary>是否门户子系统站点</summary>
        public static bool IsPortalSite(this HttpContextBase context)
        {
            return GetSubSystemCode(context) == PKSSubSystems.Portal;
        }
        /// <summary>获得子系统URL</summary>
        public static Dictionary<string, string> GetSubSystemUrls(this HttpContextBase context)
        {
            var service = GetService<IPKSSubSystemConfig>();
            if (context == null) return service.Urls;
            var items = context.Items;
            var urls = items[PKSWebConsts.HttpContext_SubSystemUrls].As<Dictionary<string, string>>();
            if (urls == null)
            {
                urls = service.Urls;
                items[PKSWebConsts.HttpContext_SubSystemUrls] = urls;
            }
            return urls;
        }
        /// <summary>获得子系统URL</summary>
        public static string GetSubSystemUrl(this HttpContextBase context, string code)
        {
            return context.GetSubSystemUrls()[code];
        }
        /// <summary>获得本站URL</summary>
        public static string GetThisSiteUrl(this HttpContextBase context)
        {
            return context.GetSubSystemUrl(context.GetSubSystemCode());
        }
        /// <summary>获得WebAPI子系统URL</summary>
        public static string GetWebApiSiteUrl(this HttpContextBase context)
        {
            return context.GetSubSystemUrl(PKSSubSystems.WEBAPI).TrimEnd('/');
        }
        /// <summary>获得WebAPI子系统服务URL</summary>
        public static string GetWebApiServiceUrl(this HttpContextBase context)
        {
            return context.GetSubSystemUrl(PKSSubSystems.WEBAPI).NormalizeUrl() + "API";
        }
        /// <summary>获得门户子系统URL</summary>
        public static string GetPortalSiteUrl(this HttpContextBase context)
        {
            return context.GetSubSystemUrl(PKSSubSystems.Portal).TrimEnd('/');
        }
        /// <summary>是否已登录</summary>
        public static bool IsLogined(this HttpContextBase context, ISecurityService service, ref string token, out IPKSPrincipal principal)
        {
            principal = null;
            var refreshSessionToken = false;
            var refreshCookiesToken = false;
            var checkCookiesToken = true;
            if (token.IsNullOrEmpty())
            {
                token = context.GetTokenFromSession();
                if (token.IsNullOrEmpty())
                {
                    refreshSessionToken = true;
                    token = context.GetTokenFromCookies();
                    if (token.IsNullOrEmpty()) return false;
                    checkCookiesToken = false;
                }
            }
            else
            {
                refreshSessionToken = true;
            }
            if (service == null) service = GetService<ISecurityService>();
            principal = context.GetPrincipal(service, token);
            if (principal == null) return false;
            if (!principal.NewToken.IsNullOrEmpty())
            {
                principal = context.GetPrincipal(service, principal.NewToken);
                if (principal == null) return false;
                token = principal.NewToken;
                refreshSessionToken = true;
                context.Items[PKSWebConsts.HttpContext_Principal] = principal;
            }
            if (!context.Request.IsAjaxRequest() && context.IsRenewToken(principal, service))
            {
                var result = service.Renew(token);
                if (result != null && result.Succeed)
                {
                    token = result.Token;
                    principal = result.Principal;
                    refreshSessionToken = true;
                    refreshCookiesToken = true;
                    context.Items[PKSWebConsts.HttpContext_Principal] = principal;
                }
            }
            if (refreshSessionToken)
            {
                context.Session[PKSWebConsts.Session_Authentication] = token;
            }
            if (!refreshCookiesToken && checkCookiesToken && !ExistsTicketFromCookies(context, principal))
            {
                refreshCookiesToken = true;
            }
            if (refreshCookiesToken)
            {
                AddAuthCookie(context, token, principal);
            }
            return true;
        }
        /// <summary>是否续期Token</summary>
        public static bool IsRenewToken(this HttpContextBase context, IPKSPrincipal principal, ISecurityService service)
        {
            var tokenExpireSettings = service.GetTokenExpireSettings();
            var loginInterval = DateTime.Now - principal.CreateTime;
            return loginInterval.TotalMinutes >= tokenExpireSettings.RenewInterval.TotalMinutes;
        }
        /// <summary>登录</summary>
        public static LoginResult Login(this HttpContextBase context, ISecurityService service, AuthenticationType authenticationType, string userName, string password)
        {
            var request = new LoginRequest();
            request.AppCode = context.GetSubSystemCode();
            request.AuthenticationType = authenticationType;
            request.UserName = userName;
            request.Password = password;
            request.UserHostAddress = context.Request.Url.Authority;
            if (service == null) service = GetService<ISecurityService>();
            var result = service.Login(request);
            if (result.Succeed) context.OnLoginSuccess(result);
            return result;
        }
        /// <summary>Windows集成自动登录</summary>
        public static LoginResult WindowsAutoLogin(this HttpContextBase context, ISecurityService service)
        {
            if (context.User == null) return null;
            if (!(context.User.Identity is WindowsIdentity)) return null;
            var windowsIdentity = context.User.Identity;
            if (!windowsIdentity.IsAuthenticated) return null;
            var windowsUser = windowsIdentity.Name.Split('\\');
            var userName = windowsUser[1];
            var password = windowsUser[1];
            return context.Login(service, AuthenticationType.Windows, userName, password);
        }
        /// <summary>Windows集成登录</summary>
        public static LoginResult WindowsLogin(this HttpContextBase context, ISecurityService service, string userName, string password, bool onlyWindows)
        {
            string domainName, windowsUserName;
            var windowsUser = userName.Split('\\', '/');
            if (windowsUser.Length > 1)
            {
                domainName = windowsUser[0];
                windowsUserName = windowsUser[1];
            }
            else
            {
                domainName = null;
                windowsUserName = userName;
            }
            var identityService = GetService<IADIdentityService>();
            var domainResult = identityService.ValidateCredentials(domainName, windowsUserName, password);
            var result = new LoginResult();
            if (!domainResult.Succeed)
            {
                result.ErrorMessage = "Windows登录失败，" + domainResult.ErrorMessage;
            }
            else if (!onlyWindows)
            {
                result = context.Login(service, AuthenticationType.Windows, userName, password);
            }
            else
            {
                result.Succeed = true;
            }
            return result;
        }
        /// <summary>Windows集成登录</summary>
        public static WebActionResult WindowsLogin(this HttpContextBase context, WindowsIdentity logonUser)
        {
            var result = new WebActionResult();
            if (!logonUser.IsAuthenticated)
            {
                result.ErrorMessage = "Windows登录失败，不允许匿名登录!";
                return result;
            }
            var identityService = GetService<IADIdentityService>();
            if (!identityService.ValidateCredentials(logonUser))
            {
                result.ErrorMessage = "Windows登录失败，账户不存在!";
                return result;
            }
            var service = GetService<ISecurityService>();
            var windowsUser = logonUser.Name.Split('\\');
            var windowsUserName = windowsUser[1];
            var loginResult = context.Login(service, AuthenticationType.Windows, windowsUserName, windowsUserName);
            if (!loginResult.Succeed)
            {
                if (identityService.AutoRegisterUser)
                {
                    var registerResult = context.AutoRegisterDomainUser(windowsUserName);
                    if (!registerResult.Succeed)
                    {
                        result.ErrorMessage = registerResult.ErrorMessage;
                        return result;
                    }
                }
                loginResult = context.Login(service, AuthenticationType.Windows, windowsUserName, windowsUserName);
                if (!loginResult.Succeed)
                {
                    result.ErrorMessage = loginResult.ErrorMessage;
                    return result;
                }
            }
            result.Succeed = true;
            var returnUrl = context.Request["returnUrl"];
            if (!returnUrl.IsNullOrEmpty()) returnUrl = returnUrl.UrlDecode();
            result.Data = context.GetPortalLoginReturnUrl(service, loginResult.Token, loginResult.Principal, returnUrl);
            context.AddAuthCookie(loginResult.Token, loginResult.Principal, false);
            return result;
        }
        /// <summary>混合登录</summary>
        public static LoginResult MixedLogin(this HttpContextBase context, ISecurityService service, string userName, string password)
        {
            var result = context.WindowsLogin(service, userName, password, true);
            if (!result.Succeed)
            {
                return context.Login(service, AuthenticationType.Forms, userName, password);
            }
            result = context.Login(service, AuthenticationType.Windows, userName, password);
            if (!result.Succeed)
            {
                var registerResult = context.AutoRegisterDomainUser(userName);
                if (registerResult.Succeed)
                {
                    result = context.Login(service, AuthenticationType.Windows, userName, password);
                }
                else
                {
                    result.ErrorMessage = registerResult.ErrorMessage;
                }
            }
            return result;
        }
        /// <summary>域用户自动注册URL</summary>
        private static string AutoRegisterDomainUserUrl = "/RoleMapManage/AutoRegisterDomainUser?token=";
        /// <summary>域用户自动注册</summary>
        public static WebActionResult AutoRegisterDomainUser(this HttpContextBase context, string userName)
        {
            var token = GetService<ICacheProvider>().ExternalCacher.AddRandom(userName);
            var client = new HttpClientWrapper();
            client.TokenProvider = GetService<IApiServiceConfig>();
            var url = context.GetSubSystemUrl(PKSSubSystems.PORTALMGMT).TrimEnd('/') + AutoRegisterDomainUserUrl + token;
            return client.Get<WebActionResult>(url);
        }
        /// <summary>登录成功的处理</summary>
        public static void OnLoginSuccess(this HttpContextBase context, LoginResult result)
        {
            context.Session[PKSWebConsts.Session_Authentication] = result.Token;
            context.Items[PKSWebConsts.HttpContext_Principal] = result.Principal;
        }
        /// <summary>从会话获得认证令牌</summary>
        public static string GetTokenFromSession(this HttpContextBase context)
        {
            return context.Session[PKSWebConsts.Session_Authentication].As<string>();
        }
        /// <summary>从Cookies获得认证令牌</summary>
        public static string GetTokenFromCookies(this HttpContextBase context)
        {
            var ticket = context.GetTicketFromCookies();
            return ticket?.UserData;
        }
        /// <summary>从Cookies获得认证票据</summary>
        public static FormsAuthenticationTicket GetTicketFromCookies(this HttpContextBase context)
        {
            FormsAuthenticationTicket ticket;
            if (context.User != null && context.User.Identity is FormsIdentity)
            {
                ticket = context.User.Identity.As<FormsIdentity>().Ticket;
            }
            else
            {
                ticket = context.Request.Cookies.ExtractTicketFromCookie();
            }
            if (ticket != null)
            {
                context.Items[PKSWebConsts.HttpContext_FormsAuthenticationTicket] = ticket;
            }
            return ticket;
        }
        /// <summary>检查Cookies中是否存在认证票据</summary>
        public static bool ExistsTicketFromCookies(this HttpContextBase context, IPKSPrincipal principal)
        {
            FormsAuthenticationTicket ticket;
            if (context.User != null && context.User.Identity is FormsIdentity)
            {
                var identity = context.User.Identity.As<FormsIdentity>();
                if (identity.Name != principal.Identity.Name) return false;
                ticket = identity.Ticket;
            }
            else
            {
                ticket = context.Request.Cookies.ExtractTicketFromCookie();
                if (ticket == null || ticket.Name != principal.Identity.Name) return false;
            }
            context.Items[PKSWebConsts.HttpContext_FormsAuthenticationTicket] = ticket;
            return true;
        }
        /// <summary>加入授权Cookie</summary>
        public static void AddAuthCookie(this HttpContextBase context, string token, IPKSPrincipal principal, bool? isPersistent = null)
        {
            var ticket = context.Items[PKSWebConsts.HttpContext_FormsAuthenticationTicket].As<FormsAuthenticationTicket>();
            if (ticket == null) ticket = new FormsAuthenticationTicket("", false, 0);
            var persistent = isPersistent.HasValue ? isPersistent.Value : ticket.IsPersistent;
            var newTicket = new FormsAuthenticationTicket(ticket.Version, principal.Identity.Name, principal.CreateTime, principal.ExpireTime, persistent, token);
            string text = FormsAuthentication.Encrypt(newTicket);
            var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, text);
            httpCookie.HttpOnly = true;
            httpCookie.Path = FormsAuthentication.FormsCookiePath;
            httpCookie.Secure = FormsAuthentication.RequireSSL;
            httpCookie.Domain = FormsAuthentication.CookieDomain;
            if (persistent) httpCookie.Expires = newTicket.Expiration;
            context.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            context.Response.Cookies.Add(httpCookie);
        }
        /// <summary>获得登录用户</summary>
        public static IPKSPrincipal GetPrincipal(this HttpContextBase context, ISecurityService service = null, string token = null)
        {
            var principal = context.Items[PKSWebConsts.HttpContext_Principal].As<IPKSPrincipal>();
            if (principal == null)
            {
                if (token.IsNullOrEmpty())
                {
                    token = GetTokenFromSession(context);
                    if (token.IsNullOrEmpty()) return null;
                }
                if (service == null) service = GetService<ISecurityService>();
                principal = service.GetPrincipal(token);
                if (principal == null || principal.Expired) return null;
                context.Items[PKSWebConsts.HttpContext_Principal] = principal;
            }
            return principal;
        }
        /// <summary>获得登录名</summary>
        public static string GetLoginUserName(this HttpContextBase context)
        {
            var user = context.User;
            if (user == null) return string.Empty;
            if (user.Identity is WindowsIdentity)
            {
                var windowsUser = user.Identity.Name.Split('\\');
                if (windowsUser.Length > 1) return windowsUser[1];
            }
            return user.Identity.Name;
        }
        /// <summary>门户登录URL</summary>
        public static string PortalLoginUrl = "/Account/Login";
        ///// <summary>门户重定向URL</summary>
        //private static string PortalRedirectUrl = "/Redirect/Index";
        /// <summary>其它网站单点登录URL</summary>
        private static string SSOLoginUrl = "/SSOAccount/Login";
        /// <summary>获得门户登录URL</summary>
        public static string GetPortalLoginUrl(this HttpContextBase context)
        {
            return context.GetPortalUrl(PortalLoginUrl);
        }
        /// <summary>当前请求是否门户登录URL</summary>
        public static bool IsPortalLoginUrl(this HttpContextBase context)
        {
            var requestUrl = context.Request.Url.ToString();
            var loginUrl = context.GetPortalLoginUrl();
            return requestUrl.StartsWith(loginUrl, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>获得重定向的门户登录URL</summary>
        public static string GetRedirectUrlToPortalLogin(this HttpContextBase context, string returnUrl = null)
        {
            var portalLoginUrl = context.GetPortalLoginUrl();
            if (returnUrl.IsNullOrEmpty()) returnUrl = context.Request.Url.ToString();
            returnUrl = returnUrl.UrlEncode();
            return portalLoginUrl + nameof(returnUrl).GetFirstQueryString(returnUrl);
        }
        /// <summary>门户未授权URL</summary>
        private static string PortalNoAuthorizeUrl = "/Information/NoAuthorize";
        /// <summary>获得门户未授权URL</summary>
        public static string GetPortalNoAuthorizeUrl(this HttpContextBase context)
        {
            return context.GetPortalUrl(PortalNoAuthorizeUrl);
        }
        /// <summary>获得重定向的门户未授权URL</summary>
        public static string GetRedirectUrlToPortalNoAuthorize(this HttpContextBase context, string returnUrl = null)
        {
            var portalUrl = context.GetPortalNoAuthorizeUrl();
            if (returnUrl.IsNullOrEmpty()) returnUrl = context.Request.Url.ToString();
            returnUrl = returnUrl.UrlEncode();
            return portalUrl + nameof(returnUrl).GetFirstQueryString(returnUrl);
        }
        /// <summary>获得门户URL</summary>
        public static string GetPortalUrl(this HttpContextBase context, string path)
        {
            var portalSiteUrl = string.Empty;
            if (context.IsPortalSite())
            {
                portalSiteUrl = context.Request.Url.GetDomainUrl();
            }
            else
            {
                portalSiteUrl = context.GetSubSystemUrl(PKSSubSystems.Portal);
            }
            return portalSiteUrl.TrimEnd('/') + path;
        }
        /// <summary>获得某个角色门户菜单</summary>
        public static PortalMenu GetPortalMenu(this HttpContextBase context, ISecurityService service, IPKSPrincipal principal)
        {
            if (service == null) service = GetService<ISecurityService>();
            var sRoleId = principal.Roles.First().Id;
            var iRoleId = 1;
            if (sRoleId.IsNullOrEmpty() || !int.TryParse(sRoleId, out iRoleId))
            {
                iRoleId = 1;
            }
            return service.GetPortalMenu(iRoleId);
        }
        /// <summary>获得某个角色默认地址</summary>
        public static string GetRoleDefaultUrl(this HttpContextBase context, ISecurityService service, IPKSPrincipal principal)
        {
            var portalMenu = GetPortalMenu(context, service, principal);
            return portalMenu.DefaultUrl;
        }
        /// <summary>获得重定向到某个角色默认地址</summary>
        public static string GetRedirectUrlToReturnUrl(this HttpContextBase context, ISecurityService service, string token, string returnUrl)
        {
            var ssoSiteUrl = new Uri(returnUrl).GetDomainUrl().TrimEnd('/');
            var siteUrl = context.Request.Url.GetDomainUrl().TrimEnd('/');
            if (siteUrl.Equals(ssoSiteUrl, StringComparison.OrdinalIgnoreCase))
            {
                return returnUrl;
            }
            returnUrl = returnUrl.UrlEncode();
            return $"{ssoSiteUrl}{SSOLoginUrl}?{nameof(token)}={token}&{nameof(returnUrl)}={returnUrl}";
        }
        /// <summary>获得门户登录返回地址</summary>
        public static string GetPortalLoginReturnUrl(this HttpContextBase context, ISecurityService service, string token, IPKSPrincipal principal, string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = GetRoleDefaultUrl(context, service, principal);
            }
            return GetRedirectUrlToReturnUrl(context, service, token, returnUrl);
        }
        /// <summary>注销</summary>
        public static async Task<string> Logout(this HttpContextBase context, ISecurityService service)
        {
            var token = context.GetTokenFromSession();
            if (!token.IsNullOrEmpty())
            {
                if (service == null) service = GetService<ISecurityService>();
                await service.LogoutAsync(token);
                context.OnLogoutSuccess();
            }
            FormsAuthentication.SignOut();
            return context.GetPortalLoginUrl();
        }
        /// <summary>注销成功的处理</summary>
        public static void OnLogoutSuccess(this HttpContextBase context)
        {
            context.Session.Remove(PKSWebConsts.Session_Authentication);
        }
        /// <summary>获得Model验证失败信息</summary>
        public static string GetModelError(this Controller controller)
        {
            var errors = controller.ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage.IsNullOrEmpty() ? e.Exception.Message : e.ErrorMessage)
                .ToArray();
            return string.Join(Environment.NewLine, errors);
        }
        /// <summary>生成登录用户</summary>
        public static Dictionary<string, object> BuildPrincipals(this Controller controller)
        {
            var principals = new Dictionary<string, object>();
            var user = controller.User;
            principals["User"] = user;
            principals["User_Identity"] = BuildIdentity(user.Identity);
            var windowUser = controller.Request.LogonUserIdentity;
            principals["LogonUser"] = windowUser;
            principals["LogonUser_Identity"] = BuildIdentity(windowUser);
            principals["PKSUser"] = controller.HttpContext.GetPrincipal();
            return principals;
        }
        /// <summary>生成用户身份信息</summary>
        private static IIdentity BuildIdentity(IIdentity identity)
        {
            var pksIdentity = new PKSIdentity();
            pksIdentity.Name = identity.Name;
            pksIdentity.AuthenticationType = identity.AuthenticationType;
            pksIdentity.IsAuthenticated = identity.IsAuthenticated;
            return pksIdentity;
        }
        /// <summary>获得授权URL</summary>
        public static string GetAuthorizeUrl(this ControllerContext context)
        {
            var path = context.GetRoutePath();
            var siteUrl = context.HttpContext.Request.Url.GetDomainUrl().NormalizeUrl();
            return siteUrl + path.Trim('/');
        }
        /// <summary>获得路由路径</summary>
        private static string GetRoutePath(this ControllerContext context)
        {
            var route = context.RouteData.Route.As<Route>();
            var urlPath = route.Url;
            foreach (var pair in route.Defaults)
            {
                var value = context.RouteData.Values[pair.Key]?.ToString();
                urlPath = urlPath.Replace("{" + pair.Key + "}", value ?? string.Empty);
            }
            return urlPath;
        }
    }
}