using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using Ninject.Activation;
using PKS.Utils;

namespace PKS.WebAPI.Services
{
    /// <summary>Mongo扩展</summary>
    public class MongoColletionProvider<T> : Provider<T>
    {
        /// <summary>构造函数</summary>
        public MongoColletionProvider(IMongoConfig config)
        {
            this.Config = config;
        }
        /// <summary>配置</summary>
        private IMongoConfig Config { get; }
        /// <summary>
        /// Creates an instance within the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The created instance.</returns>
        protected override T CreateInstance(IContext context)
        {
            var database = this.Config.Database.As<IMongoDatabase>();
            var colletionType = typeof(T);
            var docType = colletionType.GenericTypeArguments.First();
            var colletionName = this.Config.GetColletionName(docType);
            var getCollectionGenericMethod = database.GetType().GetMethod(nameof(database.GetCollection));
            var getCollectionMethod = getCollectionGenericMethod.MakeGenericMethod(docType);
            return (T)getCollectionMethod.Invoke(database, new object[] { colletionName, null });
        }
    }
}