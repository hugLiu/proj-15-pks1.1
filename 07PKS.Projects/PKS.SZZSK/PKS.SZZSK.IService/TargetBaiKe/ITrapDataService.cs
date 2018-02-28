using PKS.SZZSK.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.TargetBaiKe
{
    public interface ITrapDataService : IViewService
    {

        #region SearchTrap
        object GetTrapSearchCondition();
        IList<string> GetTrapListByName(string TrapName, int? from = 1, int? size = 100);
        IList<string> GetTrapListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100);

        #endregion

        #region Trap



        #endregion

    }
}
