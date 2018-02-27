using Jurassic.AppCenter;
using Jurassic.AppCenter.Models;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebFrame.Controllers
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 权限系统首页，检查系统环境并显示系统状态。
    /// </summary>
    [JAuth(Name = "ManageFirstPage", Ord = 0)]
    public class AppManagerController : BaseController
    {
        /// <summary>
        /// 应用程序管理起始页，嵌在首页中
        /// </summary>
        /// <returns>起始页的View</returns>
        [JAuth(VisibleType.None)]
        public ActionResult Start()
        {
            return View();
        }

        /// <summary>
        /// 重新开始初始化
        /// </summary>
        /// <returns>重定向的Setup的首页的View</returns>
        [JAuth(Name = "Restart+Initialize", Ord = 1)]
        public ActionResult Restart()
        {
            AppManager.Instance.StartReInit();
            return RedirectToAction("Index", "Setup");
        }

        /// <summary>
        /// 清空所有资源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WriteResFiles()
        {
            var resFileWriter = SiteManager.Kernel.Get<ResFileWriter>();
            try
            {
                CommOp.TryAndTry(() => resFileWriter.WriteResFiles());
            }
            catch (Exception ex)
            {
                return JsonTips("error", JStr.OperationFailed, ex.Message);
            }
            return JsonTips("success", JStr.OperationSucceed, FStr.PlzRestartWebSite);
        }

        /// <summary>
        /// 获取当前用户所有能操作的功能ID列表
        /// 用于前台的动态权限判断
        /// </summary>
        /// <returns>功能ID列表的Json数组</returns>
        [JAuth(JAuthType.AllUsers, VisibleType.None)]
        public JsonResult GetForbiddenIds()
        {
            return Json(User.Identity.GetForbiddenIds(), JsonRequestBehavior.AllowGet);
        }
    }
}
