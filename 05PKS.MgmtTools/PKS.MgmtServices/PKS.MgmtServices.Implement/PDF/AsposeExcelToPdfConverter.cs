using Aspose.Cells;
using Aspose.Cells.Rendering;
using Aspose.Cells.Rendering.PdfSecurity;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Excel转Pdf文件转换器</summary>
    public class AsposeExcelToPdfConverter : FileConverter, IPdfConverter, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "xls", "xlsx" }; }
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "pdf"; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string destFile)
        {
            var workbook = new Workbook(sourceFile);
            {
                var options = new PdfSaveOptions();
                options.AllColumnsInOnePagePerSheet = true;
                options.CalculateFormula = true;
                options.Compliance = PdfCompliance.None;
                //options.OnePagePerSheet = true;
                //options.OptimizationType = PdfOptimizationType.Standard;
                options.SecurityOptions = new PdfSecurityOptions();
                options.SecurityOptions.PrintPermission = false;
                options.SecurityOptions.ModifyDocumentPermission = false;
                options.SecurityOptions.AssembleDocumentPermission = false;
                options.SecurityOptions.ExtractContentPermission = false;
                options.SecurityOptions.ExtractContentPermissionObsolete = false;
                workbook.Save(destFile, options);
            }
            return true;
        }
    }
}