using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Jurassic.PKS.Service;
using Newtonsoft.Json.Linq;
using PKS.Core;
using PKS.DbServices.SysFrame;
using PKS.DbServices.SysFrame.Model;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.WebAPI.Controllers
{
    /// <summary>搜索服务控制器</summary>
    public class SearchServiceController : PKSApiController
    {
        /// <summary>构造函数</summary>
        public SearchServiceController(ISearchService service)
        {
            ServiceImpl = service;
        }

        /// <summary>服务实例</summary>
        private ISearchService ServiceImpl { get; }

        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "搜索服务用于搜索索引库提供索引数据"
            };
        }
        /// <summary>按短语搜索</summary>
        [HttpPost]
        public async Task<SearchResult> Search(SearchRequest request)
        {
            request.Sentence = HttpUtility.UrlDecode(request.Sentence);
            return await ServiceImpl.SearchAsync(request);
        }

        /// <summary>按ES语法搜索</summary>
        [HttpPost]
        public async Task<object> ESSearch()
        {
            var query = await this.Request.Content.ReadAsStringAsync();
            if (query.IsNullOrEmpty())
            {
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺少请求参数！", "请求内容应是ES搜索条件！");
            }
            var filter = GetRoleDataRightFilter(query);
            var searchResult = await ServiceImpl.ESSearchAsync(filter.ToJson());

            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(searchResult, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return result;
        }
        /// <summary>按ES语法搜索[多结果]</summary>
        [HttpPost]
        public async Task<object> ESMSearch()
        {
            var query = await this.Request.Content.ReadAsStringAsync();
            if (query.IsNullOrEmpty())
            {
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺少请求参数！", "请求内容应是ES搜索条件！");
            }
            return await ServiceImpl.ESMSearchAsync(query);
        }
        /// <summary>按完全匹配条件搜索</summary>
        [HttpPost]
        public async Task<MatchResult> Match(MatchRequest request)
        {
            return await ServiceImpl.MatchAsync(request);
        }
        /// <summary>按多个完全匹配条件搜索</summary>
        [HttpPost]
        public async Task<MatchResult[]> MatchMany(MatchRequest[] requests)
        {
            return await ServiceImpl.MatchManyAsync(requests);
        }
        /// <summary>根据iiid搜索</summary>
        [HttpPost]
        public async Task<Metadata> GetMetadata(SearchMetadataRequest request)
        {
            return await ServiceImpl.GetMetadataAsync(request);
        }
        /// <summary>根据iiid数组搜索</summary>
        [HttpPost]
        public async Task<MetadataCollection> GetMetadatas(SearchMetadatasRequest request)
        {
            return await ServiceImpl.GetMetadatasAsync(request);
        }
        /// <summary>根据聚合条件获取统计信息</summary>
        [HttpPost]
        public async Task<SearchStatisticsResult> Statistics(SearchStatisticsRequest request)
        {
            return await ServiceImpl.StatisticsAsync(request);
        }
        /// <summary>查询元数据定义信息</summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MetadataDefinition[]> GetMetadataDefinitions()
        {
            return await ServiceImpl.GetMetadataDefinitionsAsync();
        }

        /// <summary>
        /// 只返回source
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> ESSearchEx(object query)
        {
            if (query!=null&&query is string)
            {
                query = GetRoleDataRightFilter(query.ToString());
            }
            else if(query!=null)
            {
                query = GetRoleDataRightFilter(query.ToJson());
            }
            return await ServiceImpl.ESSearchExAsync(query);
        }

        /// <summary>
        /// 添加角色权限过滤
        /// </summary>
        /// <returns></returns>
        private JObject GetRoleDataRightFilter(string originalEsCondition)
        {
            var originalEsConditionObj = JObject.Parse(originalEsCondition);
            var roleMetadataPermissionService = GetService<RoleMetadataPermissionService>();
            var metadataItemPermissions =
                roleMetadataPermissionService.GetMetadataItemPermissions(
                    PKSUser.Roles.Select(item => Convert.ToInt32(item.Id)).ToArray());
            if (!metadataItemPermissions.Any())
                return originalEsConditionObj;
            var ableItemPermissionEsFilters = GetEsPermissionFilter(metadataItemPermissions.Where(item => item.IsAble));
            var notAbleItemPermissionEsFilters = GetEsPermissionFilter(metadataItemPermissions.Where(item => !item.IsAble));
            if (!ableItemPermissionEsFilters.Any() && !notAbleItemPermissionEsFilters.Any())
            {
                return originalEsConditionObj;
            }
       
            if (originalEsConditionObj != null)
            {
                JToken queryToken = originalEsConditionObj["query"];
                if (queryToken == null)
                {
                    queryToken = JObject.Parse("{}");
                    originalEsConditionObj["query"] = queryToken;
                }
            

                JToken boolQueryToken = queryToken["bool"];
                if (boolQueryToken == null)
                {
                    boolQueryToken = JObject.Parse("{}");
                    queryToken["bool"] = boolQueryToken;
                }
                //白名单
                if (ableItemPermissionEsFilters.Any())
                {
                    var mustToken = boolQueryToken["must"] as JArray;
                    if (mustToken == null)
                    {
                        mustToken = new JArray();
                        boolQueryToken["must"] = mustToken;
                    }
                    var mustTokenObj = mustToken;
                    ableItemPermissionEsFilters.ForEach(item => mustTokenObj.Add(JObject.Parse(item)));
                }
                //黑名单
                if (notAbleItemPermissionEsFilters.Any())
                {
                    var mustNotToken = boolQueryToken["must_not"] as JArray;
                    if (mustNotToken == null)
                    {
                        mustNotToken = new JArray();
                        boolQueryToken["must_not"] = mustNotToken;
                    }
                    var mustNotTokenObj = mustNotToken;
                    notAbleItemPermissionEsFilters.ForEach(item => mustNotTokenObj.Add(JObject.Parse(item)));
                }

                //将query下非bool的属性移到must下
                var queryChildren = queryToken.Children();
                if (queryChildren.Any())
                {
                    for (var i = queryChildren.LongCount() - 1; i > -1; i--)
                    {
                        var jProperty = queryChildren.ElementAt(Convert.ToInt32(i)) as JProperty;
                        if (jProperty.Name != "bool")
                        {
                            if (boolQueryToken["must"] == null)
                                boolQueryToken["must"] = new JArray();
                            (boolQueryToken["must"] as JArray).Add(JObject.Parse("{"+jProperty.ToString()+"}"));
                            jProperty.Remove();
                        }
                    }
                }
            }
            return originalEsConditionObj;
        }

        private List<string> GetEsPermissionFilter(IEnumerable<RoleMetadataItemPermission> itemPermissions)
        {
            List<string> termsFilter = new List<string>();
            List<string> esTags = itemPermissions.Select(item => item.MetadataName).Distinct().ToList();
            if (esTags.Any())
            {
                for (int i = 0; i < esTags.Count; i++)
                {
                    var esTag = esTags[i];
                    //todo 如果为非字符串形式，不需要加引号
                    //var firstItemPermission = itemPermissions.FirstOrDefault(item => item.MetadataName == esTag);
                    //var metadataType = firstItemPermission.MetaDataType;
                    //todo 若为日期数据，处理日期
                    var esValues = string.Join(",",
                        itemPermissions.Where(item => item.MetadataName == esTag)
                            .Select(item => "\"" + item.MetadataItemName + "\""));
                    termsFilter.Add("{\"terms\":{\"" + esTag + ".keyword\":[" + esValues + "]}}");
                  
                }
                return termsFilter;
            }
            return termsFilter;
        }
    }
}
