using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public class MongoDBService : IMongoDBService
    {
        private MongoDBAccess<MongoInfoItem> _access { get; set; }
        public MongoDBService()
        {
            _access = new MongoDBAccess<MongoInfoItem>(
                       ConfigurationManager.AppSettings["SourceMongoServer"],
                       ConfigurationManager.AppSettings["SourceMongoDB"],
                       ConfigurationManager.AppSettings["SourceMongoCol"]
                       );
        }
        public void RemoveBySource(List<SRC> sources)
            {
				foreach (SRC s in sources)
                    {
                        RemoveBySource(s);
                    }
             }
		public void RemoveBySource(SRC s)
        {
            FilterDefinition<MongoInfoItem> filter = Builders<MongoInfoItem>.Filter.And(
                        Builders<MongoInfoItem>.Filter.Eq(t => t.Usage.SRC.AdapterId, s.AdapterId),
						Builders<MongoInfoItem>.Filter.Eq(t => t.Usage.SRC.DataType, s.DataType),
						Builders<MongoInfoItem>.Filter.Eq(t => t.Usage.SRC.NatureKey, s.NatureKey));
            _access.Delete(filter);
        }
		public bool TryInInfoItemSource(SRC2 s, out MongoInfoItem infoItem)
        {
            FilterDefinition<MongoInfoItem> filter = Builders<MongoInfoItem>.Filter.And(
						Builders<MongoInfoItem>.Filter.Eq(t => t.Usage.SRC.AdapterId, s.AdapterId),
						Builders<MongoInfoItem>.Filter.Eq(t => t.Usage.SRC.DataType, s.DataType),
						Builders<MongoInfoItem>.Filter.Eq(t => t.Usage.SRC.NatureKey, s.NatureKey));
            infoItem = _access.GetOne(filter);
            return infoItem == null ? false : true;
        }
        public bool TryInInfoItemSource(string iiid, out MongoInfoItem infoItem)
        {
            FilterDefinition<MongoInfoItem> filter = Builders<MongoInfoItem>.Filter.And(
                        Builders<MongoInfoItem>.Filter.Eq(t => t.IIid, iiid));
            infoItem = _access.GetOne(filter);
            return infoItem == null ? false : true;
        }
        public void UpdateBatch(List<MongoInfoItem> infoItems)
        {
            foreach (MongoInfoItem item in infoItems)
            {
                _access.Replace(t => t._id == item._id, item);
            }
        }
        public void InsertBatch(List<MongoInfoItem> infoItems)
        {
            if (infoItems == null || infoItems.Count <= 0) return;
            _access.Insert(infoItems);
        }
    }
}
