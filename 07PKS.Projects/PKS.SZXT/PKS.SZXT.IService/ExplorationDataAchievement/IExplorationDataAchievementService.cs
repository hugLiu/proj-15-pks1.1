using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDataAchievement
{
    /// <summary>
    /// 物探化工程查询接口
    /// </summary>
    public interface IExplorationDataAchievementService: IViewService
    {
        object GetAppDataByQuery(string g, params object[] p);
        IEnumerable<object> GetEsNews(string g, int count);
    }
}
