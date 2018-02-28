using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using PKS.Models;
using PKS.SZXT.IService.Common;
using PKS.SZXT.Service.Common;
using PKS.Utils;
using PKS.Web;
using PKS.Web.Controllers;
using static Newtonsoft.Json.JsonConvert;
using System.Web;
using PKS.SZXT.Infrastructure;

namespace PKS.SZXT.Web.Controllers
{
    /// <summary>SZXT Web MVC基类控制器</summary>
    public abstract class SZXTBaseController : PKSBaseController
    {
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

        public void GoToPublicPage(string iiid)
        {
            var portalUrl = HttpContext.GetPortalSiteUrl();
            var urlParams = Request.Url.Query;
            //string url = $"{portalUrl}/Render/Content?iiid={iiid}";
            string url = $"{portalUrl}/Render/Content{urlParams}";
            Response.Redirect(url);
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
            if(wellColumnIndex < 0)
            {
                string fieldChineseName = "井号";
                if (wellField == "Well")
                {
                    fieldChineseName = "井号";
                }else if(wellField == "Block")
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
    }
}