using Newtonsoft.Json.Linq;
using PKS.Models;
using PKS.SZZSK.Core.Common;
using PKS.SZZSK.IService.Common;
using PKS.SZZSK.Service.Common;
using PKS.SZZSK.Web.Config.PageSearchService;
using PKS.SZZSK.Web.Models;
using PKS.Utils;
using PKS.Web;
using PKS.Web.Controllers;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Newtonsoft.Json.JsonConvert;

namespace PKS.SZZSK.Web.Controllers
{
    public abstract class SZZSKController : PKSBaseController
    {
        protected static List<TemplateUrl> _templateUrlList = getTemplateUrlList();

        private static List<TemplateUrl> getTemplateUrlList()
        {
            var json = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "App_Data/TemplateUrl.json");
            var templateUrlList = json.JsonTo<List<TemplateUrl>>();
            return templateUrlList;
        }

        public SZZSKController()
        {
        }

        protected virtual string ControllerName
        {
            get
            {
                return GetType().FullName;
            }
        }
        protected virtual string ActionName
        {
            get
            {
                return RouteData.Values["Action"]?.ToString();
            }
        }

        protected virtual Dictionary<string, string> SearchConfig
        {
            get
            {
                return GetService<IPageSearchConfigGrabber>().GetPageSearchConfig(ControllerName, ActionName);
            }
        }

        public void ReocrdUserebavior(string iiid)
        {
            var serchService = GetService<ISearchService>();
            var request = GetSearchMetadataRequest(iiid);
            var result = serchService.GetMetadata(request);
            if (result != null)
            {
                ReocrdUserebavior(result);
            }
        }

        public void ReocrdUserebavior(Metadata metadata)
        {
            var pageDataService = GetService<IPageDataService>();
            var userBehaviorService = GetService<IUserBehaviorService>();
            var userbehavior = new UserBehavior();
            userbehavior.Referer = Request.UrlReferrer?.ToString() ?? "";
            userbehavior.IIId = metadata.IIId;
            userbehavior.System = metadata.GetValue(MetadataConsts.System).ToString();
            userbehavior.Title = metadata.Title;
            userbehavior.Type = "预览";
            userbehavior.LogDate = DateTime.UtcNow;
            var pagedata = pageDataService.Get(metadata.PageId ?? metadata[MetadataConsts.PageId].ToString());
            userbehavior.Url = pagedata.ContentRef;
            var principal = this.PKSUser;
            if (principal == null)
            {
                principal = this.HttpContext.GetPrincipal(null, null);
            }
            if (principal != null)
            {
                userbehavior.User = principal.Identity.Name;
                userbehavior.Role = principal.Roles.First().Name;
            }
            userBehaviorService.Add(userbehavior);
        }

        public void ReocrdBaikeUserebavior(string baike, string bo)
        {
            var resourcekey = "勘探知识库\\" + baike + "\\" + bo;
            string iiid = resourcekey.ToMD5();
            ReocrdUserebavior(iiid);
        }

        public void GoToBaikePage(string iiid)
        {
            var portalUrl = HttpContext.GetPortalSiteUrl();
            var serchService = GetService<ISearchService>();

            var request = GetSearchMetadataRequest(iiid);
            var result = serchService.GetMetadata(request);
            if (result == null)
            {
                Response.Redirect($"{portalUrl}/Render/NotFound");
                return;
            }

            ReocrdUserebavior(result);

            var action = _templateUrlList.FirstOrDefault(t => t.PageId.ToString() == result.PageId).Url;
            string url = $"{HttpContext.GetSubSystemUrl(PKSSubSystems.SZZSK)}{action}?bo={result.DataId}";
            Response.Redirect(url);
        }

        public void GoToCasePage(string iiid)
        {
            var portalUrl = HttpContext.GetPortalSiteUrl();
            var serchService = GetService<ISearchService>();

            var request = GetSearchMetadataRequest(iiid);
            var result = serchService.GetMetadata(request);
            if (result == null)
            {
                Response.Redirect($"{portalUrl}/Render/NotFound");
                return;
            }

            ReocrdUserebavior(result);

            var action = "/Case/Detail";
            string url = $"{HttpContext.GetSubSystemUrl(PKSSubSystems.SZZSK)}{action}?dataid={result.DataId}";
            Response.Redirect(url);
        }

        protected SearchMetadataRequest GetSearchMetadataRequest(string iiid)
        {
            var request = new SearchMetadataRequest();
            request.IIId = iiid;
            var filter = new SearchSourceFilter();
            filter.Includes = new List<string>() { MetadataConsts.IIId, MetadataConsts.PageId, MetadataConsts.DataId, MetadataConsts.Title, MetadataConsts.System, "bt", MetadataConsts.PT };
            request.Fields = filter;
            return request;
        }


        public void GoToPublicPage(string iiid)
        {
            var portalUrl = HttpContext.GetPortalSiteUrl();
            var serchService = GetService<ISearchService>();
            var request = GetSearchMetadataRequest(iiid);
            var result = serchService.GetMetadata(request);
            if (result == null)
            {
                Response.Redirect($"{portalUrl}/Render/NotFound");
                return;
            }
            if (result.ContainsKey("bt") && (result["bt"].ToString() == "目标认识" || result["bt"].ToString() == "专业研究"))
            {
                GoToBaikePage(iiid);
            }
            else if (result.ContainsKey("bt") && (result["bt"].ToString() == "知识案例"))
            {
                GoToCasePage(iiid);
            }
            //else if (result.ContainsKey("bt") && (result["bt"].ToString() == "石油百科词条"))
            //{
            //    string url = $"{portalUrl}/OilWiKi/Entry?iiid={iiid}&dataid={result["dataid"].ToString()}";
            //    Response.Redirect(url);
            //}
            else
            {
                string url = $"{portalUrl}/Render/Content?iiid={iiid}";
                Response.Redirect(url);
            }
        }

        public void GoToSearchPage(string pt, string period)
        {
            string ptUrl = $"ischecked=on&field=pt&operator=equal&matchtext1={pt}";
            string periodUrl = period != null ? $"&ischecked=on&field=period&operator=equal&matchtext1={period}" : "";
            string url = $"{HttpContext.GetSubSystemUrl(PKSSubSystems.SOOIL)}/Search/List?isadvance=true&" + ptUrl + periodUrl;
            Response.Redirect(url);
        }
        public void GotoAdvanceSearchPage(string pts)
        {
            string ptUrl = $"ischecked=on&field=pt&operator=terms&matchtext1={pts}";
            //string periodUrl = period != null ? $"&ischecked=on&field=period&operator=equal&matchtext1={period}" : "";
            string url = $"{HttpContext.GetSubSystemUrl(PKSSubSystems.SOOIL)}/Search/List?isadvance=true&" + ptUrl;// + periodUrl;
            Response.Redirect(url);
        }

        public void GoToSooilPage(string urlParam)
        {
            if (string.IsNullOrWhiteSpace(urlParam))
                urlParam = string.Empty;
            urlParam = HttpUtility.UrlDecode(urlParam);
            if (!urlParam.StartsWith("?"))
                urlParam = "?" + urlParam;
            string url = $"{HttpContext.GetSubSystemUrl(PKSSubSystems.SOOIL)}/Search/List" + urlParam;// + periodUrl;
            Response.Redirect(url);
        }

        protected virtual string GetFragInfoOfBo(Dictionary<string, string> searchConfig, IList<string> bos, string grid, IList<int> years)
        {
            var svc = GetService<IViewService>();
            svc.SearchConfig = searchConfig;
            var model = svc.GetFragInfoOfBo(bos, grid, years);
            return SerializeObject(model);
        }

        protected virtual string GetThumbnail(Dictionary<string, string> searchConfig, string target)
        {
            var svc = GetService<ViewServiceBase>();
            var query = searchConfig["g_th"];
            query = query.ToEsQuery(target);
            var model = svc.GetEsList(query);
            return SerializeObject(model);
        }
        /// <summary>生成表集合</summary>
        protected object BuildTables(IViewService service, Dictionary<string, object> metadata, string wellTag, string wellField, string grid)
        {
            if (metadata == null) return null;
            if (!metadata.ContainsKey("Content")) return null;
            var jArray = metadata["Content"].As<JArray>();
            var jTable = jArray.First().As<JObject>();
            var table = JsonUtil.ToObject(jTable).As<Dictionary<string, object>>();
            var createdDate = ViewServiceBase.CheckDateTime(metadata[MetadataConsts.CreatedDate]);
            var tableName = nameof(HtmlTable.TableName).ToLowerInvariant();
            table[tableName] = $"[截止{createdDate.ToString("yyyy/MM/dd")}]最新";
            var columns = table[nameof(HtmlTable.Columns).ToLowerInvariant()].As<object[]>().Cast<Dictionary<string, object>>().ToArray();
            var fieldName = nameof(HtmlTableColumn.Field).ToLowerInvariant();
            var wellColumnIndex = Array.FindIndex(columns, e => e[fieldName].ToString().Equals(wellField, StringComparison.OrdinalIgnoreCase));
            if (wellColumnIndex < 0)
            {
                string fieldChineseName = "井号";
                if (wellField == "Well")
                {
                    fieldChineseName = "井号";
                }
                else if (wellField == "Block")
                {
                    fieldChineseName = "区块名";
                }

                wellColumnIndex = Array.FindIndex(columns, e => e["title"].ToString().Equals(fieldChineseName, StringComparison.OrdinalIgnoreCase));
            }
            var wellColumnField = columns[wellColumnIndex][fieldName].ToString();
            var rows = table[nameof(HtmlTable.Rows).ToLowerInvariant()].As<object[]>();
            var wells = new Dictionary<string, bool>();
            foreach (var row in rows)
            {
                string well = null;
                if (row is object[])
                {
                    well = row.As<object[]>()[wellColumnIndex].ToString();
                }
                else
                {
                    well = row.As<Dictionary<string, object>>()[wellColumnField].ToString();
                }
                wells[well.Trim()] = true;
            }
            var wells2 = wells.Select(e => e.Key).ToList();
            var tables = new List<object>();
            tables.Add(table);
            var wellDatas = service.GetIndexDatasByQuery(grid, new object[] { wells2, wells2.Count }, false);
            if (wellDatas != null)
            {
                foreach (var wellData in wellDatas)
                {
                    var wellTable = wellData["Content"].As<JArray>().First();
                    var well = wellData[wellTag];
                    if (well is JArray)
                    {
                        well = well.As<JArray>().First().As<JValue>().Value;
                    }
                    else if (well is JValue)
                    {
                        well = well.As<JValue>().Value;
                    }
                    wellTable[tableName].As<JValue>().Value = well;
                    tables.Add(wellTable);
                }
            }
            return tables;
        }

        /// <summary>
        /// 清除配置缓存
        /// </summary>
        [AllowAnonymous]
        public ActionResult ClearConfigCache()
        {
            PageSearchService.Run(this.HttpContext.ApplicationInstance);
            return Content("OK");
        }

        /// <summary>
        /// 获取相邻对象
        /// </summary>
        /// <returns></returns>
        public List<string> GetNearBos(string name)
        {
            var svc = GetService<ViewServiceBase>();
            svc.SearchConfig = SearchConfig;
            string query = svc.ReplaceParameters("NearBos", name);
            var jObject = JObject.Parse(query);
            NearRequest request = new NearRequest();
            request.BO = jObject["bo"].ToString();
            request.BOT = jObject["bot"].ToString();
            request.Distince = Convert.ToDecimal(jObject["distince"].ToString());
            request.Top = Convert.ToInt32(jObject["top"].ToString());
            return svc.GetNearBos(request) ?? new List<string>();
        }

        protected TemplateUrl GetTemplateByUrl(string url)
        {
            return _templateUrlList.FirstOrDefault(t => t.Url == url);
        }

        protected int GetPageIdByUrl(string url)
        {
            var templateurl = _templateUrlList.FirstOrDefault(t => t.Url == url);
            return templateurl.PageId;
        }
    }
}