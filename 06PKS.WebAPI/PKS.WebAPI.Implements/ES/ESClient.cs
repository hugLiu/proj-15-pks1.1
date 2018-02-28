using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace PKS.WebAPI.ES
{
    /// <summary>
    /// ES客户端
    /// </summary>
    public class EsClient
    {
        /// <summary>
        /// ES链接客户端
        /// </summary>
        public ElasticClient Client { get; }

        /// <summary>
        /// 创建url
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="host">IP地址</param>
        /// <returns></returns>
        public static Uri CreateUri(int port = 9200, string host = "localhost") => new UriBuilder("http", host, port).Uri;

        /// <summary>
        /// 根据uri单个节点ES客户端
        /// </summary>
        /// <param name="uri"></param>
        public EsClient(string uri)
        {
            var node = new Uri(uri);
            var settings = new ConnectionSettings(node);
            Client = new ElasticClient(settings);
        }

        /// <summary>
        /// 根据ip和端口创建ES客户端
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        public EsClient(int port = 9200, string host = "localhost")
        {
            var node = CreateUri(port, host);
            var settings = new ConnectionSettings(node);
            Client = new ElasticClient(settings);
        }

        /// <summary>
        /// 多个节点ES客户端
        /// </summary>
        /// <param name="uris"></param>
        public EsClient(IEnumerable<string> uris)
        {
            var nodes = uris.Select(uri => new Uri(uri)).ToList();
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            Client = new ElasticClient(settings);
        }

        /// <summary>
        /// 创建ES客户端静态方法
        /// </summary>
        /// <param name="uris"></param>
        /// <returns></returns>
        public static ElasticClient Create(params string[] uris)
        {
            return uris.Length > 1 ? new EsClient(uris).Client : new EsClient(uris.First()).Client;
        }
    }
}
