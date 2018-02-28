using System;
using System.Runtime.Serialization;

namespace Jurassic.PKS.Service.Semantics
{
    /// <summary>
    ///概念类信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class ConceptClass
    {
        /// <summary>
        /// 概念类编码
        /// </summary>
        [DataMember(Name = "cccode")]
        public string CCCode { get; set; }
        /// <summary>
        /// 概念类名称
        /// </summary>
        [DataMember(Name = "cc")]
        public string CC { get; set; }
        /// <summary>
        /// 概念类标签
        /// </summary>
        [DataMember(Name = "tag")]
        public string Tag { get; set; }
        /// <summary>
        /// 概念类类型
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
