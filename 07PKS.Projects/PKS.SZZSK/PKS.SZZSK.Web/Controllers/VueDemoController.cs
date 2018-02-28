using System.Web.Mvc;

namespace PKS.SZZSK.Web.Controllers
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

        public ActionResult Text()
        {
            return View();
        }
    }
}