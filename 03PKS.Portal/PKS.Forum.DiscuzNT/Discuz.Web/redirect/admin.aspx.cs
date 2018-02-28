using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using PKS.Forum.Web;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace Discuz.Web.Redirect
{
    /// <summary>后台管理相关重定向操作</summary>
    public partial class admin : System.Web.UI.Page
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
                returnUrl = "/admin/index.aspx";
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
        /// <summary>从门户后台管理自动登录</summary>
        private bool AutoLogin(string returnUrl)
        {
            var token = this.Request.QueryString["token"];
            if (token.IsNullOrEmpty()) return false;
            var userId = this.Context.GetPortalMgmtUserId(token);
            if (userId.IsNullOrEmpty()) return false;
            var nUserId = 0;
            if (!int.TryParse(userId, out nUserId)) return false;
            var user = this.Context.GetPortalMgmtUser(nUserId);
            if (user == null) return false;
            var admin = Users.GetUserInfo(user.USERNAME);
            var context = this.Context.GetHttpContextWrapper();
            var config = GeneralConfigs.GetConfig();
            if (admin == null)
            {
                var principal = new PKSPrincipal();
                var identity = new PKSIdentity();
                identity.Name = user.USERNAME;
                identity.Email = user.EMAIL;
                identity.PhoneNumber = user.PHONENUMBER;
                principal.Identity = identity;
                admin = context.CreateUser(config, principal, true);
            }
            else if (admin.Adminid != 1 || admin.Groupid != 1)
            {
                admin.Adminid = 1;
                admin.Groupid = 1;
                admin.Authtime = Utils.GetDateTime();
                AdminUsers.UpdateUserAllInfo(admin);
                //移除该用户的在线信息，使之重建在线表信息
                OnlineUsers.DeleteUserByUid(admin.Uid);
            }
            var adminGroup = AdminUserGroups.AdminGetUserGroupInfo(admin.Groupid);
            ForumUtils.WriteUserCookie(admin.Uid, ForumExtension.ExpireMinutes, config.Passwordkey);
            this.Context.AddAdminCookie(config, admin.Uid, admin.Password, admin.Secques, ForumExtension.ExpireMinutes);
            SoftInfo.LoadSoftInfo();
            AdminVistLogs.InsertLog(admin.Uid, admin.Username, admin.Groupid, adminGroup.Grouptitle, DNTRequest.GetIP(), "后台管理员登陆", "");
            return true;
        }
    }
}