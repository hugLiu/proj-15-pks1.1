using PKS.Core;
using PKS.SZXT.IService.ExplorationDecision;
using PKS.SZXT.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.WebAPI.Models;
using PKS.SZXT.Infrastructure;
using PKS.WebAPI;

namespace PKS.SZXT.Service.ExplorationDecision
{
    public class ExplorationDeploymentService : ViewServiceBase, IPerRequestAppService, IExplorationDeploymentService
    {
        public List<string> GetBosByQuery(Dictionary<string, List<string>> query, string bot)
        {
            return GetBOsByProperties(bot, query, null, null);
        }

        //获取bot
        public List<BOTPropertyDefinition> GetBotProtertyByBot(string bot)
        {
            return GetBotProtertysByBot(bot);
        }

        public object GetLocationProposal(string year, int size)
        {
            var query = SearchConfig["G4"];
            query = query?.ToEsQuery(year, size);
            return GetEsNews(query); ;
        }



        public object GetSeismicAcquisitionTable(string year)
        {
            return GetIndexDataByQuery("G6", new string[] { year }, false);
        }

        public object GetSeismicDeploymentImg(string year)
        {
            var query = SearchConfig["G5"];
            query = query?.ToEsQuery(year);
            return GetImageUrl(query);
        }

        public object GetSeismicRecommendation(string year, int size)
        {
            var query = SearchConfig["G8"];
            query = query?.ToEsQuery(year, size);
            return GetEsNews(query); ;
        }


        public object GetSeismicWorkAreaCensus(string year, int size)
        {
            var query = SearchConfig["G7"];
            query = query?.ToEsQuery(year, size);
            return GetEsNews(query); ;
        }

        public object GetWellLocationImg(string year)
        {
            var query = SearchConfig["G1"];
            query = query?.ToEsQuery(year);
            return GetImageUrl(query);
        }

        public object GetWellLocationTable(string year)
        {
            return GetIndexDataByQuery("G2", new string[] { year }, false);
        }

        public object GetWellsiteSurveyReport(string year, int size)
        {
            var query = SearchConfig["G3"];
            query = query?.ToEsQuery(year, size);
            return GetEsNews(query); ;
        }
    }
}
