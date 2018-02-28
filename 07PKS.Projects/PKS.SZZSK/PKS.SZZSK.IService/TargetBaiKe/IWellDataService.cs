using PKS.SZZSK.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.TargetBaiKe
{
    public interface IWellDataService : IViewService
    {

        #region SearchWell
        object GetWellSearchCondition();
        IList<string> GetWellListByName(string wellName, int? from = 1, int? size = 100);
        IList<string> GetWellListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100);

        #endregion

        #region Well


        #endregion

    }
}
