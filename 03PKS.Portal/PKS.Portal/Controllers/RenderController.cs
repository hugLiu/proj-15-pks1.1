using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PKS.Models;
using PKS.Web;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System.Text.RegularExpressions;

namespace PKS.Portal.Controllers
{
    public class RenderController : PortalBaseController
    {
        public IUserBehaviorService UserBehaviorService { get; set; }
        public IPageDataService PageDataService { get; set; }
        public ISearchService SearchService { get; set; }
        public RenderController(IUserBehaviorService userbehaviorservice,
            IPageDataService pagedataservice,
            ISearchService searchservice)
        {
            UserBehaviorService = userbehaviorservice;
            PageDataService = pagedataservice;
            SearchService = searchservice;
        }

        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View();
        }

        public new ActionResult Content(string iiid)
        {
            var handleUnknownFormat = Request["handleunknownformat"] == "1";
            var request = GetSearchMetadataRequest(iiid);
            var result = SearchService.GetMetadata(request);
            if (result == null)
            {
                return View("NotFound");
            }
            IndexPageData pagedata = null;
            var userbehavior = new UserBehavior();
            userbehavior.Referer = Request.UrlReferrer?.ToString() ?? "";
            userbehavior.IIId = result.IIId;
            userbehavior.System = result.GetValue(MetadataConsts.System).ToString();
            userbehavior.Title = result.Title;
            userbehavior.Type = "预览";
            userbehavior.LogDate = DateTime.UtcNow;
            if ((string.IsNullOrWhiteSpace(result.PageId) || result.PageId == "0"))
            {
                handleUnknownFormat = true;
                userbehavior.Url = "/DataRender/DownloadFile";
            }
            else
            {
                pagedata = PageDataService.Get(result.PageId ?? result[MetadataConsts.PageId].ToString());
                userbehavior.Url = pagedata.ContentRef;
            }
            var principal = this.PKSUser;
            userbehavior.User = principal.Identity.Name;
            userbehavior.Role = principal.Roles.First().Name;
            UserBehaviorService.Add(userbehavior);

            //处理未知格式，作下载处理
            var showType = result.ShowType;
            if (handleUnknownFormat)
            {
                return Redirect("/DataRender/DownloadFile?iiid=" + iiid + "&dataid=" + result.DataId);
            }
            //string[] list = pagedata.ContentRef.Split('/');
            string relativeUrl = pagedata.ContentRef;
            if (pagedata.System == PKSSubSystems.Forum)
            {
                relativeUrl = Regex.Replace(relativeUrl, @"\[(?<tag>[^\]+])\]", match => result[match.Groups["tag"].Value].ToString());
            }
            else if (pagedata.ContentRef.IndexOf("?") > -1)
            {
                relativeUrl = relativeUrl + "&iiid=" + iiid + "&dataid=" + result.DataId;
            }
            else
            {
                relativeUrl = relativeUrl + "?iiid=" + iiid + "&dataid=" + result.DataId;
            }
            Uri uri = new Uri(new Uri(HttpContext.GetSubSystemUrl(pagedata.System)), relativeUrl);
            return Redirect(uri.ToString());
            //return RedirectToAction(list[2], list[1], new { iiid = iiid, dataid = result.DataId });
        }

        private SearchMetadataRequest GetSearchMetadataRequest(string iiid)
        {
            var request = new SearchMetadataRequest();
            request.IIId = iiid;
            var filter = new SearchSourceFilter();
            filter.Includes = new List<string>() { MetadataConsts.IIId, MetadataConsts.PageId, MetadataConsts.DataId, MetadataConsts.Title, MetadataConsts.System, MetadataConsts.ShowType };
            request.Fields = filter;
            return request;
        }
    }
}