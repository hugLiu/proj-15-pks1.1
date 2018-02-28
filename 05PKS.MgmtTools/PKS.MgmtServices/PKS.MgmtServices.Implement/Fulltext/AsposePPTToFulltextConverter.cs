using System.IO;
using System.Text;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PPT转全文文件转换器</summary>
    public class AsposePPTToFulltextConverter : FulltextConverter//, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "ppt", "pptx" }; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            var pdfStream = new MemoryStream();
            AsposePPTToPdfConverter.ToPdf(sourceFile, pdfStream);
            pdfStream.Position = 0;
            var docxStream = new MemoryStream();
            PdfConverter.ToDocx(pdfStream, docxStream);
            docxStream.Position = 0;
            DocxConverter.ToText(docxStream, destFile);
            return true;
        }
    }
}