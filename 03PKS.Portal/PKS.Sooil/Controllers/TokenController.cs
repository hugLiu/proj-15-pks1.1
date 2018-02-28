using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PKS.Sooil.Controllers
{
    public class TokenController : SooilBaseController
    {
        [HttpPost]
        public ActionResult GetToken()
        {
            return Content(base.Token);
        }
    }
}