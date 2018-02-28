using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Plugin.PasswordMode;
using PKS.Forum.Web;
using PKS.Models;
using PKS.Utils;
using PKS.Web;

namespace Discuz.Web.Redirect
{
    /// <summary>用户管理相关重定向操作</summary>
    public partial class header : System.Web.UI.Page
    {
        ///<summary>
        ///WebAPI站点URL
        ///</summary>
        protected internal string webApiSiteUrl = PageBase.WebApiSiteUrl;
    }
}