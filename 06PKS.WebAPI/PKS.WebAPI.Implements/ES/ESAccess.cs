using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using PKS.Core;
using PKS.Models;
using PKS.WebAPI.Models;
using PKS.WebAPI.ES;
using Newtonsoft.Json.Linq;
using PKS.Utils;
using PKS.WebAPI.Services;

namespace PKS.WebAPI.ES
{
    /// <summary>
    /// ES数据访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ESAccess<T> where T : class
    {
        /// <summary>
        /// ES客户端
        /// </summary>
        private static ElasticClient Client { get; set; }
        internal static string EsUri;
        internal static string EsType;
        internal static string EsIndex;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ESAccess()
        {
            Client = EsClient.Create(EsUri);

        }
        #region 插入索引
        /// <summary>
        /// 索引一个文档
        /// </summary>
        /// <param name="doc">文档</param>
        /// <returns></returns>
        public IIndexResponse IndexOne(T doc)
        {
            return Client.Index(doc);
        }
        /// <summary>
        /// 索引一个文档（异步）
        /// </summary>
        /// <param name="doc">文档</param>
        /// <returns></returns>
        public async Task<IIndexResponse> IndexOneAsync(T doc)
        {
            return await Client.IndexAsync(doc);
        }
        #endregion

        #region 删除索引
        /// <summary>
        /// 删除索引，支持批量
        /// </summary>
        /// <param name="query">查询符合条件的索引</param>
        /// <returns></returns>
        public IDeleteByQueryResponse DeleteByQuery(QueryContainer query)
        {
            return Client.DeleteByQuery(new DeleteByQueryDescriptor<T>(EsIndex));
        }
        /// <summary>
        /// 删除索引，支持批量 （异步）
        /// </summary>
        /// <param name="query">查询符合条件的索引</param>
        /// <returns></returns>
        public async Task<IDeleteByQueryResponse> DeleteByQueryAsync(QueryContainer query)
        {
            return await Client.DeleteByQueryAsync(new DeleteByQueryDescriptor<T>(EsIndex));
        }
        #endregion

        #region 更新索引
        /// <summary>
        /// 更新全部字段（All Fields）
        /// </summary>
        /// <param name="request">更新请求</param>
        /// <returns></returns>
        public IUpdateResponse<T> UpdateAFields(IUpdateRequest<T, T> request)
        {
            return Client.Update<T, T>(request);
        }
        /// <summary>
        /// 更新全部字段（All Fields）（异步）
        /// </summary>
        /// <param name="request">更新请求</param>
        /// <returns></returns>
        public async Task<IUpdateResponse<T>> UpdateAFieldsAsync(IUpdateRequest<T, T> request)
        {
            return await Client.UpdateAsync<T, T>(request);
        }
        /// <summary>
        /// 更新部分字段（Patial Fields）
        /// </summary>
        /// <typeparam name="T2">包含部分字段的实体类</typeparam>
        /// <param name="request">更新请求</param>
        /// <returns></returns>
        public IUpdateResponse<T> UpdatePFields<T2>(IUpdateRequest<T, T2> request) where T2 : class
        {
            return Client.Update(request);
        }
        /// <summary>
        /// 更新部分字段（Patial Fields）（异步）
        /// </summary>
        /// <typeparam name="T2">包含部分字段的实体类</typeparam>
        /// <param name="request">更新请求</param>
        /// <returns></returns>
        public async Task<IUpdateResponse<T>> UpdatePFieldsAsync<T2>(IUpdateRequest<T, T2> request) where T2 : class
        {
            return await Client.UpdateAsync(request);
        }
        /// <summary>
        /// 更新部分字段（Patial Fields）T2=》object
        /// </summary>
        /// <param name="request">更新请求</param>
        /// <returns></returns>
        public IUpdateResponse<T> UpdatePFields(IUpdateRequest<T, object> request)
        {
            return Client.Update(request);
        }
        /// <summary>
        /// 更新部分字段（Patial Fields）T2=》object（异步）
        /// </summary>
        /// <param name="request">更新请求</param>
        /// <returns></returns>
        public async Task<IUpdateResponse<T>> UpdatePFieldsAsync(IUpdateRequest<T, object> request)
        {
            return await Client.UpdateAsync(request);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 根据文档唯一ID查询指定文档
        /// </summary>
        /// <param name="id">文档ID</param>
        /// <returns></returns>
        public IGetResponse<T> Get(string id)
        {
            return Client.Get(new DocumentPath<T>(id));
        }
        /// <summary>
        /// 根据文档唯一ID查询指定文档(异步)
        /// </summary>
        /// <param name="id">文档ID</param>
        /// <returns></returns>
        public async Task<IGetResponse<T>> GetAsync(string id)
        {
            return await Client.GetAsync(new DocumentPath<T>(id));
        }
        /// <summary>
        /// 根据多个id返回指定的文档列表
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        public IMultiGetResponse MultiGet(IEnumerable<string> ids)
        {
            return Client.MultiGet(m => m.GetMany<T>(ids));
        }
        /// <summary>
        /// 根据多个id返回指定的文档列表（异步）
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        public async Task<IMultiGetResponse> MultiGetAsync(IEnumerable<string> ids)
        {
            return await Client.MultiGetAsync(m => m.GetMany<T>(ids));
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="sort">排序规则</param>
        /// <param name="fields">返回字段</param>
        /// <param name="aggs">聚合条件</param>
        /// <param name="from">记录开始</param>
        /// <param name="size">记录数</param>
        /// <returns></returns>
        public ISearchResponse<T> PagingQuery(QueryContainer query, SortDescriptor<T> sort, SourceFilterDescriptor<T> fields,
            AggregationContainerDescriptor<T> aggs, int from = 0, int size = 10)
        {
            var result = Client.Search<T>(s => BuildSearchDescriptor(query, sort, fields, aggs, from, size));
            result.ThrowIfIsNotValid();
            return result;
        }
        /// <summary>
        /// 分页查询（异步）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="sort">排序规则</param>
        /// <param name="fields">返回字段</param>
        /// <param name="aggs">聚合条件</param>
        /// <param name="from">记录开始</param>
        /// <param name="size">记录数</param>
        /// <returns></returns>
        public async Task<ISearchResponse<T>> PagingQueryAsync(QueryContainer query, SortDescriptor<T> sort, SourceFilterDescriptor<T> fields,
         AggregationContainerDescriptor<T> aggs, int from = 0, int size = 10)
        {
            var result = await Client.SearchAsync<T>(s => BuildSearchDescriptor(query, sort, fields, aggs, from, size));
            result.ThrowIfIsNotValid();
            return result;
        }
        /// <summary>
        /// 分页查询，只返回文档信息
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="sort">排序规则</param>
        /// <param name="fields">返回字段</param>
        /// <param name="from">记录开始</param>
        /// <param name="size">记录数</param>
        /// <returns></returns>
        public IEnumerable<T> GetDocuments(QueryContainer query, SortDescriptor<T> sort,
           SourceFilterDescriptor<T> fields, int from = 0, int size = 10)
        {
            return PagingQuery(query, sort, fields, null, from, size).Documents;
        }
        /// <summary>
        /// 分页查询，只返回文档信息（异步）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="sort">排序规则</param>
        /// <param name="fields">返回字段</param>
        /// <param name="from">记录开始</param>
        /// <param name="size">记录数</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetDocumentsAsync(QueryContainer query, SortDescriptor<T> sort,
         SourceFilterDescriptor<T> fields, int from = 0, int size = 10)
        {
            var queryResult = await PagingQueryAsync(query, sort, fields, null, from, size);
            return queryResult.Documents;
        }
        #endregion

        #region Builder 条件构建器
        /// <summary>
        /// 构建查询描述器
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sort"></param>
        /// <param name="fields"></param>
        /// <param name="aggs"></param>
        /// <param name="from"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public SearchDescriptor<T> BuildSearchDescriptor(QueryContainer query, SortDescriptor<T> sort, ISourceFilter fields,
            IAggregationContainer aggs, int from = 0, int size = 10)
        {

            var searchdesc = new SearchDescriptor<T>()
                .Index(EsIndex)
                .Type(EsType)
                .From(from)
                .Size(size);

            if (query != null) searchdesc = searchdesc.Query(q => query);
            if (sort != null) searchdesc = searchdesc.Sort(s => sort);
            if (fields != null) searchdesc = searchdesc.Source(s => fields);
            if (aggs != null) searchdesc = searchdesc.Aggregations(a => aggs);

            return searchdesc;
        }
        #endregion

        private string ConvertToJsonQuery(ISearchRequest searchRequest)
        {
            string jsonQuery = string.Empty;
            jsonQuery = Client.Serializer.SerializeToString(searchRequest);
            return jsonQuery;
        }

        public async Task<string> GetDocumentsByRawQueryAsyn(string rawQuery, int from = 0, int size = 10)
        {
            ElasticsearchResponse<string> elasticSearchResult = null;
            elasticSearchResult = await Client.LowLevel.SearchAsync<string>(EsIndex, EsType, rawQuery);
            if (elasticSearchResult.ServerError != null && elasticSearchResult.ServerError.Error != null)
            {
                throw new UserFriendlyException(elasticSearchResult.OriginalException, "InvalidEsRequest", elasticSearchResult.OriginalException.Message, elasticSearchResult.ServerError.ToString());
            }
            return elasticSearchResult.Body;
        }


        public async Task<object> GetDocumentsByMRawQueryAsyn(string rawQuery)
        {
            ElasticsearchResponse<string> elasticSearchResult = null;
            elasticSearchResult = await Client.LowLevel.MsearchAsync<string>(EsIndex, EsType, rawQuery);
   
            if (elasticSearchResult.ServerError != null && elasticSearchResult.ServerError.Error != null)
            {
                throw new UserFriendlyException(elasticSearchResult.OriginalException, "InvalidEsRequest", elasticSearchResult.OriginalException.Message, elasticSearchResult.ServerError.ToString());
            }
            var jResult = JObject.Parse(elasticSearchResult.Body);
            var jArray = jResult["responses"] as JArray;
            List<SimpleSearchResult> results=new List<SimpleSearchResult>();
            foreach (var arrayItem in jArray)
            {
                long took = arrayItem["took"].ToObject<long>();
                long total = arrayItem["hits"]["total"].ToObject<long>();
                List<object> docs = new List<object>();
                foreach (JToken hit in arrayItem["hits"]["hits"])
                {
                    object _source = hit["_source"].ToObject<object>();
                    docs.Add(_source);
                }
                results.Add(new SimpleSearchResult() { Took = took, Total = total, Results = docs });
            }

            return results;
        }

        public async Task<object> QueryAsync(object query)
        {
            string rawQuery = query.ToJson();
            ElasticsearchResponse<string> elasticSearchResult = await Client.LowLevel.SearchAsync<string>(EsIndex, EsType, rawQuery);
            if (elasticSearchResult.ServerError != null && elasticSearchResult.ServerError.Error != null)
            {
                throw new UserFriendlyException(elasticSearchResult.OriginalException, "InvalidEsRequest", elasticSearchResult.OriginalException.Message, elasticSearchResult.ServerError.ToString());
            }

            JObject jResult = JObject.Parse(elasticSearchResult.Body);
            long took = jResult["took"].ToObject<long>();
            long total = jResult["hits"]["total"].ToObject<long>();
            List<object> docs = new List<object>();
            foreach (JToken hit in jResult["hits"]["hits"])
            {
                object _source = hit["_source"].ToObject<object>();
                docs.Add(_source);
            }

            return new SimpleSearchResult() { Took = took, Total = total, Results = docs };
        }

        public async Task<IMultiSearchResponse> MultiSearch(MultiSearchDescriptor multiSearchDescriptor)
        {
            var result2 = Client.MultiSearch(ms => multiSearchDescriptor);

            IMultiSearchResponse result = null;
            result = await Client.MultiSearchAsync(ms => multiSearchDescriptor);
            if (!result.IsValid)
            {
                result.OriginalException.Throw("InvalidEsRequest", result.ServerError.ToString());
            }
            return result;
        }
    }
}
