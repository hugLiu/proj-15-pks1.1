using PKS.SZXT.Core.Model.EsRawResult;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExprorationOverview;
using PKS.SZXT.Service.Common;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.SZXT.IService.ExplorationDataAchievement;
using Newtonsoft.Json.Linq;
using PKS.Models;

namespace PKS.SZXT.Service.ExplorationDataAchievement
{
    public class AnalysisTestService : ViewServiceBase, IAnalysisTestService
    {
        /// <summary>
        /// 点击搜索框查询按钮的服务[搜索]
        /// </summary>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public object GetExploratoryWellListByName(string wellName, int? from = null, int? size = null)
        {
            //List<string> bos = this.GetBOsByName("井", wellName, from, size);
            string[] conditions = { @"'properties.完钻日期':{$nin:[null,'']}" };
            List<string> bos = this.GetBOs("井", wellName, null, conditions, from, size);
            var result = this.GetBOPTList("G3", bos, from, size) as JObject;
            result["total"] = bos.Count;// this.GetBOCountByName("井", wellName);
            result["from"] = from;
            result["size"] = size;
            return LeftJoinBos(bos, result);
        }
        /// <summary>
        /// 将bo和bobtlist进行左连接补齐
        /// </summary>
        /// <param name="bos"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private JObject LeftJoinBos(IEnumerable<string> bos, JObject result)
        {
            var r2 = from bo in bos
                     join r in result["results"] on bo equals (string)r["well"] into rgrp
                     from item in rgrp.DefaultIfEmpty(JToken.FromObject(new
                     {
                         well = bo,
                         title = bo,
                         iiid = bo,

                     }))
                     select item;

            JArray jarr = new JArray();

            foreach (var jtoken in r2)
            {
                var base64img = (string)jtoken[MetadataConsts.Thumbnail];
                if (String.IsNullOrWhiteSpace(base64img))
                {
                    jtoken[MetadataConsts.Thumbnail] = "/Content/images/header/logo.png";
                }
                else
                {
                    jtoken[MetadataConsts.Thumbnail] = "data:image/png;base64," + base64img;
                }
                jarr.Add(jtoken);
            }

            result["results"] = jarr;
            return result;
        }

        /// <summary>
        /// 根据搜索条件查询探井列表[查询]
        /// </summary>
        /// <param name="requestCondition"></param>
        /// <returns></returns>
        public object GetExploratoryWellList(Dictionary<string, List<string>> properties, int? from = null, int? size = null)
        {
            //List<string> bos = this.GetBOsByProperties("井", properties, from, size);
            string[] conditions = { @"'properties.完钻日期':{$nin:[null,'']}" };
            List<string> bos = this.GetBOs("井", null, properties, conditions, from, size);
            var result = this.GetBOPTList("G3", bos, from, size) as JObject;
            result["total"] = bos.Count;
            result["from"] = from;
            result["size"] = size;
            return LeftJoinBos(bos, result);
        }


        public IEnumerable<object> GetAnalysisTestDetailData(string well, string grid)
        {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery(well);
            return GetEsList(query);
        }

        /// <summary>
        /// 获取临井
        /// </summary>
        /// <returns></returns>
        public List<string> GetNearWells(string wellName)
        {
            string query = ReplaceParameters("G40", wellName);
            var jObject = JObject.Parse(query);
            NearRequest request = new NearRequest();
            request.BO = jObject["bo"].ToString();
            request.BOT = jObject["bot"].ToString();
            request.Distince = Convert.ToDecimal(jObject["distince"].ToString());
            request.Top = Convert.ToInt32(jObject["top"].ToString());
            return GetNearBos(request) ?? new List<string>();
        }

        public object GetWellSearchCondition()
        {
            var result = this.GetBOTProperties("G1");
            return result;
        }


        public object GetTopHots(int topCount)
        {
            throw new NotImplementedException();
        }

        public object GetRecentlyView(string userName, int recentCount)
        {
            throw new NotImplementedException();
        }
    }

}
