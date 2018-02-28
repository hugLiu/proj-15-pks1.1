using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PKS.Core;
using PKS.Services;
using PKS.Utils;
using PKS.WebAPI.Models;
using TDocument = PKS.WebAPI.Models.IndexPageData;

namespace PKS.WebAPI.Services
{
    /// <summary>服务端页面数据服务</summary>
    public class ServerPageDataService : IServerPageDataService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public ServerPageDataService(IMongoConfig config, IMongoCollection<TDocument> accessor, IMongoCollection<MongoUploadFile> uploadFilesCollection)
        {
            Accessor = accessor;
            FileFormatExtension.Init();
            this.UploadFileHandler = new UploadFileHandler(config, uploadFilesCollection);
        }

        /// <summary>访问器</summary>
        private IMongoCollection<TDocument> Accessor { get; }

        /// <summary>上传文件处理器</summary>
        private UploadFileHandler UploadFileHandler { get; }
        /// <summary>上传文件，支持秒传和分片</summary>
        public UploadResult Upload(ServerUploadRequest request)
        {
            return Task.Run(() => UploadAsync(request)).Result;
        }

        /// <summary>上传文件，支持秒传和分片</summary>
        public async Task<UploadResult> UploadAsync(ServerUploadRequest request)
        {
            return await this.UploadFileHandler.UploadAsync(request);
        }
        /// <summary>批量插入</summary>
        public string[] InsertMany(IndexDataSaveRequest<TDocument> request)
        {
            return Task.Run(() => InsertManyAsync(request)).Result;
        }

        /// <summary>批量插入</summary>
        public async Task<string[]> InsertManyAsync(IndexDataSaveRequest<TDocument> request)
        {
            Validate(request.Values);
            await this.Accessor.InsertManyAsync(request.Values);
            return request.Values.Select(e => e.PageId).ToArray();
        }
        /// <summary>保存</summary>
        private void Validate(List<TDocument> docs)
        {
            foreach (var doc in docs)
            {
                if (doc.PageId.IsNullOrEmpty())
                {
                    //if (doc.ResourceKey.IsNullOrEmpty())
                    //{
                        doc.PageId = Guid.NewGuid().ToString();
                    //}
                    //else
                    //{
                    //    doc.PageId = doc.ResourceKey.ToUpperInvariant().ToMD5();
                    //}
                }
                doc.Id = doc.As<IMongoDocument>().Id;
            }
        }
        /// <summary>批量保存</summary>
        public string[] SaveMany(IndexDataSaveRequest<TDocument> request)
        {
            return Task.Run(() => SaveManyAsync(request)).Result;
        }

        /// <summary>批量保存</summary>
        public async Task<string[]> SaveManyAsync(IndexDataSaveRequest<TDocument> request)
        {
            var updateOptions = new UpdateOptions { IsUpsert = true };
            Validate(request.Values);
            foreach (var doc in request.Values)
            {
                var filter = Builders<TDocument>.Filter.Eq(e => e.Id, doc.Id);
                await Accessor.ReplaceOneAsync(filter, doc, updateOptions);
            }
            return request.Values.Select(e => e.PageId).ToArray();
        }
        /// <summary>批量删除</summary>
        public string[] DeleteMany(List<string> pageIds)
        {
            return Task.Run(() => DeleteManyAsync(pageIds)).Result;
        }

        /// <summary>批量删除</summary>
        public async Task<string[]> DeleteManyAsync(List<string> pageIds)
        {
            if (pageIds == null) return null;
            if (pageIds.Count == 0) return new string[0];
            var filter = Builders<TDocument>.Filter.In(e => e.Id, pageIds);
            await Accessor.DeleteManyAsync(filter);
            return pageIds.ToArray();
        }
        /// <summary>根据PageID获得一条应用数据</summary>
        public TDocument Get(string pageId)
        {
            return Task.Run(() => GetAsync(pageId)).Result;
        }

        /// <summary>根据PageID获得一条应用数据</summary>
        public async Task<TDocument> GetAsync(string pageId)
        {
            var result = await Accessor
                .AsQueryable()
                .Where(e => e.Id == pageId)
                .FirstOrDefaultAsync();
            if (result == null)
            {
                var message = "PageId相关数据不存在";
                ApiServiceExceptionCodes.PageIdNotExists.ThrowUserFriendly(message, message);
            }
            return result;
        }
        /// <summary>根据PageID数组获得对应的多条应用数据</summary>
        public Dictionary<string, TDocument> GetMany(List<string> pageIds)
        {
            return Task.Run(() => GetManyAsync(pageIds)).Result;
        }

        /// <summary>根据PageID数组获得对应的多条应用数据</summary>
        public async Task<Dictionary<string, TDocument>> GetManyAsync(List<string> pageIds)
        {
            if (pageIds == null) return null;
            if (pageIds.Count == 0) return new Dictionary<string, TDocument>();
            var result = await Accessor
                .AsQueryable()
                .Where(e => pageIds.Contains(e.Id))
                .ToListAsync();
            return result.ToDictionary(e => e.PageId);
        }
        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        public IndexDataMatchResult<TDocument> Match(IndexDataMatchRequest request)
        {
            return Task.Run(() => MatchAsync(request)).Result;
        }

        /// <summary>根据条件和分页参数获得应用数据集合</summary>
        public async Task<IndexDataMatchResult<TDocument>> MatchAsync(IndexDataMatchRequest request)
        {
            var filter = Accessor.BuildFilter<TDocument>(request.Filter);
            var options = Accessor.BuildPager(request);
            options.Sort = Accessor.BuildSort(request.Sort);
            var result = new IndexDataMatchResult<TDocument>();
            result.Total = Convert.ToInt32(await Accessor.CountAsync(filter));
            using (var cursor = await Accessor.FindAsync(filter, options))
            {
                result.Values = await cursor.ToListAsync();
                return result;
            }
        }
        /// <summary>根据PageID获得一条应用数据</summary>
        public DownloadResult Download(DownloadRequest request)
        {
            return Task.Run(() => DownloadAsync(request)).Result;
        }

        /// <summary>根据PageID或FileID获得相关文件流</summary>
        public async Task<DownloadResult> DownloadAsync(DownloadRequest request)
        {
            return await this.UploadFileHandler.DownloadAsync(request, null);
        }
    }
}