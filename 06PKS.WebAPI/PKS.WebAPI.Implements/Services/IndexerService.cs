using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Jurassic.PKS.Service;
using Nest;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using TIndexType = PKS.Models.Metadata;

namespace PKS.WebAPI.Services
{
    /// <summary>索引数据服务接口</summary>
    public class IndexerService : IIndexerService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public IndexerService(IElasticConfig elasticConfig, ISearchService searchService, IServerAppDataService appDataService)
        {
            this.Client = elasticConfig.Client.As<ElasticClient>();
            this.IndexType = elasticConfig.MetadataType.As<TypeName>();
            var metadataDefinitionCollection = searchService.GetMetadataDefinitions();
            MetadataDefinitionCollection.Instance = new MetadataDefinitionCollection(metadataDefinitionCollection);
            this.RequiredMetadataDefinitions = metadataDefinitionCollection.Where(e => e.Required && e.GroupCode != MetadataGroupCode.Inner).ToArray();
            this.AppDataService = appDataService;
        }
        /// <summary>客户端</summary>
        private ElasticClient Client { get; }
        /// <summary>索引类型</summary>
        private TypeName IndexType { get; }
        /// <summary>必需的元数据定义集合</summary>
        private MetadataDefinition[] RequiredMetadataDefinitions { get; }
        /// <summary>应用数据服务</summary>
        private IServerAppDataService AppDataService { get; }

        /// <summary>插入</summary>
        public string[] Insert(IndexInsertRequest request)
        {
            return Task.Run(() => InsertAsync(request)).Result;
        }

        /// <summary>插入</summary>
        public async Task<string[]> InsertAsync(IndexInsertRequest request)
        {
            await Validate(request.Metadatas, false);
            foreach (TIndexType metadata in request.Metadatas)
            {
                var esresult = await this.Client.IndexAsync<TIndexType>(metadata, d => UseInsertQuery(d, metadata));
                esresult.ThrowIfIsNotValid();
            }
            return request.Metadatas.Select(e => e.IIId).ToArray();
        }
        /// <summary>验证请求</summary>
        private async Task Validate(MetadataCollection metadatas, bool replace)
        {
            foreach (var metadata in metadatas)
            {
                metadata.ClearNullOrEmpty();
                if (replace)
                {
                    var tags = this.RequiredMetadataDefinitions;
                    foreach (var tag in tags)
                    {
                        if (metadata.GetValue(tag.Name, true) == null)
                        {
                            ApiServiceExceptionCodes.MetadataTagMissing.ThrowUserFriendly("缺少元数据标签！", $"元数据标签{tag.Name}不存在！");
                        }
                    }
                }
                else
                {
                    if (metadata.ResourceKey == null)
                    {
                        ApiServiceExceptionCodes.MetadataTagMissing.ThrowUserFriendly("缺少元数据标签！", $"元数据标签{MetadataConsts.ResourceKey}不存在！");
                    }
                }
                metadata.IIId = metadata.ResourceKey.ToUpperInvariant().ToMD5();
                metadata.IndexedDate = DateTime.UtcNow;

                var thumbnail = metadata.Thumbnail;
                if (!thumbnail.IsNullOrEmpty()) await this.AppDataService.BuildThumbnail(metadata, thumbnail);
                var fulltext = metadata.Fulltext;
                if (!fulltext.IsNullOrEmpty()) await this.AppDataService.BuildFulltext(metadata, fulltext);
            }
        }
        /// <summary>生成插入查询</summary>
        private IIndexRequest UseInsertQuery(IndexDescriptor<TIndexType> descriptor, TIndexType metadata)
        {
            descriptor.Type(this.IndexType)
                .Id(metadata.IIId)
                ;
            return descriptor;
        }
        /// <summary>保存</summary>
        public string[] Save(IndexSaveRequest request)
        {
            return Task.Run(() => SaveAsync(request)).Result;
        }

        /// <summary>保存</summary>
        public async Task<string[]> SaveAsync(IndexSaveRequest request)
        {
            await Validate(request.Metadatas, request.Replace);
            foreach (TIndexType metadata in request.Metadatas)
            {
                IResponse esresult = null;
                if (request.Replace)
                {
                    esresult = await this.Client.IndexAsync<TIndexType>(metadata, d => UseInsertQuery(d, metadata));
                }
                else
                {
                    var path = DocumentPath<TIndexType>.Id(metadata.IIId);
                    esresult = await this.Client.UpdateAsync<TIndexType>(path, descriptor => UseUpdateQuery(descriptor, metadata));
                }
                esresult.ThrowIfIsNotValid();
            }
            return request.Metadatas.Select(e => e.IIId).ToArray();
        }
        /// <summary>生成替换查询</summary>
        private IUpdateRequest<TIndexType, TIndexType> UseReplaceQuery(UpdateDescriptor<TIndexType, TIndexType> descriptor, TIndexType metadata)
        {
            descriptor.Type(this.IndexType)
                .Doc(metadata)
                .DocAsUpsert(true)
                .Upsert(metadata)
                ;
            return descriptor;
        }
        /// <summary>生成部分更新查询</summary>
        private IUpdateRequest<TIndexType, TIndexType> UseUpdateQuery(UpdateDescriptor<TIndexType, TIndexType> descriptor, TIndexType metadata)
        {
            descriptor.Type(this.IndexType)
                .Doc(metadata)
                .DocAsUpsert(true)
                .Upsert(metadata)
                ;
            return descriptor;
        }
        /// <summary>生成按查询更新请求</summary>
        private IUpdateByQueryRequest UseUpdateQuery(UpdateByQueryDescriptor<TIndexType> updateDescriptor, IndexSaveRequest request)
        {
            var resourceKeys = request.Metadatas.Select(e => e.ResourceKey).ToArray();
            updateDescriptor
                .Type(this.IndexType)
                .Query(queryDescriptor => queryDescriptor
                    .Terms(e => e
                        .Field(f => f[MetadataConsts.ResourceKey])
                        .Terms(resourceKeys)
                    )
                );
            return updateDescriptor;
        }
        /// <summary>删除</summary>
        public string[] Delete(List<string> iiids)
        {
            return Task.Run(() => DeleteAsync(iiids)).Result;
        }

        /// <summary>删除</summary>
        public async Task<string[]> DeleteAsync(List<string> iiids)
        {
            if (iiids == null) return null;
            if (iiids.Count == 0) return new string[0];
            //Parallel.ForEach(iiids, iiid)
            foreach (var iiid in iiids)
            {
                var path = DocumentPath<TIndexType>.Id(iiid);
                var esresult = await this.Client.DeleteAsync<TIndexType>(path);
                esresult.ThrowIfIsNotValid();
            }
            return iiids.ToArray();
        }
        /// <summary>生成删除请求</summary>
        private IDeleteRequest UseDeleteQuery(DeleteDescriptor<TIndexType> descriptor, string iiid)
        {
            descriptor.Type(this.IndexType)
                .Refresh(Refresh.True)
                ;
            return descriptor;
        }
        /// <summary>生成按查询删除请求</summary>
        private IDeleteByQueryRequest UseDeleteQuery(DeleteByQueryDescriptor<TIndexType> descriptor, List<string> iiids)
        {
            descriptor.Type(this.IndexType);
            descriptor.Query(queryDescriptor => queryDescriptor
                .Terms(e => e
                    .Field(f => f[MetadataConsts.IIId])
                    .Terms(iiids)
                    )
                )
                .Refresh(true)
                ;
            return descriptor;
        }
    }
}