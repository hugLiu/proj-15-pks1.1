using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PPT转Pdf文件转换器</summary>
    public class OfficePPTToPdfConverter : FileConverter, IPdfConverter, ISingletonAppService
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
            ApplicationClass application = null;
            Presentation persentation = null;
            try
            {
                application = new ApplicationClass();
                persentation = application.Presentations.Open(sourceFile, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.ExportAsFixedFormat(destFile, PpFixedFormatType.ppFixedFormatTypePDF);
                //persentation.SaveAs(destFile, PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
            }
            finally
            {
                if (persentation != null)
                {
                    persentation.Close();
                    Marshal.ReleaseComObject(persentation);
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