using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Com.Formating
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 自定义的日期格式化类
    /// </summary>
    public class DateFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// 创建自定义的日期格式化对象
        /// </summary>
        public DateFormatter()
        {
        }

        /// <summary>
        /// 返回对象值的格式
        /// </summary>
        /// <param name="fFormat"></param>
        /// <returns></returns>
        public object GetFormat(Type fFormat)
        {
            if (fFormat == typeof(ICustomFormatter)) return this;
            else return null;
        }

        /// <summary>
        /// 返回带格式的时间值
        /// </summary>
        /// <param name="fFormat"></param>
        /// <param name="arg"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string Format(string fFormat, object arg, IFormatProvider provider)
        {
            if (arg is DateTime || arg is DateTime?)
            {
                return ToCustomText((DateTime)arg);
            }
            return arg == null ? "" : arg.ToString();
        }

        string ToCustomText(DateTime dt)
        {
            DateTime today = DateTime.Today;
            if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
            {
                return "";
            }
            else
            {
                if (dt.Date == today)
                {
                    return dt.ToString("今天 HH:mm");
                }
                else if (dt.Date == today.AddDays(-1))
                {
                    return dt.ToString("昨天 HH:mm");
                }
                else if (dt.Year == today.Year)
                {
                    return dt.ToString("MM-dd HH:mm");
                }
                else
                {
                    return dt.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }
    }
}
