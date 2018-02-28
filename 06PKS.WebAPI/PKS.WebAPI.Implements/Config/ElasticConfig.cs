using System;
using System.Collections.Specialized;
using System.Configuration;
using Nest;
using Newtonsoft.Json;
using PKS.Core;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.ES;

namespace PKS.WebAPI.Services
{
    /// <summary>Elastic库配置实现</summary>
    public class ElasticConfig : IElasticConfig, ISingletonAppService
    {
        /// <summary>构造实现</summary>
        public ElasticConfig()
        {
            var url = ConfigurationManager.ConnectionStrings["ElasticConnection"].ConnectionString;
            var configSection = ConfigurationManager.GetSection("pks.elasticConfig").As<NameValueCollection>();
            var indexName = configSection["Index"];
            var node = new Uri(url);
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(indexName);
            var client = new ElasticClient(settings);
            //RegisterTypeMap_Metadata(client);
            this.Client = client;
            var metadataTypeName = configSection["MetadataType"];
            var userBehaviorTypeName = configSection["UserBehaviorType"];
            MetadataType = new TypeName { Name = metadataTypeName, Type = typeof(Metadata) };
            UserBehaviorType = new TypeName { Name = userBehaviorTypeName, Type = typeof(UserBehavior) };
            ESAccess<Metadata>.EsUri = url;
            ESAccess<Metadata>.EsIndex = indexName;
            ESAccess<Metadata>.EsType = metadataTypeName;
            ESAccess<UserBehavior>.EsUri = url;
            ESAccess<UserBehavior>.EsIndex = indexName;
            ESAccess<UserBehavior>.EsType = userBehaviorTypeName;
        }

        /// <summary>客户端</summary>
        [JsonIgnore]
        public object Client { get; set; }

        /// <summary>注册类型映射</summary>
        private void RegisterTypeMap_Metadata(ElasticClient client)
        {
            client.Map<Metadata>(descriptor =>
            {
                descriptor.AutoMap();
                descriptor.Meta(m =>
                {
                    m["_id"] = MetadataConsts.IIId;
                    return m;
                });
                //descriptor.Properties(p => p);
                return descriptor;
            });
        }
        /// <summary>元数据</summary>
        [JsonIgnore]
        public object MetadataType { get; set; }

        /// <summary>用户行为日志</summary>
        [JsonIgnore]
        public object UserBehaviorType { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return JsonUtil.ToJson(this);
        }
    }
}