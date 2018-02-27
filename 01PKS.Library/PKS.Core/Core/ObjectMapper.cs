using System.Collections.Generic;
using AutoMapper;

namespace PKS.Core
{
    /// <summary>对象映射扩展</summary>
    public static class ObjectMapperExtension
    {
        /// <summary>映射到目标类型数据</summary>
        public static TDestination MapTo<TDestination>(this object value)
        {
            return Mapper.Map<TDestination>(value);
        }

        /// <summary>映射到目标类型数据</summary>
        public static void MapTo(this object source, object dest)
        {
            Mapper.Map(source, dest);
        }

        /// <summary>映射到目标类型数据集合</summary>
        public static IEnumerable<TDestination> MapTo<TDestination>(this IEnumerable<object> values)
        {
            foreach (var value in values)
                yield return value.MapTo<TDestination>();
        }
    }
}