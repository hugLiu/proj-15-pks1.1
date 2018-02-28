using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDecision
{
     public interface IAnnualPlanService: IViewService
    {
        //年度物化探采集工作量
        object GetSamplingWorkload(string year);
        //年度地震处理工作量
        object GetProcessingWorkload(string year);
        //年度探井工作量
        object GetWellWorkload(string year);
        //年度勘探主要研究工作量统计表
        object GetMainResearchEffort(string year);
        //年度自营勘探预算执行情况
        object GetExplorationBudget(string year);
        //年度新发现地质储量汇总
        object GetSummaryOfGeoReserves(string year);
        //年度勘探成果图
        object GetExplorationProductionMap(string year);
        //年度勘探成果表
        object GetExplorationProductionTable(string year);
        //勘探年度总结报告
        object GetSummaryReport(string year,int size);
        //勘探部署图
        object GetExplorationDispositionMap(string year);
        //自营勘探部署表
        object GetSelfExplorationDeployment(string year);
        //合作勘探部署表
        object GetCooperativeExplorationDeployment(string year);
        //勘探年度任务书
        object GetTaskBookForExploration(string year,int size);
        //勘探年度计划报告
        object GetAnnualPlanForExploration(string year,int size);
    }
}
