using Jurassic.CommonModels;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    public class UploadDemoController : BaseController
    {
        // GET: UploadDemo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadDemo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(string uploadresult1, string uploadresult2)
        {
            //表单其他数据处理
            //
            return JsonTips("success", "提交成功",
                "上传的文件ID分别为：组件1：{0}, 组件2：{1}", (object)null, uploadresult1, uploadresult2);
        }

        public ActionResult UploadGetName(ResourceFileInfo[] results)
        {
            //TODO： 这里写自己的处理逻辑，在DEMO中，只简单演示一下返回上传文件的文件名
            return Json(results.Select(r => r.FileName));
        }

        //文件上传后的回调事件2
        public JsonResult UploadGetLength(ResourceFileInfo[] results)
        {
            //TODO： 这里写自己的处理逻辑，在DEMO中，只简单演示一下返回上传文件形成的流的长度
            return Json(results.Select(r => r.FileStream.Length).ToArray());
        }


    }
}