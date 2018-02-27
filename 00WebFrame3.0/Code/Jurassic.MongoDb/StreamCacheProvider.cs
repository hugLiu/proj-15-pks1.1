using Jurassic.AppCenter;
using Jurassic.AppCenter.Models;
using Jurassic.Com.Tools;
//using MongoDB;
//using MongoDB.Configuration;
//using MongoDB.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jurassic.MongoDb
{
    /// <summary>
    /// 基于MongoDb实现的流缓冲器
    /// </summary>
    public class StreamCacheProvider : ICacheProvider<Stream>
    {
        //MongoConfigurationBuilder config;
        //string mCollectionName;
        //Mongo mongo;

        //public StreamCacheProvider(string connstrOrKey, string collectionName)
        //{
        //    config = new MongoConfigurationBuilder();
        //    mCollectionName = collectionName;
        //    if (Regex.IsMatch(connstrOrKey, @"^[\w|\d|_]+$"))
        //    {
        //        config.ReadConnectionStringFromAppSettings(connstrOrKey);
        //    }
        //    else
        //    {
        //        config.ConnectionString(connstrOrKey);
        //    }
        //}

        //public Stream this[string key]
        //{
        //    get
        //    {
        //        Mongo mongo = new Mongo(config.BuildConfiguration());
        //        mongo.Connect();
        //        GridFile gf = new GridFile(mongo[mCollectionName]);
        //        if (!gf.Exists(key))
        //        {
        //            return null;
        //        }
             
        //        var stream = gf.OpenRead(key);
        //        return stream;
        //    }
        //    set
        //    {
        //        var mongo = new Mongo(config.BuildConfiguration());
        //        try
        //        { 
        //            mongo.Connect();
        //            GridFile gf = new GridFile(mongo[mCollectionName]);
        //            GridFileStream stream = gf.Exists(key) ? gf.OpenWrite(key) : gf.Create(key);
        //            IOHelper.CopyStream(value, stream);
        //        }
        //        finally
        //        {
        //            mongo.Disconnect();
        //        }
        //    }
        //}

        public StreamCacheProvider(string connstrOrKey, string collectionName)
        {

        }

        private Stream _inner_stream;
        public Stream this[string key]
        {
            get
            {
                return _inner_stream;
            }
            set
            {
                _inner_stream = value;
            }
        }

        public Stream Add(string key, Stream value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
        }

        public Stream Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
