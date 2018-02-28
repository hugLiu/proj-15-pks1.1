using PKS.WebAPI.Services;
using System.Collections.Generic;

namespace PKS.SZXT.IService.Common
{
    public interface IViewService
    {
        Dictionary<string,string> SearchConfig { get; set; }
        IEnumerable<object> GetFragInfoOfBo(IList<string> bo, string grid, IList<int> year);
        IEnumerable<string> GetTargets(string bot);
        IEnumerable<string> GetTargetsByConfig(string grid);
        Dictionary<string, object> GetIndexDataByQuery(string grid, object[] queryParams, bool showAsTitle);
        IEnumerable<Dictionary<string, object>> GetIndexDatasByQuery(string grid, object[] queryParams, bool showAsTitle);
        IEnumerable<Dictionary<string, object>> GetTopHots(string grid, int topCount);
        IEnumerable<Dictionary<string, object>> GetRecentlyView(string grid, string userName, int recentCount);
        IEnumerable<Dictionary<string, object>> GetNearTargetIndexDatas(string grid1, string grid2, string bo, string bot);
        List<string> GetBOsByProperties(string bot, Dictionary<string, List<string>> properties, int? from = 0, int? size = 10);
   }
}
