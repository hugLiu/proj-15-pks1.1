using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationDynamic;
using PKS.SZXT.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Service.ExplorationDynamic
{
    public class GeophysicalExpService : ViewServiceBase, IGeophysicalExpService, IPerRequestAppService
    {
        public object GetEarthquakeSampling()
        {
            var query = SearchConfig["g1"];
            return GetEsNews(query);
        }
        public object GetSamplingDynamic(DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g2"];
            query = query?.ToEsQuery(dt.ToEsDate());
            return GetAppDatas(query, true);
        }
        public object GetEarthquakeSamplingDesignReport(string swa, DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g3_1"];
            query = query?.ToEsQuery(swa, dt.ToEsDate());
            return GetEsList(query);
        }
        public object GetEarthquakeSamplingBaseForm(string swa, DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g3_2"];
            query = query?.ToEsQuery(swa, dt.ToEsDate());
            return GetAppData(query);
        }
        public object GetEarthquakeSamplingAreaPositionPicture(string swa, DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g3_3"];
            query = query?.ToEsQuery(swa, dt.ToEsDate());
            return GetImageUrl(query);
        }
        public object GetEarthquakeSamplingDailyReport(string swa,DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g3_4"];
            query = query?.ToEsQuery(swa, dt.ToEsDate());
            return GetEsList(query);
        }
    }
}
