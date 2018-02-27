using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Jurassic.MongoDb
{
    public class MongoDBAccess<T>
    {
        
        private string _connectStr = string.Empty;
        private string _database = string.Empty;
        private string _collection = string.Empty;

        #region 构造函数
        
        public MongoDBAccess(string connectStr, string database, string collection)
        {
            _connectStr = connectStr;
            _database = database;
            _collection = collection;
        }
        #endregion

        private IMongoCollection<T> GetCollection()
        {
            MongoClient client = new MongoClient(_connectStr);
            IMongoDatabase database = client.GetDatabase(_database);
            IMongoCollection<T> myCollection = database.GetCollection<T>(_collection);
            return myCollection;
        }

        #region 新增
        public Task Insert(T doc)
        {
            IMongoCollection<T> myCollection = GetCollection();
            Task result = myCollection.InsertOneAsync(doc);
            return result;
        }
        public Task Insert(IEnumerable<T> docs)
        {
            IMongoCollection<T> myCollection = GetCollection();
            Task result = myCollection.InsertManyAsync(docs);
            return result;
        }
        #endregion

        #region 更新
        public ReplaceOneResult Replace(Expression<Func<T, bool>> filter, T t)
        {
            IMongoCollection<T> myCollection = GetCollection();
            ReplaceOneResult result = myCollection.ReplaceOneAsync(filter, t).GetAwaiter().GetResult();
            return result;
        }
        public UpdateResult UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> Update)
        {
            IMongoCollection<T> myCollection = GetCollection();
            UpdateResult result = myCollection.UpdateManyAsync(filter, Update).GetAwaiter().GetResult();
            return result;
        }
        public UpdateResult UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> Update)
        {
            IMongoCollection<T> myCollection = GetCollection();
            UpdateResult result = myCollection.UpdateOneAsync(filter, Update).GetAwaiter().GetResult();
            
            return result;
        }
        public UpdateResult UpdateOne(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            IMongoCollection<T> myCollection = GetCollection();
            UpdateResult result = myCollection.UpdateOneAsync(filter, update).GetAwaiter().GetResult();
            return result;
        }
        #endregion

        #region 删除
        public DeleteResult Delete(FilterDefinition<T> filter)
        {
            IMongoCollection<T> myCollection = GetCollection();
            DeleteResult result = myCollection.DeleteManyAsync(filter).GetAwaiter().GetResult();
            return result;
        }
        public DeleteResult Delete(Expression<Func<T, bool>> filter)
        {
            IMongoCollection<T> myCollection = GetCollection();
            DeleteResult result = myCollection.DeleteManyAsync(filter).GetAwaiter().GetResult();
            return result;
        }
        #endregion

        #region IQuerable
        public IQueryable<T> GetQueryable()
        {
            IMongoCollection<T> myCollection = GetCollection();
            return myCollection.AsQueryable();
        }
        public IAggregateFluent<T> GetAggregateFluent(AggregateOptions opts)
        {
            IMongoCollection<T> myCollection = GetCollection();
            return myCollection.Aggregate(opts);
        }
        #endregion

        #region 查询
        public T GetOne(FilterDefinition<T> filter)
        {
            IMongoCollection<T> myCollection = GetCollection();
            return myCollection.Find(filter).FirstOrDefaultAsync().GetAwaiter().GetResult();
        }
        public List<T> GetMany(FilterDefinition<T> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection)
        {
            IMongoCollection<T> myCollection = GetCollection();
            AggregateOptions opts = new AggregateOptions()
            {
                AllowDiskUse = true,
                BatchSize = int.MaxValue,
                MaxTime = TimeSpan.FromMinutes(10)
            };

            IAggregateFluent<T> aggregate = GetAggregateFluent(opts);

            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            aggregate = aggregate.Match(filter);

            if (sorter != null)
            {
                aggregate.Sort(sorter);
            }
            if (projection != null)
            {
                aggregate = aggregate.Project<T>(projection);
            }

            List<T> result = aggregate.ToListAsync().GetAwaiter().GetResult();
            return result;
        }
        public List<T> GetMany(FilterDefinition<T> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection, ref PagerInfo pagerInfo)
        {
            IMongoCollection<T> myCollection = GetCollection();

            AggregateOptions opts = new AggregateOptions()
            {
                AllowDiskUse = true,
                BatchSize = int.MaxValue,
                MaxTime = TimeSpan.FromMinutes(10)
            };

            IAggregateFluent<T> aggregate = GetAggregateFluent(opts);

            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            aggregate = aggregate.Match(filter);

            if (sorter != null)
            {
                aggregate.Sort(sorter);
            }
            if (projection != null)
            {
                aggregate = aggregate.Project<T>(projection);
            }

            pagerInfo.Total = myCollection.CountAsync(filter).GetAwaiter().GetResult();

            List<T> result = aggregate.Match(filter).Sort(sorter).Skip(pagerInfo.Page).Limit(pagerInfo.PageSize)
                .ToListAsync<T>().GetAwaiter().GetResult();
            
            return result;
        }
        public List<T> GetPagination(Expression<Func<T, bool>> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection, ref PagerInfo pagerInfo)
        {
            IMongoCollection<T> myCollection = GetCollection();

            IFindFluent<T, T> finder = default(IFindFluent<T, T>);

            if (filter != null)
            {
                finder = myCollection.Find(filter);
            }
            if (sorter != null)
            {
                finder.Sort(sorter);
            }
            if (projection != null)
            {
                finder = finder.Project<T>(projection);
            }

            //记录总数
            //pagerInfo.Total = finder.CountAsync().GetAwaiter().GetResult();

            List<T> result = finder
                            .Skip((pagerInfo.Page - 1) * pagerInfo.PageSize)
                            .Limit(pagerInfo.PageSize)
                            .ToListAsync().GetAwaiter().GetResult();
            return result;
        }
        public List<T> GetPagination(FilterDefinition<T> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection, ref PagerInfo pagerInfo)
        {
            IMongoCollection<T> myCollection = GetCollection();

            AggregateOptions opts = new AggregateOptions()
            {
                AllowDiskUse = true,
                BatchSize = int.MaxValue,
                MaxTime = TimeSpan.FromMinutes(10)
            };

            IAggregateFluent<T> aggregate = GetAggregateFluent(opts);

            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            aggregate = aggregate.Match(filter);

            if (sorter != null)
            {
                aggregate.Sort(sorter);
            }

            if (projection != null)
            {
                aggregate = aggregate.Project<T>(projection);
            }

            pagerInfo.Total = myCollection.CountAsync(filter).GetAwaiter().GetResult();

            //List<T> result = finder
            //                .Skip((pagerInfo.Page) * pagerInfo.PageSize)
            //                .Limit(pagerInfo.PageSize)
            //                .ToListAsync().GetAwaiter().GetResult();

            List<T> result = myCollection.Aggregate(opts).Match(filter).Sort(sorter)
                                        .Skip((pagerInfo.Page) * pagerInfo.PageSize)
                                        .Limit(pagerInfo.PageSize).ToListAsync<T>().GetAwaiter().GetResult();

            return result;
        }
        #endregion

        #region 去重
        public List<string> Distinct(FieldDefinition<T, string> field, FilterDefinition<T> filter)
        {
            IMongoCollection<T> myCollection = GetCollection();
            using (var cursor = myCollection.DistinctAsync(field, filter).GetAwaiter().GetResult())
            {
                return cursor.ToListAsync().GetAwaiter().GetResult();
            }
        }
        public List<string> Distinct(Expression<Func<T, string>> field, FilterDefinition<T> filter)
        {
            IMongoCollection<T> myCollection = GetCollection();
            using (var cursor = myCollection.DistinctAsync(field, filter).GetAwaiter().GetResult())
            {
                return cursor.ToListAsync().GetAwaiter().GetResult();
            }
        }
        #endregion

        #region 索引

        #endregion
    }
}