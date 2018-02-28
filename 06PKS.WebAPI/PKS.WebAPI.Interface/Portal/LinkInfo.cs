using System;
using System.Collections.Generic;
using System.Drawing;

namespace PKS.WebAPI.Models
{
    /// <summary>链接信息</summary>
    [Serializable]
    public class LinkCategoryInfo
    {
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>链接信息集合</summary>
        public List<LinkInfo> Links { get; set; }
    }

    /// <summary>链接信息</summary>
    [Serializable]
    public class LinkInfo
    {
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>链接</summary>
        public string Url { get; set; }
        /// <summary>图标链接</summary>
        public string IconUrl { get; set; }
    }
}
