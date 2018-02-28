using System.Drawing;
using System.ServiceModel;
using System.Threading.Tasks;
using Ninject;
using PKS.Core;
using PKS.MgmtServices.Converters;

namespace PKS.MgmtServices.Core
{
    /// <summary>管理服务接口</summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class MgmtService : AppService, IMgmtService, ISingletonAppService
    {
        #region PDF方法
        /// <summary>PDF生成器</summary>
        [Inject]
        public ICompositePdfConverter PdfConverter { get; set; }
        /// <summary>生成PDF文件</summary>
        public async Task<bool> GeneratePdfAsync(string sourceFile, string destFile)
        {
            return await this.PdfConverter.ExecuteAsync(sourceFile, destFile).ConfigureAwait(false);
        }
        #endregion

        #region 图片方法
        /// <summary>图片生成器</summary>
        [Inject]
        public ICompositeImageConverter ImageConverter { get; set; }
        /// <summary>生成图片文件</summary>
        public async Task<bool> GenerateImageAsync(string sourceFile, string destFile, Size size)
        {
            return await this.ImageConverter.ExecuteAsync(sourceFile, destFile, size).ConfigureAwait(false);
        }
        #endregion

        #region 缩略图方法
        /// <summary>缩略图生成器</summary>
        [Inject]
        public ICompositeThumbnailConverter ThumbnailConverter { get; set; }
        /// <summary>生成缩略图</summary>
        public async Task<bool> GenerateThumbnailAsync(string sourceFile, string destFile, Size size)
        {
            return await this.ThumbnailConverter.ExecuteAsync(sourceFile, destFile, size).ConfigureAwait(false);
        }
        #endregion

        #region 全文方法
        /// <summary>全文生成器</summary>
        [Inject]
        public ICompositeFulltextConverter FulltextConverter { get; set; }
        /// <summary>生成全文文件</summary>
        public async Task<bool> GenerateFulltextAsync(string sourceFile, string pdfFile, string destFile)
        {
            return await this.FulltextConverter.ExecuteAsync(sourceFile, pdfFile, destFile).ConfigureAwait(false);
        }
        #endregion

        #region HTML方法
        /// <summary>HTML生成器</summary>
        [Inject]
        public ICompositeHtmlConverter HtmlConverter { get; set; }
        /// <summary>生成全文文件</summary>
        public async Task<bool> GenerateHtmlAsync(string sourceFile, string destFile)
        {
            return await this.HtmlConverter.ExecuteAsync(sourceFile, destFile).ConfigureAwait(false);
        }
        #endregion
    }
}