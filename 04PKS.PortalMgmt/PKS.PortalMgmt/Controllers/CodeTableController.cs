using PKS.DbServices;
using PKS.DbServices.Portal.Model;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class CodeTableController : PKSBaseController
    {

        public CodeManageService _codeManageService { get; set; }

        public CodeTableController(CodeManageService codeManageService)
        {
            _codeManageService = codeManageService;
        }

        // GET: CodeTable
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有的码表类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllCodeType()
        {
            var result = _codeManageService.GetAllCodeType();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult AddCodeType(string type)
        //{
        //    var result = _codeManageService.GetAllCodeType();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 获取码表记录
        /// </summary>
        /// <param name="codeTypeId"></param>
        /// <returns></returns>
        public JsonResult GetCodes(int? codeTypeId, string code, string title)
        {
            var dataResult = new List<CodeModel>();
            var result = _codeManageService.GetCodes(codeTypeId);

            if (!string.IsNullOrEmpty(code))
            {
                code = HttpUtility.UrlDecode(code);
                result = result.FindAll(t => t.Code.Contains(code.Trim()));
            }

            if (!string.IsNullOrEmpty(title))
            {
                title = HttpUtility.UrlDecode(title);
                result = result.FindAll(t => t.Title.Contains(title.Trim()));
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑码表信息
        /// </summary>
        /// <param name="codeModel">传入码表对象</param>
        public void UpdateCode(string codeModelStr)
        {
            if (string.IsNullOrEmpty(codeModelStr))
                return;
            codeModelStr = HttpUtility.UrlDecode(codeModelStr);
            List<CodeModel> models = codeModelStr.JsonTo<List<CodeModel>>();
            if (models != null && models.Count > 0)
                _codeManageService.UpdateCode(models, CurrentUser.Name);
        }

        public void UpdateCodeType(string codeModelStr)
        {
            if (string.IsNullOrEmpty(codeModelStr))
                return;
            codeModelStr = HttpUtility.UrlDecode(codeModelStr);
            List<CodeTypeModel> models = codeModelStr.JsonTo<List<CodeTypeModel>>();
            if (models != null && models.Count > 0)
                _codeManageService.UpdateCodeType(models, CurrentUser.Name);
        }
    }
}