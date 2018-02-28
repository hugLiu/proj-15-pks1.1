using System;
using System.IO;
using System.Text;
using Aspose.Words;
using Aspose.Words.Saving;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Word转全文文件转换器</summary>
    public class AsposeWordToFulltextConverter : FulltextConverter//, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "doc" }; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            var document = new Document(sourceFile);
            var options = new TxtSaveOptions();
            options.Encoding = Encoding.UTF8;
            options.ExportHeadersFooters = false;
            options.ParagraphBreak = Environment.NewLine;
            options.PreserveTableLayout = false;
            options.SimplifyListLabels = true;
            document.Save(destFile, options);
            return true;
        }
    }
}