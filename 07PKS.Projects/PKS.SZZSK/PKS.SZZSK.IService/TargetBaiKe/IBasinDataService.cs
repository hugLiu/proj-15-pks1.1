using PKS.SZZSK.IService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.IService.TargetBaiKe
{
    public interface IBasinDataService : IViewService
    {

        #region SearchBasin
        object GetBasinSearchCondition();
        IList<string> GetBasinListByName(string BasinName, int? from = 1, int? size = 100);
        IList<string> GetBasinListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100);

        #endregion

        #region Basin



        #endregion

    }
}
