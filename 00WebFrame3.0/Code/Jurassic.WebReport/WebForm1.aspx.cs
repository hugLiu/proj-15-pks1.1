using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jurassic.WebReport
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var a = Session["property"] as ReportProperty;
                ReportViewer1.Width = a.width > 0 ? a.width : 1100;
                ReportViewer1.Height = a.height > 0 ? a.height : 500;
                ReportViewer1.ShowToolBar = a.ShowToolBar;
                ReportViewer1.ShowPrintButton = a.ShowPrintButton;
                ReportViewer1.ShowRefreshButton = a.ShowRefreshButton;
                ReportViewer1.ShowFindControls = a.ShowFindControls;
                BindReptData();
            }
        }

        private void BindReptData()
        {
            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.Dispose();
            this.ReportViewer1.LocalReport.DataSources.Clear();

            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath((Session["property"] as ReportProperty).ReportFileName);
            this.ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource((Session["property"] as ReportProperty).DataSourceName, (Session["property"] as ReportProperty).DataSource));
            //如果是只设定必要的三个参数，则使用下面两行代码
            //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath(HttpContext.Current.Session["reportFile"] as String);
            //this.ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(HttpContext.Current.Session["dataSetName"] as String, HttpContext.Current.Session["dataSource"]));
            this.ReportViewer1.LocalReport.Refresh();
        }
    }
}