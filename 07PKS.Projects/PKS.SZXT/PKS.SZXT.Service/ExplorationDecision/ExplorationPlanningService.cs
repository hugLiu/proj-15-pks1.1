using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationDecision;
using PKS.SZXT.Service.Common;

namespace PKS.SZXT.Service.ExplorationDecision
{
    public class ExplorationPlanningService : ViewServiceBase, IExplorationPlanningService, IPerRequestAppService
    {
        public object GetIndexDataAsNews(string grid, int beginYear, int endYear, int count)
        {
            return GetIndexDatasByQuery(grid, new string[] { beginYear.ToString(), endYear.ToString(), count.ToString() }, true);
        }
        public object GetIndexData(string grid, int beginYear, int endYear)
        {
            return GetIndexDataByQuery(grid, new string[] { beginYear.ToString(), endYear.ToString() }, false);
        }
    }
}
