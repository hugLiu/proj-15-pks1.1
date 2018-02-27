using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Jurassic.Com.Tools;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using Jurassic.CommonModels.FileRepository;
using Ninject;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 文章集中管理对象
    /// </summary>
    public class ArticleManager : IDisposable
    {
        private IAuditDataService<Base_CatalogArticle> _provider;

        public ArticleManager(IAuditDataService<Base_CatalogArticle> provider)
        {
            _provider = provider;
        }

        //public ICacheProvider CacheProvider { get; set; }

        /// <summary>
        /// 获取某栏目下，当前语言的所有子栏目(也包括该栏目本身)文章，主要用于多语言环境中。
        /// </summary>
        /// <param name="rootId">主栏目ID</param>
        /// <returns>文章列表</returns>
        public IQueryable<Base_CatalogArticle> GetAllInCatalog(int rootId)
        {
            var childrenIds = SiteManager.Catalog.GetDescendantByLang(rootId).Select(cat => cat.Id).ToList();
            childrenIds.Add(rootId);

            return GetAllQuery()
                .Where(art => childrenIds.Contains(art.CatalogId));
        }

        /// <summary>
        /// 获取某些栏目下，当前语言的所有子栏目(也包括该栏目本身)文章，主要用于多语言环境中。
        /// </summary>
        /// <param name="rootIds"></param>
        /// <returns></returns>
        public IQueryable<Base_CatalogArticle> GetAllInCatalogs(params int[] rootIds)
        {
            List<int> childrenIds = new List<int>();
            rootIds.Each(rootId =>
            {
                childrenIds.AddRange(SiteManager.Catalog.GetDescendantByLang(rootId).Select(cat => cat.Id));
                childrenIds.Add(rootId);
            });

            return GetAllQuery()
                .Where(art => childrenIds.Contains(art.CatalogId));
        }

        /// <summary>
        /// 获取某栏目下所有文章，不计子栏目的文章
        /// </summary>
        /// <param name="catId">栏目ID</param>
        /// <returns></returns>
        public IQueryable<Base_CatalogArticle> GetAllAtCatalog(int catId)
        {
            return GetAllQuery()
                .Where(art => art.CatalogId == catId);
        }

        /// <summary>
        /// 根据指定筛选条件，获取指定目录中的分页记录
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="catId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Pager<Base_CatalogArticle> GetPageInCatalog(Expression<Func<Base_CatalogArticle, bool>> filter, int catId, int page, int pageSize)
        {
            var childrenIds = SiteManager.Catalog.GetDescendantByLang(catId)
                .Select(cat => cat.Id)
                .ToList();
            childrenIds.Add(catId);
            using (_provider)
            {
                return _provider.PageQuery(filter.And(art => childrenIds.Contains(art.CatalogId)), page, pageSize);
            }
        }

        /// <summary>
        /// 根据分页数据对象获取分页后的文章列表
        /// </summary>
        /// <param name="pageModel">分页数据对象</param>
        /// <param name="modelFilters">一连串筛选器</param>
        /// <returns></returns>
        public Pager<Base_CatalogArticle> GetPageInCatalog(PageModel pageModel, params Expression<Func<Base_CatalogArticle, bool>>[] modelFilters)
        {
            var list = GetAllInCatalog(pageModel.CatalogId);

            if (!modelFilters.IsEmpty())
            {
                modelFilters.Each(f => list = list.Where(f));
            }

            var exts = SiteManager.Catalog.GetAllExts(pageModel.CatalogId);
            Pager<Base_CatalogArticle> data = null;
            var catExt = exts.FirstOrDefault(ext => ext.Name.Equals(pageModel.SortField));
            //如果需要排序的是扩展属性
            if (catExt != null)
            {
                data = new Pager<Base_CatalogArticle>(list, lst => lst.OrderBy(ac => ac.Article.Exts.FirstOrDefault(ext => ext.Id == catExt.Id).Value),
                   pageModel.PageIndex, pageModel.PageSize);
            }
            else
            {
                //取分页数据
                data = new Pager<Base_CatalogArticle>(list, pageModel.SortExpression, pageModel.PageIndex, pageModel.PageSize);
            }
            return data;
        }

        /// <summary>
        /// 根据分页数据对象获取分页后的文章列表
        /// 并且包含文章中的长文本Text字段内容
        /// </summary>
        /// <param name="pageModel">分页数据对象</param>
        /// <param name="modelFilter">筛选器</param>
        /// <returns></returns>
        public Pager<Base_CatalogArticle> GetTextPageInCatalog(PageModel pageModel, params Expression<Func<Base_CatalogArticle, bool>>[] modelFilters)
        {
            var pager = GetPageInCatalog(pageModel, modelFilters);
            MatchTexts(pager.Select(p => p.Article));
            return pager;
        }

        /// <summary>
        /// 查找并填充文章和关联文章的Text属性
        /// </summary>
        /// <param name="articles"></param>
        public void MatchTexts(IEnumerable<Base_Article> articles)
        {
            //找出所有文章ID
            var arts = articles.Where(a => a.ArticleText == null);
            //找出所有关联文章的ID
            arts = arts.Union(articles.Select(p => p.Targets.Where(t => t.Target.ArticleText == null).Select(t => t.Target)).UnionAll());
            if (!arts.Any()) return;
            var texts = GetTexts(arts.Select(a => a.Id));
            arts.Each(art =>
            {
                art.ArticleText = texts.FirstOrDefault(atxt => atxt.Id == art.Id);
            });
        }

        /// <summary>
        /// 查找并填充文章和关联文章的Text属性,并返回原集合
        /// </summary>
        /// <param name="cas"></param>
        /// <returns></returns>
        public IEnumerable<Base_CatalogArticle> MatchTexts(IEnumerable<Base_CatalogArticle> cas)
        {
            //找出所有文章ID
            var arts = cas.Select(ca => ca.Article).Where(a => a.ArticleText == null);
            //找出所有关联文章的ID
            arts = arts.Union(arts.Select(p => p.Targets.Where(t => t.Target.ArticleText == null).Select(t => t.Target)).UnionAll());
            if (!arts.Any()) return cas;
            var texts = GetTexts(arts.Select(a => a.Id));
            arts.Each(art =>
            {
                art.ArticleText = texts.FirstOrDefault(atxt => atxt.Id == art.Id);
            });
            return cas;
        }

        public void MatchTexts(params Base_Article[] articles)
        {
            MatchTexts((IEnumerable<Base_Article>)articles);
        }

        private IEnumerable<Base_ArticleText> GetTexts(IEnumerable<int> ids)
        {
            var textProvider = SiteManager.Get<IAuditDataService<Base_ArticleText>>();
            var texts = textProvider.GetQuery().Where(t => ids.Contains(t.Id)).ToList();
            return texts;
        }

        /// <summary>
        /// 全站搜索
        /// </summary>
        /// <param name="key">关键词</param>
        /// <returns></returns>
        public Pager<Base_CatalogArticle> Search(string key, int page, int pageSize, params int[] rootIds)
        {
            var descCats = SiteManager.Catalog.GetDescendantByLang(rootIds)
                .Select(cat => cat.Id);

            return _provider.PageQuery(art => descCats.Contains(art.CatalogId)
                    && (art.Article.Title.Contains(key)
                    || art.Article.Abstract.Contains(key)
                    || art.Article.Keywords.Contains(key)
                    || art.Article.Title.Contains(key)
                    || art.Article.Exts.Any(ext => ext.Value.Contains(key))
                    // (ext=>ext.CatlogExtId == 123 && ext.Value.Contains(key))
                    ), page, pageSize);
        }

        public int GetCatExtId(int id, string value)
        {
            return SiteManager.Catalog.GetAllExts(id).First(ext => (ext.Name == value)).Id;
        }

        public Pager<Base_CatalogArticle> DownLoad(Expression<Func<Base_CatalogArticle, bool>> filter, int id, int page, int pageSize)
        {
            List<int> downLoadIds = GetById(id).Article.Targets.Where(t => (t.RelationType == ArticleRelationType.Download)).Select(t => t.TargetId).ToList();
            return _provider.PageQuery(filter.And(art => downLoadIds.Contains(art.Id)), page, pageSize);
        }

        public IEnumerable<Base_CatalogArticle> GetDataByCon(int catId, int topN)
        {
            var arts = GetAllQuery();
            var childrenIds = SiteManager.Catalog.GetDescendantByLang(catId)
               .Where(cat => SiteManager.Catalog.IsCurrentLangOrEmpty(cat))
               .Select(cat => cat.Id)
               .ToList();
            childrenIds.Add(catId);
            var article = (from a in arts
                           where childrenIds.Contains(a.CatalogId)
                           orderby a.Article.EditTime descending
                           select a).Take(topN);
            return article;
        }
        /// <summary>
        /// 根据栏目ID新建一篇文章，并建立和栏目对应的文章标签
        /// </summary>
        /// <param name="catId">栏目ID</param>
        /// <returns>新文章对象</returns>e
        public Base_CatalogArticle CreateByCatalog(int catId)
        {
            Base_Article art = new Base_Article()
            {
                CreateTime = DateTime.Now,
                EditTime = DateTime.Now,
            };

            Base_CatalogArticle ca = new Base_CatalogArticle
            {
                CatalogId = catId,
                Article = art
            };

            //保持文件的扩展属性和文件夹的保持一致
            foreach (Base_CatalogExt ext in SiteManager.Catalog.GetAllExts(catId))
            {
                art.Exts.Add(new Base_ArticleExt
                {
                    Article = art,
                    CatlogExtId = ext.Id,
                    Value = ext.DefaultValue
                });
            }

            return ca;
        }

        /// <summary>
        /// 保存文章，包括新增和修改
        /// </summary>
        /// <param name="ca"></param>
        /// <returns></returns>
        public Base_CatalogArticle Save(Base_CatalogArticle ca)
        {
            if (ca.Article.Id == 0)
            {
                ca.Article.CreateTime = DateTime.Now;
            }
            if (ca.Article.EditTime == default(DateTime))
            {
                ca.Article.EditTime = ca.Article.CreateTime;
            }
            if (ca.Id == 0)
            {
                ca.CreateTime = ca.Article.CreateTime;
                if (ca.ArticleId != 0)
                { //此处判断不太好，需要优化一下
                    var olda = ca.Article;
                    ca.Article = null;
                    _provider.Add(ca);
                    ca.Article = olda;
                }
                else
                {
                    _provider.Add(ca);
                }
            }
            else
            {
                _provider.Change(ca);
            }

            return ca;
        }

        /// <summary>
        /// 删除CatalogArticle中的记录
        /// </summary>
        /// <param name="id">CatalogArticle表的Id</param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var a = _provider.GetByKey(id); //GetById(id);
            if (a == null)
            {
                return 0;
            }

            return _provider.Delete(a);
        }

        /// <summary>
        /// 根据文章ID获取文章对象，包括标签集
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Base_CatalogArticle GetById(int id)
        {
            var art = _provider.GetByKey(id);

            #region 过滤文件的扩展属性（暂时未用到）

            //if (art == null || art.Article.IsDeleted())
            //{
            //    //throw new JException("文章未找到");
            //    return null;
            //}
            //int count = art.Article.Exts.Count();
            //if (count > 0)
            //{
            //    List<Base_CatalogExt> artExts = SiteManager.Catalog.GetAllExts(art.CatalogId);
            //    var exts = art.Article.Exts.ToArray();

            //    ArrayList extlist = new ArrayList();
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (!artExts.Select(artExt => artExt.Id).Contains(exts[i].CatlogExtId) || extlist.Contains(exts[i].CatlogExtId))
            //        {
            //            art.Article.Exts.Remove(exts[i]);
            //        }
            //        else
            //        {
            //            extlist.Add(exts[i].CatlogExtId);
            //        }
            //    }

            //    foreach (Base_CatalogExt ext in artExts)
            //    {
            //        if (!art.Article.Exts.Select(artExt => artExt.CatlogExtId).Contains(ext.Id))
            //        {
            //            art.Article.Exts.Add(new Base_ArticleExt
            //            {
            //                Article = art.Article,
            //                CatlogExtId = ext.Id,
            //                Value = ext.DefaultValue
            //            });
            //        }
            //    }
            //}

            #endregion

            return art;
        }

        /// <summary>
        /// 修改若干文章的状态位
        /// </summary>
        /// <param name="idArr">Base_Article表中的主键</param>
        /// <param name="state">要修改的状态</param>
        public void ChangeState(IEnumerable<int> idArr, int state)
        {
            var provider = SiteManager.Kernel.Get<EFAuditDataService<Base_Article>>();
            var arts = provider.GetQuery().Where(a => idArr.Contains(a.Id)).ToList();
            try
            {
                provider.BeginTrans();
                foreach (var art in arts)
                {
                    art.State = state;
                    provider.Change(art);
                }
            }
            finally
            {
                provider.EndTrans();
            }
        }

        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <returns></returns>
        public IQueryable<Base_CatalogArticle> GetAllQuery()
        {
            return _provider.GetQuery();
        }
        /// <summary>
        /// 获取所有文件（包括不完整的（Incomplete））
        /// </summary>
        /// <returns></returns>
        public IQueryable<Base_CatalogArticle> GetQuery()
        {
            return _provider.GetQuery();
        }

        /// <summary>
        /// 判断某文章的UrlTitle（对Url友好的标题）是否在本栏目内是重复的
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool IsUrlTitleExists(int catId, Base_Article article)
        {
            var urlTitles = article.Id == 0 ? GetAllInCatalog(catId).Where(art => !string.IsNullOrEmpty(art.Article.UrlTitle)).Select(art => art.Article.UrlTitle)
                   : GetAllInCatalog(catId).Where(art => (!string.IsNullOrEmpty(art.Article.UrlTitle) && (art.Id != article.Id))).Select(art => art.Article.UrlTitle);
            return urlTitles.Contains(article.UrlTitle);
        }

        public Base_CatalogArticle GetByUrlTitle(int rootId, string id)
        {
            var cats = SiteManager.Catalog.GetDescendantByLang(rootId).Select(cat => cat.Id);

            var articles = GetAllQuery().Where(art => cats.Contains(art.CatalogId));
            if (id != "")
            {
                var article = articles
              .First(art => art.Article.UrlTitle != null && art.Article.UrlTitle.Equals(id, StringComparison.OrdinalIgnoreCase));
                return GetById(article.Id);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取下一篇
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public Base_CatalogArticle GetNext(Base_CatalogArticle article)
        {
            var next = GetAllQuery()
                .Where(art => (art.Ord < article.Ord ? art.Ord < article.Ord : art.Ord == article.Ord && art.Article.EditTime < article.Article.EditTime) && (art.CatalogId == article.CatalogId))
                .OrderByDescending(art => art.Ord)
                .ThenByDescending(art => art.Article.EditTime)
                .FirstOrDefault();

            return next;
        }

        /// <summary>
        /// 获取上一篇
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public Base_CatalogArticle GetPrev(Base_CatalogArticle article)
        {
            var prev = GetAllQuery()
               .Where(art => (art.Ord > article.Ord ? art.Ord > article.Ord : art.Ord == article.Ord && art.Article.EditTime > article.Article.EditTime) && (art.CatalogId == article.CatalogId))
                .OrderBy(art => art.Ord)
                .ThenBy(art => art.Article.EditTime)
                .FirstOrDefault();
            return prev;
        }

        /// <summary>
        /// 获取文章的某个标签值，返回根据该文章的标签属性定义的数据类型
        /// </summary>
        /// <param name="article"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetExt(Base_CatalogArticle article, string name)
        {
            Base_CatalogExt catExt = null;
            if (article != null)
            {
                catExt = SiteManager.Catalog.GetAllExts(article.CatalogId)
               .FirstOrDefault(ext => ext.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            if (catExt == null) return null;
            var artExt = article.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == catExt.Id);
            if (artExt == null) return null;

            switch ((ExtDataType)catExt.DataType)
            {
                case ExtDataType.Currency:
                    return CommOp.ToDecimalNull(artExt.Value);
                case ExtDataType.DateAndTime:
                case ExtDataType.Date:
                    return CommOp.ToDateTimeNull(artExt.Value);
                case ExtDataType.SingleNumber:
                    return CommOp.ToLongNull(artExt.Value);
                case ExtDataType.Bool:
                    return CommOp.ToBool(artExt.Value);
                default:
                    return artExt.Value;
            }
        }

        /// <summary>
        /// 设置文章的某个标签值
        /// </summary>
        /// <param name="article"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetExt(Base_CatalogArticle article, string name, object value)
        {
            var catExt = SiteManager.Catalog.GetAllExts(article.CatalogId)
                .FirstOrDefault(ext => ext.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (catExt == null) return;
            var artExt = article.Article.Exts.First(ext => ext.CatlogExtId == catExt.Id);
            if (artExt == null) return;

            switch ((ExtDataType)catExt.DataType)
            {
                case ExtDataType.Currency:
                    value = CommOp.ToDecimalNull(value);
                    break;
                case ExtDataType.DateAndTime:
                case ExtDataType.Date:
                    value = CommOp.ToDateTimeNull(artExt.Value);
                    break;
                case ExtDataType.SingleNumber:
                    value = CommOp.ToLongNull(value);
                    break;
                case ExtDataType.Bool:
                    value = CommOp.ToBool(value);
                    break;
            }
            artExt.Value = value.ToString();
        }

        Base_Article Clone(Base_Article article)
        {
            Base_Article newArt = new Base_Article
            {
                Abstract = article.Abstract,
                Author = article.Author,
                Exts = article.Exts.Select(ext => Clone(ext)).ToList(),
                Clicks = article.Clicks,
                CreateTime = article.CreateTime,
                EditorId = article.EditorId,
                EditTime = article.EditTime,
                Id = article.Id,
                Keywords = article.Keywords,
                State = article.State,
                Targets = article.Targets.Select(tar => Clone(tar)).ToList(),
                Title = article.Title,
                UrlTitle = article.UrlTitle
            };
            if (article.ArticleText != null)
            {
                newArt.ArticleText = new Base_ArticleText
                {
                    Id = article.ArticleText.Id,
                    Text = article.ArticleText.Text,
                };
            }

            return newArt;
        }

        private Base_ArticleExt Clone(Base_ArticleExt ext)
        {
            return new Base_ArticleExt
            {
                ArticleId = ext.ArticleId,
                CatlogExtId = ext.CatlogExtId,
                Value = ext.Value,
                Id = ext.Id,
            };
        }

        private Base_ArticleRelation Clone(Base_ArticleRelation rel)
        {
            return new Base_ArticleRelation
            {
                RelationType = rel.RelationType,
                TargetId = rel.TargetId,
                Target = rel.Target,
                Id = rel.Id,
                Ord = rel.Ord
            };
        }

        public Base_CatalogArticle Clone(Base_CatalogArticle ca)
        {
            return new Base_CatalogArticle
            {
                Article = Clone(ca.Article),
                Id = ca.Id,
                CatalogId = ca.CatalogId,
                ArticleId = ca.ArticleId,
                CreateTime = ca.CreateTime,
                Ord = ca.Ord
            };
        }

        /// <summary>
        /// 通过一串在Base_Article表中的ID找到它对应的栏目-文章对象 wang
        /// </summary>
        /// <param name="articleIds">一串在Base_Article表中的ID</param>
        /// <param name="catalogIds">一串在Base_Catlaog表中的ID</param>
        /// <returns>系统栏目-文章对象集合</returns>
        public IEnumerable<Base_CatalogArticle> GetByArticleIds(IEnumerable<int> articleIds, IEnumerable<int> catalogIds)
        {
            if (articleIds.IsEmpty())
            {
                return null;
            }
            return GetQuery().Where(ca => articleIds.Contains(ca.ArticleId) && catalogIds.Contains(ca.CatalogId));
        }

        /// <summary>
        /// 通过在Base_Article表中的ID找到它对应的栏目-文章对象 wang
        /// </summary>
        /// <param name="articleId">在Base_Article表中的ID</param>
        /// <param name="catalogIds">一串在Base_Catlaog表中的ID</param>
        /// <returns>系统栏目-文章对象</returns>
        public Base_CatalogArticle GetByArticleId(int articleId, params int[] catalogIds)
        {
            if (articleId <= 0)
            {
                return null;
            }
            return GetQuery().FirstOrDefault(ca => articleId == ca.ArticleId && catalogIds.Contains(ca.CatalogId));
        }

        /// <summary>
        /// 通过一串在Base_CatalogArticle表中的ID找到它对应的Base_Article表中的ID
        /// </summary>
        /// <param name="caIds"></param>
        /// <returns></returns>
        public IEnumerable<int> GetArticleIds(IEnumerable<int> caIds)
        {
            if (caIds.IsEmpty())
            {
                return null;
            }
            return GetQuery().Where(ca => caIds.Contains(ca.Id))
                .Select(ca => ca.ArticleId)
                .Distinct();
        }


        public Base_CatalogArticle Copy(Base_CatalogArticle ca)
        {
            var newCa = Clone(ca);
            newCa.Id = 0;
            newCa.ArticleId = 0;
            newCa.Article.Id = 0;
            foreach (var ext in newCa.Article.Exts)
            {
                ext.Id = 0;
                ext.ArticleId = 0;
            }
            foreach (var rel in newCa.Article.Targets)
            {
                rel.Id = 0;
                rel.TargetId = 0;
                rel.SourceId = 0;
            }
            return newCa;
        }

        public void Dispose()
        {
            if (_provider != null)
            {
                _provider.Dispose();
                _provider = null;
            }
        }
    }
}
