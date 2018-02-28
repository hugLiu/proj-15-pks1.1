using Aspose.Words;
using Aspose.Words.Saving;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Word转Pdf文件转换器</summary>
    public class AsposeWordToPdfConverter : FileConverter, IPdfConverter//, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "doc", "docx" }; }
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "pdf"; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string destFile)
        {
            var document = new Document(sourceFile);
            var options = new PdfSaveOptions();
            options.PrettyFormat = true;
            options.UseHighQualityRendering = true;
            options.Compliance = PdfCompliance.Pdf15;
            document.Save(destFile, options);
            return true;
        }
    }
}