using System;
using System.Runtime.Serialization;

namespace Jurassic.PKS.Service.Semantics
{
    /// <summary>
    /// 叙词信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class TermInfo
    {
        /// <summary>
        /// 叙词ID
        /// </summary>
        [DataMember(Name = "termclassid")]
        public int TermClassID { get; set; }
        /// <summary>
        /// 概念类编码
        /// </summary>
        [DataMember(Name = "cccode")]
        public string CCCode { get; set; }
        /// <summary>
        /// 叙词名称
        /// </summary>
        [DataMember(Name = "term")]
        public string Term { get; set; }
        /// <summary>
        /// 叙词路径
        /// </summary>
        [DataMember(Name = "pathterm")]
        public string PathTerm { get; set; }
        /// <summary>
        /// 叙词来源
        /// </summary>
        [DataMember(Name = "source")]
        public string Source { get; set; }
        /// <summary>
        /// 叙词描述
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember(Name = "remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 叙词所属概念类
        /// </summary>
        [DataMember(Name = "cc")]
        public string CC { get; set; }
    }
}
