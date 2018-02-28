using PKS.Core;
using PKS.Web;
using PKS.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
#pragma warning disable 1591

namespace PKS.WebAPI.Services
{
    /// <summary>对象服务包装器</summary>
    public class BO2ServiceWrapper : ApiServiceWrapper, IBO2Service, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public BO2ServiceWrapper(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>构造函数</summary>
        public BO2ServiceWrapper(IApiServiceConfig config) : base(config, nameof(IBO2Service).Substring(1))
        {
        }
        /// <summary>根据类型名称和对象名称获得对象</summary>
        public BO2 GetBO(string bot, string bo)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetBOAsyncInternal(client, bot, bo)).Result;
        }
        /// <summary>根据类型名称和对象名称获得对象</summary>
        public async Task<BO2> GetBOAsync(string bot, string bo)
        {
            var client = InitHttpClient();
            return await GetBOAsyncInternal(client, bot, bo).ConfigureAwait(false);
        }
        /// <summary>根据类型名称和对象名称获得对象</summary>
        public async Task<BO2> GetBOAsyncInternal(HttpClientWrapper client, string bot, string bo)
        {
            var queryString = $"?{nameof(bot)}={bot}&{nameof(bo)}={bo}";
            return await client.GetAsync<BO2>(GetActionUrl(nameof(GetBO)) + queryString).ConfigureAwait(false);
        }
        /// <summary>根据类型名称获得对象类型</summary>
        public BOT GetBOT(string bot)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetBOTAsyncInternal(client, bot)).Result;
        }
        /// <summary>根据类型名称获得对象类型</summary>
        public async Task<BOT> GetBOTAsync(string bot)
        {
            var client = InitHttpClient();
            return await GetBOTAsyncInternal(client,  bot).ConfigureAwait(false);
        }
        /// <summary>根据类型名称获得对象类型</summary>
        public async Task<BOT> GetBOTAsyncInternal(HttpClientWrapper client, string bot)
        {
            var queryString = $"?{nameof(bot)}={bot}";
            return await client.GetAsync<BOT>(GetActionUrl(nameof(GetBOT)) + queryString).ConfigureAwait(false);
        }

        /// <summary>根据类型名称和对象名称获得对象集合</summary>
        public List<BO2> FindBOs(string bot, string bo)
        {
            var client = InitHttpClient();
            return Task.Run(() => FindBOsAsyncInternal(client, bot, bo)).Result;
        }
        /// <summary>根据类型名称和对象名称获得对象集合</summary>
        public async Task<List<BO2>> FindBOsAsync(string bot, string bo)
        {
            var client = InitHttpClient();
            return await FindBOsAsyncInternal(client, bot, bo).ConfigureAwait(false);
        }
        /// <summary>根据类型名称和对象名称获得对象集合</summary>
        public async Task<List<BO2>> FindBOsAsyncInternal(HttpClientWrapper client, string bot, string bo)
        {
            var queryString = $"?{nameof(bot)}={bot}&{nameof(bo)}={bo}";
            return await client.GetAsync<List<BO2>>(GetActionUrl(nameof(FindBOs)) + queryString).ConfigureAwait(false);
        }
        /// <summary>根据条件获得对象集合</summary>
        public List<BO2> FilterBOs(FilterRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => FilterBOsAsyncInternal(client, request)).Result;
        }
        /// <summary>根据条件获得对象集合</summary>
        public async Task<List<BO2>> FilterBOsAsync(FilterRequest request)
        {
            var client = InitHttpClient();
            return await FilterBOsAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据条件获得对象集合</summary>
        public async Task<List<BO2>> FilterBOsAsyncInternal(HttpClientWrapper client, FilterRequest request)
        {
            return await client.PostObjectAsync<List<BO2>>(GetActionUrl(nameof(FilterBOs)), request).ConfigureAwait(false);
        }
        /// <summary>根据条件获得对象类型集合</summary>
        public List<BOT> FilterBOTs(FilterRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => FilterBOTsAsyncInternal(client, request)).Result;
        }
        /// <summary>根据条件获得对象类型集合</summary>
        public async Task<List<BOT>> FilterBOTsAsync(FilterRequest request)
        {
            var client = InitHttpClient();
            return await FilterBOTsAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据条件获得对象类型集合</summary>
        public async Task<List<BOT>> FilterBOTsAsyncInternal(HttpClientWrapper client, FilterRequest request)
        {
            return await client.PostObjectAsync<List<BOT>>(GetActionUrl(nameof(FilterBOTs)), request).ConfigureAwait(false);
        }
        /// <summary>插入对象类型集合</summary>
        public void InsertBOTs(List<BOT> bots)
        {
            var client = InitHttpClient();
            Task.Run(() => InsertBOTsAsyncInternal(client, bots));
        }
        /// <summary>插入对象类型集合</summary>
        public async Task InsertBOTsAsync(List<BOT> bots)
        {
            var client = InitHttpClient();
            await InsertBOTsAsyncInternal(client, bots).ConfigureAwait(false);
        }
        /// <summary>插入对象类型集合</summary>
        public async Task InsertBOTsAsyncInternal(HttpClientWrapper client, List<BOT> bots)
        {
            await client.PostObjectAsync(GetActionUrl(nameof(InsertBOTs)), bots).ConfigureAwait(false);
        }
        /// <summary>保存对象类型集合</summary>
        public object SaveBOTs(List<BOT> bots)
        {
            var client = InitHttpClient();
            return Task.Run(() => SaveBOTsAsyncInternal(client, bots)).Result;
        }
        /// <summary>保存对象类型集合</summary>
        public async Task<object> SaveBOTsAsync(List<BOT> bots)
        {
            var client = InitHttpClient();
            return await SaveBOTsAsyncInternal(client, bots).ConfigureAwait(false);
        }
        /// <summary>保存对象类型集合</summary>
        public async Task<object> SaveBOTsAsyncInternal(HttpClientWrapper client, List<BOT> bots)
        {
            return await client.PostObjectAsync(GetActionUrl(nameof(SaveBOTs)), bots).ConfigureAwait(false);
        }
        /// <summary>删除对象类型集合</summary>
        public object DeleteBOTs(List<string> bots)
        {
            var client = InitHttpClient();
            return Task.Run(() => DeleteBOTsAsyncInternal(client, bots)).Result;
        }
        /// <summary>删除对象类型集合</summary>
        public async Task<object> DeleteBOTsAsync(List<string> bots)
        {
            var client = InitHttpClient();
            return await DeleteBOTsAsyncInternal(client, bots).ConfigureAwait(false);
        }
        /// <summary>删除对象类型集合</summary>
        public async Task<object> DeleteBOTsAsyncInternal(HttpClientWrapper client, List<string> bots)
        {
            return await client.PostObjectAsync(GetActionUrl(nameof(DeleteBOTs)), bots).ConfigureAwait(false);
        }
        /// <summary>插入对象集合</summary>
        public void InsertBOs(List<BO2> bos)
        {
            var client = InitHttpClient();
            Task.Run(() => InsertBOsAsyncInternal(client, bos));
        }
        /// <summary>插入对象集合</summary>
        public async Task InsertBOsAsync(List<BO2> bos)
        {
            var client = InitHttpClient();
            await InsertBOsAsyncInternal(client, bos).ConfigureAwait(false);
        }
        /// <summary>插入对象集合</summary>
        public async Task InsertBOsAsyncInternal(HttpClientWrapper client, List<BO2> bos)
        {
            await client.PostObjectAsync(GetActionUrl(nameof(InsertBOs)), bos).ConfigureAwait(false);
        }
        /// <summary>保存对象集合</summary>
        public object SaveBOs(List<BO2> bos)
        {
            var client = InitHttpClient();
            return Task.Run(() => SaveBOsAsyncInternal(client, bos)).Result;
        }
        /// <summary>保存对象集合</summary>
        public async Task<object> SaveBOsAsync(List<BO2> bos)
        {
            var client = InitHttpClient();
            return await SaveBOsAsyncInternal(client, bos).ConfigureAwait(false);
        }
        /// <summary>保存对象集合</summary>
        public async Task<object> SaveBOsAsyncInternal(HttpClientWrapper client, List<BO2> bos)
        {
            return await client.PostObjectAsync(GetActionUrl(nameof(SaveBOs)), bos).ConfigureAwait(false);
        }
        /// <summary>删除对象集合</summary>
        public object DeleteBOs(BO2DeleteRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => DeleteBOsAsyncInternal(client, request)).Result;
        }
        /// <summary>删除对象集合</summary>
        public async Task<object> DeleteBOsAsync(BO2DeleteRequest request)
        {
            var client = InitHttpClient();
            return await DeleteBOsAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>删除对象集合</summary>
        public async Task<object> DeleteBOsAsyncInternal(HttpClientWrapper client, BO2DeleteRequest request)
        {
            return await client.PostObjectAsync(GetActionUrl(nameof(DeleteBOs)), request).ConfigureAwait(false);
        }

        public long CountBOs(object query)
        {
            var client = InitHttpClient();
            return Task.Run(() => CountBOsAsyncInternal(client, query)).Result;
        }

        public async Task<long> CountBOsAsync(object query)
        {
            var client = InitHttpClient();
            return (long)await CountBOsAsyncInternal(client, query).ConfigureAwait(false);
        }
        public async Task<long> CountBOsAsyncInternal(HttpClientWrapper client, object query)
        {
            return (long)await client.PostObjectAsync(GetActionUrl(nameof(CountBOs)), query).ConfigureAwait(false);
        }

        public long CountBOTs(object query)
        {
            var client = InitHttpClient();
            return Task.Run(() => CountBOTsAsyncInternal(client, query)).Result;
        }

        public async Task<long> CountBOTsAsync(object query)
        {
            var client = InitHttpClient();
            return await CountBOTsAsyncInternal(client, query).ConfigureAwait(false);
        }
        public async Task<long> CountBOTsAsyncInternal(HttpClientWrapper client, object query)
        {
            return (long)await client.PostObjectAsync(GetActionUrl(nameof(CountBOTs)), query).ConfigureAwait(false);
        }

        public object Near(NearRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => NearAsyncInternal(client, request)).Result;
        }

        public async Task<object> NearAsync(NearRequest request)
        {
            var client = InitHttpClient();
            return await NearAsyncInternal(client, request).ConfigureAwait(false);
        }
        public async Task<object> NearAsyncInternal(HttpClientWrapper client, NearRequest request)
        {
            return await client.PostObjectAsync(GetActionUrl(nameof(Near)), request).ConfigureAwait(false);
        }
    }
}
