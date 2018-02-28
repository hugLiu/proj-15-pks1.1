using PKS.SZZSK.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.TargetBaiKe
{
    public interface IUnitDataService : IViewService
    {

        #region SearchUnit
        object GetUnitSearchCondition();
        IList<string> GetUnitListByName(string UnitName, int? from = 1, int? size = 100);
        IList<string> GetUnitListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100);

        #endregion

        #region Unit



        #endregion

    }
}
