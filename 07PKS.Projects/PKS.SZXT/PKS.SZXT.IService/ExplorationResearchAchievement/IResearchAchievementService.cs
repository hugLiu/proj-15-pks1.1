using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationResearchAchievement
{
    public interface IResearchAchievementService : IViewService
    {
        IEnumerable<object> GetSearchConfigTreeWithQuantity(string queryKey, params object[] esParams);
    }
}
