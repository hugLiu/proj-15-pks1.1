using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public interface IMongoDBService
    {
        void InsertBatch(List<MongoInfoItem> infoItems);
        void UpdateBatch(List<MongoInfoItem> infoItems);
        void RemoveBySource(List<SRC> sources);
        void RemoveBySource(SRC s);
        bool TryInInfoItemSource(SRC2 s, out MongoInfoItem infoItem);
        bool TryInInfoItemSource(string iiid, out MongoInfoItem infoItem);
    }
}
