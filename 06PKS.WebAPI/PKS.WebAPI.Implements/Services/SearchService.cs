using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using MongoDB.Driver;
using Nest;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
//using PKS.Utils;
using PKS.WebAPI.ES;
using PKS.WebAPI.Models;
using SearchRequest = PKS.WebAPI.Models.SearchRequest;

namespace PKS.WebAPI.Services
{
    /// <summary>搜索服务实现</summary>
    public class SearchService : AppService, ISearchService, ISingletonAppService
    {
        private ESAccess<Metadata> _esAccess = null;
        /// <summary>构造函数</summary>
        public SearchService(IElasticConfig elasticConfig, IMongoConfig mongoConfig, IMongoCollection<MetadataDefinition> accessor)
        {
            MdAccessor = accessor;
            _esAccess = new ESAccess<Metadata>();
        }

        /// <summary>访问器</summary>
        private IMongoCollection<MetadataDefinition> MdAccessor { get; }
        /// <summary>按短语搜索</summary>
        public virtual SearchResult Search(SearchRequest request)
        {
            return Task.Run(() => SearchAsync(request)).Result;
        }
        /// <summary>按短语搜索</summary>
        public virtual async Task<SearchResult> SearchAsync(SearchRequest request)
        {
            var _provider = new SearchProvider<Metadata>();
            var filterQuery = _provider.BuildFilterQuery(request.Filter);
            // var fulltextQuery = _provider.BuildFullTextQuery(request.Sentence, request.Ranks);
            var fulltextQuery = _provider.BuildCustomScoreQuery(request.Sentence, request.Boost);
            var query = _provider.CombineMustQuery(filterQuery, fulltextQuery);
            var sort = _provider.BuildSort(request.Sort);
            var aggs = _provider.BuildAggs(request.Group);
            var fields = _provider.BuildFields(request.Fields);

            var from = request.From;
            var size = request.Size;

            var searchresponse = await _esAccess.PagingQueryAsync(query, sort, fields, aggs, from, size);

            var response = searchresponse.ToMetadataCollection();
            if (searchresponse.Aggregations.Count <= 0)
                return await Task.FromResult(response);

            var groups = new Dictionary<string, Dictionary<string, long?>>();
            foreach (var agg in searchresponse.Aggregations)
            {
                var aggregates = agg.Value.As<BucketAggregate>().Items;
                Dictionary<string, long?> dic = new Dictionary<string, long?>();
                foreach (var aggregate in aggregates)
                {
                    var keyedBucket = aggregate as Nest.KeyedBucket<object>;
                    if (keyedBucket != null)
                        dic.Add(keyedBucket.Key.ToString(), keyedBucket.DocCount);
                }
                groups.Add(agg.Key, dic);
            }

            response.Groups = groups;
            return await Task.FromResult(response);
        }
        /// <summary>按ES语法搜索</summary>
        public virtual string ESSearch(string request)
        {
            return Task.Run(() => ESSearchAsync(request)).Result;
        }
        /// <summary>按ES语法搜索</summary>
        public virtual async Task<string> ESSearchAsync(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return string.Empty;
            }
            return await _esAccess.GetDocumentsByRawQueryAsyn(request);
        }
        /// <summary>按ES语法搜索</summary>
        public virtual object ESMSearch(string request)
        {
            return Task.Run(() => ESMSearchAsync(request)).Result;
        }
        /// <summary>按ES语法搜索</summary>
        public virtual async Task<object> ESMSearchAsync(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
            {
                return string.Empty;
            }
            return await _esAccess.GetDocumentsByMRawQueryAsyn(request);
        }
        /// <summary>按完全匹配条件搜索</summary>
        public virtual MatchResult Match(MatchRequest request)
        {
            return Task.Run(() => MatchAsync(request)).Result;
        }

        /// <summary>按完全匹配条件搜索</summary>
        public virtual async Task<MatchResult> MatchAsync(MatchRequest request)
        {
            var _provider = new SearchProvider<Metadata>();
            var filterQuery = _provider.BuildFilterQuery(request.Filter);
            var sort = _provider.BuildSort(request.Sort);
            var fields = _provider.BuildFields(request.Fields);

            var from = 0;
            var size = request.Top;
            var searchresponse = await _esAccess.PagingQueryAsync(filterQuery, sort, fields, null, from, size);
            var response = searchresponse.ToMetadataCollection<MatchResult>();

            return await Task.FromResult(response);
        }
        /// <summary>按多个完全匹配条件搜索</summary>
        public virtual MatchResult[] MatchMany(MatchRequest[] request)
        {
            return Task.Run(() => MatchManyAsync(request)).Result;
        }

        /// <summary>按多个完全匹配条件搜索</summary>
        public virtual async Task<MatchResult[]> MatchManyAsync(MatchRequest[] requests)
        {
            var _provider = new SearchProvider<Metadata>();
            MultiSearchDescriptor multiSearchDescriptor = new MultiSearchDescriptor();
            for (int i = 0; i < requests.Length; i++)
            {
                var request = requests[i];
                var filterQuery = _provider.BuildFilterQuery(request.Filter);
                var sort = _provider.BuildSort(request.Sort);

                var fields = _provider.BuildFields(request.Fields);

                var from = 0;
                var size = request.Top;

                var searchDescriptor = _esAccess.BuildSearchDescriptor(filterQuery, sort, fields, null, from, size);
                multiSearchDescriptor.Search<Metadata>("search" + i, s => searchDescriptor);
            }

            List<MatchResult> result = new List<MatchResult>();
            var searchResponse = await _esAccess.MultiSearch(multiSearchDescriptor);
            foreach (var responseItem in searchResponse.AllResponses)
            {
                var metadata = responseItem as ISearchResponse<Metadata>;
                if (metadata != null)
                {
                    result.Add(metadata.ToMetadataCollection<MatchResult>());
                };

            }
            return await Task.FromResult(result.ToArray());

        }
        /// <summary>根据iiid搜索</summary>
        public virtual Metadata GetMetadata(SearchMetadataRequest request)
        {
            return Task.Run(() => GetMetadataAsync(request)).Result;
        }

        /// <summary>根据iiid搜索</summary>
        public virtual async Task<Metadata> GetMetadataAsync(SearchMetadataRequest request)
        {
            var provider = new SearchProvider<Metadata>();
            var termQuery = new TermQuery { Field = "iiid.keyword", Value = request.IIId };
            var query = new BoolQuery() { Must = new List<QueryContainer> { termQuery } };

            var fields = provider.BuildFields(request.Fields);

            var from = 0;
            var size = 1;

            var searchresponse = await _esAccess.PagingQueryAsync(query, null, fields, null, from, size);

            var response = searchresponse.Documents.FirstOrDefault();

            return await Task.FromResult(response);
        }
        /// <summary>根据iiid数组搜索</summary>
        public virtual MetadataCollection GetMetadatas(SearchMetadatasRequest request)
        {
            return Task.Run(() => GetMetadatasAsync(request)).Result;
        }

        /// <summary>根据iiid数组搜索</summary>
        public virtual async Task<MetadataCollection> GetMetadatasAsync(SearchMetadatasRequest request)
        {
            var provider = new SearchProvider<Metadata>();
            var termQuery = new TermsQuery { Field = "iiid.keyword", Terms = request.IIIds };
            var query = new BoolQuery() { Must = new List<QueryContainer> { termQuery } };

            var fields = provider.BuildFields(request.Fields);

            var from = 0;
            var size = request.IIIds.Count;

            var searchresponse = await _esAccess.PagingQueryAsync(query, null, fields, null, from, size);

            var response = new MetadataCollection(searchresponse.Documents);

            return await Task.FromResult(response);
        }
        /// <summary>根据聚合条件获取统计信息</summary>
        public virtual SearchStatisticsResult Statistics(SearchStatisticsRequest request)
        {
            return Task.Run(() => StatisticsAsync(request)).Result;
        }

        /// <summary>根据聚合条件获取统计信息</summary>
        public virtual async Task<SearchStatisticsResult> StatisticsAsync(SearchStatisticsRequest request)
        {
            var provider = new SearchProvider<Metadata>();
            SearchGroupRules groups = new SearchGroupRules();
            groups.Top = int.MaxValue / 2;
            groups.Fields = request.Groups;
            var aggs = provider.BuildAggs(groups);

            var searchresponse = await _esAccess.PagingQueryAsync(null, null, null, aggs, 0, 0);

            SearchStatisticsResult result = new SearchStatisticsResult();

            var groupResult = new Dictionary<string, Dictionary<string, long?>>();
            foreach (var agg in searchresponse.Aggregations)
            {
                var aggregates = agg.Value.As<BucketAggregate>().Items;
                Dictionary<string, long?> dic = new Dictionary<string, long?>();
                foreach (var aggregate in aggregates)
                {
                    var keyedBucket = aggregate as Nest.KeyedBucket<object>;
                    if (keyedBucket != null)
                        dic.Add(keyedBucket.Key.ToString(), keyedBucket.DocCount);
                }
                groupResult.Add(agg.Key, dic);
            }


            result.Groups = groupResult;
            return result;
        }
        /// <summary>查询元数据定义信息</summary>
        public MetadataDefinition[] GetMetadataDefinitions()
        {
            return
                GetService<PKS.Data.IRepository<MetadataDefinition>>()
                    .GetQuery()
                    .Where(m => m.GroupCode != "")
                    .OrderBy(e => e.GroupOrder)
                    .ThenBy(e => e.ItemOrder).ToArray();
            //return Task.Run(() => GetMetadataDefinitionsAsync()).Result;
        }
        ///// <summary>查询元数据定义信息</summary>
        //public async Task<MetadataDefinition[]> GetMetadataDefinitionsAsync()
        //{
        //    var values = MetadataDefinitionCollection.Instance;
        //    IEnumerable<MetadataDefinition> values2 = null;
        //    if (values == null)
        //    {
        //        values2 = await this.MdAccessor.AsQueryable().ToListAsync();
        //    }
        //    else
        //    {
        //        values2 = values.Values;
        //    }
        //    return values2.OrderBy(e => e.GroupOrder).ThenBy(e => e.ItemOrder).ToArray();
        //}


        /// <summary>查询元数据定义信息</summary>
        public async Task<MetadataDefinition[]> GetMetadataDefinitionsAsync()
        {
            //var metas = await Task.Run(
            //                            ()=>
            //                            GetService<PKS.Data.IRepository<MetadataDefinition>>()
            //                            .GetQuery()
            //                          );

            var metas = await GetService<PKS.Data.IRepository<MetadataDefinition>>()
                                            .GetQuery()
                                            .Include(e => e.Items)
                                            .Where(m => m.GroupCode != "")
                                            .OrderBy(e => e.GroupOrder)
                                            .ThenBy(e => e.ItemOrder)
                                            .ToArrayAsync();
            return metas;
        }
        public object ESSearchEx(object query)
        {
            return Task.Run(() => ESSearchExAsync(query)).Result;
        }
        public async Task<object> ESSearchExAsync(object query)
        {
            return await _esAccess.QueryAsync(query);
        }
    }
}