using System.Drawing;
using System.Drawing.Imaging;

namespace PKS.MgmtServices.Converters
{
    /// <summary>图元到图片转换器</summary>
    public class MetafileToImageConverter : ImageConverter
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "wmf", "emf" }; }
        }
        /// <summary>转换源文件为目标文件</summary>
        public override bool Execute(string sourceFile, string destFile, Size size)
        {
            var meta = new Metafile(sourceFile);
            return base.Execute(meta, sourceFile, destFile, size);
        }
    }
}