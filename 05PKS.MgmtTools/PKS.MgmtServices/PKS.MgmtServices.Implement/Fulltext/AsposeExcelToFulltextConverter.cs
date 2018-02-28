using System.IO;
using System.Text;
using Aspose.Cells;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Excel转全文文件转换器</summary>
    public class AsposeExcelToFulltextConverter : FulltextConverter, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "xls", "xlsx" }; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            var workbook = new Workbook(sourceFile);
            var options = new TxtSaveOptions();
            options.Encoding = Encoding.UTF8;
            //options.FormatStrategy = CellValueFormatStrategy.None;
            options.QuoteType = TxtValueQuoteType.Never;
            //options.TrimLeadingBlankRowAndColumn = true;
            workbook.Save(destFile, options);
            var content = File.ReadAllText(destFile);
            WriteFile(destFile, content, true);
            return true;
        }
    }
}