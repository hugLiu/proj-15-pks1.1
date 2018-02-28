using PKS.DbServices.KHome;
using PKS.DbServices.KHome.Model;
using PKS.Utils;
using PKS.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PKS.PortalMgmt.Controllers
{
    public class KHomeController : PKSBaseController
    {
        private readonly KHomeService kHomeService;
        private readonly ISearchService searchService;

        public KHomeController(KHomeService kHomeService)
        {
            this.kHomeService = kHomeService;
            searchService = GetService<ISearchService>();
        }

        // GET: KHome
        public ActionResult Index()
        {
            var metadata = searchService.GetMetadataDefinitions()
                .Where(t => t.InnerTag == false)
                .Select(t => new
                {
                    id = t.Name,
                    text = t.Title
                }).ToList();
            ViewBag.Metadata = metadata.ToJson();
            return View();
        }

        public ActionResult PostModule()
        {
            //获取岗位列表
            var Posts = kHomeService.GetPostList();
            var metadata = searchService.GetMetadataDefinitions()
                .Where(t => t.InnerTag == false)
                .Select(t => new
                {
                    id = t.Name,
                    text = t.Title
                }).ToList();
            ViewBag.Metadata = metadata.ToJson();
            ViewBag.Posts = Posts.ToJson();
            return View();
        }

        public ActionResult SelectModule()
        {
            return View();
        }

        #region 模块树维护

        public JsonResult GetModuleTree()
        {
            var result = kHomeService.GetModuleTree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int SaveNewModuleNode(string nodeParams)
        {
            int id = 0;
            var models = nodeParams.JsonTo<List<ModuleTreeNode>>();
            if (models != null && models.Count == 1)
            {
                var model = models.First();
                if (model.IsModule)
                {
                    id = kHomeService.AddModule(model, CurrentUser.Name);
                }
                else
                {
                    id = kHomeService.AddModuleCategory(model, CurrentUser.Name);
                }
            }
            return id;
        }

        public void SaveModuleNode(string nodeParams)
        {
            var models = nodeParams.JsonTo<List<ModuleTreeNode>>();
            if(models!=null&&models.Count == 1)
            {
                var model = models.First();
                if (model.IsModule)
                {
                    kHomeService.UpdateModule(model, CurrentUser.Name);
                }
                else
                {
                    kHomeService.UpdateModuleCategory(model, CurrentUser.Name);
                }
            }
        }

        public void DeleteModuleNode(int id, bool isModule)
        {
            if (isModule)
            {
                kHomeService.DeleteModule(id);
            }
            else
            {
                kHomeService.DeleteModuleCategory(id);
            }
        }

        #endregion

        #region 模块查询条件

        public JsonResult GetModuleQueries(int moduleId)
        {
            var result = kHomeService.GetModuleQueries(moduleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int UpdateQuery(string data)
        {           
            var model = data.JsonTo<ModuleQueryInfo>();
            int id = model.Id;
            if (id == -1)
            {
                id = kHomeService.AddQuery(model, CurrentUser.Name);
            } else
            {
                kHomeService.UpdateQuery(model, CurrentUser.Name);
            }
            return id;
        }

        public void DeleteQuery(int queryId)
        {
            kHomeService.DeleteQuery(queryId);
        }

        public JsonResult GetModuleQueryParams(int queryId)
        {
            var result = kHomeService.GetModuleQueryParams(queryId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void UpdateQueryParam(int queryId, string data)
        {
            var model = data.JsonTo<List<QueryParamItem>>();
            kHomeService.UpdateQueryParams(queryId, model, CurrentUser.Name);
        }

        #endregion

        #region 岗位模块

        public JsonResult GetPostModules(int postid)
        {
            var result = kHomeService.GetPostModuleList(postid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void MovePostModule(int postModuleId1, int postModuleId2)
        {
            kHomeService.MovePostModule(postModuleId1, postModuleId2);
        }

        public void AddPostModule(string newRow)
        {
            var model = newRow.JsonTo<PostModule>();
            kHomeService.AddPostModule(model, CurrentUser.Name);
        }

        public void RemovePostModule(int id)
        {
            kHomeService.RemovePostModule(id);
        }

        public JsonResult GetModuleFilterParams(int queryId, int postModuleId)
        {
            var result = kHomeService.GetFilterParams(queryId, postModuleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void UpdateFilterParam(int queryId, int postModuleId,string data)
        {
            var model = data.JsonTo<List<QueryParamItem>>();
            kHomeService.UpdateFilterParams(queryId, postModuleId, model, CurrentUser.Name);
        }

        public JsonResult GetModuleSortParams(int queryId, int postModuleId)
        {
            var result = kHomeService.GetSortParams(queryId, postModuleId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void UpdateSortParam(int queryId, int postModuleId, string data)
        {
            var model = data.JsonTo<List<QueryParamItem>>();
            kHomeService.UpdateSortParams(queryId, postModuleId, model, CurrentUser.Name);
        }

        public int? GetReturnCount(int queryId, int postModuleId)
        {
            var result = kHomeService.GetReturnCount(queryId, postModuleId);
            return result;
        }

        public void UpdateReturnCount(int queryId, int postModuleId, int count)
        {
            kHomeService.UpdateReturnCount(queryId, postModuleId, count, CurrentUser.Name);
        }
        #endregion

    }
}