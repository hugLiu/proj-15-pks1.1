using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PKS.Services
{
    /// <summary>方括号模式工具</summary>
    public static class BracketPatternUtil
    {
        /// <summary>系统标签</summary>
        public static readonly string System = "system";
        /// <summary>提供者集合</summary>
        private static Dictionary<string, BracketPatternProvider> Providers = new Dictionary<string, BracketPatternProvider>();
        /// <summary>加入一个提供者</summary>
        public static void Add(BracketPatternProvider provider)
        {
            Providers[provider.TagKey] = provider;
        }
        /// <summary>匹配标签键名称</summary>
        private static string MatchTagKeyName { get; } = "tagKey";
        /// <summary>匹配标签值名称</summary>
        private static string MatchTagValueName { get; } = "tagValue";
        /// <summary>解析器</summary>
        private static Regex Resolver { get; } = new Regex(@"\[(?<tagKey>[^\]]+)\](?<tagValue>[^\[]+)\[/\k<tagKey>\]");
        /// <summary>生成方括号模式([tagKey]tagValue[/tagKey])</summary>
        public static string BuildBracketPattern(this string tagKey)
        {
            var provider = Providers.GetValueBy(tagKey);
            var tagValue = provider.GetTagValue();
            return $"[{tagKey}]{tagValue}[/{tagKey}]";
        }
        /// <summary>解析方括号模式</summary>
        public static string ResolveBracketPattern(this string pattern)
        {
            if (pattern.IsNullOrEmpty()) return pattern;
            return Resolver.Replace(pattern, Evaluator);
        }
        /// <summary>解析标签键和标签值</summary>
        public static string Evaluator(Match match)
        {
            var tagKey = match.Groups[MatchTagKeyName].Value;
            var provider = Providers.GetValueBy(tagKey);
            if (provider == null) return match.Value;
            var tagValue = match.Groups[MatchTagValueName].Value;
            return provider.Resolve(tagValue);
        }
    }
    /// <summary>方括号模式提供者</summary>
    public abstract class BracketPatternProvider
    {
        /// <summary>构造函数</summary>
        protected BracketPatternProvider(string tagKey)
        {
            this.TagKey = tagKey;
        }
        /// <summary>标签键</summary>
        public string TagKey { get; }
        /// <summary>获得标签值</summary>
        public virtual string GetTagValue()
        {
            throw new NotImplementedException();
        }
        /// <summary>解析标签值</summary>
        public virtual string Resolve(string tagValue)
        {
            throw new NotImplementedException();
        }
    }
}