using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    [BsonIgnoreExtraElements]
    public class MongoInfoItem : InfoItem2
    {
        public ObjectId _id { get; set; }

        /// <summary>
        /// 用于同步工具，表示InfoItem在Mongodb中的创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 用于同步工具，表示InfoItem在Mongodb中的最后更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 用于同步工具，表示InfoItem删除时间
        /// </summary>
        public DateTime DeleteDate { get; set; }
    }
}
