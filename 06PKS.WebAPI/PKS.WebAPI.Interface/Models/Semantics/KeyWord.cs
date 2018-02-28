using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.PKS.Service.Semantics
{
    /// <summary>
    /// 预处理返回结果
    /// </summary>
    [Serializable]
    [DataContract]
    public class KeyWord
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public KeyWord()
        {
            this.Cc = new List<string>();
            this.Translates = new List<string>();
            this.Aliases = new List<string>();
        }


        /// <summary>
        /// 叙词
        /// </summary>
        [DataMember(Name = "term")]
        public string Term { get; set; }
        /// <summary>
        /// 概念类
        /// </summary>
        [DataMember(Name = "cc")]
        public List<string> Cc { get; set; }
        /// <summary>
        /// 翻译词
        /// </summary>
        [DataMember(Name = "translates")]
        public List<string> Translates { get; set; }
        /// <summary>
        /// 同义词
        /// </summary>
        [DataMember(Name = "aliases")]
        public List<string> Aliases { get; set; }
    }
}
