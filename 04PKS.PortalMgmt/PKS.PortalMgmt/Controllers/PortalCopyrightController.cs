using PKS.Data;
using PKS.DbModels.PortalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class PortalCopyrightController : PKSBaseController
    {
        // GET: PortalCopyright
        public ActionResult Index()
        {
            var db = GetService<IRepository<PKS_PORTAL_LINKEDIN_TEXT>>();
            var model = db.GetAll().FirstOrDefault();
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult Save(PKS_PORTAL_LINKEDIN_TEXT copyright)
        {
            var db = GetService<IRepository<PKS_PORTAL_LINKEDIN_TEXT>>();
            var now = DateTime.Now;
            var un = User.Identity.Name;
            var model = db.Find(t => t.Id == copyright.Id);
            if (model != null)
            {
                model.LastUpdatedDate = now;
                model.LastUpdatedBy = un;
                model.Text = copyright.Text;
                db.Update(model);
            }
            else
            {
                copyright.CreatedDate = now;
                copyright.LastUpdatedDate = now;
                copyright.CreatedBy = un;
                copyright.LastUpdatedBy = un;
                db.Add(copyright);
            }
            return RedirectToAction("Index");
        }
    }
}