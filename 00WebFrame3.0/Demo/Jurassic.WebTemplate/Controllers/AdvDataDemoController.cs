using Jurassic.AppCenter.Logs;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    /// <summary>
    /// 利用高级数据组件，实现简单数据的增删改查
    /// </summary>
    public class AdvDataDemoController :AdvDataController<JLogInfo, JLogInfo>
    {
    }
}
