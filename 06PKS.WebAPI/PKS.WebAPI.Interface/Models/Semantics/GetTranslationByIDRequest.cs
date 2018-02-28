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
    public class GetTranslationByIDRequest
    {
        /// <summary>
        /// 叙词ID
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
        /// <summary>
        /// 语言类型，如 en-US zh-CN
        /// </summary>
        [DataMember(Name = "langcode")]
        public string LangCode { get; set; }
        /// <summary>
        /// 只包含主词
        /// </summary>
        [DataMember(Name = "onlymain")]
        public bool OnlyMain { get; set; }
    }
}
