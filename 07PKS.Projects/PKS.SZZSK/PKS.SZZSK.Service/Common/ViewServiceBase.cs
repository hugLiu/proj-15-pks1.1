using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Ninject;
using PKS.Core;
using PKS.Models;
using PKS.SZZSK.Core.DTO;
using PKS.SZZSK.IService.Common;
using PKS.Utils;
using PKS.WebAPI;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using PKS.SZZSK.Core.Common;
using PKS.SZZSK.Core.Model;

namespace PKS.SZZSK.Service.Common
{
    public class ViewServiceBase : IViewService, IPerRequestAppService
    {
        [Inject]
        public IUserBehaviorService UserBehaviorService { get; set; }
        [Inject]
        public ISearchService SearchService { get; set; }
        [Inject]
        public IAppDataService AppDataService { get; set; }
        [Inject]
        public IPageDataService PageDataService { get; set; }
        [Inject]
        public IBO2Service BO2Service { get; set; }
        [Inject]
        public WebAPI.Services.IApiServiceConfig ApiServiceConfig { get; set; }
        public Dictionary<string, string> SearchConfig { get; set; }
        public virtual EsRoot GetEsData(string esQuery)
        {
            return SearchService.ESSearch(esQuery).To<EsRoot>();
        }
        public virtual object GetAppData(string esQuery)
        {
            var dataId = SearchService.ESSearch(esQuery)
                                      .To<EsRoot>()
                                      .GetSource()
                                      .FirstOrDefault()
                                      ?[MetadataConsts.DataId];
            if (dataId == null) return null;
            return AppDataService.Get((string)dataId)?.Content;
        }
        /// <summary>
        ///为保证验收通过，增加类似的方法。
        /// </summary>
        /// <param name="esQuery"></param>
        /// <returns></returns>
        public virtual object GetAppDataEx(string esQuery)
        {
            var model = SearchService.ESSearch(esQuery)
                                      .To<EsRoot>()
                                      .GetSource()
                                      .FirstOrDefault();
            if (model == null) return null;
            var dataId = model?[MetadataConsts.DataId];
            if (dataId == null) return null;
            //return AppDataService.Get((string)dataId)?.Content;
            return new { ShowType = model["showtype"] == null ? null : model["showtype"], Content = AppDataService.Get((string)dataId)?.Content };
        }


        public virtual object[] GetAppDatas(string esQuery, bool mergeTableRows)
        {
            var dataIds = SearchService.ESSearch(esQuery)
                                      .To<EsRoot>()
                                      .GetSource()
                                      .Select(e => e[MetadataConsts.DataId].ToString())
                                      .ToList();
            if (dataIds.IsNullOrEmpty()) return null;
            var appResult = AppDataService.GetMany(dataIds);
            var appDatas = dataIds.Select(e => appResult.GetValueBy(e).Content).ToList();
            if (mergeTableRows)
            {
                var appDatas2 = appDatas.SelectMany(e => e.As<JArray>().ToArray()).Cast<JObject>().ToList();
                var appData = appDatas2[0];
                var appRows = appData["rows"].As<JArray>();
                for (int i = 1; i < appDatas2.Count; i++)
                {
                    var rows = appDatas2[i]["rows"].As<JArray>();
                    rows.ForEach(appRows.Add);
                }
                return new object[] { appData };
            }
            return appDatas.ToArray();
        }
        private IEnumerable<Dictionary<string, object>> FormatEsDateTime(EsRoot esResult)
        {
            var src = esResult.GetSource();
            DateTime date;
            foreach (var o in src)
            {
                date = (DateTime)o[MetadataConsts.IndexedDate];
                o[MetadataConsts.IndexedDate] = date.ToLocalTime().ToMonthDay();
                if (o.ContainsKey(MetadataConsts.CreatedDate))
                {
                    o[MetadataConsts.CreatedDate] = ((DateTime)o[MetadataConsts.CreatedDate]).ToLocalTime();
                }
            }
            return src;
        }
        public virtual IEnumerable<Dictionary<string, object>> GetEsNews(string esQuery)
        {
            var esResult = GetEsData(esQuery);
            return FormatEsDateTime(esResult);
        }
        public virtual IEnumerable<Dictionary<string, object>> GetTargetEsNews(string esQuery, string targetName)
        {
            var esResult = GetEsData(esQuery);
            var result = FormatEsDateTime(esResult);
            foreach (var o in result)
            {
                var targets = o[targetName];
                if (targets == null) continue;
                var target = targets.As<JArray>().First().As<JValue>().Value.ToString();
                var title = o[MetadataConsts.Title].ToString();
                if (title.IndexOf(target, StringComparison.OrdinalIgnoreCase) > -1) continue;
                o[MetadataConsts.Title] = target + title.ToString();
            }
            return result;
        }
        public virtual IEnumerable<Dictionary<string, object>> GetEsList(string esQuery)
        {
            return GetEsData(esQuery).GetSource();
        }
        public IEnumerable<object> GetFragInfoOfBo(IList<string> bos, string grid, IList<int> years)
        {
            var query = SearchConfig[grid];
            query = query.ToEsQuery(bos, years);
            return GetQuery(query);
        }
        public virtual IEnumerable<object> GetQuery(string esQuery)
        {
            var lst = GetEsList(esQuery);
            return ToViewItems(lst)
                   .GroupBy(o => new { pt = o[MetadataConsts.PT], isImageType = IsImageType((string)o[MetadataConsts.ShowType]) })
                   .Select(g => new { pt = g.Key.pt, isImageType = g.Key.isImageType, list = g.ToList() });
        }

        public IEnumerable<Dictionary<string, object>> ToViewItems(IEnumerable<Dictionary<string, object>> src)
        {
            var res = new List<Dictionary<string, object>>();
            foreach (var o in src)
            {
                res.Add(ToViewItem(o));
            }
            return res;
        }

        private bool IsImageType(string str)
        {
            return String.Equals(str, "image", StringComparison.CurrentCultureIgnoreCase);
        }
        public Dictionary<string, object> ToViewItem(Dictionary<string, object> src)
        {
            var res = FormatEsDate(src);
            return AttachDownloadUrl(res);
        }
        public virtual Dictionary<string, object> FormatEsDate(Dictionary<string, object> src)
        {
            if (src.ContainsKey(MetadataConsts.IndexedDate))
                src[MetadataConsts.IndexedDate] = ((DateTime)src[MetadataConsts.IndexedDate]).ToLocalTime()
                                                                                            .ToMonthDay();
            if (src.ContainsKey(MetadataConsts.CreatedDate))
                src[MetadataConsts.CreatedDate] = ((DateTime)src[MetadataConsts.CreatedDate]).ToLocalTime()
                                                                                            .ToMonthDay();
            return src;
        }

        public virtual Dictionary<string, object> AttachDownloadUrl(Dictionary<string, object> src)
        {
            var isCanDownload = src.ContainsKey(MetadataConsts.DataId)
                                && src.ContainsKey(MetadataConsts.ShowType)
                                && IsImageType((string)src[MetadataConsts.ShowType]);
            if (isCanDownload)
            {
                //src["thumbnail"] = GetDownloadUrl((string)src[MetadataConsts.DataId]);
                src["url"] = src["resourcekey"] = GetDownloadUrl((string)src[MetadataConsts.DataId]);
            }
            return src;
        }

        public virtual string ReplaceParameters(string grid, params object[] parameters)
        {
            string query = SearchConfig[grid];
            if (parameters == null) return query;

            foreach (var p in parameters)
            {
                Regex regValue = new Regex($"\"@\\w+\"");
                string replacement = string.Empty;

                if (p is String)
                {
                    replacement = $"\"{p.ToString()}\"";
                }
                else if (p is int || p is decimal || p is float || p is bool)
                {
                    replacement = p.ToString();
                }
                else
                {
                    replacement = p.ToJson();
                }
                query = regValue.Replace(query, replacement, 1);
            }
            return query;
        }

        public Dictionary<string, BOTPropertyDefinition> GetBOTProperties(string grid)
        {
            var properties = new Dictionary<string, BOTPropertyDefinition>();

            string query = this.ReplaceParameters(grid);
            JObject queryJObject = JObject.Parse(query);

            FilterRequest request = new FilterRequest();
            request.Query = JObject.Parse(queryJObject["query"].ToString());
            List<BOT> bots = BO2Service.FilterBOTs(request);
            if (bots != null & bots.Count > 0)
            {
                foreach (var property in bots[0].Properties.OrderBy(p => p.Sequence))
                {
                    var field = queryJObject["fields"][$"properties.{property.Name}"];
                    if (field != null && field.ToObject<int>() == 1 )
                    {
                        properties.Add(property.Name, property);
                    }
                }
            }
            //BOT objBOT = BO2Service.GetBOT(bot);
            //if(objBOT != null)
            //{
            //    foreach (var property in objBOT.Properties.OrderBy(p => p.Sequence))
            //    {
            //        properties.Add(property.Name, property.Options);
            //    }
            //}

            return properties;
        }

        public List<string> GetBOsByName(string bot, string bo, int? from, int? size)
        {
            List<string> bos = new List<string>();
            FilterRequest request = new FilterRequest();

            string json = $"{{'bot':'{bot}','bo':{{'$regex':'{bo}','$options':'i'}}}}";
            request.Query = JObject.Parse(json);
            request.Fields = JObject.Parse("{'bo':1}");
            request.Sort = JObject.Parse("{'bo':1}");
            request.Skip = from;
            request.Limit = size;

            List<BO2> bo2s = BO2Service.FilterBOs(request);
            return bo2s.Select(e => e.BO).ToList();
        }

        public long GetBOCountByName(string bot, string bo)
        {
            string json = $"{{'bot':'{bot}','bo':{{'$regex':'{bo}','$options':'i'}}}}";
            return BO2Service.CountBOs(JObject.Parse(json));
        }

        public List<string> GetBOsByProperties(string bot, Dictionary<string, List<string>> properties, int? from = 0, int? size = 10)
        {
            return GetBOs(bot, null, properties, null, from, size);
        }

        public List<BO2> GetAliasBOs(string bot, string boname = null, Dictionary<string, List<string>> properties = null, string[] conditions = null, int? from = 0, int? size = 10)
        {
            List<string> matches = new List<string>();
            if (!boname.IsNullOrEmpty())
            {
                matches.Add("'bo': { '$regex': '" + boname + "', '$options':'i'}");
            }
            if (properties != null)
            {
                foreach (var p in properties)
                {
                    matches.Add($"'properties.{p.Key}': {{ '$in': {p.Value.ToJson()} }}");
                }
            }
            if (!conditions.IsNullOrEmpty())
            {
                conditions.ForEach(matches.Add);
            }
            string json = $"{{'bot':'{bot}',{string.Join(",", matches.ToArray())}}}";
            FilterRequest request = new FilterRequest();
            request.Query = JObject.Parse(json);
            request.Fields = JObject.Parse("{'bo':1,'alias':1}");
            request.Sort = JObject.Parse("{'bo':1}");
            request.Skip = from;
            request.Limit = size;

            //List<string> aliasBOs = new List<string>();

            List<BO2> bo2s = BO2Service.FilterBOs(request);
            //bo2s.ForEach(bo => { aliasBOs.Add(bo.BO); if(bo.Alias !=null) aliasBOs.AddRange(bo.Alias); });
            return bo2s;
        }

        public List<string> GetBOs(string bot, string boname = null, Dictionary<string, List<string>> properties = null, string[] conditions = null, int? from = 0, int? size = 10)
        {
            List<string> matches = new List<string>();
            if (!boname.IsNullOrEmpty())
            {
                matches.Add("'bo': { '$regex': '" + boname + "', '$options':'i'}");
            }
            if (properties != null)
            {
                foreach (var p in properties)
                {
                    matches.Add($"'properties.{p.Key}': {{ '$in': {p.Value.ToJson()} }}");
                }
            }
            if (!conditions.IsNullOrEmpty())
            {
                conditions.ForEach(matches.Add);
            }
            string json = $"{{'bot':'{bot}',{string.Join(",", matches.ToArray())}}}";
            FilterRequest request = new FilterRequest();
            request.Query = JObject.Parse(json);
            request.Fields = JObject.Parse("{'bo':1}");
            request.Sort = JObject.Parse("{'bo':1}");
            request.Skip = from;
            request.Limit = size;

            List<BO2> bo2s = BO2Service.FilterBOs(request);
            return bo2s.Select(e => e.BO).ToList();
        }
        public long GetBOCountByProperties(string bot, Dictionary<string, List<string>> properties)
        {
            List<string> matches = new List<string>();
            foreach (var p in properties)
            {
                matches.Add($"'properties.{p.Key}': {{ '$in': {p.Value.ToJson()} }}");
            }
            string json = $"{{'bot':'{bot}',{string.Join(",", matches.ToArray())}}}";
            return BO2Service.CountBOs(JObject.Parse(json));
        }

        public object GetBOPTList(string grid, List<string> BOs, int? from = 0, int? size = 10)
        {
            string query = this.ReplaceParameters(grid, BOs, from, size);
            return SearchService.ESSearchEx(JObject.Parse(query));
        }


        public List<BOTPropertyDefinition> GetBotProtertysByBot(string bot)
        {
            return BO2Service.GetBOT(bot).Properties;
        }

        public virtual string GetDownloadUrl(string dataId)
        {
            return $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
        }
        public virtual string GetSourceDownloadUrl(string dataId)
        {
            return $"{ApiServiceConfig.Url}appdataservice/download?source=true&dataid={dataId}";
        }

        public virtual string GetImageUrl(string esQuery)
        {
            var dataId = SearchService.ESSearch(esQuery)
                                      .To<EsRoot>()
                                      .GetSource()
                                      .FirstOrDefault()
                                      ?[MetadataConsts.DataId];
            if (dataId == null) return null;
            return $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
        }
        public virtual List<string> GetNearBos(NearRequest req)
        {
            var res = new List<string>();
            var svcRes = BO2Service.Near(req);
            if (svcRes != null && svcRes is JArray)
            {
                var resArray = svcRes as JArray;
                res = resArray.Select(item => item.ToString()).ToList();
            }
            return res;
        }
        public virtual IEnumerable<Dictionary<string, object>> GetImageList(string esQuery)
        {
            var res = new List<Dictionary<string, object>>();
            var hits = SearchService.ESSearch(esQuery)
                                     .To<EsRoot>()
                                     .GetSource();
            foreach (var o in hits)
            {
                //o["thumbnail"] = GetDownloadUrl((string)o[MetadataConsts.DataId]);
                o["url"] = o["resourcekey"] = GetDownloadUrl((string)o[MetadataConsts.DataId]);
                res.Add(o);
            }
            return res;
        }

        #region UserBehaviorService
        public Dictionary<string, object> GetIndexDataByQuery(string grid, object[] queryParams, bool showInTitleList)
        {
            var query = SearchConfig[grid];
            if (!queryParams.IsNullOrEmpty())
            {
                query = query.ToEsQuery(queryParams);
            }
            var hit = SearchService.ESSearch(query).To<EsRoot>().GetSource().FirstOrDefault();
            if (hit != null)
            {
                CheckIndexData(hit, showInTitleList);
            }
            return hit;
        }
        public IEnumerable<Dictionary<string, object>> GetIndexDatasByQuery(string grid, object[] queryParams, bool showInTitleList)
        {
            var query = SearchConfig[grid];
            if (!queryParams.IsNullOrEmpty())
            {
                query = query.ToEsQuery(queryParams);
            }
            return GetIndexDatasByQuery(query, showInTitleList);
        }

        public int GetCountByQuery(string grid, object[] queryParams)
        {
            var query = SearchConfig[grid];
            if (!queryParams.IsNullOrEmpty())
            {
                query = query.ToEsQuery(queryParams);
            }
            var hits = SearchService.ESSearch(query).To<EsRoot>().hits?.total;
            if (hits!=null && hits.HasValue)
            {
                return Convert.ToInt32(hits.Value);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        public IEnumerable<string> GetTargets(string bot)
        {
            return GetBOsByProperties(bot, null, 0, short.MaxValue);
        }
        public IEnumerable<string> GetTargetsByConfig(string grid)
        {
            var query = SearchConfig[grid];
            query = query.ToNormal();
            return GetBOsByProperties(query, null, 0, short.MaxValue);
        }

        //热点
        public IEnumerable<Dictionary<string, object>> GetTopHots(string grid, int topCount)
        {
            var query = SearchConfig[grid];
            query = query.ToEsQuery(topCount.ToString());
            //根据iiid聚合
            var esBuckets = GetGroupUserBehaviorData(query);
            var iiids = esBuckets.Select(e => e.key.ToUpperInvariant()).ToArray();
            return GetIndexDatasByIiids(iiids);
        }
        private EsBuckets GetGroupUserBehaviorData(string esQuery)
        {
            string result = UserBehaviorService.Search(esQuery);
            //提取聚合结果
            string flag = "\"buckets\":";
            int fromIndex = result.IndexOf(flag);
            if (fromIndex == -1)
            {
                return new EsBuckets();
            }
            int length = result.LastIndexOf(']') - fromIndex - flag.Length + 1;
            return result.Substring(fromIndex + flag.Length, length).To<EsBuckets>();
        }
        //最近浏览
        public IEnumerable<Dictionary<string, object>> GetRecentlyView(string grid, string userName, int recentCount)
        {
            var query = SearchConfig[grid];
            var topCount = recentCount * 5;
            query = query.ToEsQuery(new string[] { userName, topCount.ToString() });
            var result = UserBehaviorService.Search(query).To<EsRoot>().GetSource();
            var iiids = result.Select(e => e[MetadataConsts.IIId].ToString()).Distinct().Take(recentCount).ToArray();
            return GetIndexDatasByIiids(iiids);
        }
        private IEnumerable<Dictionary<string, object>> FormatEsDateTimeForUserBehavior(EsRoot esResult)
        {
            var src = esResult.GetSource();
            DateTime date;
            foreach (var o in src)
            {
                date = (DateTime)o["logdate"];
                o["logdate"] = date.ToLocalTime().ToMonthDay();
                o[MetadataConsts.IndexedDate] = date.ToLocalTime().ToMonthDay();
            }
            return src;
        }
        private IEnumerable<Dictionary<string, object>> GetIndexDatasByIiids(string[] iiids)
        {
            var iiidsArray = string.Empty;
            foreach (var iiid in iiids)
            {
                iiidsArray += "\"" + iiid + "\",";
            }
            iiidsArray = iiidsArray.TrimEnd(',');
            //组合查询条件
            var query = "{\"_source\":[\"iiid\", \"dataid\", \"title\", \"abstract\", \"indexeddate\", \"createddate\"]";
            query += ",\"size\":" + iiids.Length.ToString();
            query += ",\"query\": {\"bool\": {\"must\": [{\"terms\": {\"iiid.keyword\": [" + iiidsArray + "]}}]}}}";
            var result = GetIndexDatasByQuery(query, true);
            var result2 = new List<Dictionary<string, object>>();
            foreach (var iiid in iiids)
            {
                var value = result.FirstOrDefault(e => iiid == e[MetadataConsts.IIId].ToString());
                if (value == null) continue;
                result2.Add(value);
            }
            return result2;
        }
        private IEnumerable<Dictionary<string, object>> GetIndexDatasByQuery(string query, bool showInTitleList)
        {
            var hits = SearchService.ESSearch(query).To<EsRoot>().GetSource();
            foreach (var hit in hits)
            {
                CheckIndexData(hit, showInTitleList);
            }
            return hits;
        }
        private void CheckIndexData(Dictionary<string, object> hit, bool showInTitleList)
        {
            if (showInTitleList)
            {
                var dtValue = DateTime.MinValue;
                var value = hit.GetValueBy(MetadataConsts.IndexedDate);
                if (value != null)
                {
                    dtValue = ((DateTime)value).ToLocalTime();
                    hit[MetadataConsts.IndexedDate] = dtValue;
                }
                value = hit.GetValueBy(MetadataConsts.CreatedDate);
                if (value != null)
                {
                    dtValue = CheckDateTime(value);
                    hit[MetadataConsts.CreatedDate] = dtValue;
                }
                if (dtValue > DateTime.MinValue) hit["ShortDate"] = dtValue.ToMonthDay();
                var sAbstract = hit.GetValueBy(MetadataConsts.Abstract)?.ToString();
                if (!sAbstract.IsNullOrEmpty()) hit[MetadataConsts.Title] = sAbstract;
                return;
            }
            var showType = hit.GetValueBy(MetadataConsts.ShowType)?.ToString();
            if (!showType.IsNullOrEmpty())
            {
                var dataId = hit[MetadataConsts.DataId].ToString();
                switch (showType.ToEnum<IndexShowType>())
                {
                    case IndexShowType.Image:
                        //hit["thumbnail"] = GetDownloadUrl(dataId);
                        hit["url"] = hit["resourcekey"] = GetDownloadUrl(dataId);
                        break;
                    case IndexShowType.Table:
                    case IndexShowType.PropertyGrid:
                    case IndexShowType.Html:
                    case IndexShowType.Chart:
                        hit["Content"] = AppDataService.Get(dataId)?.Content;
                        break;
                }
            }
        }
        public static DateTime CheckDateTime(object value)
        {
            if (value is DateTime) return (DateTime)value;
            return value.ToString().ToStandardDateTime();
        }
        public IEnumerable<Dictionary<string, object>> GetNearTargetIndexDatas(string grid1, string grid2, string bo, string bot)
        {
            var query = SearchConfig[grid1];
            var nearRequest = query.ToNearRequest(bo.Trim());
            var bosNear = GetNearBos(nearRequest);
            var result = GetIndexDatasByQuery(grid2, bosNear.ToArray(), true);
            foreach (var hit in result)
            {
                var bos = hit.GetValueBy(bot);
                if (bos == null) continue;
                var bo2 = bos.As<JArray>().First().As<JValue>().Value.ToString();
                var title = hit[MetadataConsts.Title].ToString();
                if (title.IndexOf(bo2, StringComparison.OrdinalIgnoreCase) > -1) continue;
                hit[MetadataConsts.Title] = bo2 + title;
            }
            return result;
        }
        /// <summary>获得全部正钻井</summary>
        private List<string> GetDrillingWells(DateTime today)
        {
            var conditions = new string[2];
            conditions[0] = @"'properties.开钻日期': { $lt: """ + today.ToISODateString() + @"""}";
            conditions[0] = @"'properties.完钻日期':{$nin:[null,'']}";
            return GetBOs("井", null, null, conditions, 0, short.MaxValue);
        }
    }
}
