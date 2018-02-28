using System.Security.Principal;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Nest;
using PKS.Models;
using PKS.WebAPI.Models;
using SearchRequest = PKS.WebAPI.Models.SearchRequest;

namespace PKS.WebAPI.Services
{
    /// <summary>搜索服务包装器接口</summary>
    public interface ISearchServiceWrapper : ISearchService, IApiServiceWrapper
    {
    }

    /// <summary>搜索服务接口</summary>
    public interface ISearchService
    {
        /// <summary>按短语搜索</summary>
        SearchResult Search(SearchRequest request);

        /// <summary>按短语搜索</summary>
        Task<SearchResult> SearchAsync(SearchRequest request);
        /// <summary>按ES语法搜索</summary>
        string ESSearch(string request);

        /// <summary>按ES语法搜索</summary>
        Task<string> ESSearchAsync(string request);

        object ESMSearch(string request);
        Task<object> ESMSearchAsync(string request);

        /// <summary>按完全匹配条件搜索</summary>
        MatchResult Match(MatchRequest request);

        /// <summary>按完全匹配条件搜索</summary>
        Task<MatchResult> MatchAsync(MatchRequest request);
        /// <summary>按多个完全匹配条件搜索</summary>
        MatchResult[] MatchMany(MatchRequest[] request);

        /// <summary>按多个完全匹配条件搜索</summary>
        Task<MatchResult[]> MatchManyAsync(MatchRequest[] request);
        /// <summary>根据iiid搜索</summary>
        Metadata GetMetadata(SearchMetadataRequest request);

        /// <summary>根据iiid搜索</summary>
        Task<Metadata> GetMetadataAsync(SearchMetadataRequest request);
        /// <summary>根据iiid数组搜索</summary>
        MetadataCollection GetMetadatas(SearchMetadatasRequest request);

        /// <summary>根据iiid数组搜索</summary>
        Task<MetadataCollection> GetMetadatasAsync(SearchMetadatasRequest request);
        /// <summary>根据聚合条件获取统计信息</summary>
        SearchStatisticsResult Statistics(SearchStatisticsRequest request);

        /// <summary>根据聚合条件获取统计信息</summary>
        Task<SearchStatisticsResult> StatisticsAsync(SearchStatisticsRequest request);
        /// <summary>查询元数据定义信息</summary>
        MetadataDefinition[] GetMetadataDefinitions();

        /// <summary>查询元数据定义信息</summary>
        Task<MetadataDefinition[]> GetMetadataDefinitionsAsync();

        object ESSearchEx(object query);
        Task<object> ESSearchExAsync(object query);
    }
}