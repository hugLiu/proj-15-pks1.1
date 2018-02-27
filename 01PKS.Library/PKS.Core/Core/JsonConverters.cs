using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>自定义对象转换器</summary>
    public class BindingJsonConverter<T> : CustomCreationConverter<T>
        where T : class
    {
        #region 序列化
        /// <summary>
        /// Creates an object which will then be populated by the serializer.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The created object.</returns>
        public override T Create(Type objectType)
        {
            return typeof(T).CreateInstance().As<T>();
        }
        #endregion
    }
}