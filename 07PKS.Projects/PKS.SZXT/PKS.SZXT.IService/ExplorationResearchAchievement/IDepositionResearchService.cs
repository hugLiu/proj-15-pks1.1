using PKS.SZXT.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDecision
{
    public interface IDepositionResearchService: IViewService
    {
        IEnumerable<object> GetImages(string boid, string year, string grid);
        IEnumerable<object> GetOthers(string boid, string year, string grid);

        object GetSearchFilters(string bot);
    }
}
