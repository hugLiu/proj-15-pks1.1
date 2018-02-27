using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebFrame.Controllers
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 由于FunctionsController在初始化阶段也要用，
    /// 所以用一个不用权限的子类包装,一但Setup完成，将不能再进入这个子类
    /// </summary>
    [JSetup]
    [JAuth(JAuthType.Ignore)]
    public class FunctionsSetupController : FunctionsController
    {
    }
}
