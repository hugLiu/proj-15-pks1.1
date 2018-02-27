using Jurassic.Com.Tools;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    /// <summary>
    /// 用户选择控件示例
    /// </summary>
    public class SelectUserDemoController : BaseController
    {
        //
        // GET: /SelectUserDemo/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string singleId, string multiIds)
        {
            //int sid = CommOp.ToInt(singleId);
            //int[] mids = CommOp.ToIntArray(multiIds, ',');

            return JsonTips("success", "提交成功", "你提交了单个用户ID：{0},多个用户ID：{1}", (object)null, singleId, multiIds);
        }
    }
}
