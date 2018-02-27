using Jurassic.AppCenter;
using Jurassic.AppCenter.Resources;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
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
    /// 提供一个内容管理文章管理的基类，包含基本的文章的CRUD操作
    /// </summary>
    public class ArticleController : BaseController
    {
        ArticleManager _article;
        public ArticleController()
        {
            _article = SiteManager.Get<ArticleManager>();
        }

        /// <summary>
        /// 筛选器，用于过滤查到的记录
        /// </summary>
        protected virtual IEnumerable<Expression<Func<Base_CatalogArticle, bool>>> ModelFilters
        {
            get
            {
                if (key != null)
                {
                    yield return art => art.Article.Title.Contains(key);
                }
            }
        }

        private string key;

        protected int _innerCatalogId;

        public override int CatalogId
        {
            get
            {
                string catName = Request.QueryString["cat"];
                if (!catName.IsEmpty())
                {
                    Base_Catalog cat = SiteManager.Catalog.GetByName(catName);
                    return cat == null ? _innerCatalogId : cat.Id;
                }
                else
                {
                    return _innerCatalogId;
                }
            }
        }


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
            var pager = _article.GetPageInCatalog(pageModel, ModelFilters.ToArray());
            AfterGetPageData(pager);
            return View(pager);
        }

        protected virtual void BeforeGetPageData(PageModel pageModel)
        {

        }

        protected virtual void AfterGetPageData(Pager<Base_CatalogArticle> pager)
        {

        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="caId">栏目文章ID</param>
        /// <returns>用户编辑页面</returns>
        public virtual ActionResult Edit(int caId = 0)
        {
            var ca = (caId > 0) ? _article.GetById(caId) : _article.CreateByCatalog(CatalogId);
            _innerCatalogId = ca.CatalogId;
            Session["Editing_" + _innerCatalogId] = ca;
            //ViewBag.ShowSearchBox = false;
            ViewBag.ShowBreadCrumb = false;
            Base_Catalog cat = SiteManager.Catalog.GetById(_innerCatalogId);
            string catName = cat == null ? null : cat.Name;
            ca.CheckExts();
            BeforeShowEditForm(ca);
            return View(ca.Article);
        }

        /// <summary>
        /// 在显示录入窗体之前调用
        /// </summary>
        /// <param name="ca"></param>
        protected virtual void BeforeShowEditForm(Base_CatalogArticle ca)
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
        public ActionResult Edit(Base_Article art, int caId = 0)
        {
            art.EditorId = CommOp.ToInt(CurrentUserId);
            art.State = ArticleState.Published;
            Base_CatalogArticle ca;
            if (caId == 0)
            {
                ca = new Base_CatalogArticle { CatalogId = CatalogId };
            }
            else
            {
                ca = _article.GetById(caId);
                _innerCatalogId = ca.CatalogId;
            }

            ca.Article = art;

            if (art.Id > 0 && caId == 0)
            {
                var oldCaId = _article.GetByArticleId(art.Id, CatalogId).Id;
                ca.Id = oldCaId;
            }

            if (BeforeSaving(ca))
            {
                _article.Save(ca);
            }
            return AfterSaved(ca);
        }

        /// <summary>
        /// 重写此方法以决定是否用系统默认的方式保存。
        /// </summary>
        /// <param name="ca">栏目文章对象</param>
        /// <returns>是否用系统默认的方式保存</returns>
        protected virtual bool BeforeSaving(Base_CatalogArticle ca)
        {
            return true;
        }

        /// <summary>
        /// 重写此方法以定义成功保存以后的返回值
        /// </summary>
        /// <param name="ca">栏目文章对象</param>
        /// <returns>返回给客户端的输出</returns>
        protected virtual ActionResult AfterSaved(Base_CatalogArticle ca)
        {
            return JsonTips("success", JStr.SuccessSaved, ca.Article.Title);
        }

        /// <summary>
        /// 提交删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Delete(string ids)
        {
            foreach (int id in CommOp.ToIntArray(ids, ','))
            {
                _article.Delete(id);
            }
            return JsonTips("success", JStr.SuccessDeleted);
        }

        protected override void Dispose(bool disposing)
        {
            _article.Dispose();
        }
    }
}
