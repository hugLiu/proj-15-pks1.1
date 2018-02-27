using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EntityBase;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.WebRepeater.Controllers
{
    /// <summary>
    /// 提供一个通用列表管理的基类，包含基本的CRUD操作
    /// </summary>
    public class MasterGridController<T> : BaseController where T : class
    {
        EFAuditDataService<T> _dataProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dataProvider">数据提供者</param>
        public MasterGridController(EFAuditDataService<T> dataProvider)
        {
            _dataProvider = dataProvider;
        }


        /// <summary>
        /// 筛选器，用于过滤查到的记录
        /// </summary>
        protected virtual IEnumerable<Expression<Func<T, bool>>> ModelFilters
        {
            get
            {
                if (key != null)
                {
                    yield return null;
                }
            }
        }

        private string key;

        /// <summary>
        /// 返回一个分页的文章列表
        /// </summary>
        /// <param name="pageModel">从视图传过来的分页数据对象</param>
        /// <returns></returns>
        public virtual ActionResult Index(PageModel pageModel)
        {
            pageModel.CatalogId = CatalogId;
            key = pageModel.Key;
            if (pageModel.PageSize <= 5)
            {
                pageModel.PageSize = 5;
            }
            BeforeGetPageData(pageModel);
            if (pageModel.SortField.IsEmpty())
            {
                pageModel.SortField = "Id";
                pageModel.SortOrder = "DESC";
            }
            var pager = new Pager<T>(_dataProvider.GetQuery(), pageModel.PageIndex, pageModel.PageSize);
            AfterGetPageData(pager);
            return View(pager);
        }

        /// <summary>
        /// 定义在获取分页数据之前的操作
        /// </summary>
        /// <param name="pageModel"></param>
        protected virtual void BeforeGetPageData(PageModel pageModel)
        {

        }

        /// <summary>
        /// 定义在获取分页数据之后的操作
        /// </summary>
        /// <param name="pager"></param>
        protected virtual void AfterGetPageData(Pager<T> pager)
        {

        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns>用户编辑页面</returns>
        public virtual ActionResult Edit(int id = 0)
        {
            var t = (id > 0) ? _dataProvider.GetByKey(id) : SiteManager.Get<T>();
            Session["Editing_" + id] = t;
            //ViewBag.ShowSearchBox = false;
            BeforeShowEditForm(t);
            return View(t);
        }

        /// <summary>
        /// 在显示录入窗体之前调用
        /// </summary>
        /// <param name="ca"></param>
        protected virtual void BeforeShowEditForm(T t)
        {

        }

        /// <summary>
        /// 提交编辑
        /// </summary>
        /// <param name="art"></param>
        /// <param name="caId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)] //因为要传递带html标签的art.Text，所以此处不能验证输入
        public ActionResult Edit(T t)
        {
            ICUDEntity cud = t as ICUDEntity;

            if (cud != null)
            {
                cud.UpdaterId = CommOp.ToInt(CurrentUserId);
            }

            if (BeforeSaving(t))
            {
                _dataProvider.Add(t);
            }
            return AfterSaved(t);
        }

        /// <summary>
        /// 重写此方法以决定是否用系统默认的方式保存。
        /// </summary>
        /// <param name="ca">栏目文章对象</param>
        /// <returns>是否用系统默认的方式保存</returns>
        protected virtual bool BeforeSaving(T t)
        {
            return true;
        }

        /// <summary>
        /// 重写此方法以定义成功保存以后的返回值
        /// </summary>
        /// <param name="t">栏目文章对象</param>
        /// <returns>返回给客户端的输出</returns>
        protected virtual ActionResult AfterSaved(T t)
        {
            return JsonTips("success", JStr.SuccessSaved, t);
        }

        /// <summary>
        /// 提交删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Delete(string ids)
        {
            _dataProvider.DeleteByKeys(CommOp.ToIntArray(ids, ','));
            return JsonTips("success", "删除成功！");
        }
    }
}
