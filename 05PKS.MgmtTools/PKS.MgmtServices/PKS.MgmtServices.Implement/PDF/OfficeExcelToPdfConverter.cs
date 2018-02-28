using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Excel转Pdf文件转换器</summary>
    public class OfficeExcelToPdfConverter : FileConverter, IPdfConverter//, ISingletonAppService
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
            ApplicationClass application = null;
            Workbook workbook = null;
            try
            {
                application = new ApplicationClass();
                application.Visible = false;
                workbook = application.Workbooks.Open(sourceFile);
                workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, destFile, XlFixedFormatQuality.xlQualityStandard, true, false);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                }
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                }
            }
            return true;
        }
    }
}