using PKS.Models;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PKS.Portal.Controllers
{
    /// <summary>账户控制器</summary>
    [AllowAnonymous]
    public class AccountController : PortalBaseController
    {
        /// <summary>构造函数</summary>
        public AccountController(ISecurityService securityService) : base()
        {
            _securityService = securityService;
        }
        /// <summary>安全服务</summary>
        private ISecurityService _securityService;
        /// <summary>登录</summary>
        public ActionResult Login(bool? autoLogin, string returnUrl)
        {
            returnUrl = returnUrl.UrlDecode();
            var isAutoLogin = false;
            if (autoLogin.HasValue)
            {
                isAutoLogin = autoLogin.Value;
            }
            else if (!returnUrl.IsNullOrEmpty())
            {
                isAutoLogin = true;
            }
            if (isAutoLogin)
            {
                string token = null;
                IPKSPrincipal principal = null;
                if (this.HttpContext.IsLogined(_securityService, ref token, out principal))
                {
                    var redirectUrl = this.HttpContext.GetPortalLoginReturnUrl(_securityService, token, principal, returnUrl);
                    return Redirect(redirectUrl);
                }
                if (PKSMvcExtension.AuthenticationConfig.IsWindowsAuthentication)
                {
                    var result = this.HttpContext.WindowsAutoLogin(_securityService);
                    if (result != null && result.Succeed)
                    {
                        var redirectUrl = this.HttpContext.GetPortalLoginReturnUrl(_securityService, result.Token, result.Principal, returnUrl);
                        return Redirect(redirectUrl);
                    }
                }
            }
            ViewBag.UserName = this.HttpContext.GetLoginUserName();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsAutoLogin = isAutoLogin;
            ViewBag.IsWindowsAuthentication = PKSMvcExtension.AuthenticationConfig.IsMixedAuthentication;
            return View();
        }
        /// <summary>Windows登录</summary>
        [HttpPost]
        private ActionResult WindowsLogin(string returnUrl)
        {
            var loginResult = new WebActionResult();
            var result = this.HttpContext.WindowsAutoLogin(_securityService);
            if (result.Succeed)
            {
                loginResult.Succeed = true;
                loginResult.Data = this.HttpContext.GetPortalLoginReturnUrl(_securityService, result.Token, result.Principal, returnUrl);
            }
            else
            {
                loginResult.ErrorMessage = result.ErrorMessage;
            }
            return Json(loginResult);
        }
        /// <summary>登录</summary>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var loginResult = new WebActionResult();
            var authenticationType = PKSMvcExtension.AuthenticationConfig.AuthenticationType;
            LoginResult result = null;
            switch(authenticationType)
            {
                case AuthenticationType.Windows:
                    result = this.HttpContext.WindowsLogin(_securityService, model.UserName, model.Password, false);
                    break;
                case AuthenticationType.Mixed:
                    result = this.HttpContext.MixedLogin(_securityService, model.UserName, model.Password);
                    break;
                default:
                    result = this.HttpContext.Login(_securityService, authenticationType, model.UserName, model.Password);
                    break;
            }
            if (result.Succeed)
            {
                loginResult.Succeed = true;
                loginResult.Data = this.HttpContext.GetPortalLoginReturnUrl(_securityService, result.Token, result.Principal, model.ReturnUrl);
                this.HttpContext.AddAuthCookie(result.Token, result.Principal, model.RememberMe);
            }
            else
            {
                loginResult.ErrorMessage = result.ErrorMessage;
            }
            return Json(loginResult, JsonRequestBehavior.DenyGet);
        }
        /// <summary>注销</summary>
        public async Task<ActionResult> Logout()
        {
            var loginUrl = await this.HttpContext.Logout(_securityService);
            return Redirect(loginUrl);
        }
    }
}