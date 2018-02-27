using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Globalization;
using System.IO;
using System.Threading;
using Jurassic.Com.Tools;

namespace Jurassic.AppCenter.Caches
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用于存储并用json序列化和反序列化各类强类型List列表的缓存类
    /// 可以在缓存依赖项更改时，立即刷新缓存，在IIS进程回收时，也可以及时回写内存数据到文件
    /// </summary>
    public class CachedList<T> : CachedObject<List<T>>, IList<T>
    {
        private string _cfgFileName;
        protected override string CfgFileName
        {
            get
            {
                if (_cfgFileName == null)
                    _cfgFileName = Path.Combine(mBasePath, typeof(T).Name + "s.json");
                return _cfgFileName;
            }
            set
            {
                _cfgFileName = value;
            }
        }
        
        #region IList<T> 成员

        public int IndexOf(T item)
        {
            return Item.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Item.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Item.RemoveAt(index);
        }

        /// <summary>
        /// 根据整形下标访问成员
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>值</returns>
        public T this[int index]
        {
            get
            {
                return Item[index];
            }
            set
            {
                Item[index] = value;
            }
        }

        #endregion

        #region ICollection<T> 成员

        public void Add(T item)
        {
            Item.Add(item);
        }

        public void Clear()
        {
            Item.Clear();
        }

        public bool Contains(T item)
        {
            return Item.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Item.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return Item.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return Item.Remove(item);
        }

        #endregion`

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return Item.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Item.GetEnumerator();
        }

        #endregion
    }
}