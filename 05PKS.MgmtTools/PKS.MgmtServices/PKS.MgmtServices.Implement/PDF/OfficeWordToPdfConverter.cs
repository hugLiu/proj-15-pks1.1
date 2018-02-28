using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Word转Pdf文件转换器</summary>
    public class OfficeWordToPdfConverter : FileConverter, IPdfConverter, ISingletonAppService
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
            ApplicationClass application = null;
            Document document = null;
            try
            {
                application = new ApplicationClass();
                application.Visible = false;
                object sourceDocFile = sourceFile;
                document = application.Documents.Open(ref sourceDocFile);
                document.ExportAsFixedFormat(destFile, WdExportFormat.wdExportFormatPDF, false, WdExportOptimizeFor.wdExportOptimizeForOnScreen);
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                    Marshal.ReleaseComObject(document);
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