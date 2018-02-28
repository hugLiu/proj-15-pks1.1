using PKS.Core;
using PKS.SZZSK.Core.Common;
using PKS.SZZSK.IService.ExplorationKB;
using PKS.SZZSK.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.Service.ExplorationKB
{
    public class ExplorationKBService : ViewServiceBase, IPerRequestAppService, IExplorationKBService
    {
        // 获取图片新闻
        public object GetPicNews(int topCount = 8)
        {
            return GetAppData(SearchConfig["G1"].ToEsQuery(topCount));
        }

        // 获取勘探目标认识
        public object GetTargetBaiKe(int topCount = 8)
        {
            return GetAppData(SearchConfig["G2"].ToEsQuery(topCount));
        }

        // 获取勘探专业研究
        public object GetProfessionalStudies(int topCount = 8)
        {
            return GetAppData(SearchConfig["G3"].ToEsQuery(topCount));
        }

        // 获取最新研究成果
        public object GetLatestResearchResults(int topCount = 8)
        {
            return GetAppData(SearchConfig["G4"].ToEsQuery(topCount));
        }

        // 获取石油百科词条
        public object GetOilWiKi(int topCount = 8)
        {
            return GetAppData(SearchConfig["G5"].ToEsQuery(topCount));
        }

        // 获取石油标准规范
        public object GetOilStandard(int topCount = 8)
        {
            return GetAppData(SearchConfig["G6"].ToEsQuery(topCount));
        }

        // 获取最近更新百科
        public object GetLatestWiKi(int topCount = 8)
        {
            return GetAppData(SearchConfig["G7"].ToEsQuery(topCount));
        }


    }
}
