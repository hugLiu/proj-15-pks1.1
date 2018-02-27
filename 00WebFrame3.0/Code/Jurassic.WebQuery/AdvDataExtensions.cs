using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 数据组件的静态扩展方法类
    /// </summary>
    public static class AdvDataExtensions
    {
        /// <summary>
        /// 用默认的选项生成数据采集组件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString AdvDataGrid(this HtmlHelper htmlHelper)
        {
            var controllerType = htmlHelper.ViewContext.Controller.GetType();
            var type = controllerType.BaseType.GetGenericArguments().FirstOrDefault();
            return htmlHelper.Partial("_AdvDataGrid", type);
        }

        /// <summary>
        /// 根据用户自定义的渲染方法生成数据采集组件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="userRenderer"></param>
        /// <returns></returns>
        public static MvcHtmlString AdvDataEdit(this HtmlHelper htmlHelper, Func<string, object, HelperResult> userRenderer = null)
        {
            htmlHelper.ViewBag.UserRenderer = userRenderer;
            return htmlHelper.Partial("_AdvDataEdit");
        }

        /// <summary>
        /// 选择用户的控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static MvcHtmlString SelectUser(this HtmlHelper helper, SelectUserFormData formData)
        {
            return helper.Partial("_SelectDepUser", formData);
        }

        /// <summary>
        /// 多语言文本控件
        /// </summary>
        /// <param name="helper">HTML帮助对象</param>
        /// <param name="formData">多语言控件需要的表单属性对象</param>
        /// <returns></returns>
        public static MvcHtmlString LangTextInput(this HtmlHelper helper, LangTextFormData formData)
        {
            helper.ViewBag.ParaModel = formData;
            return helper.Partial("_LangTextInput");
        }

        /// <summary>
        /// 多语言文本控件
        /// </summary>
        /// <param name="helper">HTML帮助对象</param>
        /// <param name="name">多语言组件的表单域名称</param>
        /// <param name="attrs">额外的属性集</param>
        /// <param name="iconType">图标显示方式，文本或旗帜</param>
        /// <returns></returns>
        public static MvcHtmlString LangTextInput(this HtmlHelper helper, string name, string attrs = null, CultureIconType iconType = CultureIconType.Text)
        {
            helper.ViewBag.ParaModel = new LangTextFormData
            {
                Name = name,
                Attributes = attrs,
                IconType = iconType
            };
            return helper.Partial("_LangTextInput");
        }
    }
}