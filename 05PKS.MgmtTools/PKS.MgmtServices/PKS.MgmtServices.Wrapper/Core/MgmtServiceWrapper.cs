using System.Drawing;
using System.ServiceModel;
using System.Threading.Tasks;
using PKS.Core;

namespace PKS.MgmtServices.Core
{
    /// <summary>管理服务接口</summary>
    public class MgmtServiceWrapper : ClientBase<IMgmtService>, IMgmtService, ISingletonAppService
    {
        /// <summary>构造函数</summary>
        public MgmtServiceWrapper() : this(typeof(IMgmtService).Name) { }
        /// <summary>构造函数</summary>
        public MgmtServiceWrapper(string endpointConfigurationName) : base(endpointConfigurationName) { }
        /// <summary>生成PDF文件</summary>
        public async Task<bool> GeneratePdfAsync(string sourceFile, string destFile)
        {
            return await base.Channel.GeneratePdfAsync(sourceFile, destFile).ConfigureAwait(false);
        }
        /// <summary>生成图片文件</summary>
        public async Task<bool> GenerateImageAsync(string sourceFile, string destFile, Size size)
        {
            return await base.Channel.GenerateImageAsync(sourceFile, destFile, size).ConfigureAwait(false);
        }
        /// <summary>生成缩略图文件</summary>
        public async Task<bool> GenerateThumbnailAsync(string sourceFile, string destFile, Size size)
        {
            return await base.Channel.GenerateThumbnailAsync(sourceFile, destFile, size).ConfigureAwait(false);
        }
        /// <summary>生成全文文件</summary>
        public async Task<bool> GenerateFulltextAsync(string sourceFile, string pdfFile, string destFile)
        {
            return await base.Channel.GenerateFulltextAsync(sourceFile, pdfFile, destFile).ConfigureAwait(false);
        }
        /// <summary>生成HTML文件</summary>
        public async Task<bool> GenerateHtmlAsync(string sourceFile, string destFile)
        {
            return await base.Channel.GenerateHtmlAsync(sourceFile, destFile).ConfigureAwait(false);
        }
    }
}