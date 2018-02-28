using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    /// <summary>对象类型</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class BOT
    {
        /// <summary>名称</summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }
        [DataMember(Name = "code", IsRequired = true)]
        public string Code { get; set; }
        [DataMember(Name = "ishbo", IsRequired = true)]
        public bool IsHBO { get; set; }
        /// <summary>位置类型</summary>
        [DataMember(Name = "locationtype", IsRequired = false)]
        public GeoJSONObjectType? LocationType { get; set; }
        /// <summary>属性模板</summary>
        [DataMember(Name = "properties", IsRequired = false)]
        public List<BOTPropertyDefinition> Properties { get; set; }
    }
}
