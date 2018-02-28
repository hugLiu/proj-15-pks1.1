using System;
using System.Runtime.Serialization;

namespace Jurassic.PKS.Service.Semantics
{
    /// <summary>
    /// 语义关系类型
    /// </summary>
    [Serializable]
    [DataContract]
    public class SemanticsType
    {
        /// <summary>
        /// 语义关系类型
        /// </summary>
        [DataMember(Name = "sr")]
        public string SR { get; set; }
        /// <summary>
        /// 概念类编码1
        /// </summary>
        [DataMember(Name = "cccode1")]
        public string CCCode1 { get; set; }
        /// <summary>
        ///概念类编码2
        /// </summary>
        [DataMember(Name = "cccode2")]
        public string CCCode2 { get; set; }
        /// <summary>
        /// 语义关系类型描述
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember(Name = "remark")]
        public string Remark { get; set; }
    }
}
