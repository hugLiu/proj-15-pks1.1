using Discuz.Entity;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PKS.Forum.Web
{
    /// <summary>论坛主题变量定义</summary>
    public static class ForumTopicVariables
    {
        /// <summary>全文(发帖人/跟帖人所有的文本)</summary>
        public const string Fulltext = "topicFulltext";
        /// <summary>主题ID</summary>
        public const string TopicId = "topicId";
        /// <summary>主题标题</summary>
        public const string TopicTitle = "topicTitle";
        /// <summary>主题内容</summary>
        public const string TopicMessage = "topicMessage";
        /// <summary>主题作者</summary>
        public const string TopicPoster = "topicPoster";
        /// <summary>主题发布时间</summary>
        public const string TopicPostDateTime = "topicPostDateTime";
        /// <summary>主题相关论坛全称</summary>
        public const string TopicForumFullName = "topicForumFullName";
    }

    /// <summary>论坛主题索引提供者</summary>
    internal class ForumTopicIndexProvider
    {
        /// <summary>模板</summary>
        public string Template { get; set; }
        /// <summary>变量匹配集合</summary>
        public List<Match> Matches { get; set; }
    }

    /// <summary>论坛主题索引处理器</summary>
    public class ForumTopicIndexHandler
    {
        /// <summary>同步根</summary>
        private static object s_SyncRoot { get; } = new object();
        /// <summary>是否初始化</summary>
        private static bool s_Inited { get; set; }
        /// <summary>主题索引标签定义模板</summary>
        private string TopicIndexTemplate { get; set; }
        /// <summary>主题索引标签定义模板</summary>
        private Dictionary<string, ForumTopicIndexProvider> TopicIndexTags { get; set; }
        /// <summary>使用的主题变量标志</summary>
        private Dictionary<string, bool> UsedTopicVariables { get; set; }
        /// <summary>初始化</summary>
        public void Initialize()
        {
            if (s_Inited) return;
            lock (s_SyncRoot)
            {
                if (s_Inited) return;
                InitializeInternal();
            }
        }
        /// <summary>初始化</summary>
        private void InitializeInternal()
        {
            var searchService = ForumExtension.GetService<ISearchService>();
            var metadataDefinitionCollection = searchService.GetMetadataDefinitions();
            MetadataDefinitionCollection.Instance = new MetadataDefinitionCollection(metadataDefinitionCollection);
            this.TopicIndexTemplate = "{" + ConfigurationManager.AppSettings["TopicIndexTemplate"] + "}";
            this.UsedTopicVariables = new Dictionary<string, bool>();
            this.UsedTopicVariables[ForumTopicVariables.Fulltext] = false;
            this.UsedTopicVariables[ForumTopicVariables.TopicForumFullName] = false;
            this.UsedTopicVariables[ForumTopicVariables.TopicId] = false;
            this.UsedTopicVariables[ForumTopicVariables.TopicMessage] = false;
            this.UsedTopicVariables[ForumTopicVariables.TopicPostDateTime] = false;
            this.UsedTopicVariables[ForumTopicVariables.TopicPoster] = false;
            this.UsedTopicVariables[ForumTopicVariables.TopicTitle] = false;
            this.TopicIndexTags = new Dictionary<string, ForumTopicIndexProvider>();
            var templates = this.TopicIndexTemplate.JsonTo<Dictionary<string, string>>();
            var regex = new Regex(@"\[(?<tag>[^\]]+)\]");
            foreach (var pair in templates)
            {
                var matches = regex.Matches(pair.Value);
                var provider = new ForumTopicIndexProvider();
                provider.Template = pair.Value;
                if (matches.Count > 0)
                {
                    provider.Matches = matches.Cast<Match>().ToList();
                    provider.Matches.Reverse();
                    foreach (var match in provider.Matches)
                    {
                        var variableName = match.Groups[1].Value;
                        this.UsedTopicVariables[variableName] = true;
                    }
                }
                this.TopicIndexTags[pair.Key] = provider;
            }
            s_Inited = true;
        }
        /// <summary>生成主题</summary>
        public Dictionary<string, string> BuildTopic(ForumInfo forumInfo, TopicInfo topicInfo, PostInfo postInfo)
        {
            var variables = new Dictionary<string, string>();
            if (this.UsedTopicVariables[ForumTopicVariables.TopicId])
            {
                variables[ForumTopicVariables.TopicId] = topicInfo.Tid.ToString();
            }
            if (this.UsedTopicVariables[ForumTopicVariables.TopicTitle])
            {
                if (topicInfo.Title != null) variables[ForumTopicVariables.TopicTitle] = topicInfo.Title.Trim();
            }
            if (this.UsedTopicVariables[ForumTopicVariables.TopicPoster])
            {
                if (topicInfo.Poster != null) variables[ForumTopicVariables.TopicPoster] = topicInfo.Poster.Trim();
            }
            if (this.UsedTopicVariables[ForumTopicVariables.TopicPostDateTime])
            {
                if (topicInfo.Postdatetime != null) variables[ForumTopicVariables.TopicPostDateTime] = DateTime.Parse(topicInfo.Postdatetime).ToISODateString();
            }
            if (postInfo != null && this.UsedTopicVariables[ForumTopicVariables.TopicMessage])
            {
                variables[ForumTopicVariables.TopicMessage] = NormalizeMessage(postInfo.Message);
            }
            //if (this.UsedTopicVariables[ForumTopicVariables.Fulltext]) { }
            if (this.UsedTopicVariables[ForumTopicVariables.TopicForumFullName])
            {
                var fullName = forumInfo.Pathlist.Trim().Replace("</a>", "");
                fullName = Regex.Replace(fullName, @"\<a[^\>]+\>", "\\");
                fullName = Regex.Replace(fullName, @"&[^;]+;", "");
                fullName = Regex.Replace(fullName, @"\s", "");
                variables[ForumTopicVariables.TopicForumFullName] = fullName;
            }
            return variables;
        }
        /// <summary>规范化消息</summary>
        private string NormalizeMessage(object message)
        {
            return NormalizeMessage(message?.ToString());
        }
        /// <summary>规范化消息</summary>
        private string NormalizeMessage(string message)
        {
            if (message == null) return string.Empty;
            var newMessage = message.Trim();
            if (newMessage.Length > 0)
            {
                newMessage = Regex.Replace(newMessage, @"\[[^\]]+\]", "");
                newMessage = Regex.Replace(newMessage, @":-?[a-zA-Z]+", ",");
                newMessage = Regex.Replace(newMessage, @"\s+", ",");
            }
            return newMessage;
        }
        /// <summary>生成回复</summary>
        public Dictionary<string, string> BuildPost(ForumInfo forumInfo, TopicInfo topicInfo, PostInfo postInfo)
        {
            var posts = Discuz.Forum.Posts.GetPostListTitle(topicInfo.Tid);
            var variables = BuildTopic(forumInfo, topicInfo, null);
            if (this.UsedTopicVariables[ForumTopicVariables.TopicMessage])
            {
                variables[ForumTopicVariables.TopicMessage] = NormalizeMessage(posts.Rows[0]["message"]);
            }
            if (this.UsedTopicVariables[ForumTopicVariables.Fulltext])
            {
                var sBuilder = new StringBuilder(255);
                for (int i = 1; i < posts.Rows.Count; i++)
                {
                    var message = NormalizeMessage(posts.Rows[i]["message"]);
                    if (message.Length == 0) continue;
                    sBuilder.AppendLine(message);
                }
                if (sBuilder.Length > 0)
                {
                    variables[ForumTopicVariables.Fulltext] = sBuilder.ToString();
                }
            }
            return variables;
        }
        /// <summary>处理</summary>
        public Metadata Handle(Dictionary<string, string> variables)
        {
            var indexData = new Metadata();
            var builder = new StringBuilder(255);
            foreach (var tag in this.TopicIndexTags)
            {
                var ignore = false;
                var provider = tag.Value;
                object value = null;
                if (provider.Matches == null)
                {
                    value = provider.Template;
                }
                else
                {
                    builder.Length = 0;
                    builder.Append(provider.Template);
                    foreach (var match in provider.Matches)
                    {
                        var variableName = match.Groups[1].Value;
                        var variableValue = variables.GetValueBy(variableName);
                        builder.Remove(match.Index, match.Length);
                        if (!variableValue.IsNullOrEmpty())
                        {
                            builder.Insert(match.Index, variableValue);
                        }
                    }
                    value = builder.ToString();
                }
                if (ignore) continue;
                indexData.SetValue(tag.Key, value);
            }
            return indexData;
        }
    }
}
