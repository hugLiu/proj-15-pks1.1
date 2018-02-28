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
    /// <summary>索引页面数据</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class IndexPageData : IMongoDocument
    {
        /// <summary>文档ID</summary>
        [JsonIgnore]
        public string Id { get; set; }
        /// <summary>文档ID</summary>
        [JsonIgnore]
        string IMongoDocument.Id
        {
            get { return this.PageId; }
        }
        /// <summary>页面数据ID</summary>
        public string PageId { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>是否模板</summary>
        public bool IsTemplate { get; set; }
        /// <summary>是否基类渲染</summary>
        public bool IsBaseRender { get; set; }
        /// <summary>存储类型</summary>
        public IndexStorageType StorageType { get; set; }
        /// <summary>内容引用，用于存储类型为URL和File</summary>
        public string ContentRef { get; set; }
        /// <summary>系统</summary>
        public string System { get; set; }
        /// <summary>页面展示类型</summary>
        public string ShowType { get; set; }
        /// <summary>数据类型</summary>
        public string DataType { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}