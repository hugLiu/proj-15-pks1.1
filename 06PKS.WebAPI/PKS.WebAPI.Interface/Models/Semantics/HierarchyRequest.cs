using System;
using System.Runtime.Serialization;

namespace Jurassic.PKS.WebAPI.Semantics
{
    /// <summary>获得树的层级结构</summary>
    [Serializable]
    [DataContract]
    public class HierarchyRequest
    {
        /// <summary>叙词ID</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
        /// <summary>指定关系叙词的概念类</summary>
        [DataMember(Name = "cc")]
        public string Cc { get; set; }
        /// <summary>返回树的层级</summary>
        [DataMember(Name = "deeplevel")]
        public int DeepLevel { get; set; }
    }
}