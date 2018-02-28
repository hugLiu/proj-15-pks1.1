using PKS.SZZSK.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.ExplorationKB
{
    public interface IExplorationKBService : IViewService
    {
        // 获取图片新闻
        object GetPicNews(int topCount = 8);
        // 获取勘探目标认识
        object GetTargetBaiKe(int topCount = 8);
        // 获取勘探专业研究
        object GetProfessionalStudies(int topCount = 8);
        // 获取最新研究成果
        object GetLatestResearchResults(int topCount = 8);
        // 获取石油百科词条
        object GetOilWiKi(int topCount = 8);
        // 获取石油标准规范
        object GetOilStandard(int topCount = 8);
        // 获取最近更新百科
        object GetLatestWiKi(int topCount = 8);
    }
}
