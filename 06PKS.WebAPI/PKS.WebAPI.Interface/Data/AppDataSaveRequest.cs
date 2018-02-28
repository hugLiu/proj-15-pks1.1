using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PKS.Models;
using PKS.Utils;
using PKS.Validation;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引应用数据保存请求</summary>
    public class AppDataSaveRequest : IParameterValidation
    {
        /// <summary>应用数据ID:如果不合法则使用资源键生成MD5值</summary>
        public string DataId { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>索引数据内容类型</summary>
        public IndexAppContentType ContentType { get; set; }
        /// <summary>索引数据内容：DataType IN (HTML,JSON)时有效</summary>
        public object Content { get; set; }
        /// <summary>文件存储类型</summary>
        public FileStorageType StorageType { get; set; }
        /// <summary>生成缩略图</summary>
        public bool GenerateThumbnail { get; set; } = true;
        /// <summary>生成全文</summary>
        public bool GenerateFulltext { get; set; } = true;
        /// <summary>是否使用在线方式上传:StorageType=FileSystem时有效，非在线方式指用手工或其它方式把文件上传到服务器</summary>
        public bool IsOnline { get; set; }
        /// <summary>上传文件后生成的FileId</summary>
        public string UploadFileId { get; set; }
        /// <summary>源文件名：StorageType=FileSystem时有效，指定存储路径和文件名</summary>
        public string SourceFile { get; set; }
        /// <summary>上传人</summary>
        public string Uploader { get; set; }
        /// <summary>获取内容的适配器信息</summary>
        public string RawAdapter { get; set; }
        /// <summary>系统</summary>
        public string System { get; set; }
        /// <summary>资源类型</summary>
        public string ResourceType { get; set; }
        /// <summary>资源键，用来唯一标识数据源数据</summary>
        public string ResourceKey { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>索引应用数据保存结果</summary>
    public class AppDataSaveResult
    {
        /// <summary>应用数据ID</summary>
        public string DataId { get; set; }
        /// <summary>缩略图</summary>
        public string Thumbnail { get; set; }
        /// <summary>全文</summary>
        public string Fulltext { get; set; }
    }
}