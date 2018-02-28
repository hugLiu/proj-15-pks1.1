using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PPT转全文文件转换器</summary>
    public class OfficePPTToFulltextConverter : FulltextConverter, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "ppt", "pptx" }; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            ApplicationClass application = null;
            Presentation presentation = null;
            try
            {
                application = new ApplicationClass();
                presentation = application.Presentations.Open(sourceFile, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);
                var sBuilder = new StringBuilder();
                foreach (Slide slide in presentation.Slides)
                {
                    foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                    {
                        if (shape.HasTextFrame != MsoTriState.msoTrue) continue;
                        var textFrame = shape.TextFrame;
                        if (textFrame.HasText != MsoTriState.msoTrue) continue;
                        sBuilder.AppendLine(textFrame.TextRange.Text);
                    }
                }
                return WriteFile(destFile, sBuilder.ToString());
            }
            finally
            {
                if (presentation != null)
                {
                    presentation.Close();
                    Marshal.ReleaseComObject(presentation);
                }
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                }
            }
        }
    }
}