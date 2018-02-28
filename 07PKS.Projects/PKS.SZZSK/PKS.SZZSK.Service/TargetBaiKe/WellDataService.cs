using Newtonsoft.Json.Linq;
using PKS.Core;
using PKS.SZZSK.Core.Common;
using PKS.SZZSK.IService.Common;
using PKS.SZZSK.IService.TargetBaiKe;
using PKS.SZZSK.Service.Common;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZZSK.Service.TargetBaiKe
{
    public class WellDataService : ViewServiceBase, IWellDataService
    {
        public object GetWellSearchCondition()
        {
            return  GetBOTProperties("G1");
        }

        public IList<string> GetWellListByName(string wellName, int? from = 1, int? size = 100)
        {
            //string[] conditions = { @"'properties.完钻日期':{$nin:[null,'']}" };
            List<string> bos = GetBOs("井", wellName, null, null, from, size);
            return bos;
        }

        public IList<string> GetWellListByFeature(Dictionary<string, List<string>> feature, int? from = 1, int? size = 100)
        {
            List<string> bos = GetBOs("井", null, feature, null, from, size);
            return bos;
        }
        
    }
}
