using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PKS.Utils
{
    /// <summary>集合工具</summary>
    public static class CollectionUtil
    {
        #region 查询方法
        /// <summary>根据键获得值，如果值不存在返回默认值</summary>
        public static TValue GetValueBy<TKey, TValue>(this IDictionary<TKey, TValue> values, TKey key)
        {
            TValue value;
            if (values.TryGetValue(key, out value)) return value;
            return default(TValue);
        }
        /// <summary>根据键获得值，如果值不存在返回默认值</summary>
        public static TValue GetValueOrDefaultBy<TKey, TValue>(this IDictionary<TKey, TValue> values, TKey key, TValue defaultValue)
        {
            TValue value;
            if (values.TryGetValue(key, out value)) return value;
            return defaultValue;
        }
        #endregion

        #region 修改方法
        /// <summary>加入集合</summary>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> values, IDictionary<TKey, TValue> values2)
        {
            foreach (var value2 in values2)
            {
                values.Add(value2.Key, value2.Value);
            }
        }
        #endregion

        #region 逻辑方法
        /// <summary>是null或空</summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> values)
        {
            return values == null || values.Count == 0;
        }
        #endregion

        #region 迭代方法
        /// <summary>迭代集合</summary>
        public static void ForEach<T>(this ICollection<T> values, Action<T> action)
        {
            foreach (var value in values) action(value);
        }
        /// <summary>检查并释放</summary>
        public static void TryDispose<T>(this ICollection<T> values)
        {
            foreach (var value in values)
            {
                var disposable = value.As<IDisposable>();
                if (disposable != null) disposable.Dispose();
            }
        }
        #endregion

        #region 转换方法
        /// <summary>转换为强类型集合</summary>
        public static TList ToList<TList, TItem>(this IEnumerable<TItem> values, TList container)
            where TList : List<TItem>
        {
            container.AddRange(values);
            return container;
        }
        /// <summary>转换为强类型字典</summary>
        public static TDictionary ToDictionary<TDictionary, TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values, TDictionary container)
            where TDictionary : IDictionary<TKey, TValue>
        {
            foreach (var pair in values)
            {
                container[pair.Key] = pair.Value;
            }
            return container;
        }
        #endregion

        #region 其它方法
        #endregion
    }
}
