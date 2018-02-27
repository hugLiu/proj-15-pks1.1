using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using PKS.Models;
using PKS.Web.Controllers;
using PKS.WebAPI.Services;

namespace PKS.Web.Core
{
    /// <summary>信息提供控制器</summary>
    public class CommonInfoController : PKSBaseController
    {
        /// <summary>
        /// 获取菜单及头部信息
        /// </summary>
        public virtual ActionResult GetHeadMenuInfo()
        {
            var principal = base.PKSUser;
            var siteUrl = this.HttpContext.GetSubSystemUrl(PKSSubSystems.WEBAPI);
            var infos = new Dictionary<string, string>();
            infos.Add("messagecount", "6");
            var headImgUrl = "/Content/images/header/icon01.jpg";
            var logoUrl = "/Content/images/header/logo.png";
            var currentSystem = this.HttpContext.GetSubSystemCode();
            if (currentSystem == PKSSubSystems.Forum)
            {
                headImgUrl = siteUrl + headImgUrl;
                logoUrl = siteUrl + logoUrl;
            }
            var userCenterUrl = HttpContext.GetSubSystemUrl(PKSSubSystems.Portal) + "/UserCenter/Index";
            infos.Add("imgsource", headImgUrl);
            infos.Add("logosource", logoUrl);
            infos.Add("gisurl", HttpContext.GetSubSystemUrl(PKSSubSystems.GIS));
            infos.Add("portalmgrurl", HttpContext.GetSubSystemUrl(PKSSubSystems.PORTALMGMT));
            infos.Add("logouturl", HttpContext.GetSubSystemUrl(PKSSubSystems.Portal) + "/account/logout?from=" + currentSystem);
            infos.Add("sooilurl", HttpContext.GetSubSystemUrl(PKSSubSystems.SOOIL) + "/search/list");
            infos.Add("currentuser", principal.Identity.Name);
            userCenterUrl += $"?userId={principal.Identity.Id}";
            infos.Add("apipath", GetPortalMenuUrl(siteUrl, principal));
            infos.Add("userCenterUrl", userCenterUrl);
            return NewtonJson(infos);
        }

        /// <summary>
        /// 菜单Url
        /// </summary>
        /// <returns></returns>
        public string GetPortalMenuUrl()
        {
            var principal = base.PKSUser;
            var siteUrl = this.HttpContext.GetSubSystemUrl(PKSSubSystems.WEBAPI);
            return GetPortalMenuUrl(siteUrl, principal);
        }
        /// <summary>
        /// 菜单Url
        /// </summary>
        /// <returns></returns>
        private string GetPortalMenuUrl(string siteUrl, IPKSPrincipal principal)
        {
            return siteUrl + "/api/SecurityService/GetPortalMenu?roleId=" + principal.Roles.FirstOrDefault().Id;
        }
    }
}