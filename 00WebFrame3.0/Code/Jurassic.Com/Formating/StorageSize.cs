using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Jurassic.Com.Formating
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 支持存储器容量的格式化显示
    /// </summary>
    public class StorageSize : IFormattable
    {
        private decimal mSize;

        /// <summary>
        /// 根据指定的字节数，新建一个存储器容量格式化类
        /// </summary>
        /// <param name="size">字节数</param>
        public StorageSize(long size)
        {
            this.mSize = size;
        }
        /// <summary>
        /// 返回字节数的KB值
        /// </summary>
        public decimal KB
        {
            get { return mSize / 1024; }
        }

        /// <summary>
        /// 返回字节数的MB值
        /// </summary>
        public decimal MB
        {
            get { return mSize / 1024 / 1024; }
        }

        /// <summary>
        /// 返回字节数的GB值
        /// </summary>
        public decimal GB
        {
            get { return mSize / 1024 / 1024 / 1024; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString("G", null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return this.ToString(format, null);
        }
        /// <summary>
        /// 重写TOSTRING函数
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider provider)
        {
            // Handle null or empty arguments.
            if (String.IsNullOrEmpty(format)) format = "G";
            // Remove any white space and convert to uppercase.
            format = format.Trim().ToUpperInvariant();

            if (provider == null) provider = NumberFormatInfo.CurrentInfo;

            switch (format)
            {
                case "KB":
                    return this.KB.ToString("N1", provider) + " KB";
                case "MB":
                    return this.MB.ToString("N1", provider) + " MB";
                case "GB":
                    return this.GB.ToString("N1", provider) + " GB";
                case "G":
                    if (GB > 1) return ToString("GB");
                    if (MB > 1) return ToString("MB");
                    if (KB > 1) return ToString("KB");
                    return mSize.ToString() + " B";
                default:
                    throw new FormatException(String.Format("不支持'{0}'的格式符.", format));
            }
        }
    }
}
