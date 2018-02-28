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
    public class BasinDataService : ViewServiceBase, IBasinDataService
    {
        public object GetBasinSearchCondition()
        {
            return  GetBOTProperties("G1");
        }

        public IList<string> GetBasinListByName(string BasinName, int? from = 1, int? size = 100)
        {
            List<string> bos = GetBOs("盆地", BasinName, null, null, from, size);
            return bos;
        }

        public IList<string> GetBasinListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100)
        {
            List<string> bos = GetBOs("盆地", null, feature, null, from, size);
            return bos;
        }
    }
}
