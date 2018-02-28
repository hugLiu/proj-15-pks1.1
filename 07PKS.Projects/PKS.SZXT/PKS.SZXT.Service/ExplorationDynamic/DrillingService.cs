using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService;
using PKS.SZXT.Service.Common;
using System;

namespace PKS.SZXT.Service.ExplorationDynamic
{
    public class DrillingService : ViewServiceBase, IDrillingService, IPerRequestAppService
    {
        public object GetComplexWells()
        {
            var query = SearchConfig["g1"];
            return GetEsNews(query);
        }
        public object GetDynamic(DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g2"];
            query = query?.ToEsQuery(dt.ToEsDate());
            return GetAppData(query);
        }
        public object GetWellBaseForm(string wellId)
        {
            var query = SearchConfig["g3_1"];
            query = query?.ToEsQuery(wellId);
            return GetAppData(query);
        }
        public object GetWellGeoDesignReport(string wellId)
        {
            var query = SearchConfig["g3_2"];
            query = query?.ToEsQuery(wellId);
            return GetEsNews(query);
        }
        public object GetWellProjDesignReport(string wellId)
        {
            var query = SearchConfig["g3_3"];
            query = query?.ToEsQuery(wellId);
            return GetEsNews(query);
        }
        public object GetWellDesignConstruct(string wellId)
        {
            var query = SearchConfig["g3_4"];
            query = query?.ToEsQuery(wellId);
            return GetImageUrl(query);
        }
        public object GetWellActualConstruct(string wellId)
        {
            var query = SearchConfig["g3_5"];
            query = query?.ToEsQuery(wellId);
            return GetImageUrl(query);
        }
        public object GetDrillingProjArgument(string wellId)
        {
            var query = SearchConfig["g3_6"];
            query = query?.ToEsQuery(wellId);
            return GetEsNews(query);
        }
        public object GetWellDailyReport(string wellId, DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g3_7"];
            query = query?.ToEsQuery(wellId,dt.ToEsDate());
            return GetEsNews(query);
        }
        public object GetWellNearby(string wellId)
        {
            var query = SearchConfig["g3_8"];
            var nearRequest = query.ToNearRequest(wellId);
            var bos = GetNearBos(nearRequest);
            query = SearchConfig["g3_9"];
            query = query.ToEsQuery(bos);
            return GetTargetEsNews(query, "well");
        }
    }
}
