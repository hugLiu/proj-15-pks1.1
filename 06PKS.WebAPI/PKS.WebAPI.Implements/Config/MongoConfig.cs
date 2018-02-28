using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Newtonsoft.Json;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;

namespace PKS.WebAPI.Services
{
    /// <summary>Mongo库配置实现</summary>
    public class MongoConfig : IMongoConfig, ISingletonAppService
    {
        /// <summary>构造实现</summary>
        public MongoConfig()
        {
            var configSection = ConfigurationManager.GetSection("pks.mongoConfig").As<NameValueCollection>();

            var mongoConnectionString = ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(configSection["Database"]);
            this.Database = database;
            RegisterConventions();
            RegisterClassMap_MetadataDefinition();
            RegisterClassMap_IndexData();
            this.ColletionMapper = new Dictionary<Type, string>();
            this.ColletionMapper[typeof(MetadataDefinition)] = configSection["MetadataDefinitionCollection"];
            this.ColletionMapper[typeof(MongoUploadFile)] = configSection["UploadFilesCollection"];
            this.ColletionMapper[typeof(FileFormat)] = configSection["FileFormatsCollection"];
            this.ColletionMapper[typeof(IndexAppData)] = configSection["AppDataCollection"];
            this.ColletionMapper[typeof(IndexPageData)] = configSection["PageDataCollection"];

            this.BOCollectionName = configSection["BOCollection"];
            this.BOTCollectionName = configSection["BOTCollection"];
            this.BOBsonDocumentCollection = database.GetCollection<BsonDocument>(this.BOCollectionName);
            this.BOTBsonDocumentCollection = database.GetCollection<BsonDocument>(this.BOTCollectionName);
            this.BOCollection = database.GetCollection<BO2>(this.BOCollectionName);
            this.BOTCollection = database.GetCollection<BOT>(this.BOTCollectionName);

            var context = HttpContext.Current;
            var rootPath = context.Server.MapPath("/");
            this.IndexUploadFilesDir = configSection["IndexUploadFilesPath"].Trim('\\');
            this.IndexUploadFilesPath = Path.Combine(rootPath, this.IndexUploadFilesDir);
            this.IndexUploadTempPath = Path.Combine(rootPath, configSection["IndexUploadTempPath"].Trim('\\'));
        }

        /// <summary>库</summary>
        [JsonIgnore]
        public object Database { get; set; }

        /// <summary>根据文档类型获得集合名称</summary>
        private Dictionary<Type, string> ColletionMapper { get; }
        /// <summary>根据文档类型获得集合名称</summary>
        public string GetColletionName(Type docType)
        {
            return this.ColletionMapper[docType];
        }

        /// <summary>BO使用Collection</summary>
        public string BOCollectionName { get; set; }

        /// <summary>BOT使用Collection</summary>
        public string BOTCollectionName { get; set; }
        /// <summary>业务对象属性及坐标信息使用Collection</summary>
        [JsonIgnore]
        public object BOBsonDocumentCollection { get; set; }
        [JsonIgnore]
        public object BOCollection { get; set; }

        /// <summary>业务对象类型的属性定义使用Collection</summary>
        [JsonIgnore]
        public object BOTBsonDocumentCollection { get; set; }
        [JsonIgnore]
        public object BOTCollection { get; set; }

        /// <summary>上传文件目录</summary>
        public string IndexUploadFilesDir { get; }
        /// <summary>上传文件保存路径，第一级是年月(201707)，第二级是日小时(0808)</summary>
        public string IndexUploadFilesPath { get; }
        /// <summary>上传文件临时路径</summary>
        public string IndexUploadTempPath { get; set; }
        /// <summary>注册约定</summary>
        private void RegisterConventions()
        {
            var conventionPack = new ConventionPack();
            conventionPack.Add(LowerCaseElementNameConvention.Instance);
            conventionPack.Add(new EnumRepresentationConvention(BsonType.String));
            ConventionRegistry.Register("CustomConventionPack", conventionPack, type => true);
        }
        /// <summary>注册映射_元数据定义</summary>
        private void RegisterClassMap_MetadataDefinition()
        {
            BsonClassMap.RegisterClassMap<MetadataValueItem>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<MetadataDefinition>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                map.SetIgnoreExtraElements(true);
            });
        }
        /// <summary>注册映射_索引相关数据</summary>
        private void RegisterClassMap_IndexData()
        {
            BsonClassMap.RegisterClassMap<FileFormat>(map =>
            {
                var conventionPack = map.ConventionPack.As<ConventionPack>();
                conventionPack.Remove(LowerCaseElementNameConvention.Instance.Name);
                conventionPack.Add(new CamelCaseElementNameConvention());
                map.AutoMap();
                map.MapIdMember(c => c.Id);
                map.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<MongoUploadFile>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.FileId).SetIdGenerator(GuidIdGenerator.Instance);
                map.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<IndexAppData>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id).SetIdGenerator(Md5IdGenerator.Instance);
                map.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<IndexPageData>(map =>
            {
                map.AutoMap();
                map.MapIdMember(c => c.Id).SetIdGenerator(Md5IdGenerator.Instance);
                map.SetIgnoreExtraElements(true);
            });
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return JsonUtil.ToJson(this);
        }
        #region 小写元素名称约定
        /// <summary>小写元素名称约定</summary>
        private class LowerCaseElementNameConvention : IMemberMapConvention
        {
            /// <summary>实例</summary>
            public static LowerCaseElementNameConvention Instance { get; } = new LowerCaseElementNameConvention();
            /// <summary>名称</summary>
            public string Name { get { return "LowerCaseElementName"; } }
            /// <summary>应用约定</summary>
            public void Apply(BsonMemberMap memberMap)
            {
                memberMap.SetElementName(memberMap.MemberName.ToLower());
            }
        }
        #endregion
        #region Guid的ID生成器
        /// <summary>MD5的ID生成器</summary>
        private class GuidIdGenerator : IIdGenerator
        {
            /// <summary>实例</summary>
            public static IIdGenerator Instance { get; } = new GuidIdGenerator();
            /// <summary>生成ID</summary>
            public object GenerateId(object container, object document)
            {
                return Guid.NewGuid().ToString();
            }
            /// <summary>是否空</summary>
            public bool IsEmpty(object id)
            {
                return id.As<string>().IsNullOrEmpty();
            }
        }
        #endregion
        #region MD5的ID生成器
        /// <summary>MD5的ID生成器</summary>
        private class Md5IdGenerator : IIdGenerator
        {
            /// <summary>实例</summary>
            public static IIdGenerator Instance { get; } = new Md5IdGenerator();
            /// <summary>生成ID</summary>
            public object GenerateId(object container, object document)
            {
                return document.As<IMongoDocument>().Id;
            }
            /// <summary>是否空</summary>
            public bool IsEmpty(object id)
            {
                return id.As<string>().IsNullOrEmpty();
            }
        }
        #endregion
    }
}