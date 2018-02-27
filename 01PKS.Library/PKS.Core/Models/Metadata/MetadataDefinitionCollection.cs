using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>元数据定义集合</summary>
    public class MetadataDefinitionCollection : Dictionary<string, MetadataDefinition>
    {
        /// <summary>构造函数</summary>
        public MetadataDefinitionCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
        /// <summary>构造函数</summary>
        public MetadataDefinitionCollection(IEnumerable<MetadataDefinition> values) : base(StringComparer.OrdinalIgnoreCase)
        {
            foreach (var tag in values)
            {
                Add(tag.Name, tag);
            }
        }
        /// <summary>元数据定义集合</summary>
        public static MetadataDefinitionCollection Instance { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
