using System.IO;
using System.Text;
using Aspose.Pdf;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Pdf转全文文件转换器</summary>
    public class AsposePdfToFulltextConverter : FulltextConverter//, ISingletonAppService
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "pdf" }; }
        }
        /// <summary>生成PDF文件</summary>
        public override bool Execute(string sourceFile, string pdfFile, string destFile)
        {
            var stream = new MemoryStream();
            using (var pdf = new Document(sourceFile))
            {
                var options = new DocSaveOptions();
                options.Format = DocSaveOptions.DocFormat.DocX;
                pdf.Save(stream, options);
            }
            stream.Position = 0;
            using (var document = Novacode.DocX.Load(stream))
            {
                var content = document.Text;
                File.WriteAllText(destFile, content, Encoding.UTF8);
            }
            return true;
        }
    }
}