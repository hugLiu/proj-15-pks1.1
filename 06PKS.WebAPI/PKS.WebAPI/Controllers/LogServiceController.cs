using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PKS.Models;
using PKS.WebAPI.Models;
using System.Web.Security;

namespace PKS.WebAPI.Controllers
{
    /// <summary>日志服务控制器</summary>
    public class LogServiceController : PKSApiController
    {
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "日志服务用于记录前端脚本发生的错误"
            };
        }

        /// <summary>记录JS错误</summary>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Log(PKS_Log log)
        {
            PKS.Core.Bootstrapper.Log(log);
            return Ok();
        }
        /// <summary>清空外部缓存</summary>
        [HttpGet]
        [AllowAnonymous]
        public FormsAuthenticationTicket GetTicket()
        {
            return this.Request.GetTicketFromCookie();
        }
        /// <summary>测试</summary>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Test()
        {
            //var s = @"D:\Sites\SZXT.Prototype\Content\SZXT\Images1\2.png";
            //var d = @"D:\2.png";
            //PKS.Core.Bootstrapper.Get<PKS.MgmtServices.Converters.IThumbnailConverter>().Execute(s, d, new System.Drawing.Size(32, 32));
            return Ok();
        }
    }
}
