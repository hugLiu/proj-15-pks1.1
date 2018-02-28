using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.GeoFeature.Model;
using Jurassic.GeoFeature.BLL;
using System.Xml;
using Jurassic.AppCenter;

using System.IO;


namespace GeoFeature.Controllers
{
    public class objectTypeController : BaseController
    {
        //
        // GET: /objectTypeManager/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /objectTypeManager/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /objectTypeManager/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /objectTypeManager/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /objectTypeManager/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /objectTypeManager/Edit/5

        [HttpPost]
        public ActionResult Edit(string json)
        {
            try
            {
                // TODO: Add update logic here
                var data= HttpUtility.UrlDecode(json);
                List<TypeClassTree> tcList = JsonHelper.FromJson<List<TypeClassTree>>(data);
                new TypeClassTreeManager().Save(tcList);
               // List<TypeClassTree> oldFuncList = mFunctionMgr.GetAll().ToList();
                return JsonTipsLang("success", "Success_Save");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /objectTypeManager/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /objectTypeManager/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult GetTreeData()
        {

            List<TypeClassTree> tcl = new Jurassic.GeoFeature.BLL.TypeClassTreeManager().GetList().ToList();
            return Json(tcl);
        }

        public ActionResult GetObjTypeProperty(string ObjTypeId)
        {
            List<ObjTypePropertyModel> optl = new ObjTypePropertyManager().GetObjTypePropertyBoid(ObjTypeId).ToList() ;
            return Json(optl, JsonRequestBehavior.AllowGet);
        }
        static readonly string DimFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
"App_Data", "DimUnit.json");
        public ActionResult GetDimUnit()
        {
            var content = System.IO.File.ReadAllText(DimFileName, System.Text.Encoding.GetEncoding("gb2312"));
            List<D> dl = JsonHelper.FromJson<List<D>>(content);
            return Json(dl
            .Select(cat => new
            {
                dim = cat.DType
            }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnit(string dim)
        {
            var content = System.IO.File.ReadAllText(DimFileName, System.Text.Encoding.GetEncoding("gb2312"));
            List<D> dl = JsonHelper.FromJson<List<D>>(content);
            List<D> d = dl
                .Where(cat => cat.DType == dim).ToList();
            List<string> ul = d.Count > 0 ? d[0].Ul : null;
            if (ul == null) return null;
            return Json(ul.Select(cat => new { U = cat }), JsonRequestBehavior.AllowGet);
        }
    }
    class D
    {
        public string DType { get; set; }
        public List<string> Ul { get; set; }
    }
}
