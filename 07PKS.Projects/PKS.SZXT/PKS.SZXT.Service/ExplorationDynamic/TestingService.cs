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
    public class TestingService : ViewServiceBase, ITestingService, IPerRequestAppService
    {
        public object GetOilGasTesting()
        {
            var query = SearchConfig["g1"];
            return GetEsNews(query);
        }
        public object GetTestingDynamic(DateTime? date)
        {
            var query = SearchConfig["g2"];
            query = query?.ToEsQuery(date.Value.ToEsDate());
            return GetAppDatas(query, true);
        }
        public object GetOilGasDetectGeoDesignReport(string wellId)
        {
            var query = SearchConfig["g3_1"];
            query = query?.ToEsQuery(wellId);
            return GetEsList(query);
        }

        public object GetOilGasDetectProjDesignReport(string wellId)
        {
            var query = SearchConfig["g3_2"];
            query = query?.ToEsQuery(wellId);
            return GetEsList(query);
        }
        public object GetFormationTestingProductData(string wellId)
        {
            var query = SearchConfig["g3_3"];
            query = query?.ToEsQuery(wellId);
            return GetAppData(query);
        }

        public object GetTestingGeoDailyReport(string wellId, DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g3_4"];
            query = query?.ToEsQuery(wellId, dt.ToEsDate());
            return GetEsList(query);
        }

        public object GetWellNearby(string wellId)
        {
            var query = SearchConfig["g3_5"];
            var nearRequest = query?.ToNearRequest(wellId);
            var bos = GetNearBos(nearRequest);
            query = SearchConfig["g3_6"];
            query = query.ToEsQuery(bos);
            return GetEsList(query);
        }
    }
}
