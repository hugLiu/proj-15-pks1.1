using Jurassic.Com.OfficeLib;
using Jurassic.CommonModels;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebTemplate.Controllers
{
    public class UploadExcelController : BaseController
    {
        // GET: ExcelPreview
        public ActionResult Index()
        {
            return View();
        }

        //文件上传后的回调事件
        public ActionResult GetExcelData(ResourceFileInfo[] results)
        {
            ExcelHelper excelHelper = new ExcelHelper(results[0].FileStream);
            DataTable tb = excelHelper.ExcelToDataTable("myTable", false);
            return JsonNT(tb);
        }


    }
}