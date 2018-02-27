using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>键值对</summary>
    public class PKSKeyValuePair<TKey, TValue>
    {
        /// <summary>构造函数</summary>
        public PKSKeyValuePair() { }
        /// <summary>构造函数</summary>
        public PKSKeyValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
        /// <summary>键</summary>
        public TKey Key { get; set; }
        /// <summary>值</summary>
        public TValue Value { get; set; }
        /// <summary>生成键值对</summary>
        public static PKSKeyValuePair<T, T> Parse<T>(T[] values)
        {
            return new PKSKeyValuePair<T, T>(values[0], values[1]);
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
