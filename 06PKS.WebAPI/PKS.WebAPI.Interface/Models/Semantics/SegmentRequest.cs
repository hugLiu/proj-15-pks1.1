using Jurassic.PKS.Service.Semantics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.PKS.WebAPI.Semantics
{
    [Serializable]
    [DataContract]
    public class SegmentRequest
    {
        /// <summary>
        /// 待处理的短语
        /// </summary>
        [DataMember(Name = "sentence")]
        public string Sentence { get; set; }
        /// <summary>
        /// 分词选项
        /// </summary>
        [DataMember(Name = "option")]
        public SegmentOption Option { get; set; }
    }
}
