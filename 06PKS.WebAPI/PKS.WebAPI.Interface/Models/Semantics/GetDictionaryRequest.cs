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
    public class GetDictionaryRequest
    {
        [DataMember(Name = "cc")]
        public List<string> Cc { get; set; }
    }
}
