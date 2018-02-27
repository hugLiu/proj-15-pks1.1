using Jurassic.AppCenter;
//using MongoDB;
//using MongoDB.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    /// <summary>
    /// MongoDB实现的键值对访问类
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        //Mongo mongo;
        //MongoConfigurationBuilder config;
        //string mCollectionName;

        //public CacheProvider(string connstrOrKey, string collectionName)
        //{
        //    mCollectionName = collectionName;
        //    config = new MongoConfigurationBuilder();
        //    if (Regex.IsMatch(connstrOrKey, @"^[\w|\d|_]+$"))
        //    {
        //        config.ReadConnectionStringFromAppSettings(connstrOrKey);
        //    }
        //    else
        //    {
        //        config.ConnectionString(connstrOrKey);
        //    }
        //    mongo = new Mongo(config.BuildConfiguration());
        //}

        //public object this[string key]
        //{
        //    get
        //    {
        //        mongo.Connect();
        //        var db = mongo["simple"];
        //        var categories = db.GetCollection<Document>(mCollectionName);
        //        var doc = categories.FindOne(new Document { { "_id", key } });
        //        if (doc == null) return null;
        //        mongo.Disconnect();
        //        return doc["content"];
        //    }
        //    set
        //    {
        //        mongo.Connect();
        //        var db = mongo["simple"];
        //        var categories = db.GetCollection<Document>(mCollectionName);
        //        var doc = new Document { { "_id", key } };
        //        doc["content"] = value;
        //        categories.Save(doc);
        //        mongo.Disconnect();
        //    }
        //}

        public CacheProvider(string connstrOrKey, string collectionName)
        {

        }

        private object _inner_value;
        public object this[string key]
        {
            get
            {
                return _inner_value;
            }
            set
            {
                _inner_value = value;
            }
        }

        public object Add(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
        }

        public object Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
