using System.Collections.Generic;
using System.Web.Mvc;
using PKS.DbServices.XEditor.Model;
using PKS.PortalMgmt.Controllers;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using PKS.Web;
using System.Net.Http;
using System;
using System.IO;
using System.Web;
using PKS.DbServices.WEditor;

namespace PKS.PortalMgmt.Controllers
{
    [AllowAnonymous]
    public class WEditorController : PKSBaseController
    {
        private WEditorService weditorService;
        private IBO2Service _bo2Service;

        public WEditorController(WEditorService weditorService)
        {
            _bo2Service = GetService<IBO2Service>();
            this.weditorService = weditorService;
        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadFile()
        {
            string uploadFileName = @"目标认识-井百科-LW3-2-1井页面.docx";
            //string fileName = @"目标认识-井百科-LW3-2-1井页面";



            //服务器根目录
            string serverRoorPath = Server.MapPath("~/");
            string currentDateTime = DateTime.Now.ToString("yyyyMMdd");

            string serverUrl = $"http://{HttpContext.Request.Url.Host}:{HttpContext.Request.Url.Port}/upload/html/{currentDateTime}/";

            //word文件夹路径
            string wordFileFolderPath = $"{serverRoorPath}upload/word/{currentDateTime}/";
            if (!Directory.Exists(wordFileFolderPath))
            {
                Directory.CreateDirectory(wordFileFolderPath);
            }
            //word文件路径
            //string sordFilePath = $"{wordFileFolderPath}{GetTimeStamp()}_{uploadFileName}";
            string sordFilePath = $"{wordFileFolderPath}{uploadFileName}";

            //html文件夹路径
            string htmlFileFolderPath = $"{serverRoorPath}upload/html/{currentDateTime}/";
            if (!Directory.Exists(htmlFileFolderPath))
            {
                Directory.CreateDirectory(htmlFileFolderPath);
            }
            //html文件路径          
            string htmlFileNamePath = $"{htmlFileFolderPath}{GetTimeStamp()}.html";


            //转换
            WordToHtmlConvert(sordFilePath, htmlFileNamePath);
            //解析
            string htmlStr = weditorService.AnalysisHtml(serverUrl, htmlFileNamePath);

            //html字符串编码
            string encodeHtmlStr = HttpUtility.HtmlEncode(htmlStr);
            //保存数据库



            return Content(htmlStr); //new EmptyResult();
        }




        public ActionResult InitBoTemplates()
        {
            return new EmptyResult();
        }

        public List<BO2> GetBosByBot(string bot)
        {
            var result = new List<object>();
            FilterRequest request = new FilterRequest
            {
                Query = new { bot = bot },
                Fields = new { bo = 1 },
                Sort = new { boid = 1 }
            };
            return _bo2Service.FilterBOs(request);

        }


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        private bool WordToHtmlConvert(string sourceFile, string destFile)
        {
            bool success = false;

            string webApiServiceUrl = this.HttpContext.GetWebApiServiceUrl();
            string url = $"{webApiServiceUrl}/AppDataService/Convert?type=Html&sourceFile={sourceFile}&destFile={destFile}";

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            string result = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(result)) success = true;

            return success;
        }




    }
}