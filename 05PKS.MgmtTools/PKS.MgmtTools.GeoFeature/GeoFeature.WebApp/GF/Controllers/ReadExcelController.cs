using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.OfficeLib;
using System.IO;
using Jurassic.CommonModels;
using System.Data;

namespace GeoFeature.Controllers
{
    public class ReadExcelController : BaseController
    {
        //
        // GET: /ReadExcel/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReadXls(int fileId)
        {
            Stream stream = SiteManager.Get<ResourceFileService>().GetFileStream(fileId);
            ExcelHelper helper = new ExcelHelper(stream);
           DataSet ds = helper.ExcelToDataSet(true);
           return JsonNT(ds.Tables[0]);
        }

    }
}
