using Jurassic.CommonModels.Articles;
using Jurassic.WebFrame;
using Newtonsoft.Json;
using PKS.Data;
using PKS.DBModels;
using PKS.PortalMgmt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class PageRegisterController : PKSBaseController
    {
        private IRepository<PKS_SUBSYSTEM> db;

        public PageRegisterController(IRepository<PKS_SUBSYSTEM> db)
        {
            this.db = db;
        }

        // GET: PageRegister
        public ActionResult Index()
        {
            ViewBag.Token = Token;
            return View();
        }


        public string GetStorageType()
        {
            return "[{\"id\":\"Url\",\"text\":\"Url\"},{\"id\":\"File\",\"text\":\"File\"}]";//这里使用的是get请求
        }

        public JsonResult GetSubSystem()
        {
            var result = db.GetQuery().Select(p=>new {
                         Id = p.Id.ToString(),
                         Code = p.Code,
                         Name = p.Name,
                         RootUrl = p.RootUrl
            });
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetData()
        {
            string resultStr = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/PageData.json"));
            //return Json(resultStr, JsonRequestBehavior.AllowGet);
            return resultStr;
        }

        [HttpGet]
        public JsonResult GetData1()
        {
            string resultStr = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/PageData.json"));
            return Json(resultStr, JsonRequestBehavior.AllowGet);
            //return resultStr;
        }




        [HttpPost]
         public ActionResult Upload()
         {
           

            //string fileName = Request["name"];
            //int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            //var chunks = Request["chunks"];
            //var guid = Request["guid"];//前端传来的GUID号
            //var Md5 = Request["Md5"];//文件名
           
            //var dir = Server.MapPath("~/Upload");//文件上传目录
            //dir = Path.Combine(dir, guid);//临时保存分块的目录
            //if (!System.IO.Directory.Exists(dir))
            //    System.IO.Directory.CreateDirectory(dir);
            //string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            //var data = Request.Files["file"];//表单中取得分块文件
            ////if (data != null)//为null可能是暂停的那一瞬间
            ////{
            //data.SaveAs(filePath);//报错
            ////}
            return Json(new { erron = 0 });//Demo，随便返回了个值，请勿参考
        }

        public ActionResult Merge()
        {
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/Upload");//Upload 文件夹
            var dir = Path.Combine(uploadDir, guid);//临时文件夹
            var fileName = Request["fileName"];//文件名
           
            var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件
            var finalPath = Path.Combine(uploadDir, fileName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Flush();
            fs.Close();
            System.IO.Directory.Delete(dir);//删除文件夹
            return Json(new { error = 0 });//随便返回个值，实际中根据需要返回
        }

        [HttpPost]
        public JsonResult CheckExistFileByMd5() {
            var md5 = Request["md5"];//文件名
            var fileName = Request["fileName"];//文件名
            var exist= Request["exist"];
            if (exist=="true")
            {
                return Json(new { ifExist = true });
            }

            return Json(new { ifExist = false });
        }
    }
}