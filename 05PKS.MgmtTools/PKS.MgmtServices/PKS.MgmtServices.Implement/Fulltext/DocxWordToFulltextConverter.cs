using System;
using System.IO;
using System.Text;
using Novacode;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Word转全文文件转换器</summary>
    public class DocxWordToFulltextConverter : FulltextConverter//, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "docx" }; }
        }
        /// <summary>生成全文文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            using (var document = DocX.Load(sourceFile))
            {
                return WriteFile(destFile, document.Text);
            }
        }
    }
}