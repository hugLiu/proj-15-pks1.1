using Jurassic.AppCenter.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 基于本地json文件存储的数据提供类
    /// </summary>
    /// <typeparam name="T">存储的数据类型</typeparam>
    public class LocalDataProvider<T> : DataProvider<T>
    {
        CachedList<T> mTList;
        /// <summary>
        /// 从json文件加载全部T的列表
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<T> GetData()
        {
            if (mTList == null)
            {
                mTList = new CachedList<T>();
            }
            return mTList;
        }
    }
}
