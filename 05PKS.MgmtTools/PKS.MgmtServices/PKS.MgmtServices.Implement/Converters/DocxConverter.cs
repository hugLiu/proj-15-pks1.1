using System.IO;
using System.Text;
using Novacode;

namespace PKS.MgmtServices.Converters
{
    /// <summary>Word转换器</summary>
    public class DocxConverter
    {
        /// <summary>转换为文本文件</summary>
        public static void ToText(Stream sourceStream, string destFile)
        {
            using (var document = DocX.Load(sourceStream))
            {
                var content = document.Text;
                File.WriteAllText(destFile, content, Encoding.UTF8);
            }
        }
    }
}