using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Jurassic.WebMap
{
    public static class MapExtensions
    {
        /// <summary>
        /// 创建一个地图控件
        /// </summary>
        /// <param name="htmlHelper">HTML帮助类</param>
        /// <param name="formData">的配置对象</param>
        /// <returns>Html结果以相关前台脚本</returns>
        public static MvcHtmlString Map(this HtmlHelper htmlHelper,
            MapFormData formData)
        {
            return htmlHelper.Partial("_Map", formData);
        }
    }
}