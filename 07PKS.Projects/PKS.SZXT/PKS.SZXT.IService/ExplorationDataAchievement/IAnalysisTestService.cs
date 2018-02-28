using PKS.SZXT.IService.Common;
using System.Collections.Generic;

namespace PKS.SZXT.Service.ExplorationDataAchievement
{
    public interface IAnalysisTestService : IViewService, IUserBehaviorAnalysis
    {
        object GetWellSearchCondition();
        IEnumerable<object> GetAnalysisTestDetailData(string well, string grid);
        object GetExploratoryWellList(Dictionary<string, List<string>> properties, int? from = default(int?), int? size = default(int?));
        object GetExploratoryWellListByName(string wellName, int? from = default(int?), int? size = default(int?));
        List<string> GetNearWells(string wellName);
    }
}