using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Jurassic.AppCenter;
using System.Collections.Generic;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebFrame.Controllers
{
    /// <summary>
    /// 分类目录的管理控制器
    /// </summary>
    public class CatalogController : BaseController
    {
        private void InitMenuBar()
        {
            var buttons = this.Function.GetChildren();
            if (buttons.Count() > 0) return;
            string controllerName = this.GetType().FullName;
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_add_brother",
                Name = "AddBrother",
                IconClass = "icon-new-addpeers",
                AuthType = JAuthType.AllUsers,
                Ord = 1,
                Visible = VisibleType.Button | VisibleType.Menu,
            });
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_add_child",
                AuthType = JAuthType.AllUsers,
                Name ="AddChild",
                IconClass = "icon-new-addchild",
                Ord = 2,
                Visible = VisibleType.Button
            });
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_save",
                Name = "Save",
                Location = "~/AppCenter/Catalog/Edit",
                IconClass = "icon-new-save",
                Method = WebMethod.POST,
                AuthType = JAuthType.AllUsers,
                Description = "保存所有修改",
                Ord = 3,
                Visible = VisibleType.Button | VisibleType.Menu,
                ControllerName = controllerName,
                ActionName = "Edit",
            });
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_delete",
                Name = "Delete",
                Location = "~/AppCenter/Catalog/Delete",
                IconClass = "icon-new-delete",
                Method = WebMethod.POST,
                AuthType = JAuthType.AllUsers,
                Ord = 4,
                Visible = VisibleType.Button | VisibleType.Menu,
                ControllerName = controllerName,
                ActionName = "Delete",
            });
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_up",
                Name = "MoveUp",
                Location = "~/AppCenter/Catalog/Sort",
                IconClass = "icon-new-moveup",
                Method = WebMethod.POST,
                AuthType = JAuthType.AllUsers,
                Ord = 5,
                Visible = VisibleType.Button | VisibleType.Menu,
                ControllerName = controllerName,
                ActionName = "Sort",
            });
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_down",
                Name = "MoveDown",
                Location = "~/AppCenter/Catalog/Sort",
                IconClass = "icon-new-movedown",
                Method = WebMethod.POST,
                AuthType = JAuthType.AllUsers,
                Ord = 6,
                Visible = VisibleType.Button | VisibleType.Menu,
                ControllerName = controllerName,
                ActionName = "Sort",
            });
            AppManager.Instance.FunctionManager.Add(new AppFunction
            {
                ParentId = Function.Id,
                Id = Function.Id + "_move",
                Name = "Move",
                Location = "~/AppCenter/Catalog/Move",
                IconClass = "icon-new-movedown",
                Method = WebMethod.POST,
                AuthType = JAuthType.AllUsers,
                Ord = 7,
                Visible = VisibleType.Button | VisibleType.Menu,
                ControllerName = controllerName,
                ActionName = "Move",
            });

            ViewBag.ReloadMenu = true;
        }

        /// <summary>
        /// 目录管理的主界面
        /// </summary>
        /// <param name="root">管理的目录树的根结点ID或名称</param>
        /// <returns>主界面视图</returns>
        public ActionResult Index(string root = "")
        {
            InitMenuBar();
            Base_Catalog rootCat = null;
            if (CommOp.IsNumeric(root))
            {
                rootCat = SiteManager.Catalog.GetById(CommOp.ToInt(root));
            }
            if (rootCat == null && !root.IsEmpty())
            {
                rootCat = SiteManager.Catalog.GetByName(root);
            }
            if (rootCat == null)
            {
                rootCat = SiteManager.Get<Base_Catalog>();
            }
            return View(rootCat);
        }

        /// <summary>
        /// 返回功能列表, 该方法只用于后台菜单管理
        /// </summary>
        /// <returns>Catalog的Json数组</returns>
        public virtual JsonResult GetAll(int id = 0)
        {
            List<Base_Catalog> cats = null;
            if (id == 0)
            {
                cats = SiteManager.Catalog.GetAllValid();
            }
            else
            {
                cats = SiteManager.Catalog.GetDescendantByLang(id);
                cats.Insert(0, SiteManager.Catalog.GetById(id));
            }
            return Json(cats
            .OrderBy(d => d.Ord)
            .ThenBy(d => d.Id)
            .Select(cat => new
            {
                id = cat.Id,
                pId = cat.ParentId,
                name = cat.Id + ":" + cat.Name,
                icon = cat.IconLocation,
                ord = cat.Ord
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回要编辑的功能的详细信息
        /// </summary>
        /// <param name="id">要编辑的树结点ID</param>
        /// <returns>功能对象的的Json</returns>
        public virtual JsonResult Edit(int id)
        {
            Base_Catalog cat = null;
            cat = SiteManager.Catalog.GetById(id) ?? SiteManager.Catalog.Create();
            // Session["EditingFunction"] = func;
            return JsonTips(cat, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交修改并返回树结点信息
        /// </summary>
        /// <param name="cat">要修改的功能</param>
        /// <returns>修改后的功能信息Json</returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public virtual JsonResult Edit(Base_Catalog cat)
        {
            if (Request.Form["ExtId"] != null)
            {
                SaveExts(cat, Request.Form);
            }
            SiteManager.Catalog.Save(cat);
            return JsonTips("success", JStr.SuccessSaved, JStr.SuccessSaved0, (object)cat.Id, cat.Name);
        }

        //保存扩展属性
        private void SaveExts(Base_Catalog cat, NameValueCollection form)
        {
            cat.Exts.Clear();
            var ids = form["ExtId"].Split(',');
            var names = form["ExtName"].Split(',');
            var defaultValues = form["DefaultValue"].Split(',');
            var dataTypes = form["DataType"].Split(',');
            var maxLengths = form["MaxLength"].Split(',');
            //var allownulls = form["AllowNull"].Split(',');
            var states = form["ExtState"].Split(',');
            var dataSourceTypes = form["DataSourceType"].Split(',');
            var dataSources = form["DataSource"].Split(',');

            for (int i = 1; i < names.Length; i++)
            {
                if (!string.IsNullOrEmpty(names[i]))
                {
                    Base_CatalogExt ext = new Base_CatalogExt
                    {
                        Id = CommOp.ToInt(ids[i]),
                        CatalogId = cat.Id,
                        Name = names[i],
                        DefaultValue = defaultValues[i],
                        DataType = CommOp.ToEnum<ExtDataType>(dataTypes[i]),
                        State = states[i] == "" ? ArticleState.Published : CommOp.ToInt(states[i]),
                        Ord = i,
                        //AllowNull = CommOp.ToBool(allownulls[i] == "on" ? true : false),
                        MaxLength = CommOp.ToInt(maxLengths[i]),
                        DataSourceType = CommOp.ToEnum<ExtDataSourceType>(dataSourceTypes[i]),
                        DataSource = dataSources[i]
                    };
                    cat.Exts.Add(ext);
                }

            }
        }

        /// <summary>
        /// 移动结点
        /// </summary>
        /// <param name="id">要移动的结点</param>
        /// <param name="dir">移动方向</param>
        /// <returns>移动以后的序号</returns>
        [HttpPost]
        public void Sort(string id, int dir)
        {
            SiteManager.Catalog.Sort(Convert.ToInt32(id), dir);
        }


        /// <summary>
        /// 将选中结点移到另一个结点下
        /// </summary>
        /// <param name="ids">要移动的结点ID列表</param>
        /// <param name="pId">要移到的新结点ID，0代表移动到根结点</param>
        /// <returns>移动成功与否的JsonTips</returns>
        [HttpPost]
        public JsonResult Move(string ids, int pId)
        {
            var idArr = ids.Split(',')
                 .Where(id => !id.IsEmpty())
                 .Select(id => CommOp.ToInt(id))
                 .ToList();
            SiteManager.Catalog.Move(idArr, pId);
            return JsonTips("success", JStr.SuccessMoved);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">要删除的ID列表</param>
        /// <returns>删除的结果提示</returns>
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            var idArr = ids.Split(',')
                 .Where(id => !id.IsEmpty())
                 .Select(id => CommOp.ToInt(id))
                 .ToList();
            return JsonTips("success",JStr.SuccessDeleted, SiteManager.Catalog.Delete(idArr.ToArray()));
        }
    }
}
