using Jurassic.AppCenter.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.Tools;
using Jurassic.AppCenter;
using Jurassic.CommonModels;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebFrame.Controllers
{
    /// <summary>
    /// 日志管理的控制器
    /// </summary>
    public class LogController : BaseController
    {
        private LogManager mLogManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logManager">日志管理器</param>
        public LogController(LogManager logManager)
        {
            mLogManager = logManager;
        }

        /// <summary>
        /// 显示日志管理主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页显示的日志数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public JsonResult GetAll(int pageIndex, int pageSize, string key, string sortField, string sortOrder)
        {
            pageIndex++;//MiniUI的分页数字是从0开始，实际的分页是从1开始的，这里+1同步。
            string sortExpression = sortField + " " + sortOrder;
            if (String.IsNullOrWhiteSpace(sortExpression))
            {
                sortExpression = "Id desc";
            }

            //调用服务层业务逻辑获取分页数据
            var data = mLogManager.GetPage(pageIndex, pageSize, key, sortExpression);

            //根据mini-UI的mini-grid控件的约定返回数据
            return Json(new
            {
                data = data,
                total = data.RecordCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清除所有日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Clear()
        {
            mLogManager.Clear();
            return JsonTips("success", FStr.ClearSucceed);
        }

        /// <summary>
        /// 删除几条选定的日志
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            int[] idArr = CommOp.ToIntArray(ids, ',');

            mLogManager.DeleteByKeys(idArr);
            return JsonTips("success", JStr.SuccessDeleted);
        }

        /// <summary>
        /// 获取单用户的登录日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetUserLoginLogs(int pageIndex, int pageSize)
        {
            pageIndex++;
            var pager = mLogManager.GetUserLoginLogs(User.Identity.Name, pageIndex, pageSize);
            return Json(new
            {
                data = pager.ToList().Select(p=>new {
                   ClientIP = p.ClientIP,
                   Result = p.LogType == JLogType.Warning.ToString()? @FStr.LoginFailed:
                   FStr.LoginSucceed,
                   OpTime = p.OpTime,
                   Id = p.Id
                }),
                total = pager.RecordCount
            });
        }
    }
}
