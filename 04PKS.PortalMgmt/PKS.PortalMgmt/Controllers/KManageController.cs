using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbModels.PortalMgmt;
using PKS.DbServices.KManage;
using PKS.PortalMgmt.Models;
using PKS.PortalMgmt.Models.KManage;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class KManageController : PKSBaseController
    {
        private KManageService _kManageService;
        public KManageController(KManageService kManageService)
        {
            _kManageService = kManageService;
        }
        // GET: KManege
        public ActionResult Index()
        {
            return View("GlobalParameter");
        }

        /// <summary>
        /// 全局参数
        /// </summary>
        /// <returns></returns>
        public ActionResult GlobalParameter()
        {
            return View();
        }

        public JsonResult GetGlobalParas()
        {
            var viewModel = new List<TemplateParameterDTO>();
            _kManageService.GetGlobalParas().ForEach(p =>
            {
                viewModel.Add(p.MapTo<TemplateParameterDTO>());
            });
            var re = viewModel.OrderBy(w => w.OrderNumber);
            return Json(re, JsonRequestBehavior.AllowGet);
        }

        public void SaveGlobalParas(string data)
        {
            _kManageService.SaveGlobalParas(data);
        }

        public void DeleteGlobalParas(int[] ids)
        {
            _kManageService.DeleteGlobalParas(ids);
        }

        /// <summary>
        /// 组件注册
        /// </summary>
        /// <returns></returns>
        public ActionResult WidgetRegister()
        {
            return View();
        }

        public JsonResult GetWidgets()
        {
            var viewModel = new List<WidgetTypeDTO>();
            _kManageService.GetWidgets().ForEach(p =>
            {
                viewModel.Add(p.MapTo<WidgetTypeDTO>());
            });
            var re=viewModel.OrderBy(w => w.Category).ThenBy(w=>w.OrderNumber);
            return Json(re, JsonRequestBehavior.AllowGet);
        }

        public void SaveWidgets(string data)
        {
            var viewModel = data.JsonTo<List<WidgetTypeDTO>>();
            var widgets = new List<PKS_KFRAGMENT_TYPE>();
            viewModel.ForEach(p =>
            {
                widgets.Add(p.MapTo<PKS_KFRAGMENT_TYPE>());
            });
            _kManageService.SaveWidgets(widgets);
        }

        public void DeleteWidgets(int[] ids)
        {
            _kManageService.DeleteWidgets(ids);
        }


        public JsonResult GetWidgetParas(int typeId)
        {
            var viewModel = new List<WidgetTypeParamDTO>();
            _kManageService.GetWidgets().Where(w => w.Id == typeId).FirstOrDefault().PKS_KFRAGMENT_TYPE_PARAMETER.ToList().ForEach(p =>
            {
                viewModel.Add(p.MapTo<WidgetTypeParamDTO>());
            });

            return Json(viewModel.OrderByDescending(w => w.Id), JsonRequestBehavior.AllowGet);
        }

        public void SaveWidgetParas(int typeId, string data)
        {
            var viewModel = data.JsonTo<List<WidgetTypeParamDTO>>();
            var paras = new List<PKS_KFRAGMENT_TYPE_PARAMETER>();
            viewModel.ForEach(p =>
            {
                paras.Add(p.MapTo<PKS_KFRAGMENT_TYPE_PARAMETER>());
            });
            _kManageService.SaveWidgetParas(typeId, paras);
        }

        public void DeleteWidgetParas(int id)
        {
            _kManageService.DeleteWidgetParas(id);
        }

        public ActionResult XEditor()
        {
            return View();
        }
    }
}