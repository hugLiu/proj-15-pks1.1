namespace PKS.SZZSK.IService.Common
{
    public interface IUserBehaviorAnalysis
    {
        object GetTopHots(int topCount);
        object GetRecentlyView(string userName, int recentCount);
    }
}
