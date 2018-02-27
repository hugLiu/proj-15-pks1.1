using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    /// <summary>
    /// 高级查询示例, 代码是从系统自带的日志管理COPY过来
    /// </summary>
    public class AdvQueryDemoController : BaseController //, IKeyFilter<JLogInfo>
    {
        //
        // GET: /AdvQueryDemo/

        [Inject]
        public LogManager LogManager { get; set; }

        public AdvQueryDemoController()
        {
        }

        public ActionResult Index()
        {
            ViewBag.QueryType = typeof(JLogInfo);
            ViewBag.SearchEmptyText = "可以输入带空格关键词";
            return View();
        }

        /// <summary>
        /// 供前台获取数据，注意此处只需要返回原始的查询
        /// 筛选器会自动根据传递过来的参数对原始查询进行再次筛选
        /// </summary>
        /// <returns>返回的结果集</returns>
        [AdvQuery]
        public ActionResult GetData()
        {
            return Json(LogManager.GetQuery());
        }

        [HttpPost]
        public JsonResult Clear()
        {
            LogManager.Clear();
            return JsonTips("success", "Log_Clear_Success");
        }

        [HttpPost]
        public JsonResult Delete(string ids)
        {
            int[] idArr = CommOp.ToIntArray(ids, ',');

            LogManager.DeleteByKeys(idArr);
            return JsonTips("success", "Log_Delete_Success");
        }

        public System.Linq.Expressions.Expression<Func<JLogInfo, bool>> GetKeyFilter(string key)
        {
            return log => log.ActionName.Contains(key) || log.ModuleName.Contains(key);
        }
    }
}
