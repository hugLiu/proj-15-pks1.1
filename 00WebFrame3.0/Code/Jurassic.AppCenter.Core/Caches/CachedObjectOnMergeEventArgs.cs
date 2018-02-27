using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter.Caches
{
    /// <summary>
    /// 当CachedObject的缓存依赖项更改时，新数据和旧数据的合并事件参数
    /// </summary>
    /// <typeparam name="T">缓存的类型</typeparam>
    public class CachedObjectOnMergeEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 根据旧对象和新对象创建一个合并数据类
        /// </summary>
        /// <param name="oldObj"></param>
        /// <param name="newObj"></param>
        public CachedObjectOnMergeEventArgs(T oldObj, T newObj)
        {
            NewObject = newObj;
            OldObject = oldObj;
        }

        /// <summary>
        /// 新对象
        /// </summary>
        public T NewObject { get; set; }

        /// <summary>
        /// 旧对象
        /// </summary>
        public T OldObject { get; set; }
    }
}
