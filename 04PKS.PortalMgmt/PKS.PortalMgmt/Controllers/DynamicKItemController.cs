using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class DynamicKItemController : BaseController
    {
        // GET: DynamicKItem
        public ActionResult Index()
        {
            return View();
        }
    }
}