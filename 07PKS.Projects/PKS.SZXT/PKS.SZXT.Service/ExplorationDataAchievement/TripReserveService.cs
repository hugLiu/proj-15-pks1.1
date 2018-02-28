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
    public class TripReserveService : ViewServiceBase, ITripReserveService
    {
        /// <summary>
        /// 获取明细图
        /// </summary>
        /// <param name="target"></param>
        /// <param name="year"></param>
        /// <param name="queryKey"></param>
        /// <returns></returns>
        public object GetDetailChart(string year)
        {
            var query = SearchConfig["G11"];
            query = query?.ToEsQuery(year);
            return GetEsModels(query).FirstOrDefault();
        }
        /// <summary>
        /// 圈闭储备汇总表
        /// </summary>
        /// <returns></returns>
        public object GetTrapStatisticsTable(string year, string bo,string grid)
        {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery(bo,year);
            return GetAppData(query);

        }

        public object GetSummaryImageUrl(string grid)
        {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery();
            return GetEsModels(query).FirstOrDefault();
        }

        private IEnumerable<object> GetEsModels(string query)
        {
            var src = SearchService.ESSearch(query)
                                 .To<EsRoot>()
                                 .GetSource();
            foreach (var o in src)
            {
                var dataId = o[MetadataConsts.DataId];
                o["url"] = o[MetadataConsts.ResourceKey] = $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
                o[MetadataConsts.Thumbnail] = $"{ApiServiceConfig.Url}appdataservice/download?dataid={dataId}";
                var date = (DateTime)o[MetadataConsts.IndexedDate];
                o[MetadataConsts.IndexedDate] = date.ToLocalTime().ToMonthDay();
            }
            return src;
        }
        public IEnumerable<object> GeTripReserveSummaryData(string well, string grid)
        {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery(well);
            return GetEsList(query);
        }      

        public object GetTripReserveCondition()
        {
            var result = this.GetBOTProperties("G11");
            return result;
        }

        /// <summary>
        /// 获取二级构造单元目标对象
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="properties"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<string> GetSecondaryStructurals(string bot)
        {
            var secondaryStructurals = GetBOsByProperties(bot, null,null,null);
            return secondaryStructurals;
        }

        public List<BO2> GetAliasTraps(string bot)
        {
            return this.GetAliasBOs(bot, null, null, null);
        }

        public object GetRecentlyView(string userName, int recentCount)
        {
            throw new NotImplementedException();
        }

        public object GetTopHots(int topCount)
        {
            throw new NotImplementedException();
        }

        public object GetTrapTable(string trap,string grid)
        {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery(trap);
            return GetAppData(query);
        }

        public IEnumerable<object> GetTrapItems(string trap,string grid) {
            var query = SearchConfig[grid];
            query = query?.ToEsQuery(trap);
            return GetEsList(query);
        }
    }
}
