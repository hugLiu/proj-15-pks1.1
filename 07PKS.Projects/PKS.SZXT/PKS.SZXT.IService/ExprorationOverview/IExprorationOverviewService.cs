using PKS.SZXT.Core.DTO;
using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExprorationOverview
{
    public interface IExprorationOverviewService:IViewService,IUserBehaviorAnalysis
    {
        //获取油气新发现数据
        object GetNewDiscovery(int topCount);
        //获取复杂井情况数据
        object GetComplicatedWell(int topCount);
        //获取钻井完成情况统计图数据
        object GetCompletionDrilling();
        //获取钻井完成情况统计图数据(进尺)
        object GetCompletionDrilling_1();
        //获取二维地震完成情况统计图数据
        object GetCompletion2DSeismic();
        //获取三维地震完成情况统计图数据
        object GetCompletion3DSeismic();
        //获取项目完成情况 统计图数据
        object GetCompletionProject();
        //获取钻井地质作业动态表数据
        object GetWellDynamic();
        //获取录井原始数据表数据
        object GetLogging();
        //获取地层测试求产成果数据
        object GetFormationTestYieldResults();
        //获取地层测试作业日报表
        object GetFormationTestDaily();
        //获取地震资料采集作业动态表数据
        object GetSeismicDynamic();
        //获取项目进展
        object GetProjectDebriefing(int topCount);
        //获取最新成果
        object GetLatestAchievements(int topCount);
        //获取最新部署
        object GetLatestDeployment(int topCount);
        

    }
}
