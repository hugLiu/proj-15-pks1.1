using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Jurassic.WebReport
{
    public class ReportProperty
    {
        public int width { set; get; }
        public int height { set; get; }
        public bool ShowToolBar { set; get; }
        public bool ShowFindControls { set; get; }
        public bool ShowPrintButton { set; get; }
        public bool ShowRefreshButton { set; get; }
        public string ReportFileName { set; get; }
        public string DataSourceName { set; get; }
        public dynamic DataSource { set; get; }
    }
}