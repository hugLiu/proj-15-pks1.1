using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using PKS.Core;
using PKS.Utils;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Pdf转全文文件转换器</summary>
    public class ITextSharpPdfToFulltextConverter : FulltextConverter, ISingletonAppService
    {
        /// <summary>Pdf转全文文件转换器</summary>
        public ITextSharpPdfToFulltextConverter()
        {
            Assembly.Load("itext.font_asian");
        }
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "pdf" }; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            var sBuilder = new StringBuilder();
            using (var stream = File.OpenRead(sourceFile))
            {
                using (var reader = new PdfReader(stream))
                {
                    var document = new PdfDocument(reader);
                    var pageCount = document.GetNumberOfPages();
                    for (int i = 1; i <= pageCount; i++)
                    {
                        var page = document.GetPage(i);
                        var content = GetTextFromPage(page);
                        sBuilder.AppendLine(content);
                    }
                }
            }
            return WriteFile(destFile, sBuilder.ToString());
        }
        /// <summary>获得某页文本</summary>
        private string GetTextFromPage(PdfPage page)
        {
            try
            {
                return PdfTextExtractor.GetTextFromPage(page);
            }
            catch (Exception ex)
            {
                Bootstrapper.Error(this.GetType().Name + ":", ex);
            }
            return string.Empty;
        }
    }
}