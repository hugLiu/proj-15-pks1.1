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
    public class SemanticsRequest
    {
        [DataMember(Name = "term")]
        public string Term { get; set; }
        [DataMember(Name = "sr")]
        public string SR { get; set; }
        [DataMember(Name = "direction")]
        public string Direction { get; set; }
    }
}
