using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PKS.DbServices.Portal.UserCenter;
using PKS.Models;
using PKS.Utils;
using PKS.Web;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.Portal.Controllers
{
    public class DataRenderController : PortalBaseController
    {
        private IAppDataService AppDataService { get; set; }
        private ISearchService SearchService { get; set; }
        public DataRenderController(IAppDataService appdataservice, ISearchService searchService)
        {
            AppDataService = appdataservice;
            SearchService = searchService;
        }
        // GET: DataRender
        public ActionResult Image(string iiid, string dataid)
        {
            ViewBag.data = GetConfig(iiid, dataid, true);
            ViewBag.Title = ViewBag.data.title;
            ViewBag.hasFavorite = HasFavorite(iiid);
            return View(ViewBag.data);
        }

        public ActionResult PDF(string iiid, string dataid)
        {
            ViewBag.data = GetConfig(iiid, dataid, true);
            ViewBag.Title = ViewBag.data.title;
            ViewBag.hasFavorite = HasFavorite(iiid);
            return View(ViewBag.data);
        }


        public ActionResult HTML(string iiid, string dataid)
        {
            ViewBag.data = GetConfig(iiid, dataid, false);
            ViewBag.Title = ViewBag.data.title;
            return View(ViewBag.data);
        }

        public ActionResult Table(string iiid, string dataid)
        {
            ViewBag.data = GetConfig(iiid, dataid, false);
            ViewBag.Title = ViewBag.data.title;
            return View(ViewBag.data);
        }
        public ActionResult Histogram(string iiid, string dataid)
        {
            ViewBag.data = GetConfig(iiid, dataid, false);
            ViewBag.Title = ViewBag.data.title;
            return View(ViewBag.data);
        }

        public ActionResult PropertyGrid(string iiid, string dataid)
        {
            ViewBag.data = GetConfig(iiid, dataid, false);
            ViewBag.Title = ViewBag.data.title;
            return View(ViewBag.data);
        }

        private SearchMetadataRequest GetSearchMetadataRequest(string iiid)
        {
            SearchMetadataRequest request = new SearchMetadataRequest();
            request.IIId = iiid;
            SearchSourceFilter filter = new SearchSourceFilter();
            filter.Includes = new List<string>() { MetadataConsts.IIId, MetadataConsts.ShowType, MetadataConsts.PageId, MetadataConsts.DataId, MetadataConsts.Title, MetadataConsts.CreatedDate, MetadataConsts.Author, MetadataConsts.Abstract };
            request.Fields = filter;
            return request;
        }

        private dynamic GetConfig(string iiid, string dataid, bool fromUrl)
        {
            var result = SearchService.GetMetadata(GetSearchMetadataRequest(iiid));
            
            var dataSource = GetFullAPIUrl(dataid);
            if (!fromUrl)
            {
                var content = (AppDataService.Get(dataid) as PKS.WebAPI.Models.IndexAppData).Content;
                dataSource = Convert.ToString(content);
            }
            dynamic data=new ExpandoObject();
            data.userid = PKSUser.Identity.Id;
            data.iiid = iiid;
            data.showtype = result.ShowType;
            data.dataid = result.DataId;
            data.createddate = result.GetValueBy(MetadataConsts.CreatedDate);
            data.title = result.Title;
            data.author = result.Author;
            data.Abstract = result.Abstract;
            data.dataSource = dataSource;
            return data;
        }

        private string GetFullAPIUrl(string dataid)
        {
            return HttpContext.GetSubSystemUrl(PKSSubSystems.WEBAPI) + "/api/appdataservice/download?dataid=" + dataid;
        }

        public ActionResult Show(string iiid)
        {
            ViewBag.data = GetDataSource(iiid);
            return View();
        }

        public string GetDataSource(string iiid)
        {
            var result = SearchService.GetMetadata(GetSearchMetadataRequest(iiid));
            var dataId = result.DataId;
            var dataSource =string.Empty;
            var showType = result.ShowType;
            if (string.Equals(showType, "image",StringComparison.OrdinalIgnoreCase)||
                string.Equals(showType, "pdf",StringComparison.OrdinalIgnoreCase))
            {
                dataSource = GetFullAPIUrl(dataId);
            }
            else
            {
                var pageData = (AppDataService.Get(dataId) as PKS.WebAPI.Models.IndexAppData);
                var content = pageData.Content;
                dataSource = Convert.ToString(content);              
            }
            return SerializeObject(new { iiid = iiid, showtype = showType, createddate = result.GetValueBy(MetadataConsts.CreatedDate), title = result.Title, author = result.Author, Abstract = result.Abstract, dataSource = dataSource });
        }

        public ActionResult External(string iiid, string dataid)
        {
            return Redirect(dataid);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public bool DownloadFile(string iiid, string dataid)
        {
            var usercenterService = GetService<UserCenterService>();
            usercenterService.AddDownLoad(PKSUser.Identity.Id.ToInt32(), iiid);

            try
            {
                DownloadRequest downloadRequest = new DownloadRequest();
                downloadRequest.DataId = dataid;
                downloadRequest.Source = true;
                downloadRequest.Download = true;

                var response = AppDataService.Download(downloadRequest);
                byte[] bytes = new byte[(int)response.Content.Length];
                response.Content.Read(bytes, 0, bytes.Length);
                response.Content.Close();
                Response.ContentType = response.ContentType;
                //Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(response.FileName, System.Text.Encoding.UTF8));
                Response.AddHeader("Content-Disposition", "attachment;  filename=" + response.FileName);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception e)
            {
                string content = "下载失败：\r\n" + e.Message;
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Disposition", "attachment;  filename=error.txt");
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            return true;
        }

        private bool HasFavorite(string iiid)
        {
            var usercenterService = GetService<UserCenterService>();
            return usercenterService.HasFavorite(PKSUser.Identity.Id.ToInt32(), iiid);
        }
    }
}