using PKS.Core;
using PKS.SZXT.Core.Model;
using PKS.SZXT.Core.Model.EsRawResult;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ProjectDynamic;
using PKS.SZXT.Service.Common;
using PKS.WebAPI.Services;
using System;

namespace PKS.SZXT.Service.ExplorationDynamic
{
    public class OperationProjectService : ViewServiceBase, IOperationProject, IPerRequestAppService
    {
        public object GetProjectHeadlines(int topCount = 8)
        {
            var query = SearchConfig["G1"];
            query = query?.ToEsQuery(topCount);
            return GetEsNews(query);
        }

        public object GetProjectProgress()
        {
            var query = SearchConfig["G2"];
            return GetAppData(query);
        }

        public object GetProjectProgress2()
        {
            var query = SearchConfig["G3"];
            return GetAppData(query);
        }

        public object GetProjectManagement()
        {
            var query = SearchConfig["G4"];
            query = query?.ToEsQuery();
            return GetEsNews(query);
        }
        //热点
        public object GetTopHots(int topCount)
        {
            return GetTopHots("G5", topCount);
        }

        //最近浏览
        public object GetRecentlyView(string userName, int recentCount)
        {
            return GetRecentlyView("G6", userName, recentCount);
        }
    }
}
