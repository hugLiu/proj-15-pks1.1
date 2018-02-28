using System;
using System.IO;
using System.Runtime.Serialization;
using PKS.Utils;
using PKS.Web;

namespace PKS.WebAPI.Models
{
    /// <summary>文件下载请求</summary>
    /// <remarks>
    /// 有2种下载方式
    /// 1. 通过DataId和Source下载指定的文件
    /// 2. 通过StorageType和ContentRef下载
    /// </remarks>
    public class DownloadRequest : IParameterValidation
    {
        /// <summary>是否使用下载保存文件方式，默认是False</summary>
        public bool Download { get; set; }
        /// <summary>数据ID</summary>
        public string DataId { get; set; }
        /// <summary>下载源文件，默认是False</summary>
        public bool Source { get; set; }
        /// <summary>存储类型</summary>
        public IndexStorageType StorageType { get; set; }
        /// <summary>文件内容引用</summary>
        public string ContentRef { get; set; }
        /// <summary>文件名称</summary>
        /// <remarks>如果文件名称存在，则为下载文件名并设置ContentType头</remarks>
        public string FileName { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>文件下载结果</summary>
    public class DownloadResult
    {
        /// <summary>文件名</summary>
        public string FileName { get; set; }
        /// <summary>内容类型</summary>
        public string ContentType { get; set; }
        /// <summary>内容流</summary>
        public Stream Content { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}