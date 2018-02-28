using PKS.Data;
using PKS.DbModels.PortalMgmt;
using PKS.DbServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class PortalExternLinkController : PKSBaseController
    {
        // GET: PortalExternLink
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Links()
        {
            var db = GetService<IRepository<PKS_PORTAL_EXTERN_LINK>>();
            var model = db.GetAll().OrderBy(l=>l.OrderNum).ToList();
            return Json(model,JsonRequestBehavior.AllowGet);
        }

        // just insurance transaction,if transaction is not necessary ,then this can split into three simple action
        public void UpIrd(IEnumerable<PKS_PORTAL_EXTERN_LINK> alinks,
                          IEnumerable<PKS_PORTAL_EXTERN_LINK> ulinks,
                          IEnumerable<PKS_PORTAL_EXTERN_LINK> dlinks)
        {
            var db = GetService<IRepository<PKS_PORTAL_EXTERN_LINK>>();
            var now = DateTime.Now;
            var un = User.Identity.Name;
            PKS_PORTAL_EXTERN_LINK temp = null;
            if (alinks != null)
            {
                foreach (var link in alinks)
                {
                    link.CreatedDate = now;
                    link.LastUpdatedDate = now;
                    link.CreatedBy = un;
                    link.LastUpdatedBy = un;
                    db.Add(link,false);
                }
            }
            if (ulinks != null)
            {
                foreach (var link in ulinks)
                {
                    temp = db.Find(l => l.Id == link.Id);
                    if (temp != null)
                    {
                        temp.LastUpdatedDate = now;
                        temp.LastUpdatedBy = un;
                        temp.Name = link.Name;
                        temp.Url = link.Url;
                        temp.Category = link.Category;
                        temp.OrderNum = link.OrderNum;
                        db.Update(temp,false);
                    }
                }
            }
            if (dlinks != null)
            {
                foreach (var link in dlinks)
                {
                    temp = db.Find(l => l.Id == link.Id);
                    db.Delete(temp,false);
                }
            }
            db.Submit();
        }
    }
}