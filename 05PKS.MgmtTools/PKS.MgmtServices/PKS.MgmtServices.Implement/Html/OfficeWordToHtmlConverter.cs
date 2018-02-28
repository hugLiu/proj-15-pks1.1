using Microsoft.Office.Interop.Word;
using PKS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PKS.MgmtServices.Converters
{
    /// <summary>
    /// Word转HTML文件转换器
    /// </summary>
    public class OfficeWordToHtmlConverter : FileConverter, IHtmlConverter, ISingletonAppService
    {
        /// <summary>
        /// 能处理的扩展名集合
        /// </summary>
        public override string[] Exts
        {
            get { return new string[] { "doc", "docx" }; }
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "html"; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string destFile)
        {
            bool success = true;
            Microsoft.Office.Interop.Word.ApplicationClass application = null;
            Microsoft.Office.Interop.Word.Documents documents = null;
            Microsoft.Office.Interop.Word.Document document = null;
            try
            {
                application = new Microsoft.Office.Interop.Word.ApplicationClass();
                //Type wordType = application.GetType();
                documents = application.Documents;

                // 打开文件  
                Type docsType = documents.GetType();
                document = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open",
                            System.Reflection.BindingFlags.InvokeMethod, null, documents, new Object[] { sourceFile, true, true });

                Type docType = document.GetType();
                docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod,
                            null, document, new object[] { destFile, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });
            }
            catch (Exception)
            {
                success = false;
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
                //关闭文档
                //docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, document, new object[] { null, null, null });

                //退出 Word
                //wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, application, null);
            }

            return success;
        }
    }
}
