using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebHtmlEditor
{
    /// <summary>
    /// 用于初始化HtmlEditor的实体类
    /// </summary>
    public class EditorFormData
    {
        public string JsObjectName { get; set; }

        /// <summary>
        /// 和编辑对应的TextArea控件的名称(Name属性）
        /// </summary>
        public string TextAreaName { get; set; }

        /// <summary>
        /// 和编辑器对应的TextArea的ID属性，当无法确知Name属性时，可指定ID
        /// </summary>
        public string TextAreaId { get; set; }

        /// <summary>
        /// 是否采用全部工具栏（默认为否）
        /// </summary>
        public bool FullToolbar { get; set; }
    }
}