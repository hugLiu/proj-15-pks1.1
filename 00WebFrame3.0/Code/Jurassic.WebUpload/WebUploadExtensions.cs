using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Jurassic.WebUpload
{
    /// <summary>
    /// 对上传组件进行的HTML帮助类扩展
    /// </summary>
    public static class WebUploadExtensions
    {
        /// <summary>
        /// 加载上传组件代码
        /// </summary>
        /// <param name="htmlHelper">HTML帮助对象</param>
        /// <param name="formDataName">UploadScriptsLoaded</param>
        /// <param name="actionName">上传完毕后服务端要执行的方法名称</param>
        /// <param name="controllerName">上传完毕后服务端要执行的方法所在控制器名称</param>
        /// <param name="jsDoneFunction">服务端处理完后回调的js函数名称</param>
        /// <returns></returns>
        public static MvcHtmlString Upload(this HtmlHelper htmlHelper,
            string formDataName,
            string actionName,
            string controllerName,
            string jsDoneFunction)
        {
            UploadFormData data = new UploadFormData
            {
                FormDataName = formDataName,
                ActionName = actionName,
                ControllerName = controllerName,
                JsDoneFunction = jsDoneFunction
            };
            return htmlHelper.Partial("_FileUpload", data);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, UploadFormData data)
        {
            return htmlHelper.Partial("_FileUpload", data);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper,
          string formDataName,
          string actionName,
          string jsDoneFunction)
        {
            UploadFormData data = new UploadFormData
            {
                FormDataName = formDataName,
                ActionName = actionName,
                JsDoneFunction = jsDoneFunction
            };
            return htmlHelper.Partial("_FileUpload", data);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper,
          string formDataName,
          string jsDoneFunction)
        {
            UploadFormData data = new UploadFormData
            {
                FormDataName = formDataName,
                ActionName = null,
                JsDoneFunction = jsDoneFunction
            };
            return htmlHelper.Partial("_FileUpload", data);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper,
           string formDataName)
        {
            UploadFormData data = new UploadFormData
            {
                FormDataName = formDataName,
            };
            return htmlHelper.Partial("_FileUpload", data);
        }

        public static void SetFriendFileName(this HttpResponseBase response, string fileName)
        {
           response.AddHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(fileName));
        }
    }
}