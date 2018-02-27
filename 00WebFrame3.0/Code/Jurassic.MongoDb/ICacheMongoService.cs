using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Jurassic.MongoDb
{
    public interface ICacheMongoService
    {
        void Insert(CacheInfoItem info);
        void InsertBatch(List<CacheInfoItem> infos);
        void UpdateBatch(FilterDefinition<CacheInfoItem> filter, UpdateDefinition<CacheInfoItem> updates);
        List<CacheInfoItem> GetMany(FilterDefinition<CacheInfoItem> filter, SortDefinition<CacheInfoItem> sorter, ProjectionDefinition<CacheInfoItem> projection);
		List<CacheInfoItem> GetPagination(string taskLogId, double smallRate, double largeRate, ref PagerInfo pagerInfo);
		List<CacheInfoItem> GetPagination(string adapterId, string dataType, double smallRate, double largeRate, ref PagerInfo pagerInfo);
        List<string> GetAdapterIdListFromCache();
        List<string> GetDataTypesFromCache(string adapterId);


    }
}
