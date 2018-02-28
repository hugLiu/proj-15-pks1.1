using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Collections;
using System.Data.Entity;
using Jurassic.AppCenter;
using PKS.DBModels;
using PKS.Data;
using PKS.PortalMgmt.Models;
using PKS.Core;
using CacheManager.Core;

namespace PKS.PortalMgmt.Controllers
{
    public class SubSystemController : PKSBaseController
    {
        private IRepository<PKS_SUBSYSTEM> db;

        public ActionResult Index()
        {
            ViewBag.ShowToolBar = false;
            return View();
        }

        public SubSystemController(IRepository<PKS_SUBSYSTEM> db)
        {
            this.db = db;
        }

        public JsonResult EditConfig(int pageIndex = 0, int pageSize = 10)
        {
            var data = db.GetQuery()
                         .OrderBy(p => p.Id)
                         .Skip(pageIndex * pageSize)
                         .Take(pageSize)
                         .Select(p => new SubSystemEntity
                         {
                             Id = p.Id.ToString(),
                             Code = p.Code,
                             Name = p.Name,
                             RootUrl = p.RootUrl
                         })
                         .ToList();
            return Json(new { data = data, pageSize = data.Count, total = data.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGridData(int pageIndex = 0, int pageSize = 10)
        {
            IQueryable<PKS_SUBSYSTEM> allList = db.GetQuery();
            Pager<PKS_SUBSYSTEM> pager = new Pager<PKS_SUBSYSTEM>(allList.OrderBy(d => d.Id), pageIndex + 1, pageSize);
            List<PKS_SUBSYSTEM> lstData = new List<PKS_SUBSYSTEM>();

            IEnumerator<PKS_SUBSYSTEM> pages = pager.GetEnumerator();

            while (pages.MoveNext())
            {
                lstData.Add(pages.Current);
            }
            //List<PKS_SUBSYSTEM> lstData = db.PKS_SUBSYSTEM.ToList();
            Hashtable result = new Hashtable();
            result["data"] = lstData;
            result["total"] = pager.RecordCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新或增加
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <returns></returns>
        public ActionResult SavaSubData(string data)
        {
            List<Models.SubSystemEntity> subSystemEntity = Common.Serialize.JSONStringToList<Models.SubSystemEntity>(data);

            if (ModelState.IsValid)
            {
                for (int i = 0; i < subSystemEntity.Count; i++)
                {
                    if (subSystemEntity[i]._state == "added")
                    {
                        PKS_SUBSYSTEM subsystem = new PKS_SUBSYSTEM();
                        subsystem.Id = int.Parse(subSystemEntity[i].Id);
                        subsystem.Name = subSystemEntity[i].Name;
                        subsystem.RootUrl = subSystemEntity[i].RootUrl;
                        subsystem.Code = subSystemEntity[i].Code;
                        subsystem.CreatedBy = CurrentUser.Name;
                        subsystem.CreatedDate = DateTime.Now;
                        subsystem.LastUpdatedBy = CurrentUser.Name;
                        subsystem.LastUpdatedDate = DateTime.Now;
                        Create(subsystem);

                    }
                    else if (subSystemEntity[i]._state == "modified")
                    {
                        PKS_SUBSYSTEM subsystem = new PKS_SUBSYSTEM();
                        subsystem.Id = int.Parse(subSystemEntity[i].Id);
                        subsystem.Name = subSystemEntity[i].Name;
                        subsystem.RootUrl = subSystemEntity[i].RootUrl;
                        subsystem.Code = subSystemEntity[i].Code;
                        subsystem.CreatedBy = subSystemEntity[i].CreatedBy;
                        subsystem.CreatedDate = subSystemEntity[i].CreatedDate;
                        subsystem.LastUpdatedBy = CurrentUser.Name;
                        subsystem.LastUpdatedDate = DateTime.Now;
                        Edit(int.Parse(subSystemEntity[i].Id), subsystem);
                    }
                }
            }

            return RedirectToAction("EditConfig", "SubSystem");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id">编号</param>
        /// <returns></returns>

        public ActionResult Delete(int Id)
        {
            var subsystem = db.GetQuery().Where(u => u.Id == Id).FirstOrDefault();
            if (subsystem != null)
            {
                db.Delete(subsystem);
            }
            return RedirectToAction("GetGridData", "SubSystem");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pks_subsystem"></param>
        public void Edit(int id, PKS_SUBSYSTEM pks_subsystem)
        {
            PKS_SUBSYSTEM subsystem = db.GetQuery().Where(u => u.Id == id).FirstOrDefault();
            subsystem.Name = pks_subsystem.Name;
            subsystem.RootUrl = pks_subsystem.RootUrl;
            subsystem.Code = pks_subsystem.Code;
            subsystem.CreatedBy = pks_subsystem.CreatedBy;
            subsystem.CreatedDate = pks_subsystem.CreatedDate;
            subsystem.LastUpdatedBy = pks_subsystem.LastUpdatedBy;
            subsystem.LastUpdatedDate = pks_subsystem.LastUpdatedDate;
            db.Update(subsystem);

        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="pKS_SUBSYSTEM"></param>
        public void Create(PKS_SUBSYSTEM pKS_SUBSYSTEM)
        {
            if (ModelState.IsValid)
            {
                db.Add(pKS_SUBSYSTEM);
            }
        }
    }
}