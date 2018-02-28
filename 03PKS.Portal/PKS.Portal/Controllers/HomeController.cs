using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.Portal.Controllers
{
    public class HomeController : PortalBaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}