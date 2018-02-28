using PKS.Core;
using PKS.SZZSK.Core.Common;
using PKS.SZZSK.IService.Common;
using PKS.SZZSK.IService.TargetBaiKe;
using PKS.SZZSK.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.Service.TargetBaiKe
{
    public class UnitDataService : ViewServiceBase, IUnitDataService
    {
        public object GetUnitSearchCondition()
        {
            return  GetBOTProperties("G1");
        }

        public IList<string> GetUnitListByName(string UnitName, int? from = 1, int? size = 100)
        {
            List<string> bos = GetBOs("二级构造单元", UnitName, null, null, from, size);
            return bos;
        }

        public IList<string> GetUnitListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100)
        {
            List<string> bos = GetBOs("二级构造单元", null, feature, null, from, size);
            return bos;
        }
    }
}
