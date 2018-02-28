using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using PKS.Forum.Web;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using System;
using System.Web;

namespace Discuz.Web.Redirect
{
    /// <summary>用户管理相关重定向操作</summary>
    public partial class user : System.Web.UI.Page
    {
        /// <summary>返回地址</summary>
        protected internal string returnUrl;
        /// <summary>加载页面</summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            var type = this.Request.QueryString["type"];
            returnUrl = this.Request.QueryString["returnUrl"];
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = "/index.aspx";
            }
            else
            {
                returnUrl = HttpUtility.UrlDecode(returnUrl);
            }
            switch (type)
            {
                case "login":
                    if (AutoLogin(returnUrl)) return;
                    break;
            }
        }
        /// <summary>自动认证登录</summary>
        private bool AutoLogin(string returnUrl)
        {
            var context = this.Context.GetHttpContextWrapper();
            string token = null;
            IPKSPrincipal principal = null;
            if (!context.IsLogined(null, ref token, out principal))
            {
                var redirectUrl = context.GetRedirectUrlToPortalLogin(returnUrl);
                this.Context.Response.Redirect(redirectUrl);
                this.Context.Response.End();
                return true;
            }
            var userName = principal.Identity.Name;
            var user = Users.GetUserInfo(userName);
            var config = GeneralConfigs.GetConfig();
            if (user == null) user = context.CreateUser(config, principal, false);
            ForumUtils.WriteUserCookie(user.Uid, ForumExtension.ExpireMinutes, config.Passwordkey);
            if (user.Groupid == 1)
            {
                var admin = user;
                var adminGroup = AdminUserGroups.AdminGetUserGroupInfo(admin.Groupid);
                this.Context.AddAdminCookie(config, admin.Uid, admin.Password, admin.Secques, ForumExtension.ExpireMinutes);
                //AdminVistLogs.InsertLog(admin.Uid, admin.Username, admin.Groupid, adminGroup.Grouptitle, DNTRequest.GetIP(), "后台管理员登陆", "");
            }
            else
            {
                this.Context.Response.AppendCookie(new HttpCookie("dntadmin"));
            }
            return false;
        }
    }
}