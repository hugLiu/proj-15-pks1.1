using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Jurassic.Com.Tools;

namespace Jurassic.WebHtmlEditor
{
    public static class EditorExtensions
    {
        /// <summary>
        /// 根据指定名称的TextArea元素创建一个HtmlEditor编辑器
        /// </summary>
        /// <param name="htmlHelper">HTML帮助类</param>
        /// <param name="textAreaName">TextArea元素的名称(不是ID，是name属性）</param>
        /// <param name="textAreaName">用于js编程的编辑器对象名称</param>
        /// <returns>Html编辑器的调用代码</returns>
        public static MvcHtmlString HtmlEditor(this HtmlHelper htmlHelper,
            string textAreaName, string jsObjectName = null, bool fullToolbar = false)
        {
            if (jsObjectName.IsEmpty()) jsObjectName = textAreaName;
            EditorFormData data = new EditorFormData
            {
                TextAreaName = textAreaName,
                JsObjectName = jsObjectName,
                FullToolbar = fullToolbar
            };
            return htmlHelper.Partial("_HtmlEditor", data);
        }

        /// <summary>
        /// 根据指定EditorFormData配置创建一个HtmlEditor编辑器
        /// </summary>
        /// <param name="htmlHelper">HTML帮助类</param>
        /// <param name="formData">配置信息</param>
        /// <returns>Html编辑器的调用代码</returns>
        public static MvcHtmlString HtmlEditor(this HtmlHelper htmlHelper, EditorFormData formData)
        {
            if (formData.JsObjectName.IsEmpty()) formData.JsObjectName = formData.TextAreaName;
            return htmlHelper.Partial("_HtmlEditor", formData);
        }
    }
}