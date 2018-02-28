using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>索引数据内容类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexAppContentType
    {
        /// <summary>HTML片段</summary>
        Html,
        /// <summary>JSON</summary>
        Json,
        /// <summary>文件</summary>
        File,
    }

    /// <summary>索引相关数据类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexAppDataType
    {
        /// <summary>原生类型</summary>
        Raw,
        /// <summary>HTML片段</summary>
        Html,
        /// <summary>JSON</summary>
        Json,
        /// <summary>图片</summary>
        Image,
        /// <summary>文档</summary>
        Pdf,
        /// <summary>音频</summary>
        Audio,
        /// <summary>视频</summary>
        Video,
    }

    /// <summary>文件存储类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileStorageType
    {
        /// <summary>无</summary>
        None,
        /// <summary>保存在Mongo库</summary>
        Mongo,
        /// <summary>保存在文件系统</summary>
        FileSystem,
    }

    /// <summary>索引相关数据存储类型</summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexStorageType
    {
        /// <summary>数据内容保存在Content</summary>
        Content,
        /// <summary>文件内容保存在GridFS内</summary>
        File,
        /// <summary>使用URL指向内容</summary>
        Url,
        /// <summary>使用适配器获取内容</summary>
        Adapter,
    }
}