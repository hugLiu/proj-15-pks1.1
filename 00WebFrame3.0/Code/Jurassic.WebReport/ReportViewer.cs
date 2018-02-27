using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using System.Data;


namespace Jurassic.WebReport
{
    public static class ReportViewer
    {
        /// <summary>
        /// 直接设置报表的各个参数
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="reportFile">报表设计器文件名称</param>
        /// <param name="dataSetName">数据集名称</param>
        /// <param name="dataSource">数据源</param>
        /// <returns></returns>
        public static MvcHtmlString Report(this HtmlHelper htmlHelper, string reportFile, string dataSetName, dynamic dataSource)
        {
            HttpContext.Current.Session["reportFile"] = reportFile;
            HttpContext.Current.Session["dataSetName"] = dataSetName;
            HttpContext.Current.Session["dataSource"] = dataSource;
            string aspxUrl = (htmlHelper.ViewContext.Controller as Controller).Url.Content("~/WebForm1.aspx");
            string aspx = "<iframe src=\"" + aspxUrl + "\" style=\"width:100%;height:580px;\"></iframe>";
            return new MvcHtmlString(aspx);
             
        }
        /// <summary>
        /// 通过属性类，设置报表的必要属性，以及选择性设置工具栏的工具显示
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="obj">属性集对象</param>
        /// <returns></returns>
        public static MvcHtmlString Report(this HtmlHelper htmlHelper, ReportProperty obj)
        {
            HttpContext.Current.Session["property"] = obj;
            string aspxUrl = (htmlHelper.ViewContext.Controller as Controller).Url.Content("~/WebForm1.aspx");
            string aspx = "<iframe src=\"" + aspxUrl + "\" style=\"width:100%;height:600px;border:1px solid #A09C9C;\"></iframe>";
            return new MvcHtmlString(aspx);
        }
    }
}

