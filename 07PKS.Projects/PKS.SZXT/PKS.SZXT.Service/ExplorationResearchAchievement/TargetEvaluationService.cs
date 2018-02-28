using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using PKS.Models;
using PKS.SZXT.Core.Model.EsRawResult;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationResearchAchievement;
using PKS.SZXT.Service.Common;

namespace PKS.SZXT.Service.ExplorationResearchAchievement
{
    class TargetEvaluationService : ViewServiceBase, ITargetEvaluationService
    {
        public object GetTrapProperties()
        {
            var result = this.GetBOTProperties("G1");
            return result;
        }

        public object GetTrapListByName(string trapName, int? from, int? size)
        {
            List<string> bos = this.GetBOsByName("圈闭", trapName, from, size);
            var result = this.GetBOPTList("G2", bos, from, size) as JObject;
            result["total"] = this.GetBOCountByName("圈闭", trapName);
            result["from"] = from;
            result["size"] = size;
            return LeftJoinBos(bos, result);
        }

        public object GetTrapListByProperties(Dictionary<string, List<string>> properties, int? from, int? size)
        {
            List<string> bos = this.GetBOsByProperties("圈闭", properties, from, size);
            var result = this.GetBOPTList("G2", bos, from, size) as JObject;
            result["total"] = this.GetBOCountByProperties("圈闭", properties);
            result["from"] = from;
            result["size"] = size;
            return LeftJoinBos(bos, result);
        }

        public object GetTrapPTByName(string trap)
        {
            return new
            {
                G1_1 = GetTrapPTByName(trap, "G1_1"),
                G1_2 = GetTrapPTByName(trap, "G1_2"),
                G2_1 = GetTrapPTByName(trap, "G2_1"),
                G2_2 = GetTrapPTByName(trap, "G2_2"),
                G3_1 = GetTrapPTByName(trap, "G3_1"),
                G3_2 = GetTrapPTByName(trap, "G3_2"),
                G4_1 = GetTrapPTByName(trap, "G4_1"),
                G4_2 = GetTrapPTByName(trap, "G4_2"),
            };
        }

        public object GetTrapPTByName(string trap,string grid)
        {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery(trap);
            return getEsModels(query);
        }

        /// <summary>
        /// 将bo和boptlist进行左连接补齐
        /// </summary>
        /// <param name="bos"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private JObject LeftJoinBos(IEnumerable<string> bos, JObject result)
        {
            var r2 = from bo in bos
                     join r in result["results"] on bo equals (string)r["trap"] into rgrp
                     from item in rgrp.DefaultIfEmpty(JToken.FromObject(new
                     {
                         trap = bo,
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
                    //jtoken[MetadataConsts.Thumbnail] = "/Content/images/header/logo.png";
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



        private IEnumerable<object> getEsModels(string query)
        {
            var src = SearchService.ESSearch(query)
                                 .To<EsRoot>()
                                 .GetSource();
            foreach (var o in src)
            {
                var dataId =(string) o[MetadataConsts.DataId];
                o["url"] = o[MetadataConsts.ResourceKey] = $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
                o[MetadataConsts.Thumbnail] = $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
                var date = (DateTime)o[MetadataConsts.IndexedDate];
                o[MetadataConsts.IndexedDate] = date.ToLocalTime().ToMonthDay();
            }
            return src;
        }
    }
}
