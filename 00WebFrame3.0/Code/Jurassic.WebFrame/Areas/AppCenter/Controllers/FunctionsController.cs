using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Resources;
using System.Data;

namespace Jurassic.WebFrame.Controllers
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 功能管理，对功能项的CRUD，除了首页以外，
    /// 其他的都返回Json的ReturnValueWithTips对象
    /// 它的另一个版本，参见<see cref="Jurassic.WebFrame.Controllers.FunctionsSetupController"/>
    /// </summary>
    public class FunctionsController : BaseController
    {
        DataManager<AppFunction> mFunctionMgr = AppManager.Instance.FunctionManager;
        //
        // GET: /App_Center/Functions/

        /// <summary>
        /// 首页，注意它的视图是Shared/FunctionIndex.
        /// 因为这个视图要被两个控制器共用。一个是有权限的功能管理(本控制器），
        /// 一个是不需要权限的初始化操作。
        /// </summary>
        /// <returns>功能管理的View</returns>
        [JAuth(Ord = 0)]
        public ActionResult Index()
        {
            //如果AppFunction没有扩展成子类
            if (mFunctionMgr.DataType == typeof(AppFunction))
            {
                return View("FunctionIndex");
            }
            else
            {
                return View("FunctionIndex_Spc");
            }
        }

        /// <summary>
        /// 返回功能列表, 该方法只用于后台菜单管理
        /// </summary>
        /// <returns>AppFunction的Json数组</returns>
        [JAuth(JAuthType.Ignore)]
        public virtual JsonResult GetAll(string key = null)
        {
            //查询关键字放入Session
            Session["FunctionQueryKey"] = !key.IsEmpty() ? key : "";


            var list = mFunctionMgr.GetAll();
            if (!key.IsEmpty())
            {
                list = SearchKey(list, key);
            }

            return Json(list
                .OrderBy(d => d.Ord)
                .ThenBy(d => d.Id), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 从集合中搜索key
        /// </summary>
        private List<AppFunction> SearchKey(IList<AppFunction> allList, string key)
        {
            List<AppFunction> list = allList.Where(f => f.Id.Contains(key) || f.Name.Contains(key)).ToList();
            foreach (var f in list.ToArray())
            {
                foreach (var ff in f.GetFamily())
                {
                    if (!list.Contains(ff))
                    {
                        list.Add(ff);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 返回需要角色的功能列表,供角色管理使用
        /// </summary>
        /// <returns>AppFunction的Json数组</returns>
        [JAuth(JAuthType.Ignore)]
        public virtual JsonResult GetAllRoleMenus()
        {
            return Json(AppManager.Instance.GetAllRoleMenus()
                .Select(func => GetTreeJsonObject(func)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回功能列表, 该方法用于前台生成菜单
        /// 其中功能的名称已根据资源文件作本地化处理
        /// <code>
        /// //返回的json对象为：
        /// {
        ///   UserMenu:[{Id:xx，ParentId:xxx, Name:xxx,IconLocation,xxx,Location:xxx},...],
        ///   ForbiddenIds:[Id1,Id2,....]
        /// }
        /// </code>
        /// </summary>
        /// <returns>AppFunction的Json数组</returns>
        [JAuth(JAuthType.Ignore)]
        public virtual JsonResult GetUserMenu()
        {
            var userMenu = CurrentUser == null ? null : new
            {
                UserMenu = AppManager.Instance.GetUserMenus(User.Identity.Name)
                .Select(func => new
                {
                    Id = func.Id,
                    ParentId = func.ParentId,
                    Name = ResHelper.GetStr(func.Name),
                    IconLocation = func.IconLocation,
                    IconClass = func.IconClass,
                    Location = func.Location.IsEmpty() ? null : Url.Content(func.Location),
                    Method = func.Method,
                    Visible = func.Visible
                }),
                ForbiddenIds = User.Identity.GetForbiddenIds()
            };

            return Json(userMenu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查是否有ID相同的功能
        /// </summary>
        /// <param name="id">功能ID</param>
        /// <returns>是/否</returns>
        [OutputCache(Duration = 0)]
        [JAuth(JAuthType.Ignore)]
        public virtual JsonResult CheckFunctionId(string id)
        {
            var func = Session["EditingFunction"] as AppFunction;
            var tFunc = mFunctionMgr.GetById(id);
            return Json(tFunc == null || func == null || tFunc.Id == func.Id, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回要编辑的功能的详细信息
        /// </summary>
        /// <param name="id">要编辑的树结点ID</param>
        /// <param name="parentId">父结点ID</param>
        /// <returns>功能对象的的Json</returns>
        public virtual JsonResult Edit(string id, string parentId)
        {
            AppFunction func = null;
            func = mFunctionMgr.GetById(id) ?? mFunctionMgr.Create();
            Session["EditingFunction"] = func;
            return JsonTips(func, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交修改并返回树结点信息---所有节点提交
        /// </summary>
        /// <param name="form">要修改的功能</param>
        /// <returns>修改后的功能信息Json</returns>
        [HttpPost]
        [JAuth(Name = "Edit")]
        public JsonResult Edit(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //整体提交的数据
                var data = Request.Form["data"];
                List<AppFunction> funcList = JsonHelper.FromJson<List<AppFunction>>(data);

                List<AppFunction> oldFuncList = mFunctionMgr.GetAll().ToList();

                //从Session取查询关键字
                var queryKey = Session["FunctionQueryKey"] as string;
                if (!queryKey.IsEmpty())
                {
                    oldFuncList = SearchKey(oldFuncList, queryKey);
                }

                //清除
                foreach (AppFunction func in oldFuncList)
                {
                    mFunctionMgr.Remove(func);
                }
                //新增
                foreach (AppFunction func in funcList)
                {
                    mFunctionMgr.Add(func);
                }
                //保存
                mFunctionMgr.Save();

                return JsonTips("success", JStr.SuccessSaved);
            }

            return JsonTips();
        }

        private void SaveParameters(AppFunction func)
        {
            string parameters = Request.Form["Parameter"];
            func.Parameters.Clear();
            if (parameters.IsEmpty()) return;
            StringSpliter ss = new StringSpliter(parameters, "\n", "=");
            foreach (var ky in ss.Keys)
            {
                func.Parameters.Add(new AppParameter
                {
                    Name = ky,
                    ValuePattern = ss[ky]
                });
            }
        }

        private object GetTreeJsonObject(AppFunction func)
        {
            return new
            {
                id = func.Id,
                pId = func.ParentId,
                name = func.ToString(),
                icon = func.IconLocation,
                iconCls = func.IconClass,
                ord = func.Ord,
                visibleType = func.Visible,
                relatedIds = func.RelatedIds
            };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">要删除的ID列表</param>
        /// <returns>删除的结果提示</returns>
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            List<string> idArr = ids.Split(',')
                .Where(id => !id.IsEmpty()).ToList();
            List<string> deleted = new List<string>();

            foreach (var id in idArr)
            {
                var func = mFunctionMgr.GetById(id);
                var childIds = mFunctionMgr.GetAll()
                    .Where(f => f.ParentId == id).Select(f => f.Id);

                //ztree会把半选的父结点也传过来，所以要判断，如果所选父结点的子结点
                //有一个不删除，则父结点不删
                if (childIds.Any(cid => !idArr.Contains(cid)))
                {
                    continue;
                }
                mFunctionMgr.Remove(func);
                deleted.Add(func.Id);
            }

            return JsonTips("success", JStr.SuccessDeleted, deleted);
        }

        /// <summary>
        /// 移动结点
        /// </summary>
        /// <param name="id">要移动的结点</param>
        /// <param name="dir">移动方向</param>
        /// <returns>移动以后的序号</returns>
        [HttpPost]
        public JsonResult Sort(string id, int dir)
        {
            var func = mFunctionMgr.GetById(id);
            var funcs = mFunctionMgr.GetAll().Where(f => f.ParentId == func.ParentId)
                .OrderBy(f => f.Ord).ToList();

            int index = funcs.IndexOf(func);
            if (index == 0 && dir < 0)
            {
                func.Ord = index + 1;
                return JsonTips("warning", "已经位于首位", func.Ord);
            }
            if (index == (funcs.Count - 1) && dir > 0)
            {
                func.Ord = index + 1;
                return JsonTips("warning", "已经位于末尾", func.Ord);
            }
            funcs.Remove(func);
            funcs.Insert(index + dir, func);
            int ord = 1;
            funcs.Each(f => { f.Ord = ord++; mFunctionMgr.Change(f); });
            return Json(func.Ord);
        }

        string parentId;
        string[] idArr;

        /// <summary>
        /// 将选中结点移到另一个结点下
        /// </summary>
        /// <param name="ids">要移动的结点ID列表</param>
        /// <param name="pId">要移到的新结点ID，0代表移动到根结点</param>
        /// <returns>移动成功与否的JsonTips</returns>
        [HttpPost]
        public JsonResult Move(string ids, string pId)
        {
            parentId = pId;
            if (parentId != "0")
            {
                var func = mFunctionMgr.GetById(parentId);
                if (func == null)
                {
                    return JsonTips("error", "Parent_Node_NotFound");
                }
            }
            else
            {
                parentId = null;
            }
            idArr = ids.Split(',')
                 .Where(id => !id.IsEmpty()).ToArray();
            if (idArr.Contains(pId))
            {
                return JsonTips("error", FStr.NodeCannotMoveToItself);
            }
            AppFunction movedFunc = null;
            foreach (var func in GetParents())
            {
                func.ParentId = parentId;
                movedFunc = func;
            }

            return JsonTipsLang("success", JStr.SuccessMoved, JStr.SuccessMoved0, GetTreeJsonObject(movedFunc), movedFunc.Name);
        }

        List<AppFunction> GetParents()
        {
            List<AppFunction> funcList = new List<AppFunction>();
            for (int i = 0; i < idArr.Length; i++)
            {
                var id = idArr[i];
                if (id.IsEmpty()) continue;
                var childIds = GetDescendant(id);
                var func = mFunctionMgr.GetById(id);
                //ztree会把半选的父结点也传过来，所以要判断，如果所选父结点的子结点
                //全选，则是对父结点操作，否则是对子结点操作
                if (childIds.Count == 0 || childIds.All(cid => idArr.Contains(cid)))
                {
                    funcList.Add(func);
                    for (int j = 0; j < idArr.Length; j++)
                    {
                        if (childIds.Contains(idArr[j]))
                        {
                            idArr[j] = null;
                        }
                    }
                }
            }
            return funcList;
        }

        /// <summary>
        /// 获取所有后代
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetDescendant(string id)
        {
            List<string> descs = new List<string>();

            var children = mFunctionMgr.GetAll()
                            .Where(f => f.ParentId == id);

            foreach (var child in children)
            {
                descs.Add(child.Id);
                descs.AddRange(GetDescendant(child.Id));
            }

            return descs;
        }

        private string CreateNewId(AppFunction func)
        {
            var brothers = mFunctionMgr.GetAll().Where(f => f.ParentId == func.ParentId);

            string maxId = brothers.Max(f => f.Id);
            while (mFunctionMgr.GetById(maxId) != null)
            {
                maxId = (maxId.ToInt() + 1).ToString().PadLeft(maxId.Length, '0');
            }
            return maxId;
        }

        /// <summary>
        /// 获取系统所有定义过的语言Key供Menu菜单选择它的Name
        /// </summary>
        /// <returns></returns>
        public JsonResult GetResKeys()
        {
            var keys = ResHelper.GetAllResourceKeys();
            return Json(keys.OrderBy(k => k)
                .Select(k => new
                {
                    id = k,
                    text = ResHelper.GetStr(k)
                }), JsonRequestBehavior.AllowGet);
        }
    }
}
