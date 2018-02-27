using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using System.Data;


namespace Jurassic.WebReport.ReportExtensions
{
    public static class ReportViewer
    { 
        public static MvcHtmlString Report(this HtmlHelper htmlHelper, string reportFile, string dataSetName, DataTable dataSource)
        {
            HttpContext.Current.Session["reportFile"] = reportFile;
            HttpContext.Current.Session["dataSetName"] = dataSetName;
            HttpContext.Current.Session["dataSource"] = dataSource;
            string aspx = "<iframe src=\"Jurassic.WebReport/WebForm/WebForm1.aspx\" style=\"width:100%;height:580px;\"></iframe>";
            //var sw = new StringWriter();
            //System.Web.HttpContext.Current.Server.Execute(aspx, sw, true);
            //return new MvcHtmlString(sw.ToString());
            return new MvcHtmlString(aspx);
        }
    }
}