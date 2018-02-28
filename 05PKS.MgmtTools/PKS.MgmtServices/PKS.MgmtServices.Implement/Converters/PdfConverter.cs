using System.IO;
using System.Text;
using Aspose.Pdf;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>PDF转换器</summary>
    public class PdfConverter
    {
        /// <summary>转换为WORD文档</summary>
        public static void ToDocx(Stream sourceStream, Stream destStream)
        {
            using (var pdf = new Document(sourceStream))
            {
                var options = new DocSaveOptions();
                options.Format = DocSaveOptions.DocFormat.DocX;
                pdf.Save(destStream, options);
            }
        }
    }
}