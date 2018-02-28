using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Word转全文文件转换器</summary>
    public class OfficeWordToFulltextConverter : FulltextConverter, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "doc", "docx" }; }
        }
        /// <summary>生成全文文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            ApplicationClass application = null;
            Document document = null;
            try
            {
                application = new ApplicationClass();
                application.Visible = false;
                object sourceDocFile = sourceFile;
                document = application.Documents.Open(ref sourceDocFile);
                object fileName = destFile;
                object fileFormat = WdSaveFormat.wdFormatEncodedText;
                object missing = Missing.Value;
                object encoding = MsoEncoding.msoEncodingUTF8;
                document.SaveAs2(ref fileName, ref fileFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref encoding);
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