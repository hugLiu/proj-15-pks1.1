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
    public class GetTermInfoRequest
    {
        /// <summary>
        /// 指定的正式叙词列表
        /// </summary>
        [DataMember(Name = "term")]
        public List<string> Term { get; set; }
        /// <summary>
        /// 指定的返回词项的类型
        /// </summary>
        [DataMember(Name = "cc")]
        public string Cc { get; set; }
    }
}
