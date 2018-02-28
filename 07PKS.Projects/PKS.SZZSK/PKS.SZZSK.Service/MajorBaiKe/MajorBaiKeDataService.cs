using PKS.Core;
using PKS.SZZSK.IService.MajorBaiKe;
using PKS.SZZSK.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.WebAPI.Models;

namespace PKS.SZZSK.Service.MajorBaiKe
{
    public class MajorBaiKeDataService : ViewServiceBase, IPerRequestAppService, IMajorBaiKeDataService
    {
        public BOT GetBot(string bot)
        {
            return BO2Service.GetBOT(bot);
        }

        public List<BOT> GetBots()
        {
            FilterRequest request = new FilterRequest
            {
                Query = new {},
                Fields = new { },
                Sort = new { }
            };
            return BO2Service.FilterBOTs(request);
        }

        public List<string> GetBoListByFeature(string bot, Dictionary<string, List<string>> feature, int? from = 1, int? size = 100)
        {
            List<string> bos = GetBOs(bot, null, feature, null, from, size);
            return bos;
        }

        public IList<string> GetMajorListByName(string name, string type, int? from = 1, int? size = 100)
        {
            throw new NotImplementedException();
        }

        public object GetSearchCondition(string grid)
        {
            return GetBOTProperties(grid);
        }

        public List<string> GetBoListByName(string bot, string bo, int? from = 1, int? size = 100)
        {
            return GetBOs(bot, bo, null, null, from, size);
        }
    }
}
