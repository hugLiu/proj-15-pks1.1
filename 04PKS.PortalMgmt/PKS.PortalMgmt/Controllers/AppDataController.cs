using Jurassic.WebFrame;
using PKS.Models;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class AppDataController : PKSBaseController
    {
        /// <summary>服务实例</summary>
        //private ISearchService ServiceImpl { get; }


        ///// <summary>构造函数</summary>
        //public AppDataController(ISearchService service)
        //{
        //    ServiceImpl = service;
        //}

        // GET: AppData
        public ActionResult Index()
        {
            ViewBag.ShowToolBar = false;
            ViewBag.Token = Token;
            ViewBag.UserName = PKSUser != null ? PKSUser.Identity.Name : CurrentUser.Name;
            return View();
        }

        [HttpGet]
        public ActionResult GetMetadataDefinition()
        {
            List<MetadataDefinition> metadataDefinitionList = new List<MetadataDefinition>();
            var service = GetService<ISearchServiceWrapper>();
            MetadataDefinition[] metadataDefinitionCollection = service.GetMetadataDefinitions();

            var query = from u in metadataDefinitionCollection
                        where u.UiType != MetadataUiType.Image.ToString()
                        orderby u.GroupOrder descending, u.ItemOrder descending
                        select u;

            foreach (var item in query)
            {
                metadataDefinitionList.Add(item);
            }

            return Json(metadataDefinitionList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOpratorData()
        {
            string resultStr = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Operators.json"));
            return Json(resultStr, JsonRequestBehavior.AllowGet);
            //return resultStr;
        }






    }
}