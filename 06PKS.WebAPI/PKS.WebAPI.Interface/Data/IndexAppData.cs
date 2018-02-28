using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.Models;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;
using PKS.WebAPI.Services;

namespace PKS.WebAPI.Models
{
    /// <summary>索引应用数据</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class IndexAppData : IMongoDocument
    {
        /// <summary>文档ID</summary>
        [JsonIgnore]
        public string Id { get; set; }

        /// <summary>文档ID</summary>
        [JsonIgnore]
        string IMongoDocument.Id
        {
            get { return this.DataId; }
        }
        /// <summary>应用数据ID</summary>
        public string DataId { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>数据类型</summary>
        public IndexAppDataType DataType { get; set; }
        /// <summary>存储类型</summary>
        public IndexStorageType StorageType { get; set; }
        /// <summary>内容引用，用于存储类型为URL和File</summary>
        public string ContentRef { get; set; }
        /// <summary>内容，用于数据类型为HTML和JSON，存储类型为Content</summary>
        public object Content { get; set; }
        ///// <summary>文件MD5，用于文件秒传</summary>
        //public string Md5 { get; set; }
        /// <summary>源名称</summary>
        public string SourceName { get; set; }
        /// <summary>源存储类型</summary>
        public IndexStorageType? SourceStorageType { get; set; }
        /// <summary>源引用，用于存储类型为URL和File</summary>
        public string SourceContentRef { get; set; }
        /// <summary>是否在线上传</summary>
        public bool IsOnline { get; set; }
        ///// <summary>源内容，用于数据类型为HTML和JSON，存储类型为Content</summary>
        //public object SourceContent { get; set; }
        ///// <summary>源文件MD5，用于文件秒传</summary>
        //public string SourceMd5 { get; set; }
        /// <summary>获取内容的适配器信息</summary>
        public string RawAdapter { get; set; }
        /// <summary>系统</summary>
        public string System { get; set; }
        /// <summary>资源类型</summary>
        public string ResourceType { get; set; }
        /// <summary>资源键，用来唯一标识数据源数据</summary>
        public string ResourceKey { get; set; }
        /// <summary>创建人</summary>
        public string CreateBy { get; set; }
        /// <summary>创建时间</summary>
        public DateTime CreateDate { get; set; }
        /// <summary>上一次修改人</summary>
        public string LastUpdatedBy { get; set; }
        /// <summary>上一次修改时间</summary>
        public DateTime LastUpdatedDate { get; set; }
        ///// <summary>相关的元数据</summary>
        //public IMetadata KMD { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}