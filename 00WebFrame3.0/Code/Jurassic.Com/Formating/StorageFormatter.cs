using Jurassic.Com.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Com.Formating
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 供显示控件格式化存储容量的格式化类
    /// </summary>
    public class StorageFormatter : IFormatProvider, ICustomFormatter
    {
        #region IFormatProvider 成员
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

        #endregion

        #region ICustomFormatter 成员
        /// <summary>
        /// 返回对象值的格式
        /// </summary>
        /// <param name="fFormat"></param>
        /// <param name="arg"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string Format(string fFormat, object arg, IFormatProvider provider)
        {
            long size = CommOp.ToLong(arg);
            return new StorageSize(size).ToString();
        }

        #endregion
    }
}
