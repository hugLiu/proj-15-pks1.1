using Jurassic.So.Application.SearchHistory.Dto;
using PKS.Core;
using PKS.Data;
using PKS.DbModels.Portal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.Portal.SearchHistory
{
    public class SearchHistoryService : AppService, IPerRequestAppService
    {
        public IRepository<PKS_SearchHistory> _searchHistoryService;
        public SearchHistoryService(IRepository<PKS_SearchHistory> searchHistoryService)
        {
            _searchHistoryService = searchHistoryService;
        }

        public List<HotwordModel> GetHotSearchWord(int topCount)
        {
            DateTime date = DateTime.Today.AddDays(-7);
            var query = _searchHistoryService.GetQuery().Where(o => ((o.InputWord != null)) &&(o.SourceTime>date));
            var lst= query.ToList();
            List<HotwordModel> result = lst.GroupBy(x => x.InputWord.Trim())
                                .Select(g => new HotwordModel
                                {
                                    Word = g.Key,
                                    Num = g.Count()
                                })
                                .OrderByDescending(x => x.Num)
                                .Take(topCount).ToList();
            return result;
        }



        /// <summary>
        /// 保存基本的历史记录信息
        /// </summary>
        /// <param name="searhHistoryModel">基本历史记录信息的实体模型</param>
        /// <param name="isStartPage">是否是起始页</param>
        public void SaveClickLog(PKS_SearchHistory searhHistoryModel, string isStartPage)
        {
            _searchHistoryService.Add(searhHistoryModel);
        }

        public void SaveLoginLog(PKS_SearchHistory searhHistoryModel, string isStartPage)
        {
            PKS_SearchHistory searchHistory = new PKS_SearchHistory();
            searchHistory = _searchHistoryService.GetQuery().FirstOrDefault(o => o.TargetId == searhHistoryModel.TargetId);
            if (searchHistory == null)
            {
                return;
            }
            searchHistory.TargetPageNameEnum = searhHistoryModel.TargetPageNameEnum;
            searchHistory.PageResultsEnum = searhHistoryModel.PageResultsEnum;
            TimeSpan ts = new TimeSpan();
            // DateTime startTime = Convert() searhHistoryModel.TargetTime.ToString();
            searchHistory.TargetTime = DateTime.Now;
            ts = Convert.ToDateTime(searchHistory.TargetTime) - Convert.ToDateTime(searchHistory.SourceTime);
            searchHistory.RunTime = ts.Minutes * 60 + ts.Seconds + Convert.ToDouble(ts.Milliseconds) / 1000;
            searchHistory.TargetPageNameEnum = searhHistoryModel.TargetPageNameEnum;
            if (searhHistoryModel.TargetPageNameEnum.Equals("SearchResultDetailed"))
            {
                searchHistory.BO = searhHistoryModel.BO;
                searchHistory.BOT = searhHistoryModel.BOT;
                searchHistory.PT = searhHistoryModel.PT;
                searchHistory.BP = searhHistoryModel.BP;
                searchHistory.Iiid = searhHistoryModel.Iiid;
                searchHistory.Title = searhHistoryModel.Title;
                //historylist.AdapterName = searhHistoryModel.AdapterName;
                searchHistory.ResourcesName = searhHistoryModel.ResourcesName;
                searchHistory.ResourcesFormat = searhHistoryModel.ResourcesFormat;
            }
            _searchHistoryService.Update(searchHistory);
        }
    }
}
