using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebQuery.Models;
using Jurassic.WebQuery.Repository;
using Jurassic.WebFrame;
using Jurassic.AppCenter;
using Jurassic.Com.Tools;

namespace Jurassic.WebQuery.Controllers
{
    /// <summary>
    /// 高级查询的控制器
    /// </summary>
    public class AdvQueryController : BaseController
    {
        AdvQueryManager _queryMgr;
        public AdvQueryController(AdvQueryManager queryMgr)
        {
            _queryMgr = queryMgr;
        }

        protected override IEnumerable<ModelError> CheckModelError()
        {
            return null;
        }

        /// <summary>
        /// 保存用户定义的查询表达式，并返回当页页的整个查询列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveQueryItem(AdvQueryItem item)
        {
            item.Nodes = JsonHelper.FromJson<List<AdvQueryNode>>(Request.Form["Nodes"]) ?? new List<AdvQueryNode>();

            _queryMgr.Save(item);
            return JsonTipsLang("success", "Query_Saved_Success", item.Id);
        }

        /// <summary>
        /// 删除已有查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteQueryItem(int id)
        {
            _queryMgr.Delete(id);
            return JsonTipsLang("success", "Delete_Success");
        }

        /// <summary>
        /// 获取当前用户的当前Model下的查询名称
        /// </summary>
        /// <param name="modelName">模型类型名称</param>
        /// <returns></returns>
        public ActionResult GetUserQuerys(string modelName)
        {
            var querys = _queryMgr.GetUserQuerys(modelName).ToList();
            return Json(querys, JsonRequestBehavior.AllowGet);
        }
    }
}