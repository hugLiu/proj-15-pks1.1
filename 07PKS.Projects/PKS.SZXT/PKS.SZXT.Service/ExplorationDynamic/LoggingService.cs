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
    public class LoggingService : ViewServiceBase, ILoggingService, IPerRequestAppService
    {
        public object GetLoggingDiscovery()
        {
            var query = SearchConfig["g1"];
            return GetEsNews(query);
        }
        public object GetLoggingDynamic(DateTime? date)
        {
            var dt = date ?? DateTime.Now;
            var query = SearchConfig["g2"];
            query = query?.ToEsQuery(dt.ToEsDate());
            return GetAppDatas(query, true);
        }
        public object GetMontage(string wellId)
        {
            var query = SearchConfig["g3_1"];
            query = query?.ToEsQuery(wellId);
            var data = GetEsNews(query);
            return data;
            //return GetImageUrl(query);
        }
        public object GetLoggingDraft(string wellId)
        {
            var query = SearchConfig["g3_2"];
            query = query?.ToEsQuery(wellId);
            return GetEsNews(query);
            //return GetImageUrl(query);
        }
        public object GetOilGasForm(string wellId)
        {
            var query = SearchConfig["g3_3"];
            query = query?.ToEsQuery(wellId);
            return GetAppData(query);
        }
        public object GetDrillingGeoDailyReport(string wellId, DateTime? date)
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
            return GetTargetEsNews(query, "well");
        }
    }
}
