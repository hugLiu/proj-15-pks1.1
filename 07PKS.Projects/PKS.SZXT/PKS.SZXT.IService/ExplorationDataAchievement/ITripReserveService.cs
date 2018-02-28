using PKS.SZXT.IService.Common;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationDataAchievement
{
    public interface ITripReserveService : IViewService, IUserBehaviorAnalysis
    {
        object GetSummaryImageUrl(string grid);
      
        object GetDetailChart(string year);
        object GetTrapStatisticsTable(string year, string bo, string grid);
        IEnumerable<object> GeTripReserveSummaryData(string well, string grid);
        object GetTripReserveCondition();
        List<string> GetSecondaryStructurals(string bot);

        List<BO2> GetAliasTraps(string bot);

        object GetTrapTable(string trap,string grid);
        IEnumerable<object> GetTrapItems(string trap,string grid);
    }
}
