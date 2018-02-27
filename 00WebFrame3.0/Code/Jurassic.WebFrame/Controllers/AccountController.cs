using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using Jurassic.AppCenter.Models;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebFrame.Controllers
{
    /// <summary>
    /// 用户登录控制器
    /// </summary>
    public class AccountController : BaseController
    {
        /// <summary>
        /// 登录页
        /// </summary>
        /// <param name="returnUrl">登录成功后要转到的URL</param>
        /// <returns>登录页面</returns>
        [JAuth(JAuthType.EveryOne)]
        public virtual async Task<ActionResult> Login(string returnUrl)
        {
            ViewBag.ReturnUrl = null;// returnUrl;
            //if (User.Identity.IsAuthenticated)
            //{
            //    //将登录页的目标返回页用cookie记录，在index首页加载时自动加载到它的iframe内。
            //    //  SetStartPageCookie(returnUrl);
            //    return Redirect(Url.Content("~/"));
            //}
            var model = this.GetLoginModel();

            if (!model.Password.IsEmpty() && model.RememberMe)
            {
                Login(model, null);
                if (_loginResult == LoginState.OK)
                {
                    //将登录页的目标返回页用cookie记录，在index首页加载时自动加载到它的iframe内。
                    // SetStartPageCookie(returnUrl);
                    return Redirect(Url.Content("~/"));
                }
            }

            return View(model);
        }


        LoginState _loginResult = LoginState.UnKnown;

        /// <summary>
        /// 异步登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [JAuth(JAuthType.EveryOne)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return JsonTips();
            }
            _loginResult =  AppManager.Instance.Login(model);
            LogInfo.UserName = model.UserName;
            if (_loginResult == LoginState.OK)
            {
                //验证是否强制修改密码_by_zjf
                if (model.IsChangedPassword)
                {
                    return JsonTips("success", null, FStr.MustChangePassword, new { Url = Url.Action("changepassword", "account", new { isLogin = true }) });
                }

                var user = AppManager.Instance.UserManager.GetByName(model.UserName);
                //在同一台机器上不同用户登录时，通知AppManger设置原用户为离线
                // AppManager.Instance.Logout(User.Identity.Name);
                this.SetStartPageCookie(returnUrl);
                if (model.RememberMe)
                {
                    this.SetLoginModel(model);
                }
                else
                {
                    this.ClearLoginModel();
                }
                return JsonTips("success", null, FStr.LoginSucceed, new { Url = Url.Content("~/"), User = user });//User =  CurrentUser});
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            // ModelState.AddModelError("", "提供的用户名或密码不正确。");
            LogInfo.LogType = JLogType.Warning.ToString();
            return JsonTipsLang("error", null, "LoginState." + _loginResult);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public virtual ActionResult Logout(string returnUrl)
        {
            AppManager.Instance.Logout(User.Identity.Name);
            this.ClearLoginModel();
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = Url.Content("~/");
            return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 处理修改密码的POST请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult ChangePassword(PasswordChangeModel model)
        {
            //添加验证修改密码不能与原密码一致
            if (model.OldPassword == model.Password)
            {
                return JsonTips("error", FStr.NewPassowrdMustNotEqualToOld);
            }

            var IsLogin = Request["IsLogin"];
            LoginState resState = AppManager.Instance.StateProvider.ChangePassword(model);
            if (LoginState.OK == resState)
            {
                //修改默认的初始密码跳转到登录页, 首页修改密码后跳转到桌面
                //if (IsLogin != null && IsLogin.ToUpper() == "TRUE")
                //    return JsonTipsLang("success", null, "Password_Change_Success", new { Url = Url.Action("Logout", "Account") });
                //return JsonTipsLang("success", null, "Password_Change_Success", new { Url = Url.Action("StartPage", "Home") });

                //修改默认的初始密码跳转到登录页
                AppManager.Instance.Logout(User.Identity.Name);
                this.ClearLoginModel();
                return JsonTipsLang("success", null, FStr.PasswordChangeSucceed, new { Url = Url.Action("Logout", "Account") });
            }
            return JsonTipsLang("error", null, resState.ToString());
        }

        public virtual ActionResult ChangePassword(string isLogin)
        {
            if (string.IsNullOrEmpty(isLogin))
                isLogin = "false";
            ViewBag.IsLogin = isLogin;
            return View();
        }

        #region 重置密码
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult ForgotPassword(string userName, string email, string Email1)
        {
            var state = AppManager.Instance.StateProvider.SendPasswordResetMessage(userName, email, new Uri(Request.Url, Url.Action("ResetPassword")).ToString());
            if (state == LoginState.OK)
            {
                //修改默认的初始密码跳转到登录页
                return JsonTips("success", null, FStr.ResetPasswordEmailSent, new { Url = Url.Action("Logout", "Account") });
                //return JsonTipsLang("success", null, "Reset_Password_Email_Sent");
            }
            else
            {
                return JsonTips("error", state.ToString(), CommOp.ToStr(Session["ResetPasswordEmailBody"]));
            }
        }

        public virtual ActionResult ResetPassword(string userName, string confirmToken)
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult ResetPassword(PasswordResetModel model)
        {
            var state = AppManager.Instance.StateProvider.ResetPassword(model);
            if (state == LoginState.OK)
            {
                return JsonTips("success", null, FStr.ResetPasswordSucceed);
            }
            else
            {
                return JsonTipsLang("error", null, state.ToString());
            }
        }
        #endregion
    }
}
