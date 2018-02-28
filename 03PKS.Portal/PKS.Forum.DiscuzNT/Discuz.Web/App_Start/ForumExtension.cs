using System;
using System.Web;
using System.Web.Mvc;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using PKS.Core;
using PKS.DbModels;
using PKS.DbServices.SysFrame;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Services;
using System.Web.Configuration;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PKS.WebAPI.Models;
using Discuz.Common;
using Discuz.Plugin.PasswordMode;

namespace PKS.Forum.Web
{
    /// <summary>论坛扩展</summary>
    public static class ForumExtension
    {
        /// <summary>获得注入服务</summary>
        public static TService GetService<TService>()
        {
            return (TService)Bootstrapper.Get<TService>();
        }
        /// <summary>检查是否登录</summary>
        public static bool OnAuthorization(HttpContext httpContext, OnlineUserInfo userInfo)
        {
            var context = httpContext.GetHttpContextWrapper();
            string token = null;
            IPKSPrincipal principal = null;
            if (!context.IsLogined(null, ref token, out principal))
            {
                var redirectUrl = context.GetForumRedirectUrlToPortalLogin();
                httpContext.Response.Redirect(redirectUrl);
                httpContext.Response.End();
                return false;
            }
            if (userInfo == null || userInfo.Userid <= 0 || userInfo.Username != principal.Identity.Name)
            {
                var redirectUrl = context.GetForumRedirectUrlToAuth();
                httpContext.Response.Redirect(redirectUrl);
                httpContext.Response.End();
                return false;
            }
            return true;
        }
        /// <summary>获得到门户登录的重定向URL</summary>
        public static string GetForumRedirectUrlToPortalLogin(this HttpContextBase context, string returnUrl = null)
        {
            if (returnUrl.IsNullOrEmpty()) returnUrl = context.Items[PKSWebConsts.HttpContext_RawRequestUrl]?.ToString();
            if (returnUrl.IsNullOrEmpty()) returnUrl = context.Request.Url.ToString();
            returnUrl = returnUrl.UrlDecode();
            var siteUrl = context.Request.Url.GetDomainUrl().TrimEnd('/');
            returnUrl = $"{siteUrl}/redirect/user.aspx?type=login&{nameof(returnUrl)}={returnUrl}";
            return context.GetRedirectUrlToPortalLogin(returnUrl);
        }
        /// <summary>获得到用户认证的重定向URL</summary>
        public static string GetForumRedirectUrlToAuth(this HttpContextBase context)
        {
            var returnUrl = context.Items[PKSWebConsts.HttpContext_RawRequestUrl]?.ToString();
            if (returnUrl.IsNullOrEmpty()) returnUrl = context.Request.Url.ToString();
            returnUrl = returnUrl.UrlDecode();
            return $"/redirect/user.aspx?type=login&{nameof(returnUrl)}={returnUrl}";
        }
        /// <summary>创建用户</summary>
        public static UserInfo CreateUser(this HttpContextBase context, GeneralConfigInfo config, IPKSPrincipal principal, bool isAdmin)
        {
            var userInfo = CreateUser(config, principal, isAdmin);
            #region 发送欢迎信息
            if (config.Welcomemsg == 1)
            {
                // 收件箱
                PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
                privatemessageinfo.Message = config.Welcomemsgtxt;
                privatemessageinfo.Subject = "欢迎您的加入! (请勿回复本信息)";
                privatemessageinfo.Msgto = userInfo.Username;
                privatemessageinfo.Msgtoid = userInfo.Uid;
                privatemessageinfo.Msgfrom = PrivateMessages.SystemUserName;
                privatemessageinfo.Msgfromid = 0;
                privatemessageinfo.New = 1;
                privatemessageinfo.Postdatetime = userInfo.Joindate;
                privatemessageinfo.Folder = 0;
                PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
            }
            #endregion
            //发送同步数据给应用程序
            Sync.UserRegister(userInfo.Uid, userInfo.Username, userInfo.Password, "");
            //SetUrl("index.aspx");
            //SetShowBackLink(false);
            //SetMetaRefresh(config.Regverify == 0 ? 2 : 5);
            Statistics.ReSetStatisticsCache();
            //if (inviteCode != null)
            //{
            //    Invitation.UpdateInviteCodeSuccessCount(inviteCode.InviteId);
            //    if (config.Regstatus == 3)
            //    {
            //        if (inviteCode.SuccessCount + 1 >= inviteCode.MaxCount)
            //            Invitation.DeleteInviteCode(inviteCode.InviteId);
            //    }
            //}
            var oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
            if (config.Regverify == 0)
            {
                UserCredits.UpdateUserCredits(userInfo.Uid);
                //ForumUtils.WriteUserCookie(user, -1, config.Passwordkey);
                OnlineUsers.UpdateAction(oluserinfo.Olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
                //MsgForward("register_succeed");
                //AddMsgLine("注册成功, 返回登录页");
            }
            else
            {
                if (config.Regverify == 1)
                {
                    //AddMsgLine("注册成功, 请您到您的邮箱中点击激活链接来激活您的帐号");
                }
                else if (config.Regverify == 2)
                {
                    //AddMsgLine("注册成功, 但需要系统管理员审核您的帐户后才可登录使用");
                }
            }
            //ManyouApplications.AddUserLog(userInfo.Uid, UserLogActionEnum.Add);
            return userInfo;
        }
        /// <summary>创建用户信息</summary>
        private static UserInfo CreateUser(GeneralConfigInfo config, IPKSPrincipal principal, bool isAdmin)
        {
            var tmpUsername = principal.Identity.Name;
            // 如果找不到0积分的用户组则用户自动成为待验证用户
            UserInfo userinfo = new UserInfo();
            userinfo.Username = tmpUsername;
            userinfo.Nickname = tmpUsername;
            userinfo.Password = Guid.NewGuid().ToString();
            userinfo.Secques = "";
            userinfo.Gender = 0;
            userinfo.Adminid = isAdmin ? 1 : 0;
            userinfo.Groupexpiry = 0;
            userinfo.Extgroupids = "";
            userinfo.Regip = DNTRequest.GetIP();
            userinfo.Joindate = Discuz.Common.Utils.GetDateTime();
            userinfo.Lastip = userinfo.Regip;
            userinfo.Lastvisit = userinfo.Joindate;
            userinfo.Lastactivity = userinfo.Joindate;
            userinfo.Lastpost = userinfo.Joindate;
            userinfo.Lastpostid = 0;
            userinfo.Lastposttitle = "";
            userinfo.Posts = 0;
            userinfo.Digestposts = 0;
            userinfo.Oltime = 0;
            userinfo.Pageviews = 0;
            userinfo.Credits = 0;
            userinfo.Extcredits1 = Scoresets.GetScoreSet(1).Init;
            userinfo.Extcredits2 = Scoresets.GetScoreSet(2).Init;
            userinfo.Extcredits3 = Scoresets.GetScoreSet(3).Init;
            userinfo.Extcredits4 = Scoresets.GetScoreSet(4).Init;
            userinfo.Extcredits5 = Scoresets.GetScoreSet(5).Init;
            userinfo.Extcredits6 = Scoresets.GetScoreSet(6).Init;
            userinfo.Extcredits7 = Scoresets.GetScoreSet(7).Init;
            userinfo.Extcredits8 = Scoresets.GetScoreSet(8).Init;
            //userinfo.Avatarshowid = 0;
            userinfo.Email = principal.Identity.Email ?? "";
            userinfo.Bday = "";
            userinfo.Sigstatus = 1;
            userinfo.Tpp = 0;
            userinfo.Ppp = 0;
            userinfo.Templateid = 0;
            userinfo.Pmsound = 0;
            userinfo.Showemail = 0;
            userinfo.Salt = "";

            int receivepmsetting = 3;//关于短信息枚举值的设置看ReceivePMSettingType类型注释，此处不禁止用户接受系统短信息
            //foreach (string rpms in DNTRequest.GetString("receivesetting").Split(','))
            //{
            //    if (!Utils.StrIsNullOrEmpty(rpms))
            //        receivepmsetting = receivepmsetting | int.Parse(rpms);
            //}

            //if (config.Regadvance == 0)
            //    receivepmsetting = 7;

            userinfo.Newsletter = (ReceivePMSettingType)receivepmsetting;
            userinfo.Invisible = 0;
            userinfo.Newpm = config.Welcomemsg == 1 ? 1 : 0;
            userinfo.Medals = "";
            userinfo.Accessmasks = 0;
            userinfo.Website = "";
            userinfo.Icq = "";
            userinfo.Qq = "";
            userinfo.Yahoo = "";
            userinfo.Msn = "";
            userinfo.Skype = "";
            userinfo.Location = "";
            userinfo.Customstatus = "";
            //userinfo.Avatar = @"avatars\common\0.gif";
            //userinfo.Avatarwidth = 0;
            //userinfo.Avatarheight = 0;
            userinfo.Bio = "";
            userinfo.Signature = "";

            var usergroupid = isAdmin ? 1 : 7;
            var usergroupinfo = UserGroups.GetUserGroupInfo(usergroupid);

            PostpramsInfo postpramsinfo = new PostpramsInfo();
            postpramsinfo.Usergroupid = usergroupid;
            postpramsinfo.Attachimgpost = config.Attachimgpost;
            postpramsinfo.Showattachmentpath = config.Showattachmentpath;
            postpramsinfo.Hide = 0;
            postpramsinfo.Price = 0;
            postpramsinfo.Sdetail = userinfo.Signature;
            postpramsinfo.Smileyoff = 1;
            postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
            postpramsinfo.Parseurloff = 1;
            postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
            postpramsinfo.Allowhtml = 0;
            postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsinfo.Smiliesmax = config.Smiliesmax;
            userinfo.Sightml = UBB.UBBToHTML(postpramsinfo);

            userinfo.Authtime = userinfo.Joindate;

            if (isAdmin)
            {
                userinfo.Authstr = "";
                userinfo.Authflag = 0;
                userinfo.Groupid = usergroupid;
            }
            //邮箱激活链接验证
            else if (config.Regverify == 1)
            {
                userinfo.Authstr = ForumUtils.CreateAuthStr(20);
                userinfo.Authflag = 1;
                userinfo.Groupid = 8;
                //SendEmail(tmpUsername, DNTRequest.GetString("password").Trim(), DNTRequest.GetString(config.Antispamregisteremail).Trim(), userinfo.Authstr);
                //Emails.DiscuzSmtpMail(tmpUsername, emailaddress, password, authstr);
            }
            //系统管理员进行后台验证
            else if (config.Regverify == 2)
            {
                userinfo.Authstr = "";
                userinfo.Groupid = 8;
                userinfo.Authflag = 1;
            }
            else
            {
                userinfo.Authstr = "";
                userinfo.Authflag = 0;
                userinfo.Groupid = UserCredits.GetCreditsUserGroupId(0).Groupid;
            }
            userinfo.Realname = "";
            userinfo.Idcard = "";
            userinfo.Mobile = "";
            userinfo.Phone = principal.Identity.PhoneNumber;

            //第三方加密验证模式
            if (config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            {
                userinfo.Uid = PasswordModeProvider.GetInstance().CreateUserInfo(userinfo);
            }
            else
            {
                userinfo.Password = Discuz.Common.Utils.MD5(userinfo.Password);
                userinfo.Uid = Users.CreateUser(userinfo);
            }
            return userinfo;
        }
        /// <summary>获得门户后台登录用户ID</summary>
        public static string GetPortalMgmtUserId(this HttpContext context, string token)
        {
            return GetService<ICacheProvider>().ExternalCacher.GetRandom<string>(token);
        }
        /// <summary>获得门户后台登录用户信息</summary>
        public static VI_USERINFO GetPortalMgmtUser(this HttpContext context, int userId)
        {
            return GetService<IdentityService>().GetUserInfoById(userId);
        }
        /// <summary>获得默认管理员</summary>
        public static int GetDefaultAdmin(this HttpContext context)
        {
            return Users.GetDefaultAdminId();
        }
        /// <summary>过期分钟</summary>
        public static int ExpireMinutes
        {
            get { return (int)GetService<ISecurityService>().GetTokenExpireSettings().ExpireInterval.TotalMinutes; }
        }
        /// <summary>加入管理员Cookie</summary>
        public static void AddAdminCookie(this HttpContext context, GeneralConfigInfo config, OnlineUserInfo oluserinfo, string secques)
        {
            AddAdminCookie(context, config, oluserinfo.Userid, oluserinfo.Password, null, ExpireMinutes);
        }
        /// <summary>加入管理员Cookie</summary>
        public static void AddAdminCookie(this HttpContext context, GeneralConfigInfo config, int userId, string password, string secques, int expires)
        {
            var cookie = new HttpCookie("dntadmin");
            if (config == null) config = GeneralConfigs.GetConfig();
            if (secques == null) secques = Users.GetUserInfo(userId).Secques;
            cookie.Values["key"] = ForumUtils.SetCookiePassword(password + secques + userId.ToString(), config.Passwordkey);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            context.Response.AppendCookie(cookie);
        }
        /// <summary>WebResource.axd查询串解码</summary>
        public static string DecryptWebResource(string urlEncodedData)
        {
            var encryptedData = HttpServerUtility.UrlTokenDecode(urlEncodedData);
            var machineKeySection = typeof(MachineKeySection);
            var paramTypes = new Type[] { typeof(bool), typeof(byte[]), typeof(byte[]), typeof(int), typeof(int) };
            var encryptOrDecryptData = machineKeySection.GetMethod("EncryptOrDecryptData", BindingFlags.Static | BindingFlags.NonPublic, null, paramTypes, null);
            try
            {
                var decryptedData = (byte[])encryptOrDecryptData.Invoke(null, new object[] { false, encryptedData, null, 0, encryptedData.Length });
                var decrypted = Encoding.UTF8.GetString(decryptedData);
                return decrypted;
            }
            catch (TargetInvocationException)
            {
            }
            return String.Empty;
        }
        /// <summary>屏蔽用户名称(设置匿名)</summary>
        public static void MaskUserName(this HttpContext context, List<ShowtopicPagePostInfo> posts, OnlineUserInfo oluserinfo)
        {
            var adminNames = Users.GetAdminUsers();
            foreach (var post in posts)
            {
                if (oluserinfo != null && post.Poster == oluserinfo.Username) continue;
                if (adminNames.Any(e => e == post.Poster)) continue;
                post.Poster = "******";
                post.Username = post.Poster;
                post.Nickname = post.Poster;
            }
        }
        /// <summary>屏蔽用户名称(设置匿名)</summary>
        public static void MaskUserName(this HttpContext context, List<TopicInfo> topics, OnlineUserInfo oluserinfo)
        {
            var adminNames = Users.GetAdminUsers();
            foreach (var topic in topics)
            {
                if (oluserinfo != null && topic.Poster == oluserinfo.Username) continue;
                if (adminNames.Any(e => e == topic.Poster)) continue;
                topic.Poster = "******";
            }
            foreach (var topic in topics)
            {
                if (oluserinfo != null && topic.Lastposter == oluserinfo.Username) continue;
                if (adminNames.Any(e => e == topic.Lastposter)) continue;
                topic.Lastposter = "******";
            }
        }
        /// <summary>论坛主题索引处理器</summary>
        private static ForumTopicIndexHandler s_ForumTopicIndexHandler = new ForumTopicIndexHandler();
        /// <summary>保存论坛索引数据</summary>
        public static void SaveForumTopicToIndex(ForumInfo forumInfo, TopicInfo topicInfo, PostInfo postInfo)
        {
            Func<Dictionary<string, string>> handler = () => s_ForumTopicIndexHandler.BuildTopic(forumInfo, topicInfo, postInfo);
            SaveForumIndex(handler, true);
        }
        /// <summary>保存论坛索引数据</summary>
        public static void SaveForumPostToIndex(ForumInfo forumInfo, TopicInfo topicInfo, PostInfo postInfo)
        {
            Func<Dictionary<string, string>> handler = () => s_ForumTopicIndexHandler.BuildPost(forumInfo, topicInfo, postInfo);
            SaveForumIndex(handler, true);
        }
        /// <summary>生成论坛索引数据</summary>
        private static void SaveForumIndex(Func<Dictionary<string, string>> handler, bool replace)
        {
            s_ForumTopicIndexHandler.Initialize();
            var service = GetService<IndexerServiceWrapper>();
            var request = new IndexSaveRequest();
            request.Replace = replace;
            request.Metadatas = new MetadataCollection();
            var variables = handler();
            var indexData = s_ForumTopicIndexHandler.Handle(variables);
            request.Metadatas.Add(indexData);
            service.Save(request);
        }
    }
}