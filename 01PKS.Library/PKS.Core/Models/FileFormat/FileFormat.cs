using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PKS.Services;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>文件格式</summary>
    public class FileFormat
    {
        /// <summary>ID</summary>
        [JsonIgnore]
        public object Id { get; set; }
        /// <summary>扩展名数组</summary>
        public string[] Ext { get; set; }
        /// <summary>应用数据的数据类型</summary>
        public string AppDataType { get; set; }
        /// <summary>索引的数据类型</summary>
        public string IndexDataType { get; set; }
        /// <summary>媒体类型</summary>
        public string MediaType { get; set; }
        /// <summary>是否流</summary>
        public bool IsStream { get; set; }
        /// <summary>生成PDF</summary>
        public bool GeneratePdf { get; set; }
        /// <summary>生成图片</summary>
        public bool GenerateImage { get; set; }
        /// <summary>生成缩略图</summary>
        public bool GenerateThumbnail { get; set; }
        /// <summary>生成全文</summary>
        public bool GenerateFulltext { get; set; }
        /// <summary>生成HTML文件</summary>
        public bool GenerateHtml { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}