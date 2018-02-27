using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Models;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.DbServices.IndexApp
{
   public class IndexMetadataService : AppService, IPerRequestAppService
   {
       private IAppDataService _appDataService;
       private IIndexerService _indexerService;
        public IndexMetadataService(IAppDataService appDataService, IIndexerService indexerService)
        {
            _appDataService = appDataService;
            _indexerService = indexerService;
        }
        /// <summary>
        /// 保存索引数据
        /// </summary>
        public bool SaveAppIndex(Dictionary<Metadata, AppDataSaveRequest> indexMetadatas)
        {
            //保存App Data
            var resourceKeys = indexMetadatas.Where(item => item.Value != null).Select(item => item.Value.ResourceKey).ToList();
            var existsResourceKeys = new List<string>();
            var notExistsDbResourceKeys = new List<string>();
            var appDataMatchResult = GetExistsResourceKeys(resourceKeys.Select(item => item as object).ToArray());
            if (appDataMatchResult != null && appDataMatchResult.Total > 0)
            {
                existsResourceKeys = appDataMatchResult.Values.Select(item => item.ResourceKey).ToList();
            }
            if (existsResourceKeys.Any())
            {
                IndexDataSaveRequest<AppDataSaveRequest> appDataSaveRequests = new IndexDataSaveRequest<AppDataSaveRequest>();
                appDataSaveRequests.Values = indexMetadatas.Where(item => item.Value != null && notExistsDbResourceKeys.Contains(item.Value.ResourceKey)).Select(item => item.Value).ToList();
                var appDataSaveResult = _appDataService.SaveMany(appDataSaveRequests);
            }
            notExistsDbResourceKeys = resourceKeys.Where(item => !existsResourceKeys.Contains(item)).ToList();

            if (notExistsDbResourceKeys.Any())
            {
                IndexDataSaveRequest<AppDataSaveRequest> appDataInsertRequests =
                    new IndexDataSaveRequest<AppDataSaveRequest>();
                appDataInsertRequests.Values = indexMetadatas.Where(item => item.Value != null && notExistsDbResourceKeys.Contains(item.Value.ResourceKey)).Select(item => item.Value).ToList();
                var appDataResult = _appDataService.InsertMany(appDataInsertRequests);
                //
                for (int i = 0; i < appDataResult.Length; i++)
                {
                    var resourceKey = notExistsDbResourceKeys[i];
                    var kvpIndexMetadata = indexMetadatas.FirstOrDefault(item => item.Value.ResourceKey == resourceKey);
                    if (!string.IsNullOrWhiteSpace(appDataResult[i].DataId))
                    {
                        kvpIndexMetadata.Key.DataId = appDataResult[i].DataId;
                    }
                    if (!string.IsNullOrWhiteSpace(appDataResult[i].Thumbnail))
                    {
                        kvpIndexMetadata.Key.Thumbnail = appDataResult[i].Thumbnail;
                    }
                    if (!string.IsNullOrWhiteSpace(appDataResult[i].Fulltext))
                    {
                        kvpIndexMetadata.Key.Fulltext = appDataResult[i].Fulltext;
                    }
                }
            }



            //保存Es Metadta
            var iiids = indexMetadatas.Select(item => item.Key.IIId).ToList();
            //var existsDbIIIds = new List<string>();
            //var notExistsDbIIIds = new List<string>();
            //var esMetadataMatchResult = GetExistsIIIds(iiids.Select(item => item as object).ToArray());
            //if (esMetadataMatchResult != null && esMetadataMatchResult.Total > 0)
            //{
            //    existsDbIIIds = esMetadataMatchResult.Metadatas.Select(item => item.IIId).ToList();
            //}
            //notExistsDbIIIds = iiids.Where(item => !existsDbIIIds.Contains(item)).ToList();
            //if (existsDbIIIds.Any())
            //{
                var saveRequest = new IndexSaveRequest();
               // var metadatas = indexMetadatas.Where(item => existsDbIIIds.Contains(item.Key.IIId)).Select(item => item.Key).ToList();
            var metadatas = indexMetadatas.Select(item => item.Key).ToList();
            saveRequest.Metadatas = new MetadataCollection(metadatas);
                saveRequest.Replace = true;
                _indexerService.Save(saveRequest);
           // }
            //if (notExistsDbIIIds.Any())
            //{
            //    var request = new IndexInsertRequest();
            //    var metadatas = indexMetadatas.Where(item => notExistsDbIIIds.Contains(item.Key.IIId)).Select(item => item.Key).ToList();
            //    request.Metadatas = new MetadataCollection(metadatas);
            //    _indexerService.Insert(request);
            //}
            return true;
        }

        private IndexDataMatchResult<IndexAppData> GetExistsResourceKeys(object[] resourceKeysFilter)
        {
            if (resourceKeysFilter == null || resourceKeysFilter.Length == 0)
                return null;
         
            var indexDataMatchRequest = new IndexDataMatchRequest();
            indexDataMatchRequest.From = 0;
            indexDataMatchRequest.Size = 10;
            var dataMatchFilter = new Dictionary<string, object[]>();
            dataMatchFilter.Add("resourcekey", resourceKeysFilter);
            indexDataMatchRequest.Filter = dataMatchFilter;
            var dataMatchSort = new List<PKSKeyValuePair<string, int>>();
            dataMatchSort.Add(new PKSKeyValuePair<string, int>("createby", 1));
            indexDataMatchRequest.Sort = dataMatchSort;
            return _appDataService.Match(indexDataMatchRequest);
        }

        private MatchResult GetExistsIIIds(object[] iiidsFilter)
        {
            if (iiidsFilter == null || iiidsFilter.Length == 0)
                return null;
            var searchService = GetService<ISearchService>();
            var matchRequest = new MatchRequest();
            matchRequest.Top = 10;
            var dataMatchFilter = new Dictionary<string, object[]>();
            dataMatchFilter.Add("iiid", iiidsFilter);
            matchRequest.Filter = dataMatchFilter;
            var dataMatchSort = new List<PKSKeyValuePair<string, object>>();
            dataMatchSort.Add(new PKSKeyValuePair<string, object>("iiid", 1));
            matchRequest.Sort = dataMatchSort;
            var fieldFilter = new SearchSourceFilter();
            fieldFilter.Includes = new List<string> { "iiid", "dataid" };
            matchRequest.Fields = fieldFilter;
            return searchService.Match(matchRequest);
        }

       public bool ExistsMetadata(string iiid)
       {
           var matchResult = GetExistsIIIds(new object[] {iiid});
           if (matchResult != null && matchResult.Total > 0)
           {
               return true;
           }
           return false;
       }

       public bool DeleteEsRecord(string iiid)
       {
           var result=_indexerService.Delete(new List<string>() {iiid});
            //todo 删除Mongodb相关记录
           return true;
       }
   }
}
