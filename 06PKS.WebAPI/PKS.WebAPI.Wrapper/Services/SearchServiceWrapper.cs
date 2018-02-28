using Newtonsoft.Json.Linq;
using PKS.Core;
using PKS.Models;
using PKS.Web;
using PKS.WebAPI.Models;
using System.Threading.Tasks;

namespace PKS.WebAPI.Services
{
    /// <summary>搜索服务实现</summary>
    public class SearchServiceWrapper : ApiServiceWrapper, ISearchService, ISearchServiceWrapper, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public SearchServiceWrapper(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>构造函数</summary>
        public SearchServiceWrapper(IApiServiceConfig config) : base(config, nameof(ISearchService).Substring(1))
        {
        }
        /// <summary>按短语搜索</summary>
        public SearchResult Search(SearchRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => SearchAsyncInternal(client, request)).Result;
        }
        /// <summary>按短语搜索</summary>
        public async Task<SearchResult> SearchAsync(SearchRequest request)
        {
            var client = InitHttpClient();
            return await SearchAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>按短语搜索</summary>
        private async Task<SearchResult> SearchAsyncInternal(HttpClientWrapper client, SearchRequest request)
        {
            return await client.PostObjectAsync<SearchResult>(GetActionUrl(nameof(Search)), request).ConfigureAwait(false);
        }
        /// <summary>按ES语法搜索</summary>
        public string ESSearch(string request)
        {
            var client = InitHttpClient();
            return Task.Run(() => ESSearchAsyncInternal(client, request)).Result;
        }
        /// <summary>按ES语法搜索</summary>
        public async Task<string> ESSearchAsync(string request)
        {
            var client = InitHttpClient();
            return await ESSearchAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>按ES语法搜索</summary>
        private async Task<string> ESSearchAsyncInternal(HttpClientWrapper client, string request)
        {
            return await client.PostAsync<string>(GetActionUrl(nameof(ESSearch)), request).ConfigureAwait(false);
        }
        /// <summary>按完全匹配条件搜索</summary>
        public MatchResult Match(MatchRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => MatchAsyncInternal(client, request)).Result;
        }
        /// <summary>按完全匹配条件搜索</summary>
        public async Task<MatchResult> MatchAsync(MatchRequest request)
        {
            var client = InitHttpClient();
            return await MatchAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>按完全匹配条件搜索</summary>
        private async Task<MatchResult> MatchAsyncInternal(HttpClientWrapper client, MatchRequest request)
        {
            return await client.PostObjectAsync<MatchResult>(GetActionUrl(nameof(Match)), request).ConfigureAwait(false);
        }
        /// <summary>按多个完全匹配条件搜索</summary>
        public MatchResult[] MatchMany(MatchRequest[] request)
        {
            var client = InitHttpClient();
            return Task.Run(() => MatchManyAsyncInternal(client, request)).Result;
        }

        /// <summary>按多个完全匹配条件搜索</summary>
        public async Task<MatchResult[]> MatchManyAsync(MatchRequest[] request)
        {
            var client = InitHttpClient();
            return await MatchManyAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>按多个完全匹配条件搜索</summary>
        private async Task<MatchResult[]> MatchManyAsyncInternal(HttpClientWrapper client, MatchRequest[] request)
        {
            return await client.PostObjectAsync<MatchResult[]>(GetActionUrl(nameof(Match)), request).ConfigureAwait(false);
        }
        /// <summary>根据iiid搜索</summary>
        public Metadata GetMetadata(SearchMetadataRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetMetadataAsyncInternal(client, request)).Result;
        }

        /// <summary>根据iiid搜索</summary>
        public async Task<Metadata> GetMetadataAsync(SearchMetadataRequest request)
        {
            var client = InitHttpClient();
            return await GetMetadataAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据iiid搜索</summary>
        private async Task<Metadata> GetMetadataAsyncInternal(HttpClientWrapper client, SearchMetadataRequest request)
        {
            return await client.PostObjectAsync<Metadata>(GetActionUrl(nameof(GetMetadata)), request).ConfigureAwait(false);
        }
        /// <summary>根据iiid数组搜索</summary>
        public MetadataCollection GetMetadatas(SearchMetadatasRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => GetMetadatasAsyncInternal(client, request)).Result;
        }
        /// <summary>根据iiid数组搜索</summary>
        public async Task<MetadataCollection> GetMetadatasAsync(SearchMetadatasRequest request)
        {
            var client = InitHttpClient();
            return await GetMetadatasAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据iiid数组搜索</summary>
        private async Task<MetadataCollection> GetMetadatasAsyncInternal(HttpClientWrapper client, SearchMetadatasRequest request)
        {
            return await client.PostObjectAsync<MetadataCollection>(GetActionUrl(nameof(GetMetadatas)), request).ConfigureAwait(false);
        }
        /// <summary>根据聚合条件获取统计信息</summary>
        public SearchStatisticsResult Statistics(SearchStatisticsRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => StatisticsAsyncInternal(client, request)).Result;
        }
        /// <summary>根据聚合条件获取统计信息</summary>
        public async Task<SearchStatisticsResult> StatisticsAsync(SearchStatisticsRequest request)
        {
            var client = InitHttpClient();
            return await StatisticsAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>根据聚合条件获取统计信息</summary>
        private async Task<SearchStatisticsResult> StatisticsAsyncInternal(HttpClientWrapper client, SearchStatisticsRequest request)
        {
            return await client.PostObjectAsync<SearchStatisticsResult>(GetActionUrl(nameof(Statistics)), request).ConfigureAwait(false);
        }
        /// <summary>查询元数据定义信息</summary>
        public MetadataDefinition[] GetMetadataDefinitions()
        {
            var client = InitHttpClient();
            return Task.Run(() => GetMetadataDefinitionsAsyncInternal(client)).Result;
        }
        /// <summary>查询元数据定义信息</summary>
        public async Task<MetadataDefinition[]> GetMetadataDefinitionsAsync()
        {
            var client = InitHttpClient();
            return await GetMetadataDefinitionsAsyncInternal(client).ConfigureAwait(false);
        }
        /// <summary>查询元数据定义信息</summary>
        private async Task<MetadataDefinition[]> GetMetadataDefinitionsAsyncInternal(HttpClientWrapper client)
        {
            return await client.GetAsync<MetadataDefinition[]>(GetActionUrl(nameof(GetMetadataDefinitions))).ConfigureAwait(false);
        }
        /// <summary>ES搜索</summary>
        public object ESSearchEx(object query)
        {
            var client = InitHttpClient();
            return Task.Run(() => ESSearchExAsyncInternal(client, query)).Result;
        }
        /// <summary>ES搜索</summary>
        public async Task<object> ESSearchExAsync(object query)
        {
            var client = InitHttpClient();
            return await ESSearchExAsyncInternal(client, query).ConfigureAwait(false);
        }
        /// <summary>ES搜索</summary>
        private async Task<object> ESSearchExAsyncInternal(HttpClientWrapper client, object query)
        {
            return await client.PostObjectAsync<JObject>(GetActionUrl(nameof(ESSearchEx)), query).ConfigureAwait(false);
        }
        /// <summary>ES多结果搜索</summary>
        public object ESMSearch(string request)
        {
            var client = InitHttpClient();
            return Task.Run(() => ESMSearchAsyncInternal(client, request)).Result;
        }
        /// <summary>ES多结果搜索</summary>
        public async Task<object> ESMSearchAsync(string request)
        {
            var client = InitHttpClient();
            return await ESMSearchAsyncInternal(client, request).ConfigureAwait(false);
        }
        /// <summary>ES多结果搜索</summary>
        private async Task<object> ESMSearchAsyncInternal(HttpClientWrapper client, string request)
        {
            return await client.PostAsync<string>(GetActionUrl(nameof(ESMSearch)), request).ConfigureAwait(false);
        }
    }
}