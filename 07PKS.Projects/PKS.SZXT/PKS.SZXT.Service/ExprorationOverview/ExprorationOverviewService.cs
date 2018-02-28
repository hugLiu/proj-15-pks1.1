using PKS.Core;
using PKS.SZXT.Core.Model;
using PKS.SZXT.Core.Model.EsRawResult;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExprorationOverview;
using PKS.SZXT.Service.Common;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PKS.SZXT.Service.ExprorationOverview
{
    public class ExprorationOverviewService : ViewServiceBase, IExprorationOverviewService, IPerRequestAppService
    {
        //二维地震完成情况
        public object GetCompletion2DSeismic()
        {
            var query = SearchConfig["G4"];
            return GetAppData(query);
        }
        //三维地震完成情况
        public object GetCompletion3DSeismic()
        {
            var query = SearchConfig["G5"];
            return GetAppData(query);
        }
        //钻井完成情况
        public object GetCompletionDrilling()
        {
            var query = SearchConfig["G3"];
            return GetAppData(query);
        }
        //钻井完成情况(进尺)
        public object GetCompletionDrilling_1()
        {
            var query = SearchConfig["G3_1"];
            return GetAppData(query);
        }        
        //项目完成情况
        public object GetCompletionProject()
        {
            var query = SearchConfig["G6"];
            return GetAppData(query);
        }
        //复杂井情况
        public object GetComplicatedWell(int topCount)
        {
            var query = SearchConfig["G2"];
            query = query.ToEsQuery(topCount);
            return GetEsNews(query);
        }
        //地层测试地质日报
        public object GetFormationTestDaily()
        {
            var query = SearchConfig["G15"];
            return GetAppData(query);
        }
        //地层测试求产成果
        public object GetFormationTestYieldResults()
        {
            var query = SearchConfig["G14"];
            return GetAppData(query);
        }
        //最新成果
        public object GetLatestAchievements(int topCount)
        {
            var query = SearchConfig["G8"];
            query = query.ToEsQuery(topCount);
            return GetEsNews(query);
        }
        //最新部署
        public object GetLatestDeployment(int topCount)
        {
            var query = SearchConfig["G9"];
            query = query.ToEsQuery(topCount);
            return GetEsNews(query);
        }
        //录井数据
        public object GetLogging()
        {
            var query = SearchConfig["G13"];
            return GetAppData(query);
        }
        //油气新发现
        public object GetNewDiscovery(int topCount)
        {
            var query = SearchConfig["G1"];
            query = query.ToEsQuery(topCount);
            return GetEsNews(query);
        }
        //项目进展
        public object GetProjectDebriefing(int topCount)
        {
            var query = SearchConfig["G7"];
            query = query.ToEsQuery(topCount);
            return GetEsNews(query);
        }

        //热点
        public object GetTopHots(int topCount)
        {
            return GetTopHots("G10", topCount);
        }

        //最近浏览
        public object GetRecentlyView(string userName, int recentCount)
        {
            return GetRecentlyView("G11", userName, recentCount);
        }

        //地震采集动态
        public object GetSeismicDynamic()
        {
            var query = SearchConfig["G16"];
            return GetAppData(query);
        }
        
        //钻井动态数据
        public object GetWellDynamic()
        {
            var query = SearchConfig["G12"];
            return GetAppData(query);
        }
    }
}




