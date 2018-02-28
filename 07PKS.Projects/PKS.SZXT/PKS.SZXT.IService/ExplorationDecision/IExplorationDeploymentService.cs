using PKS.SZXT.IService.Common;
using PKS.WebAPI;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDecision
{
    public interface IExplorationDeploymentService: IViewService
    {
        //获取bot属性
        List<BOTPropertyDefinition> GetBotProtertyByBot(string bot);
        //根据条件获取bos
        List<string> GetBosByQuery(Dictionary<string,List<string>> query, string bot);
        //获取井位部署图
        object GetWellLocationImg(string bo);
        //获取井位部署表
        object GetWellLocationTable(string bo);
        //获取井场调查报告
        object GetWellsiteSurveyReport(string bo,int size);
        //获取井位建议书
        object GetLocationProposal(string bo,int size);
        //获取地震部署图
        object GetSeismicDeploymentImg(string bo);
        //获取地震采集建议表
        object GetSeismicAcquisitionTable(string bo);
        //获取采集工区调查报告
        object GetSeismicWorkAreaCensus(string bo,int size);
        //获取地震采集处理建议书
        object GetSeismicRecommendation(string bo,int size);
    }
}
