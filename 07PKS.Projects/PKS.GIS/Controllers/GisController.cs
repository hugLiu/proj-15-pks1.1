using System.Web.Mvc;
using PKS.Web.Controllers;

namespace PKS.GIS.Controllers
{
    public class GisController : PKSBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}