using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationDecision;
using PKS.SZXT.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Service.ExplorationDecision
{
    public class AnnualPlanService : ViewServiceBase, IPerRequestAppService, IAnnualPlanService
    {
        public object GetAnnualPlanForExploration(string year, int size)
        {
            var query = SearchConfig["G14"];
            query = query?.ToEsQuery(year,size);
            return GetEsNews(query);
        }

        public object GetCooperativeExplorationDeployment(string year)
        {
            var query = SearchConfig["G12"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetSelfExplorationDeployment(string year)
        {
            var query = SearchConfig["G11"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetExplorationBudget(string year)
        {
            var query = SearchConfig["G5"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetExplorationDispositionMap(string year)
        {
            var query = SearchConfig["G10"];
            query = query?.ToEsQuery(year);
            return GetImageUrl(query);
        }

        public object GetExplorationProductionMap(string year)
        {
            return GetIndexDatasByQuery("G7", new string[] { year}, false);
        }

        public object GetExplorationProductionTable(string year)
        {
            var query = SearchConfig["G8"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetMainResearchEffort(string year)
        {
            var query = SearchConfig["G4"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetProcessingWorkload(string year)
        {
            var query = SearchConfig["G2"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetSamplingWorkload(string year)
        {
            var query = SearchConfig["G1"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

      

        public object GetSummaryOfGeoReserves(string year)
        {
            var query = SearchConfig["G6"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }

        public object GetSummaryReport(string year, int size)
        {
            var query = SearchConfig["G9"];
            query = query?.ToEsQuery(year, size);
            return GetEsNews(query);
        }

        public object GetTaskBookForExploration(string year, int size)
        {
            var query = SearchConfig["G13"];
            query = query?.ToEsQuery(year, size);
            return GetEsNews(query);
        }

        public object GetWellWorkload(string year)
        {
            var query = SearchConfig["G3"];
            query = query?.ToEsQuery(year);
            return GetAppDataEx(query);
        }
    }
}
