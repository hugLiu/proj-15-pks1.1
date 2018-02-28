using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.SZXT.Web.Controllers
{
    [AllowAnonymous]
    public class VueDemoController:Controller
    {
        public ActionResult SingleTitle2()
        {
            return View();
        }

        public ActionResult SingleImglist2()
        {
            return View();
        }


        public ActionResult SlideImg()
        {
            return View();
        }

        public ActionResult SlidePDF()
        {
            return View();
        }
    }
}