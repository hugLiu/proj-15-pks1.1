using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EntityBase;
using Jurassic.CommonModels.ModelBase;
using Jurassic.WebFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 通用的列表界面控制器基类，实体主键是任意类型
    /// </summary>
    /// <typeparam name="TModel">模型的类型</typeparam>
    /// <typeparam name="TKey">数据主键的类型</typeparam>
    public class BaseDataController<TModel, TKey> : BaseController, IPagedDataController<TModel>
        where TModel : class, IId<TKey>
    {
        protected IModelDataService<TModel> _dataService;

        /// <summary>
        /// 数据服务对象的接口实现
        /// </summary>
        protected virtual IModelDataService<TModel> DataService
        {
            get
            {
                if (_dataService == null)
                {
                    _dataService = SiteManager.Get<IModelDataService<TModel>>();
                }
                return _dataService;
            }
        }

        private string _modelSessionKey = typeof(TModel).Name;
        /// <summary>
        /// ctor,根据指定的数据提供者创建
        /// </summary>
        public BaseDataController()
        {
        }

        protected override IEnumerable<ModelError> CheckModelError()
        {
            return null;
        }

        /// <summary>
        /// 返回列表页视图，一般情况下不需要重写此方法
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            ViewBag.QueryType = typeof(TModel);
            ViewBag.SearchEmptyText = ResHelper.GetStr("Enter_Keyword");

            if (!Function.HasChildren() && !Function.Id.IsEmpty())
            {
                //当没有为Index或Edit配置按钮时，添加一些默认按钮
                CreateDefaultChildren();
                ViewBag.ReloadMenu = true;
            }
            return View(typeof(TModel));
        }

        private void CreateDefaultChildren()
        {
            List<AppFunction> children = new List<AppFunction>();
            string controllerName = this.GetType().FullName;
            children.Add(new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                IconClass = "icon-new-edit",
                ParentId = Function.Id,
                Id = "DefaultAdd-" + Function.Id,
                LogType = AppCenter.Logs.JLogType.Info,
                Visible = VisibleType.Button | VisibleType.Menu,
                Ord = 1,
                Name = "新增",
            });

            children.Add(new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                IconClass = "icon-new-edit",
                ParentId = Function.Id,
                Id = "DefaultEdit-" + Function.Id,
                LogType = AppCenter.Logs.JLogType.Info,
                Visible = VisibleType.Button | VisibleType.Menu,
                Ord = 2,
                Name = "编辑",
            });
            children.Add(new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                IconClass = "icon-new-delete",
                ParentId = Function.Id,
                Id = "DefaultDelete-" + Function.Id,
                Method = WebMethod.POST,
                LogType = AppCenter.Logs.JLogType.Info,
                ActionName = "Delete",
                ControllerName = controllerName,
                Location = Url.Action("Delete"),
                Visible = VisibleType.Button | VisibleType.Menu,
                Ord = 3,
                Name = "删除",
            });

            var editPage = new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                ParentId = Function.Id,
                Id = "editpage-" + Function.Id,
                Method = WebMethod.GET,
                LogType = AppCenter.Logs.JLogType.Info,
                ActionName = "Edit",
                ControllerName = controllerName,
                Location = Url.Action("Edit"),
                Visible = VisibleType.Role,
                Ord = 3,
                Name = "编辑页",
            };

            children.Add(editPage);

            children.Add(new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                IconClass = "icon-new-save",
                ParentId = editPage.Id,
                Id = "DefaultSave-" + editPage.Id,
                Method = WebMethod.POST,
                LogType = AppCenter.Logs.JLogType.Info,
                ActionName = "Edit",
                ControllerName = controllerName,
                Location = Url.Action("Edit"),
                Visible = VisibleType.Button | VisibleType.Menu,
                Ord = 1,
                Name = "保存",
            });
            children.Add(new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                IconClass = "icon-new-copy",
                ParentId = editPage.Id,
                Id = "DefaultCopy-" + editPage.Id,
                Method = WebMethod.POST,
                LogType = AppCenter.Logs.JLogType.Info,
                ActionName = "Copy",
                ControllerName = controllerName,
                Location = Url.Action("Copy"),
                Visible = VisibleType.Button | VisibleType.Menu,
                Ord = 2,
                Name = "复制",
            });
            children.Add(new AppFunction
            {
                AuthType = JAuthType.AllUsers,
                IconClass = "icon-new-return",
                ParentId = editPage.Id,
                Id = "DefaultReturn-" + Function.Id,
                Visible = VisibleType.Button | VisibleType.Menu,
                Ord = 3,
                Name = "返回",
            });

            var rule = ModelRule.Get<TModel>();

            //添加子表Grid上方工具栏按钮
            foreach (var r in rule.CollectionRules)
            {
                var detailMenu = new AppFunction
                {
                    AuthType = JAuthType.AllUsers,
                    IconClass = "icon-new-add",
                    ParentId = editPage.Id,
                    Id = r.Name + "-" + editPage.Id,
                    Visible = VisibleType.QuckAccessBar,
                    Ord = 1,
                    Name = r.Name,
                };
                children.Add(detailMenu);
                children.Add(new AppFunction
                {
                    AuthType = JAuthType.AllUsers,
                    IconClass = "icon-new-add",
                    ParentId = detailMenu.Id,
                    Id = "DefaultAdd-" + detailMenu.Id,
                    Visible = VisibleType.Button | VisibleType.Menu,
                    Ord = 1,
                    Name = "新增",
                });
                children.Add(new AppFunction
                {
                    AuthType = JAuthType.AllUsers,
                    IconClass = "icon-new-delete",
                    ParentId = detailMenu.Id,
                    Id = "DefaultDelete-" + r.Name + "-" + editPage.Id,
                    Visible = VisibleType.Button | VisibleType.Menu,
                    Ord = 2,
                    Name = "删除",
                });
            }

            foreach (var func in children)
            {
                AppManager.Instance.FunctionManager.Add(func);
            }
        }

        /// <summary>
        /// 返回列表页视图所需要的列表数据，和高级查询配合，因此
        /// 只需要返回原始数据集
        /// </summary>
        /// <returns></returns>
        [AdvQuery]
        public virtual JsonResult GetData()
        {
            var query = DataService.GetQuery();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public virtual void BeforeShowingPage(Pager<TModel> pagedData)
        {

        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            var result = filterContext.Result as JsonResult;
            if (result == null)
            {
                return;
            }
            var data = RefHelper.GetValue(result.Data, "data") as Pager<TModel>;
            if (data == null)
            {
                return;
            }

            BeforeShowingPage(data);
        }

        /// <summary>
        /// 删除一串ID代表的数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Delete(string ids)
        {
            string[] idArr = ids.Split(',');
            DataService.DeleteByKeys(idArr);
            return JsonTips("success", ResHelper.GetStr("Delete_Success"));
        }

        /// <summary>
        /// 返回数据模型中子类的集合
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        public virtual ActionResult Details(string name, PageModel pm)
        {
            var obj = Session[typeof(TModel).Name] as TModel;
            var propVal = RefHelper.GetValue(obj, name);
            if (propVal == null)
            {
                return null;
            }

            return JsonNT(propVal);
        }

        /// <summary>
        /// 打开新增或编辑器
        /// </summary>
        /// <param name="id">id=0时，打开的是新增页，不为0时，打开的是编缉页</param>
        /// <returns></returns>
        public virtual ActionResult Edit(TKey id = default(TKey))
        {
            TModel t = null;
            if (!id.IsDefault())
            {
                t = DataService.GetByKey(id);
            }
            if (t == null)
            {
                t = SiteManager.Get<TModel>();

                var cuModel = t as ICUModel;

                if (cuModel != null)
                {
                    cuModel.CreateTime = DateTime.Now;
                    cuModel.UpdateTime = DateTime.Now;
                }
            }
            Session[_modelSessionKey] = t;
            ViewBag.ShowSearchBox = false; //在详细页，不显示搜索框
            BeforeShowing(t);
            return View(t);
        }

        /// <summary>
        /// 在派生类中重写，在编辑页面显示前调用
        /// </summary>
        /// <param name="t"></param>
        protected virtual void BeforeShowing(TModel t)
        {

        }

        private TModel GetCurrentModel(TKey id)
        {
            return Session[_modelSessionKey] as TModel ?? (id.IsDefault() ? SiteManager.Get<TModel>() : DataService.GetByKey(id));
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Copy(TKey id)
        {
            var t = GetCurrentModel(id);
            t.Id = default(TKey);
            BeforeShowing(t);
            Session[_modelSessionKey] = t;
            return JsonTips("success", ResHelper.GetStr("Copy_Success"), (object)JsonHelper.ToJson(t, true));
        }

        /// <summary>
        /// 是否使用MVC的强制验证,默认为否
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        /// 接受从新增或编辑页提交的数据交持久化保存
        /// </summary>
        /// <param name="t">从前台Edit页面传递过来的业务对象, 在这里并没有集合属性的值</param>
        /// <returns>保存成功或失败的提示</returns>
        [HttpPost]
        public virtual ActionResult Edit(TModel t)
        {
            if (Strict && !ModelState.IsValid)
            {
                return JsonTips();
            }
            var modelRule = ModelRule.Get<TModel>();
            if (!t.Id.IsDefault()) //在修改操作时，复制原有对象集合到新对象的集合
            {
                var oldt = GetCurrentModel(t.Id);
                foreach (var collrule in modelRule.CollectionRules)
                {
                    collrule.Attr.Property.SetValue(t, modelRule.GetCollectionValue(oldt, collrule.Name), null);
                }
            }
            MethodInfo mi = this.GetType().GetMethod("SaveDetails", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            foreach (ModelRule rule in modelRule.CollectionRules)
            {
                var gmi = mi.MakeGenericMethod(rule.ModelType);
                gmi.Invoke(this, new object[] { t, rule.Name });
            }
            if (!BeforeSaving(t))
            {
                return JsonTips();
            }

            if (t.Id.IsDefault())
            {
                DataService.Add(t);
                AfterSaved(t);
                t = DataService.GetByKey(t.Id);
                BeforeShowing(t);
                Session[_modelSessionKey] = t;
                return JsonTips("success", ResHelper.GetStr("Create_Success"), (object)JsonHelper.ToJson(t, true));
            }
            else
            {
                var id = DataService.Change(t);
                AfterSaved(t);
                t = DataService.GetByKey(t.Id);
                BeforeShowing(t);
                Session[_modelSessionKey] = t;
                return JsonTips("success", ResHelper.GetStr("Update_Success"), (object)JsonHelper.ToJson(t, true));
            }
        }

        /// <summary>
        /// 在派生类中重写，在成功保存后调用
        /// </summary>
        /// <param name="t">保存后的对象</param>
        protected virtual void AfterSaved(TModel t)
        {
        }

        /// <summary>
        /// 在派生类中重写，在保存前调用
        /// </summary>
        /// <param name="t">要保存的对象</param>
        /// <returns>是否继续保存</returns>
        protected virtual bool BeforeSaving(TModel t)
        {
            return true;
        }

        /// <summary>
        /// 分析页面传过来的修改过的子表对象数组，并更新原有子表对象集合
        /// 此方法被自动反射调用
        /// </summary>
        /// <typeparam name="TItemModel">子表对象类型</typeparam>
        /// <param name="t">主表对象</param>
        /// <param name="propName">子表集合在主表对象中的属性名称</param>
        protected void SaveDetails<TItemModel>(TModel t, string propName)
            where TItemModel : IId<TKey>
        {
            String json = Request["DetailChanges_" + propName];

            var prop = typeof(TModel).GetProperty(propName);

            var itemList = prop.GetValue(t, null) as IList<TItemModel>;
            if (itemList == null)
            {
                itemList = new List<TItemModel>();
                prop.SetValue(t, itemList, null);
            }
            //string listTypeName = "System.Collections.Generic.List`1[[" + itemType.AssemblyQualifiedName + "]],mscorlib";
            //Type listType = RefHelper.LoadType(listTypeName);
            var rows = (ArrayList)JsonHelper.FormJson(json);
            var itemModels = JsonHelper.FromJson<List<TItemModel>>(json);
            foreach (var itemModel in itemModels)
            {
                if (itemModel.Id.IsDefault())
                {
                    itemList.Add(itemModel);
                }
            }

            foreach (Hashtable row in rows)
            {
                string id = CommOp.ToStr(row["Id"]);
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                if (state == "removed" || state == "deleted")
                {
                    var oldObj = itemList.FirstOrDefault(item => CommOp.ToStr(item.Id).Equals(id));

                    if (oldObj != null)
                    {
                        itemList.Remove(oldObj);
                    }
                }
                else if (state == "modified" || state == "") //更新：_state为空或modified
                {
                    var oldObj = itemList.FirstOrDefault(item => CommOp.ToStr(item.Id).Equals(id));
                    var newObj = itemModels.First(item => CommOp.ToStr(item.Id).Equals(id));
                    if (oldObj != null)
                    {
                        int idx = itemList.IndexOf(oldObj);
                        itemList[idx] = newObj;

                    }
                }
                else if (state == "added")
                {
                    var oldObj = itemList.FirstOrDefault(item => CommOp.ToStr(item.Id).Equals(id));
                    var newObj = itemModels.FirstOrDefault(item => CommOp.ToStr(item.Id).Equals(id) && !item.Id.IsDefault());
                    if (oldObj == null && newObj != null)
                    {
                        itemList.Add(newObj);
                    }
                }
            }
        }

        /// <summary>
        /// 获取属性中定义的下拉列表选项集合
        /// </summary>
        /// <param name="prop">属性名称</param>
        /// <returns>{id,text}的集合</returns>
        public JsonResult GetPropList(string prop)
        {
            ModelRule rule = ModelRule.Get<TModel>();

            var catAttr = rule.SingleRules.FirstOrDefault(attr => attr.Name.Equals(prop, StringComparison.OrdinalIgnoreCase));
            return GetList(catAttr);
        }

        /// <summary>
        /// 传递用户自定义的下拉列表数据类型
        /// </summary>
        /// <param name="prop">属性名称</param>
        /// <returns>返回{id, text}对象的Json数据</returns>
        public virtual JsonResult GetUserDefineList(string prop)
        {
            return null;
        }

        /// <summary>
        /// 在派生类中重写，获取主表中联动的下拉列表
        /// </summary>
        /// <param name="linkedValue">联动的值</param>
        /// <param name="prop">属性名称</param>
        /// <returns>Json序列化后的{id, text}集合</returns>
        public virtual JsonResult GetLinkedList(string linkedValue, string prop)
        {
            return null;
        }

        JsonResult GetList(CatalogExtAttribute catAttr)
        {
            if (catAttr == null)
            {
                return null;
            }
            IEnumerable<SelectListItem> items = new List<SelectListItem>();
            switch (catAttr.DataSourceType)
            {
                case ExtDataSourceType.DirectList:
                case ExtDataSourceType.MultipleList:
                    items = catAttr.GetSelectList();
                    break;
                case ExtDataSourceType.SqlQuery:
                case ExtDataSourceType.SqlQueryMultipleList:
                    items = catAttr.GetSqlList();
                    break;
                case ExtDataSourceType.UserDefine:
                    return GetUserDefineList(catAttr.Property.Name);
                default:
                    if (catAttr.Property.PropertyType.IsEnum)
                    {
                        items = catAttr.Property.PropertyType.GetSelectList();
                    }
                    break;
            }
            return Json(items.Select(it => new
            {
                id = it.Value,
                text = it.Text,
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取子表中Grid的下拉列表选项
        /// </summary>
        /// <param name="detail">子表中模型中的集合属性名称</param>
        /// <param name="prop">集合属性中单个子项的属性名称</param>
        /// <returns>Json序列化后的{id, text}集合</returns>
        public JsonResult GetDetailPropList(string detail, string prop)
        {
            ModelRule rule = ModelRule.Get<TModel>();
            var collRule = rule.CollectionRules.FirstOrDefault(r => r.Name.Equals(detail, StringComparison.OrdinalIgnoreCase));
            if (collRule == null)
            {
                return null;
            }

            var catAttr = collRule.SingleRules.FirstOrDefault(attr => attr.Name.Equals(prop, StringComparison.OrdinalIgnoreCase));
            return GetList(catAttr);
        }

        /// <summary>
        /// 在派生类中重写，用于返回在子表中某列的联动下拉列表数据
        /// </summary>
        /// <param name="detail">子表中模型中的集合属性名称</param>
        /// <param name="linkedValue">发起联动的源控件的值</param>
        /// <param name="prop">数据源的列字段名</param>
        /// <returns>Json序列化后的{id, text}集合</returns>
        public virtual JsonResult GetDetailLinkedList(string detail, string linkedValue, string prop)
        {
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_dataService != null)
                _dataService.Dispose();
        }
    }
}