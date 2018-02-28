using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Collections;
using System.Data.Entity;
using Jurassic.AppCenter;
using PKS.DBModels;
using PKS.Data;
using PKS.PortalMgmt.Models;
using PKS.Core;
using CacheManager.Core;

namespace PKS.PortalMgmt.Controllers
{
    /// <summary>
    /// 论坛后台管理
    /// </summary>
    public class ForumController : PKSBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获得论坛后台登录Token
        /// </summary>
        [HttpPost]
        public ActionResult GetForumAdminToken()
        {
            var token = GetService<PKS.Core.ICacheProvider>().ExternalCacher.AddRandom(this.CurrentUserId);
            return Content(token);
        }
    }
}