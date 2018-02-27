using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用于GDI+画图的帮助类
    /// </summary>
    public class DrawingHelper
    {
        /// <summary>
        /// 截取图片的一部分
        /// </summary>
        /// <param name="bitmapPathAndName">图片路径名称</param>
        /// <param name="width">部分的宽</param>
        /// <param name="height">截取部分的高</param>
        /// <param name="offsetX">截取部分的x坐标</param>
        /// <param name="offsetY">截取部分的y坐标</param>
        /// <returns>wanted graphic</returns>
        public static Bitmap GetPartOfImage(string bitmapPathAndName, int width, int height, int offsetX, int offsetY)
        {
            Bitmap sourceBitmap = new Bitmap(bitmapPathAndName);
            return GetPartOfImage(sourceBitmap, width, height, offsetX, offsetY);
        }

        /// <summary>
        /// 截取图片的一部分
        /// </summary>
        /// <param name="bitmapPathAndName">要截取的原图</param>
        /// <param name="width">部分的宽</param>
        /// <param name="height">截取部分的高</param>
        /// <param name="offsetX">截取部分的x坐标</param>
        /// <param name="offsetY">截取部分的y坐标</param>
        /// <returns>wanted graphic</returns>
        public static Bitmap GetPartOfImage(Bitmap sourceBitmap, int width, int height, int offsetX, int offsetY)
        {
            Bitmap resultBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resultBitmap))
            {
                Rectangle resultRectangle = new Rectangle(0, 0, width, height);
                Rectangle sourceRectangle = new Rectangle(0 + offsetX, 0 + offsetY, width, height);
                g.DrawImage(sourceBitmap, resultRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }
            return resultBitmap;
        }
        /// <summary>
        /// 截取图片的一部分
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Bitmap GetPartOfImage(Bitmap sourceBitmap, Rectangle rect)
        {
            return GetPartOfImage(sourceBitmap, rect.Width, rect.Height, rect.X, rect.Y);
        }

        /// <summary>
        /// 获取图片的缩略图
        /// </summary>
        /// <param name="img">图像</param>
        /// <param name="w">缩略图宽</param>
        /// <param name="h">缩略图高</param>
        /// <returns>缩略图对象</returns>
        public static Image GetThumbnail(Image img, int w, int h)
        {
            return img.GetThumbnailImage(w, h, callback, IntPtr.Zero);
        }

        /// <summary>
        /// 获取图片的缩略图的流
        /// </summary>
        /// <param name="img">图像</param>
        /// <param name="w">缩略图宽</param>
        /// <param name="h">缩略图高</param>
        /// <returns>缩略图的流对象</returns>
        public static MemoryStream GetThumbnailStream(Image img, int w, int h)
        {
            MemoryStream ms = new MemoryStream();
            img = GetThumbnail(img, w, h);
            img.Save(ms, ImageFormat.Jpeg);
            return ms;
        }

        /// <summary>
        /// 将Byte[]数组转换成Image对象
        /// </summary>
        /// <param name="b">Byte数组</param>
        /// <returns>Image</returns>
        public static Image BytesToImage(byte[] b)
        {
            if (b == null || b.Length == 0) return null;
            MemoryStream stream = new MemoryStream(b, true);
            stream.Write(b, 0, b.Length);
            return new Bitmap(stream);
        }

        /// <summary>
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns>byte数组</returns>
        public static byte[] ImageToBytes(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    data = new byte[ms.Length];
                    ms.Read(data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return data;
        }

        static bool callback()
        {
            return false;
        }
    }
}
