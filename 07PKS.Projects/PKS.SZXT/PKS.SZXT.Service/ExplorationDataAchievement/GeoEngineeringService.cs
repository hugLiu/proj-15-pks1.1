using PKS.SZXT.IService.ExplorationDataAchievement;
using PKS.SZXT.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;
using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.Models;

namespace PKS.SZXT.Service.ExplorationDataAchievement
{
    public class GeoEngineeringService : ViewServiceBase, IGeoEngineeringService
    {
        /// <summary>
        /// 获取筛选项
        /// </summary>
        /// <returns></returns>
        public object GetSWAProperties()
        {
            var result = this.GetBOTProperties("G1");
            return result;
        }

        private JObject Scan(JObject result)
        {
            foreach (var jtoken in result["results"])
            {
                //处理日期
                var jdate = jtoken[MetadataConsts.IndexedDate];
                if (jdate != null)
                {
                    var date = (DateTime)jdate;
                    jtoken[MetadataConsts.IndexedDate] = date.ToLocalTime().ToMonthDay();
                }

                //处理缩略图
                var base64img = (string)jtoken[MetadataConsts.Thumbnail];
                if (String.IsNullOrWhiteSpace(base64img))
                {
                    //jtoken[MetadataConsts.Thumbnail] = "/Content/images/header/logo.png";
                }
                else
                {
                    jtoken[MetadataConsts.Thumbnail] = "data:image/png;base64," + base64img;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取物探化详情
        /// </summary>
        /// <param name="swa"></param>
        /// <returns></returns>
        public object GetSWAPTByName(string swa)
        {
            //    var g1 = this.GetAppData("G1", swa);
            var g1 = this.GetBOPTList("G1", new List<string> { swa }, 0, 1) as JObject;
            var g2_1 = this.GetBOPTList("G2-1", new List<string> { swa }, 0, 20) as JObject;
            var g2_2 = this.GetBOPTList("G2-2", new List<string> { swa }, 0, 20) as JObject;
            var g2_3 = this.GetBOPTList("G2-3", new List<string> { swa }, 0, 20) as JObject;
            var g2_4 = this.GetBOPTList("G2-4", new List<string> { swa }, 0, 20) as JObject;
            var g3_1 = this.GetBOPTList("G3-1", new List<string> { swa }, 0, 20) as JObject;
            var g3_2 = this.GetBOPTList("G3-2", new List<string> { swa }, 0, 20) as JObject;
            Scan(g1);
            Scan(g2_1);
            Scan(g2_2);
            Scan(g2_3);
            Scan(g2_4);
            Scan(g3_1);
            Scan(g3_2);
            if (g1["results"].Count() > 0)
            {
                var result = g1["results"][0];
                var dataId = result[MetadataConsts.DataId];
                result["url"] = result["src"] = $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
                result[MetadataConsts.Thumbnail] = $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
            }
            return new
            {
                g1,
                g2_1,
                g2_2,
                g2_3,
                g2_4,
                g3_1,
                g3_2,
            };
        }

        /// <summary>
        /// 物探化点击搜索框查询按钮的服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public object GetSWAListByName(string name, int from, int size)
        {
            List<string> bos = this.GetBOsByName("地震工区", name, from, size);
            var result = this.GetBOPTList("G3", bos, from, size) as JObject;
            result["total"] = this.GetBOCountByName("地震工区", name);
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
                     join r in result["results"] on bo equals (string)r["swa"] into rgrp
                     from item in rgrp.DefaultIfEmpty(JToken.FromObject(new
                     {
                         swa = bo,
                         title = bo,// + "工区位置图",
                         iiid = bo,

                     }))
                     select item;

            JArray jarr = new JArray();
            foreach (var r in r2)
            {
                jarr.Add(r);
            }
            result["results"] = jarr;
            return Scan(result);
        }

        /// <summary>
        /// 物探化点击筛选按钮的服务
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public object GetSWAListByProperties(Dictionary<string, List<string>> properties, int? from = null, int? size = null)
        {
            List<string> bos = this.GetBOsByProperties("地震工区", properties, from, size);
            var result = this.GetBOPTList("G3", bos, from, size) as JObject;
            result["total"] = this.GetBOCountByProperties("地震工区", properties);
            result["from"] = from;
            result["size"] = size;
            return LeftJoinBos(bos, result);
        }

        public object GetSWAListByName(string name, int? from, int? size)
        {
            throw new NotImplementedException();
        }
    }
}
