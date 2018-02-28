using System.IO;
using Aspose.Slides;
using Aspose.Slides.Export;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PPT转Pdf文件转换器</summary>
    public class AsposePPTToPdfConverter : FileConverter, IPdfConverter//, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "ppt", "pptx" }; }
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "pdf"; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string destFile)
        {
            using (var ppt = new Presentation(sourceFile))
            {
                var options = new PdfOptions();
                options.Compliance = PdfCompliance.Pdf15;
                ppt.Save(destFile, SaveFormat.Pdf, options);
            }
            return true;
        }
        /// <summary>生成PDF流</summary>
        public static void ToPdf(string sourceFile, Stream destStream)
        {
            using (var ppt = new Presentation(sourceFile))
            {
                var options = new PdfOptions();
                options.Compliance = PdfCompliance.Pdf15;
                ppt.Save(destStream, SaveFormat.Pdf, options);
            }
        }
    }
}