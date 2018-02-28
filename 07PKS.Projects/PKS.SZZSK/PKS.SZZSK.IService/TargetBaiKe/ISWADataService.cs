using PKS.SZZSK.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.TargetBaiKe
{
    public interface ISWADataService : IViewService
    {

        #region SearchSWA
        object GetSWASearchCondition();
        IList<string> GetSWAListByName(string SWAName, int? from = 1, int? size = 100);
        IList<string> GetSWAListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100);

        #endregion

        #region SWA



        #endregion

    }
}
