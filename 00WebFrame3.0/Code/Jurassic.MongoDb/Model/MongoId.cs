using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public abstract class MongoObjectId
    {
        public ObjectId _id { get; set; }
    }
}
