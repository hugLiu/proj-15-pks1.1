using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Jurassic.MongoDb
{
    public class CacheMongoService : ICacheMongoService
    {
        private MongoDBAccess<CacheInfoItem> _access { get; set; }
        public CacheMongoService()
        {
            _access = new MongoDBAccess<CacheInfoItem>(
                       ConfigurationManager.AppSettings["CacheMongoServer"],
                       ConfigurationManager.AppSettings["CacheMongoDB"],
                       ConfigurationManager.AppSettings["CacheMongoCol"]
                       );
        }
        public void Insert(CacheInfoItem info)
        {
            _access.Insert(info);
        }
        public void InsertBatch(List<CacheInfoItem> infos)
        {
            if (infos == null || infos.Count <= 0) return;
            _access.Insert(infos);
        }
        public List<CacheInfoItem> GetPagination(ref PagerInfo pagerInfo)
        {
            return _access.GetPagination(Builders<CacheInfoItem>.Filter.Empty, null, null, ref pagerInfo);
        }
        public CacheInfoItem GetOne(string id)
        {
            var f = Builders<CacheInfoItem>.Filter.Eq(t => t._id, new ObjectId(id));
            return _access.GetOne(f);
        }
        public List<CacheInfoItem> GetMany(FilterDefinition<CacheInfoItem> filter, SortDefinition<CacheInfoItem> sorter, ProjectionDefinition<CacheInfoItem> projection)
        {
            return _access.GetMany(filter, sorter, projection);
        }
        public List<CacheInfoItem> GetMany(string taskLogId)
        {
            var filter = Builders<CacheInfoItem>.Filter.Eq(t => t.TaskLogInfoId, taskLogId);
            var sorter = Builders<CacheInfoItem>.Sort.Descending(t => t.StartDate);
            return GetMany(filter, sorter, null);
        }
        public List<CacheInfoItem> GetPagination(string adapterId, string dataType, double smallRate,double largeRate, ref PagerInfo pagerInfo)
        {
            IQueryable<CacheInfoItem> query = _access.GetQueryable();
            var list = query.Where(t => t.AdapterId == adapterId && t.DataType == dataType)
                                    .OrderByDescending(t => t.StartDate).ToList();
            string taskLogId = list.Select(t => t.TaskLogInfoId).Distinct().FirstOrDefault();
            var filter = Builders<CacheInfoItem>.Filter.And(
                Builders<CacheInfoItem>.Filter.Eq(t => t.TaskLogInfoId, taskLogId),
				Builders<CacheInfoItem>.Filter.Gte(t => t.CompletelyRate, smallRate),
				Builders<CacheInfoItem>.Filter.Lt(t => t.CompletelyRate, largeRate)
                );
            var sorter = Builders<CacheInfoItem>.Sort.Descending(t => t.StartDate);
            var projection = Builders<CacheInfoItem>.Projection.Exclude(t => t.InfoItem.Usage.SRC)
                .Exclude(t => t.InfoItem.FT)
                .Exclude(t => t.InfoItem.TB)
                .Exclude(t => t.InfoItem.CR);

            return _access.GetPagination(filter, sorter, null, ref pagerInfo);
        }
		public List<CacheInfoItem> GetPagination(string taskLogId, double smallRate, double largeRate, ref PagerInfo pagerInfo)
        {
				
		    var filter = Builders<CacheInfoItem>.Filter.And(
			   Builders<CacheInfoItem>.Filter.Eq(t => t.TaskLogInfoId, taskLogId),
			   Builders<CacheInfoItem>.Filter.Gte(t => t.CompletelyRate, smallRate),
			   Builders<CacheInfoItem>.Filter.Lt(t => t.CompletelyRate, largeRate)
			   );
            var sorter = Builders<CacheInfoItem>.Sort.Descending(t => t.StartDate);
            var projection = Builders<CacheInfoItem>.Projection
				.Exclude(t => t.InfoItem.Usage.SRC)
                .Exclude(t => t.InfoItem.FT)
                .Exclude(t => t.InfoItem.TB)
                .Exclude(t => t.InfoItem.CR);
			return _access.GetPagination(filter, sorter, null, ref pagerInfo);
        }


        public bool TryInInfoItemSource(TagManagerModel model, out CacheInfoItem infoItem)
        {
            FilterDefinition<CacheInfoItem> filter = Builders<CacheInfoItem>.Filter.And(
						Builders<CacheInfoItem>.Filter.Eq(t => t.InfoItem.Usage.SRC.AdapterId, model.AdapterId),
						Builders<CacheInfoItem>.Filter.Eq(t => t.InfoItem.Usage.SRC.DataType, model.DataType),
						Builders<CacheInfoItem>.Filter.Eq(t => t.InfoItem.Usage.SRC.NatureKey, model.NatureKey));
            infoItem = _access.GetOne(filter);
            if (infoItem != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> GetAdapterIdListFromCache()
        {
            List<string> adapterIds = _access.Distinct("AdapterId", Builders<CacheInfoItem>.Filter.Eq(t => t.InInfoItemSource, false));
            return adapterIds;
        }
        public List<string> GetDataTypesFromCache(string adapterId)
        {
            var f = Builders<CacheInfoItem>.Filter.And(
                Builders<CacheInfoItem>.Filter.Eq(t => t.AdapterId, adapterId),
                Builders<CacheInfoItem>.Filter.Eq(t => t.InInfoItemSource, false));
            List<string> dataTypes = _access.Distinct(t => t.DataType, f);
            return dataTypes;
        }
        public IndexDataQueryModel GetCurrentQueryModel(string adapterId, string dataType)
        {
            AggregateOptions opts = new AggregateOptions()
            {
                AllowDiskUse = true,
                BatchSize = int.MaxValue,
                MaxTime = TimeSpan.FromMinutes(10)
            };
            IAggregateFluent<CacheInfoItem> query = _access.GetAggregateFluent(opts);
            var model = query.Match(t => t.AdapterId == adapterId && t.DataType == dataType)
                .Project(t => new IndexDataQueryModel()
                {
                    TaskLogInfoId = t.TaskLogInfoId,
                    AdapterId = t.AdapterId,
                    DataType = t.DataType,
                    AdapterName = t.AdapterName,
                    CompletelyRate = t.CompletelyRate,
                    OperationState = t.OperationState,
                    InInfoItemSource = t.InInfoItemSource,
                    StartDate = t.StartDate
                })
                .SortByDescending(t => t.StartDate).FirstOrDefaultAsync().GetAwaiter().GetResult();
            return model;
        }
        public class DataTypeComparer : IEqualityComparer<IndexDataQueryModel>
        {
            bool IEqualityComparer<IndexDataQueryModel>.Equals(IndexDataQueryModel x, IndexDataQueryModel y)
            {
                if (Object.ReferenceEquals(x, y))
                    return true;
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;
                return x.AdapterId == y.AdapterId && x.DataType == y.DataType;
            }

            int IEqualityComparer<IndexDataQueryModel>.GetHashCode(IndexDataQueryModel obj)
            {
                return obj.DataType.GetHashCode();
            }
        }
        public void Update(string id, UpdateDefinition<CacheInfoItem> updates)
        {
            var f = Builders<CacheInfoItem>.Filter.Eq(t => t._id, new ObjectId(id));
            _access.UpdateOne(f, updates);
        }
        public void UpdateBatch(FilterDefinition<CacheInfoItem> filter, UpdateDefinition<CacheInfoItem> updates)
        {
            _access.UpdateMany(filter, updates);
        }
    }
}
