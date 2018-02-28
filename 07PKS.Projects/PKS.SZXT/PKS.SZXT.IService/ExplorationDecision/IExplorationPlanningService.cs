using PKS.SZXT.IService.Common;

namespace PKS.SZXT.IService.ExplorationDecision
{
    public interface IExplorationPlanningService : IViewService
    {
        object GetIndexDataAsNews(string grid, int beginYear, int endYear, int count);
        object GetIndexData(string grid, int beginYear, int endYear);
    }
}
