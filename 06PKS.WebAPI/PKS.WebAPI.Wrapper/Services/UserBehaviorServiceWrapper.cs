using PKS.Core;
using PKS.Models;
using PKS.Web;
using PKS.WebAPI.Models;
using System.Threading.Tasks;

namespace PKS.WebAPI.Services
{
    /// <summary>用户行为日志服务包装器</summary>
    public class UserBehaviorServiceWrapper : ApiServiceWrapper, IUserBehaviorService, IUserBehaviorServiceWrapper, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public UserBehaviorServiceWrapper(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>构造函数</summary>
        public UserBehaviorServiceWrapper(IApiServiceConfig config) : base(config, nameof(IUserBehaviorService).Substring(1))
        {
        }
        /// <summary>添加用户行为日志</summary>
        public void Add(UserBehavior request)
        {
            var client = InitHttpClient();
            Task.Run(() => AddLogInternal(client, request));
        }
        /// <summary>添加用户行为日志</summary>
        public async Task AddLog(UserBehavior request)
        {
            var client = InitHttpClient();
            await AddLogInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>添加用户行为日志</summary>
        private async Task AddLogInternal(HttpClientWrapper client, UserBehavior request)
        {
            await client.PostObjectAsync(GetActionUrl(nameof(Add)), request).ConfigureAwait(false);
        }
        /// <summary>搜索用户行为日志</summary>
        public string Search(string request)
        {
            var client = InitHttpClient();
            return Task.Run(() => SearchAsyncInternal(client, request)).Result;
        }
        /// <summary>搜索用户行为日志</summary>
        public async Task<string> SearchAsync(string request)
        {
            var client = InitHttpClient();
            return await SearchAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>搜索用户行为日志</summary>
        private async Task<string> SearchAsyncInternal(HttpClientWrapper client, string request)
        {
            return await client.PostAsync<string>(GetActionUrl(nameof(Search)), request).ConfigureAwait(false);
        }
    }
}