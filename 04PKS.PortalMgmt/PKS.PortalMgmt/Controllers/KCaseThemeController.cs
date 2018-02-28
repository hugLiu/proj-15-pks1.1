using PKS.DbServices;
using PKS.DbServices.KCase.Model;
using PKS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class KCaseThemeController : PKSBaseController
    {
        private readonly KCaseThemeService kCaseThemeService;

        public KCaseThemeController(KCaseThemeService kCaseThemeService)
        {
            this.kCaseThemeService = kCaseThemeService;
        }

        // GET: KCaseTheme
        public ActionResult Index()
        {
            return View();
        }

        #region 主题树

        public JsonResult GetCaseTree()
        {
            return Json(kCaseThemeService.GetCaseTree(), JsonRequestBehavior.AllowGet);
        }

        public int SaveNewCaseNode(string nodeParams)
        {
            int id = 0;
            List<CaseTreeNode> models = nodeParams.JsonTo<List<CaseTreeNode>>();
            if (models != null && models.Count == 1)
            {
                id = kCaseThemeService.AddNewCaseNode(models.First(), CurrentUser.Name);
            }
            return id;
        }

        public void SaveCaseNode(string nodeParams)
        {
            List<CaseTreeNode> models = nodeParams.JsonTo<List<CaseTreeNode>>();
            if (models != null && models.Count == 1)
            {
                kCaseThemeService.UpdateCaseNode(models.First(), CurrentUser.Name);
            }
        }

        public void DeleteCaseNode(int id, bool isCase)
        {
            kCaseThemeService.DeleteCaseNode(id, isCase);
        }

        #endregion

        public String GetThemeDesc(int themeId)
        {
            return kCaseThemeService.GetThemeDesc(themeId);
        }

        public void UpdateThemeDesc(int themeId, String description)
        {
            kCaseThemeService.UpdateThemeDesc(themeId, description, CurrentUser.Name);
        }

        #region 主题参数

        public JsonResult GetParamTreeByThemeId(int id)
        {
            return Json(kCaseThemeService.GetParamTreeByThemeId(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParamInfo(int id)
        {
            return Json(kCaseThemeService.GetParamInfoById(id), JsonRequestBehavior.AllowGet);
        }

        public void SaveParamInfo(string paramInfo)
        {
            var paramModel = paramInfo.JsonTo<ParamModel>();
            kCaseThemeService.SaveParamInfo(paramModel, CurrentUser.Name);
        }

        public int SaveNewParamNode(int themeId, string nodeParams)
        {
            int id = -1;
            List<ParamTreeNode> models = nodeParams.JsonTo<List<ParamTreeNode>>();
            if (models != null && models.Count == 1)
            {
                id = kCaseThemeService.AddNewParamNode(themeId, models.First(), CurrentUser.Name);
            }
            return id;
        }

        public void SaveParamNode(string nodeParams)
        {
            List<ParamTreeNode> models = nodeParams.JsonTo<List<ParamTreeNode>>();
            if (models != null && models.Count == 1)
            {
                kCaseThemeService.UpdateParamNode(models.First(), CurrentUser.Name);
            }
        }

        public void DeleteParamNode(int id, bool isParam)
        {
            kCaseThemeService.DeleteParamNode(id, isParam);
        }

        #endregion

        #region 图版/公式

        public JsonResult GetChartsByThemeId(int themeId)
        {
            var result = kCaseThemeService.GetChartsByThemeId(themeId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void UpdataChart(int themeId, string nodeParams)
        {
            var models = nodeParams.JsonTo<List<ChartModel>>();
            if (models != null && models.Count > 0)
                kCaseThemeService.UpdateChart(themeId, models, CurrentUser.Name);
        }

        #endregion
    }
}