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
    public class ResearchProjectService : ViewServiceBase, IResearchProject, IPerRequestAppService
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

        /// <summary>
        /// 项目立项
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public object GetProjectApproval(string projectName)
        {
            var query = SearchConfig["G3_1"];
            query = query?.ToEsQuery(projectName);
            return GetEsNews(query);
        }
        /// <summary>
        /// 项目实施
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public object GetProjectImplement(string projectName)
        {
            var query = SearchConfig["G3_2"];
            query = query?.ToEsQuery(projectName);
            return GetEsNews(query);

        }
         /// <summary>
        /// 项目验收
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public object GetProjectAcceptance(string projectName)
        {
            var query = SearchConfig["G3_3"];
            query = query?.ToEsQuery(projectName);
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
