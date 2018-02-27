using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 高级查询的扩展方法静态类
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// 创建一个查询类
        /// </summary>
        public static MvcHtmlString AdvQuery<T>(this HtmlHelper htmlHelper)
            where T : class
        {
            QueryFormData data = new QueryFormData
            {
                ModelType = typeof(T)
            };
            return htmlHelper.Partial("_AdvQuery", data);
        }

    }

}