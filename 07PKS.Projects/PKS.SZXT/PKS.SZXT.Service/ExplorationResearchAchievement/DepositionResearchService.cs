using Newtonsoft.Json.Linq;
using PKS.Core;
using PKS.SZXT.Infrastructure;
using PKS.SZXT.IService.ExplorationDecision;
using PKS.SZXT.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using PKS.DbModels.SZXT;
using PKS.WebAPI;
using PKS.SZXT.Core.Model.EsRawResult;
using PKS.Models;

namespace PKS.SZXT.Service.ExplorationDecision
{
    public class DepositionResearchService : ViewServiceBase, IPerRequestAppService, IDepositionResearchService
    {
        public IEnumerable<object> GetImages(string boid, string year, string grid)
        {
            var query = SearchConfig[grid];
            query = query.ToEsQuery(boid, year);
            return GetImageList(query);
        }
        public IEnumerable<object> GetOthers(string boid, string year, string grid)
        {
            var query = SearchConfig[grid];
            query = query.ToEsQuery(boid, year);
            return GetEsNews(query);
        }

        public object GetSearchFilters(string bot)
        {
            var lstSearch = new List<PKS_SearchItem>();
            var years = new string[10];
            int curYear = DateTime.Now.Year;
            for (int i = 0; i < 10; i++)
            {
                years[i] = curYear.ToString();
                curYear--;
            }
            var searchItem = new PKS_SearchItem();
            searchItem.catelog = "年度";
            searchItem.type = "radio";
            searchItem.list = years;

            var lstMB = GetBOsByProperties(bot, null, 0, short.MaxValue);
            var areas = lstMB.ToArray();

            var searchItem1 = new PKS_SearchItem();
            searchItem1.catelog = "目标区";
            searchItem1.type = "radio";
            searchItem1.list = areas;

            lstSearch.Add(searchItem);
            lstSearch.Add(searchItem1);

            return lstSearch;
        }
    }
}
