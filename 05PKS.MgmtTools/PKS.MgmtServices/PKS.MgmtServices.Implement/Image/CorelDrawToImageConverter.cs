using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PKS.MgmtServices.Converters
{
    /// <summary>CorelDraw矢量图到图片转换器</summary>
    public class CorelDrawToImageConverter : ImageConverter
    {
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "cdr" }; }
        }
        /// <summary>转换源文件为目标文件</summary>
        public override bool Execute(string sourceFile, string destFile, Size size)
        {
            throw new NotImplementedException();
            //var meta = new Metafile(sourceFile);
            //meta.Save(destFile, ImageFormat.Png);
            //return true;
        }
    }
}