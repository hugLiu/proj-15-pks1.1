using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>图片转换器</summary>
    public class ImageConverter : FileConverter, IImageConverter, ISingletonAppService
    {
        /// <summary>获得默认大小</summary>
        public virtual Size MaxSize { get { return Size.Empty; } }
        /// <summary>能处理的扩展名集合</summary>
        public override string[] Exts
        {
            get { return new string[] { "jpg", "jpeg", "exif", "bmp", "png", "tif", "tiff", "gif" }; }
        }
        /// <summary>新文件扩展名</summary>
        public override string NewExt
        {
            get { return "png"; }
        }
        /// <summary>生成指定大小的图片</summary>
        public virtual async Task<bool> ExecuteAsync(string sourceFile, string destFile, Size size)
        {
            return await Task.FromResult(Execute(sourceFile, destFile, size));
        }
        /// <summary>生成指定大小的图片</summary>
        public virtual bool Execute(string sourceFile, string destFile, Size size)
        {
            var image = Image.FromFile(sourceFile);
            return Execute(image, sourceFile, destFile, size);
        }
        /// <summary>生成指定大小的图片</summary>
        protected virtual bool Execute(Image image, string sourceFile, string destFile, Size size)
        {
            int destWidth, destHeight;
            if (image.Width <= size.Width && image.Height <= size.Height)
            {
                var ext = GetExt(sourceFile);
                if (ext == this.NewExt) return false;
                destWidth = image.Width;
                destHeight = image.Height;
            }
            else
            {
                // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置
                var rate = Math.Max(image.Width * 1.0d / size.Width, image.Height * 1.0d / size.Height);
                destWidth = (int)(image.Width / rate);
                destHeight = (int)(image.Height / rate);
            }
            using (var bmp = new Bitmap(destWidth, destHeight))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    // 用白色清空
                    g.Clear(Color.White);
                    // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。
                    g.DrawImage(image, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                    bmp.Save(destFile, ImageFormat.Png);
                }
            }
            return true;
        }
    }
}