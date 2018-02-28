using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models
{
    /// <summary>
    /// 注册页面模版
    /// </summary>
    public class PageRegisterModel
    {
        public int pageid { get; set; }
        public string name { get; set; }
        public bool istemplate { get; set; }
        public bool isbaserender { get; set; }
        public string storagetype { get; set; }
        public string contentref { get; set; }
        public string system { get; set; }
        public string resourcetype { get; set; }
        public string resourcekey { get; set; }
    }
}