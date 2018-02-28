using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.PKS.Service.Semantics
{
    /// <summary>
    /// 分词条件
    /// </summary>
    [Serializable]
    [DataContract]
    public class SegmentOption
    {
        /// <summary>词库名称</summary>
        [DataMember(Name = "cc")]
        public List<string> Cc { get; set; }
        /// <summary>是否包含翻译词列表</summary>
        [DataMember(Name = "includetrans")]
        public bool IncludeTrans { get; set; }
        /// <summary>是否包含同义词列表</summary>
        [DataMember(Name = "includealias")]
        public bool IncludeAlias { get; set; }
        /// <summary>匹配规则</summary>
        [DataMember(Name = "matchrule")]
        public string MatchRule { get; set; }
    }
}