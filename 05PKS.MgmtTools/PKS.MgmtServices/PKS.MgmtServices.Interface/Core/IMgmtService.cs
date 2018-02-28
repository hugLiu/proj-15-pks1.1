using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using PKS.Utils;

namespace PKS.MgmtServices.Core
{
    /// <summary>管理服务接口</summary>
    [ServiceContract]
    public interface IMgmtService
    {
        /// <summary>生成PDF文件</summary>
        [OperationContract]
        Task<bool> GeneratePdfAsync(string sourceFile, string destFile);
        /// <summary>生成图片文件</summary>
        [OperationContract]
        Task<bool> GenerateImageAsync(string sourceFile, string destFile, Size size);
        /// <summary>生成缩略图文件</summary>
        [OperationContract]
        Task<bool> GenerateThumbnailAsync(string sourceFile, string destFile, Size size);
        /// <summary>生成全文文件</summary>
        [OperationContract]
        Task<bool> GenerateFulltextAsync(string sourceFile, string pdfFile, string destFile);
        /// <summary>生成HTML文件</summary>
        [OperationContract]
        Task<bool> GenerateHtmlAsync(string sourceFile, string destFile);
    }
}