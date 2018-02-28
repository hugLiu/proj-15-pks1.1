using PKS.Core;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PKS.Utils;
using PKS.WebAPI.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BsonDocument = MongoDB.Bson.BsonDocument;

namespace PKS.WebAPI.Services
{
    /// <summary>对象服务</summary>
    public class BO2Service : IBO2Service, ISingletonAppService
    {
        public BO2Service(IMongoConfig config)
        {
            this.BOBsonDocumentCollection = (IMongoCollection<MongoDB.Bson.BsonDocument>)config.BOBsonDocumentCollection;
            this.BOTBsonDocumentCollection = (IMongoCollection<MongoDB.Bson.BsonDocument>)config.BOTBsonDocumentCollection;
            this.BOTCollection = (IMongoCollection<BOT>)config.BOTCollection;
            this.BOCollection = (IMongoCollection<BO2>)config.BOCollection;
        }

        public IMongoCollection<MongoDB.Bson.BsonDocument> BOBsonDocumentCollection { get; private set; }
        public IMongoCollection<MongoDB.Bson.BsonDocument> BOTBsonDocumentCollection { get; private set; }

        public IMongoCollection<BOT> BOTCollection { get; private set; }
        public IMongoCollection<BO2> BOCollection { get; private set; }

        #region BOT查询
        /// <summary>获得对象类型</summary>
        public BOT GetBOT(string bot)
        {
            return Task.Run(() => GetBOTAsync(bot)).Result;
        }
        /// <summary>获得对象类型</summary>
        public async Task<BOT> GetBOTAsync(string bot)
        {
            var request = new FilterRequest();
            request.Query = JObject.Parse($"{{'name':'{bot}'}}");
            var bots = await FilterBOTsAsync(request);
            return bots?.FirstOrDefault();
        }
        /// <summary>根据指定条件获得对象类型</summary>
        public List<BOT> FilterBOTs(FilterRequest request)
        {
            return Task.Run(() => FilterBOTsAsync(request)).Result;
        }
        /// <summary>根据指定条件获得对象类型</summary>
        public async Task<List<BOT>> FilterBOTsAsync(FilterRequest request)
        {
            return await FilterCollectionAsync<BOT>(this.BOTBsonDocumentCollection, request);
        }

        public long CountBOTs(object query)
        {
            return Task.Run(() => CountBOTsAsync(query)).Result;
        }
        public async Task<long> CountBOTsAsync(object query)
        {
            return await this.BOTBsonDocumentCollection.CountAsync(new JsonFilterDefinition<MongoDB.Bson.BsonDocument>(query.ToJson()));
        }
        #endregion

        #region BO查询
        public BO2 GetBO(string bot, string bo)
        {
            return Task.Run(() => GetBO(bot, bo)).Result;
        }

        public async Task<BO2> GetBOAsync(string bot, string bo)
        {
            FilterRequest request = new FilterRequest();
            request.Query = JObject.Parse($"{{'bot':'{bot}','bo':'{bo}'}}");
            List<BO2> bolist = await FilterBOsAsync(request);
            return (bolist == null || bolist.Count == 0) ? null : bolist[0];
        }

        public List<BO2> FindBOs(string bot, string bo)
        {
            return Task.Run(() => FindBOsAsync(bot, bo)).Result;
        }
        public async Task<List<BO2>> FindBOsAsync(string bot, string bo)
        {
            FilterRequest request = new FilterRequest();
            request.Query = JObject.Parse($"{{'bot':'{bot}','bo':{{'$regex':'{bo}','$options':'i'}}}}");
            return await FilterBOsAsync(request);
        }

        public List<BO2> FilterBOs(FilterRequest request)
        {
            return Task.Run(() => FilterBOsAsync(request)).Result;
        }
        public async Task<List<BO2>> FilterBOsAsync(FilterRequest request)
        {
            return await FilterCollectionAsync<BO2>(this.BOBsonDocumentCollection, request);
        }

        public long CountBOs(object query)
        {
            return Task.Run(() => CountBOTsAsync(query)).Result;
        }
        public async Task<long> CountBOsAsync(object query)
        {
            return await this.BOBsonDocumentCollection.CountAsync(new JsonFilterDefinition<MongoDB.Bson.BsonDocument>(query.ToJson()));
        }

        #endregion

        private async Task<List<T>> FilterCollectionAsync<T>(IMongoCollection<MongoDB.Bson.BsonDocument> docs, FilterRequest request)
        {
            IFindFluent<BsonDocument, BsonDocument> find = null;
            if (request.Query != null && request.Query.As<JObject>().Count > 0)
            {
                find = docs.Find(new JsonFilterDefinition<BsonDocument>(request.Query.ToJson()));
            }
            else
            {
                find = docs.Find(FilterDefinition<BsonDocument>.Empty);
            }
            if (request.Sort != null && request.Sort.As<JObject>()?.Count > 0)
            {
                find = find.Sort(new JsonSortDefinition<BsonDocument>(request.Sort.ToJson()));
            }
            var jProject = request.Fields.As<JObject>();
            if (jProject == null) jProject = new JObject();
            jProject.Add(new JProperty("_id", 0));
            find = find.Project(new JsonProjectionDefinition<BsonDocument>(jProject.ToJson()));
            var result = await find.Skip(request.Skip).Limit(request.Limit).ToListAsync();
            return result.Select(e => e.ToString().JsonTo<T>()).ToList<T>();
        }

        private MongoDB.Bson.BsonDocument ConvertFrom<T>(T entity)
        {
            return MongoDB.Bson.BsonDocument.Parse(entity.ToJson());
        }

        private List<MongoDB.Bson.BsonDocument> ConvertFrom<T>(List<T> entities)
        {
            List<MongoDB.Bson.BsonDocument> docs
                = entities.Select(
                    e => ConvertFrom<T>(e))
                  .ToList<MongoDB.Bson.BsonDocument>();
            return docs;
        }

        #region BOT增、删、改功能
        public void InsertBOTs(List<BOT> bots)
        {
            Task.Run(() => InsertBOTsAsync(bots));
        }
        public async Task InsertBOTsAsync(List<BOT> bots)
        {
            await this.BOTCollection.InsertManyAsync(bots);
        }
        public object SaveBOTs(List<BOT> bots)
        {
            return Task.Run(() => SaveBOTsAsync(bots).Result);
        }
        public async Task<object> SaveBOTsAsync(List<BOT> bots)
        {
            var saveBOTs = new List<string>();
            var updateOptions = new UpdateOptions { IsUpsert = true };
            foreach (var doc in bots)
            {
                //var filter = Builders<BOT>.Filter.Where(bot=>bot.Name.ToLower() == doc.Name.ToLower());
                //var saveResult = await this.BOTCollection.ReplaceOneAsync(filter, doc, updateOptions);
                var jsonFilter = $"{{'name':{{'$regex':'^{doc.Name}$','$options':'i'}}}}";
                var filter = new JsonFilterDefinition<MongoDB.Bson.BsonDocument>(jsonFilter);
                var saveResult = await this.BOTBsonDocumentCollection.ReplaceOneAsync(filter, this.ConvertFrom<BOT>(doc), updateOptions);
                if (saveResult.IsAcknowledged && (saveResult.ModifiedCount > 0 || saveResult.UpsertedId != null))
                {
                    saveBOTs.Add(doc.Name);
                }
            }
            return saveBOTs;
        }
        public object DeleteBOTs(List<string> bots)
        {
            return Task.Run(() => DeleteBOTs(bots)).Result;
        }
        public async Task<object> DeleteBOTsAsync(List<string> bots)
        {
            var deleteBOTs = new List<string>();
            foreach (var name in bots)
            {
                //var filter = Builders<BOT>.Filter
                //    .Where(bo => bo.Name.ToLower() == name.ToLower());
                var jsonFilter = $"{{'name':{{'$regex':'^{name}$','$options':'i'}}}}";
                var filter = new JsonFilterDefinition<MongoDB.Bson.BsonDocument>(jsonFilter);
                var deleteResult = await this.BOTBsonDocumentCollection.DeleteOneAsync(filter);
                if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
                {
                    deleteBOTs.Add(name);
                }
            }
            return deleteBOTs;
        }
        #endregion

        #region BO增、删、改功能
        public void InsertBOs(List<BO2> bos)
        {
            Task.Run(() => InsertBOsAsync(bos));
        }
        public async Task InsertBOsAsync(List<BO2> bos)
        {
            List<MongoDB.Bson.BsonDocument> docs = ConvertFrom(bos);
            await this.BOBsonDocumentCollection.InsertManyAsync(docs);
        }
        public object SaveBOs(List<BO2> bos)
        {
            return Task.Run(() => SaveBOsAsync(bos)).Result;
        }
        public async Task<object> SaveBOsAsync(List<BO2> bos)
        {
            var saveBOs = new List<string>();
            var updateOptions = new UpdateOptions { IsUpsert = true };
            var filterDict = new Dictionary<string, object[]>();
            filterDict["bot"] = new object[] { string.Empty };
            filterDict["bo"] = new object[] { string.Empty };
            foreach (var bo in bos)
            {
                filterDict["bot"][0] = bo.BOT;
                filterDict["bo"][0] = bo.BO;
                var filter = this.BOBsonDocumentCollection.BuildFilter(filterDict);
                using (var cursor = (await this.BOBsonDocumentCollection.FindAsync(filter)))
                {
                    var oldDoc = await cursor.FirstOrDefaultAsync();
                    if (oldDoc == null)
                    {
                        var doc = ConvertFrom<BO2>(bo);
                        await this.BOBsonDocumentCollection.ReplaceOneAsync(filter, doc, updateOptions);
                        saveBOs.Add(bo.BO);
                        continue;
                    }
                    if (!bo.BOID.IsNullOrEmpty())
                    {
                        oldDoc["boid"] = new MongoDB.Bson.BsonString(bo.BOID);
                    }
                    if (!bo.Alias.IsNullOrEmpty())
                    {
                        oldDoc["alias"] = new MongoDB.Bson.BsonArray(bo.Alias);
                    }
                    if (bo.Location != null)
                    {
                        var js = bo.Location.ToJson();
                        var doc = MongoDB.Bson.BsonDocument.Parse(js);
                        oldDoc["location"] = doc;
                    }
                    if (!bo.Properties.IsNullOrEmpty())
                    {
                        var oldProperties = oldDoc["properties"];
                        if (oldProperties == null)
                        {
                            oldDoc["properties"] = MongoDB.Bson.BsonValue.Create(bo.Properties);
                        }
                        else
                        {
                            foreach (var pair in bo.Properties)
                            {
                                oldProperties[pair.Key] = MongoDB.Bson.BsonValue.Create(pair.Value);
                            }
                        }
                    }
                    await this.BOBsonDocumentCollection.ReplaceOneAsync(filter, oldDoc, updateOptions);
                    saveBOs.Add(bo.BO);
                }
            }
            return saveBOs;
        }
        public object DeleteBOs(BO2DeleteRequest request)
        {
            return Task.Run(() => DeleteBOs(request)).Result;
        }
        public async Task<object> DeleteBOsAsync(BO2DeleteRequest request)
        {
            var deleteBOs = new List<string>();
            foreach (var name in request.BOs)
            {
                //var filter = Builders<BO2>.Filter
                //    .Where(bo => bo.BO.ToLower() == name.ToLower()
                //            && bo.BOT.ToLower() == request.BOT.ToLower());
                var jsonFilter = $"{{'bot':{{'$regex':'^{request.BOT}$','$options':'i'}},'bo':{{'$regex':'^{name}$','$options':'i'}}}}";
                var filter = new JsonFilterDefinition<MongoDB.Bson.BsonDocument>(jsonFilter);
                var deleteResult = await this.BOBsonDocumentCollection.DeleteOneAsync(filter);
                if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
                {
                    deleteBOs.Add(name);
                }
            }
            return deleteBOs;
        }
        #endregion

        #region 空间计算
        public object Near(NearRequest request)
        {
            return Task.Run(() => NearAsync(request)).Result;
        }
        public async Task<object> NearAsync(NearRequest request)
        {
            BO2 bo2 = await this.GetBOAsync(request.BOT, request.BO);
            if (bo2 == null) return null;
            FilterRequest filterRequest = new FilterRequest();
            filterRequest.Query = JObject.Parse($"{{bot:'{bo2.BOT}',bo:{{$ne:'{request.BO}'}},location:{{$near:{{$geometry:{bo2.Location.ToJson()},$maxDistance:{request.Distince}}}}}}}");
            filterRequest.Fields = JObject.Parse("{bo:1}");
            filterRequest.Limit = request.Top;
            List<BO2> nearestBOList = await this.FilterBOsAsync(filterRequest);
            return (nearestBOList == null || nearestBOList.Count == 0)
                ? null : nearestBOList.Select(bo => bo.BO).ToList();
        }
        #endregion
    }
}
