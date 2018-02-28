using System;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.WebAPI.Services
{
    /// <summary>Mongo库配置接口</summary>
    public interface IMongoConfig
    {
        /// <summary>库</summary>
        object Database { get; }
        /// <summary>根据文档类型获得集合名称</summary>
        string GetColletionName(Type docType);
        /// <summary>上传文件目录</summary>
        string IndexUploadFilesDir { get; }
        /// <summary>上传文件保存路径，第一级是年月(201707)，第二级是日小时(0808)</summary>
        string IndexUploadFilesPath { get; }
        /// <summary>上传文件临时路径</summary>
        string IndexUploadTempPath { get; }

        /// <summary>
        /// 业务对象属性及坐标信息使用Collection
        /// </summary>
        object BOBsonDocumentCollection { get; }

        /// <summary>
        /// 业务对象类型的属性定义集合
        /// </summary>
        object BOTBsonDocumentCollection { get; }

        object BOCollection { get; }
        object BOTCollection { get; }
    }

    /// <summary>Mongo文档接口</summary>
    public interface IMongoDocument
    {
        /// <summary>提供自定义ID</summary>
        string Id { get; }
    }
}