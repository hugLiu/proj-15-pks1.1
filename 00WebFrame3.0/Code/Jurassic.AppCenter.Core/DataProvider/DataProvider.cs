using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// DataManager的数据提供接口的默认实现，应用系统通过继承此类来实现自己的基础数据存取。
    /// </summary>
    public class DataProvider<T> : IDataProvider<T>
    {
        /// <summary>
        /// 获取全部T的列表
        /// </summary>
        /// <returns>关于T的泛型集合</returns>
        public virtual IEnumerable<T> GetData()
        {
            return new List<T>();
        }

        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="t">要新增的T对象</param>
        /// <returns>成功返回大于零的数，不成功返回0</returns>
        public virtual int Add(T t)
        {
            return 1;
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="t">要修改的对象T</param>
        /// <returns>成功返回大于零的数，不成功返回0</returns>
        public virtual int Change(T t)
        {
            return 1;
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="t">要删除的T对象</param>
        /// <returns>成功返回大于零的数，不成功返回0</returns>
        public virtual int Delete(T t)
        {
            return 1;
        }
    }
}
