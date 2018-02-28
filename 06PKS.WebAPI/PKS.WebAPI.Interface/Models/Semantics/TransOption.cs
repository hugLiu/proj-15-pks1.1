using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.PKS.Service.Semantics
{
    [Serializable]
    [DataContract]
    public class TransOption
    {
        [DataMember(Name = "langcode")]
        public IEnumerable<string> LangCode { get; set; }
        [DataMember(Name = "onlymain")]
        public bool OnlyMain { get; set; }
    }
}