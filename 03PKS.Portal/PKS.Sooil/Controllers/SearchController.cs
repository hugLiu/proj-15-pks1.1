using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PKS.Sooil.Controllers
{
    public class SearchController : SooilBaseController
    {
        //[AllowAnonymous]
        public ActionResult AdvanceSearch()
        {
            return View();
        }

        public ActionResult List()
        {
            ViewBag.token = base.Token;
            return View();
        }

        public ActionResult SearchResultDetail()
        {
            return View();
        }
    }

    
}