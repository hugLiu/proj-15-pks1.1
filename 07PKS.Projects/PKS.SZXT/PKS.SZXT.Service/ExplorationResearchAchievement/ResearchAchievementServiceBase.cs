using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PKS.Models;
using PKS.SZXT.Core.Model.EsRawResult;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationResearchAchievement;
using PKS.SZXT.Service.Common;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PKS.SZXT.Service.ExplorationResearchAchievement
{
    public class ResearchAchievementServiceBase: ViewServiceBase
    {
        private UIMetadata ConvertToUIMetadata(Dictionary<string,object> esItem)
        {
            UIMetadata metadta = new UIMetadata();
            if (esItem.ContainsKey("iiid"))
            {
                metadta.IIId = esItem["iiid"].ToString();
            }
            if (esItem.ContainsKey("indexeddate"))
            {
                metadta.IndexedDate = ((DateTime)esItem["indexeddate"]).ToLocalTime();
            }
            if (esItem.ContainsKey("showtype"))
            {
                metadta.ShowType = esItem["showtype"].ToString().ToLower();
            }
            if (esItem.ContainsKey("pageid"))
            {
                metadta.PageId = esItem["pageid"].ToString();
            }
            if (esItem.ContainsKey("dataid"))
            {
                metadta.DataId = esItem["dataid"].ToString();
            }
            if (esItem.ContainsKey("thumbnail"))
            {
                metadta.Thumbnail = esItem["thumbnail"].ToString();
            }
            if (esItem.ContainsKey("title"))
            {
                metadta.Title = esItem["title"].ToString();
            }
            if (esItem.ContainsKey("author"))
            {
                metadta.Author = esItem["author"].ToString();
            }
            if (esItem.ContainsKey("createddate"))
            {
                metadta.CreatedDate = ((DateTime)esItem["createddate"]).ToLocalTime();
            }
            if (esItem.ContainsKey("system"))
            {
                metadta.System = esItem["system"].ToString();
            }
            if (esItem.ContainsKey("resourcetype"))
            {
                metadta.ResourceType = esItem["resourcetype"].ToString();
            }
            if (esItem.ContainsKey("pc"))
            {
                metadta.PC = Convert.ToString(esItem["pc"]);
            }
            if (esItem.ContainsKey("pt"))
            {
                metadta.PT = Convert.ToString(esItem["pt"]);
            }
            return metadta;
        }
        public List<UIMetadata> GetUIMetadatasWithFileData(string query)
        {
            var src = SearchService.ESSearch(query)
                           .To<EsRoot>()
                           .GetSource();
            List<UIMetadata> metadatas = new List<UIMetadata>();
            foreach (var o in src)
            {
                var metadata = ConvertToUIMetadata(o);
                var dataSource = GetFullAPIUrl(metadata.DataId);
                if (metadata.ShowType!=null&&metadata.ShowType!="image"&& metadata.ShowType != "pdf")
                {
                    var content = (AppDataService.Get(metadata.DataId) as PKS.WebAPI.Models.IndexAppData).Content;
                    dataSource = Convert.ToString(content);
                }
                metadata.Datasource = dataSource;
                metadatas.Add(metadata);
            }
            return metadatas;
        }

        private string GetFullAPIUrl(string dataid)
        {
            return ApiServiceConfig.Url + "/appdataservice/download?dataid=" + dataid;
        }

        public IEnumerable<object> GetSearchConfigTreeWithQuantity(string queryKey,params object[] esParams)
        {
            var treeConfig = SearchConfig[queryKey];
            if (!string.IsNullOrEmpty(treeConfig))
            {
                var queryObject = JObject.Parse(treeConfig);
                var treeObjectArray = queryObject["tree"];
                List<QueryTreeConfig> gridConfigs = JsonConvert.DeserializeObject<List<QueryTreeConfig>>(treeObjectArray.ToString());

                foreach (var item in gridConfigs)
                {
                    CalculateTreeQuantity(item, esParams);
                }

                return gridConfigs;
            }
            return null;
        }

        private void CalculateTreeQuantity(QueryTreeConfig parent,params object[] esParams)
        {
            var queryKey = parent.QueryKey;
            if (queryKey != null && SearchConfig.ContainsKey(queryKey))
            {
                var treeQueryObj = JObject.Parse(SearchConfig[queryKey]);
                treeQueryObj["size"] = 0;
               // RemoveQueryParams(treeQueryObj);
                var esRoot = GetEsData(treeQueryObj.ToString().ToEsQuery(esParams));
                parent.Total=esRoot.hits.total.HasValue?Convert.ToInt32(esRoot.hits.total.Value):0;
            }

            if (parent.Children != null)
            {
                var parentTotal = 0;
                for (int i = 0; i < parent.Children.Count(); i++)
                {
                    CalculateTreeQuantity(parent.Children[i], esParams);
                    parentTotal += parent.Children[i].Total;
                }
                parent.Total = parentTotal;
            }
        }


        private void RemoveQueryParams(JToken treeQueryObj)
        {
            var queryObj = treeQueryObj["query"];
            if (queryObj == null)
                return;
            var boolObj= queryObj["bool"];
            if (boolObj == null)
                return;
            var mustQueryObj = boolObj["must"];
            if (mustQueryObj == null)
                return;
            var mustConditions= mustQueryObj as JArray;
            for (int i= mustConditions.Count-1; i >-1; i--)
            {
                for (int j = 0; j < mustConditions[i].Children().Count(); j++)
                {
                    var childTokens = mustConditions[i].Children().ToList();
                    if(childTokens.Any(item=>item.ToString().Contains("@")))
                    {
                        mustConditions.Remove(mustConditions[i]);
                        break;
                    }               
                }
            }
        }
  
        [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
        private class QueryTreeConfig
        {
            [JsonProperty("text")]
            public string Text { get; set; }
            [JsonProperty("tag")]
            public object Tag { get; set; }
            [JsonProperty("_children")]
            public List<QueryTreeConfig> Children { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonIgnore]
            public string QueryKey
            {
                get
                {
                    if (Tag == null)
                        return null;
                    return JObject.Parse(Tag.ToString())["querykey"].ToString();
                }
            }
        }
    }
}
