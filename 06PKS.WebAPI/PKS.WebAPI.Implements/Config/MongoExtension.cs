using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using MongoDB.Driver;
using PKS.Core;
using PKS.Models;
using PKS.Utils;

namespace PKS.WebAPI.Services
{
    /// <summary>Mongo扩展</summary>
    public static class MongoExtension
    {
        /// <summary>根据键值对条件生成Mongo条件</summary>
        public static FilterDefinition<TDocument> BuildFilter<TDocument>(this IMongoCollection<TDocument> accessor, Dictionary<string, object[]> requestFilter)
        {
            FilterDefinition<TDocument> result = null;
            if (!requestFilter.IsNullOrEmpty())
            {
                var builder = Builders<TDocument>.Filter;
                var filters = new List<FilterDefinition<TDocument>>(requestFilter.Count);
                foreach (var pair in requestFilter)
                {
                    FieldDefinition<TDocument, object> field = pair.Key;
                    if (pair.Value.Length > 1)
                    {
                        filters.Add(builder.In(field, pair.Value));
                    }
                    else
                    {
                        filters.Add(builder.Eq(field, pair.Value[0]));
                    }
                }
                result = builder.And(filters);
            }
            return result ?? FilterDefinition<TDocument>.Empty;
        }
        /// <summary>生成Mongo分页条件</summary>
        public static FindOptions<TDocument, TDocument> BuildPager<TDocument>(this IMongoCollection<TDocument> accessor, IPager pager)
        {
            var options = new FindOptions<TDocument, TDocument>();
            options.Skip = pager.From;
            options.Limit = pager.Size;
            return options;
        }
        /// <summary>根据键值对生成Mongo排序</summary>
        public static SortDefinition<TDocument> BuildSort<TDocument>(this IMongoCollection<TDocument> accessor, IList<PKSKeyValuePair<string, int>> sortRules)
        {
            SortDefinition<TDocument> result = null;
            if (!sortRules.IsNullOrEmpty())
            {
                var builder = Builders<TDocument>.Sort;
                var sorts = new List<SortDefinition<TDocument>>(sortRules.Count);
                foreach (var pair in sortRules)
                {
                    FieldDefinition<TDocument> field = pair.Key;
                    if (pair.Value == 0)
                    {
                        sorts.Add(builder.Ascending(field));
                    }
                    else
                    {
                        sorts.Add(builder.Descending(field));
                    }
                }
                result = builder.Combine(sorts);
            }
            return result;
        }
    }
}