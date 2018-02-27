using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PKS.Utils;

namespace PKS.Validation
{
    /// <summary>指定需要集合字段值。</summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionRequiredAttribute : ValidationAttribute
    {
        /// <summary>构造函数</summary>
        public CollectionRequiredAttribute()
        {
        }
        /// <summary>获取或设置一个值，该值指示是否允许空集合。</summary>
        /// <returns>如果允许空集合则为 true；否则为 false。默认值为 false。</returns>
        public bool AllowEmpty { get; set; }
        /// <summary>检查必填集合字段的值是否不为空。</summary>
        /// <returns>如果验证成功，则为 true；否则为 false。</returns>
        /// <param name="value">要验证的集合字段值。</param>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">集合字段值为 null。</exception>
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var collection = value.As<ICollection>();
            if (collection == null) return false;
            if (collection.Count > 0) return true;
            return this.AllowEmpty;
        }
    }
}
