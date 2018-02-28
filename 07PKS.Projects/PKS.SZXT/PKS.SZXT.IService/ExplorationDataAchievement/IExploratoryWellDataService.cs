using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDataAchievement
{
    public interface IExploratoryWellDataService : IViewService, IUserBehaviorAnalysis
    {
        //根据井名和grid名字获取list类型的列表
        IEnumerable<object> GetExploratoryWellDetailData(string well, string grid);

        //获取图片
        object GetImageData(string well, string grid);
        //获取表
        object GetTableData(string well, string grid);
        //获取html片段
        object GetHtmlData(string well, string grid);

        //获取年度探井统计图数据
        object GetAnnualExploratoryWellStatistics(string year);
        //获取年度探井统计汇总表
        object GetAnnualExploratoryWellStatisticsTable(string year);
        //获取探井统计搜索条件
        object GetWellSearchCondition();
        //根据井名获取探井列表
        object GetExploratoryWellListByName(string wellName, int? from = null, int? size = null);
        //根据搜索条件查询探井列表
        object GetExploratoryWellList(Dictionary<string, List<string>> properties, int? from = null, int? size = null);
        /// <summary>
        /// 临井
        /// </summary>
        /// <param name="wellName"></param>
        /// <returns></returns>
        List<string> GetNearWells(string wellName);
    }
}
