using PKS.Core;
using PKS.Web;
using PKS.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PKS.WebAPI.Services
{
    /// <summary>索引数据服务接口</summary>
    public class IndexerServiceWrapper : ApiServiceWrapper, IIndexerService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public IndexerServiceWrapper(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>构造函数</summary>
        public IndexerServiceWrapper(IApiServiceConfig config) : base(config, nameof(IIndexerService).Substring(1))
        {
        }

        /// <summary>插入</summary>
        public string[] Insert(IndexInsertRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => InsertAsyncInternal(client, request)).Result;
        }

        /// <summary>插入</summary>
        public async Task<string[]> InsertAsync(IndexInsertRequest request)
        {
            var client = InitHttpClient();
            return await InsertAsyncInternal(client, request).ConfigureAwait(false);
        }

        /// <summary>插入</summary>
        public async Task<string[]> InsertAsyncInternal(HttpClientWrapper client, IndexInsertRequest request)
        {
            return await client.PostObjectAsync<string[]>(GetActionUrl(nameof(Insert)), request).ConfigureAwait(false);
        }
        /// <summary>保存</summary>
        public string[] Save(IndexSaveRequest request)
        {
            var client = InitHttpClient();
            return Task.Run(() => SaveAsyncInternal(client, request)).Result;
        }

        /// <summary>保存</summary>
        public async Task<string[]> SaveAsync(IndexSaveRequest request)
        {
            var client = InitHttpClient();
            return await SaveAsyncInternal(client, request).ConfigureAwait(false);
        }

        /// <summary>保存</summary>
        public async Task<string[]> SaveAsyncInternal(HttpClientWrapper client, IndexSaveRequest request)
        {
            return await client.PostObjectAsync<string[]>(GetActionUrl(nameof(Save)), request).ConfigureAwait(false);
        }
        /// <summary>删除</summary>
        public string[] Delete(List<string> iiids)
        {
            var client = InitHttpClient();
            return Task.Run(() => DeleteAsyncInternal(client, iiids)).Result;
        }

        /// <summary>删除</summary>
        public async Task<string[]> DeleteAsync(List<string> iiids)
        {
            var client = InitHttpClient();
            return await DeleteAsyncInternal(client, iiids).ConfigureAwait(false);
        }
        /// <summary>删除</summary>
        public async Task<string[]> DeleteAsyncInternal(HttpClientWrapper client, List<string> iiids)
        {
            return await client.PostObjectAsync<string[]>(GetActionUrl(nameof(Delete)), iiids).ConfigureAwait(false);
        }
    }
}