using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationDynamic;
using PKS.SZXT.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Service.ExplorationDynamic
{
    public class DetectionService : ViewServiceBase, IDetectionService, IPerRequestAppService
    {
        public object GetDetectionInformation()
        {
            return GetIndexDatasByQuery("g1", null, true);
        }

        public object GetDetectionInformation(DateTime date)
        {
            return GetIndexDataByQuery("g2", new string[] { date.ToEsDate() }, false);
        }
        public object GetPrimaryExplationPicture(string wellId)
        {
            var query = SearchConfig["g3_1"];
            query = query.ToEsQuery(wellId);
            return GetImageUrl(query);
        }
        public object GetPrimaryExplationForm(string wellId)
        {
            var query = SearchConfig["g3_2"];
            query = query.ToEsQuery(wellId);
            return GetAppData(query);
        }
        public object GetWellNearby(string wellId)
        {
            var query = SearchConfig["g3_3"];
            var nearRequest = query?.ToNearRequest(wellId);
            var bos = GetNearBos(nearRequest);
            query = SearchConfig["g3_4"];
            query = query.ToEsQuery(bos);
            return GetEsList(query);
        }
    }
}
