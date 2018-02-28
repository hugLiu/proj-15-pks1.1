using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PKS.Web;
#pragma warning disable 1591

namespace PKS.WebAPI.Controllers
{
    /// <summary>
    /// 用户行为服务
    /// </summary>
    public class UserBehaviorServiceController : PKSApiController
    {
        private IUserBehaviorService ServiceImpl { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service"></param>
        public UserBehaviorServiceController(IUserBehaviorService service)
        {
            ServiceImpl = service;
        }

        /// <summary>
        /// 插入一条用户行为记录
        /// </summary>
        [HttpPost]
        public void Add(UserBehavior request)
        {
            request.User = request?.User ?? this.PKSUser.Identity.Name;
            request.Role = request?.Role ?? this.PKSUser.Roles.First().Name;
            request.LogDate = request?.LogDate ?? DateTime.UtcNow;
            request.LogId = request?.LogId ?? Guid.NewGuid().ToString();
            ServiceImpl.Add(request);
        }
        [HttpPost]
        public async Task<object> Search()
        {
            var query = await this.Request.Content.ReadAsStringAsync();
            if (query.IsNullOrEmpty())
            {
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺少请求参数！", "请求内容应是ES搜索条件！");
            }
            var searchResult = await ServiceImpl.SearchAsync(query);
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(searchResult, Encoding.UTF8, MimeTypes.JSON)
            };
            return result;
        }
    }
}