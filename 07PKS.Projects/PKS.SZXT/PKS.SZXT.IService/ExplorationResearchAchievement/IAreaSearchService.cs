using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationResearchAchievement
{
    public interface IAreaSearchService:IViewService
    {
        IEnumerable<object> GetImages(string boid, int year, string grid);
        IEnumerable<object> GetOthers(string boid, int year, string grid);
    }
}
