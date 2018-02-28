using Newtonsoft.Json;
using PKS.DbModels.Portal;
using PKS.DbServices.Portal.SearchHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PKS.Sooil.Controllers
{

    public class SearchHistoryController : SooilBaseController
    {
        private SearchHistoryService _searchHistoryService;
        public SearchHistoryController(SearchHistoryService searchHistoryService)
        {
            _searchHistoryService = searchHistoryService;
        }

        /// 保存方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveLogResult(string model)
        {
            string userId = this.PKSUser.Identity.Id;          
            var searchHistory = JsonConvert.DeserializeObject<PKS_SearchHistory>(model);
            if (!string.IsNullOrEmpty(searchHistory.SourceWayEnum))
            {
                searchHistory.SourceTime = DateTime.Now;
                searchHistory.ClientIP = GetIp();
                searchHistory.BrowserName = Request.Browser.Type;
                searchHistory.UserId = userId;
                _searchHistoryService.SaveClickLog(searchHistory, "false");
            }
            else
            {
                _searchHistoryService.SaveLoginLog(searchHistory, "false");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取前10热词列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHotSearchWord(int topCount = 5)
        {
            var hotWords = _searchHistoryService.GetHotSearchWord(topCount);
            return Json(hotWords, JsonRequestBehavior.AllowGet);
        }

       
        private string GetIp()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }
}