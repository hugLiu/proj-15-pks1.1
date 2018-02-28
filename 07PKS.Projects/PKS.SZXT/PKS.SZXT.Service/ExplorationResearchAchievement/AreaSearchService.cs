using System;
using System.Collections.Generic;
using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationResearchAchievement;
using PKS.SZXT.Service.Common;

namespace PKS.SZXT.Service.ExplorationResearchAchievement
{
    public class AreaSearchService : ViewServiceBase,IAreaSearchService, IPerRequestAppService
    {
        public IEnumerable<object> GetImages(string boid, int year, string grid)
        {
            var query = SearchConfig[grid];
            query = query.ToEsQuery(boid, year);
            return GetImageList(query);
        }
        public IEnumerable<object> GetOthers(string boid, int year, string grid)
        {
            var query = SearchConfig[grid];
            query = query.ToEsQuery(boid, year);
            return GetEsNews(query);
        }

    }
}
