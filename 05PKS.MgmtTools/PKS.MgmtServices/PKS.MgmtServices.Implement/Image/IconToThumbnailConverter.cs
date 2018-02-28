using System.Drawing;
using System.Drawing.Imaging;

namespace PKS.MgmtServices.Converters
{
    /// <summary>图标到缩略图文件转换器</summary>
    public class IconToThumbnailConverter : ThumbnailConverter
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "ico", "icon" }; }
        }
        /// <summary>生成指定大小的缩略图</summary>
        public override bool Execute(string sourceFile, string destFile, Size size)
        {
            var icon = new Icon(sourceFile, size);
            return base.Execute(icon.ToBitmap(), sourceFile, destFile, size);
        }
    }
}