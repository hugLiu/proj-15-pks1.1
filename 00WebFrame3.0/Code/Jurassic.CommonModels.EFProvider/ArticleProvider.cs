using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ninject;

namespace Jurassic.CommonModels.EFProvider
{
    /// <summary>
    /// 内容存储的数据持久化的EF实现
    /// </summary>
    public class ArticleProvider : EFAuditDataService<Base_CatalogArticle>, IDisposable
    {
        new ModelContext _context;
        public ArticleProvider(ModelContext context)
            : base(context)
        {
            _context = context;
            _context.Configuration.AutoDetectChangesEnabled = false;
            _context.Configuration.ValidateOnSaveEnabled = false;
        }

        public override IQueryable<Base_CatalogArticle> GetQuery()
        {
            return _context.CatalogArticles.Include(a => a.Article)
                    .Include(a => a.Article.Exts)
                    .Include(a => a.Article.Targets)
                    .Include(a => a.Article.Targets.Select(t => t.Target))
                    .Include(a => a.Article.Targets.Select(t => t.Target.Exts))
                .Where(art => art.Article.IsDeleted == false);
            
            //&& (art.Article.State & ArticleState.Published) == ArticleState.Published
        }

        public override int Add(Base_CatalogArticle ca)
        {
            //if (t.Article != null && t.Article.ArticleText != null)
            //{
            //    _context.Entry<Base_ArticleText>(t.Article.ArticleText).State = EntityState.Added;
            //}
            if (ca.Article != null)
            {
                foreach (var rel in ca.Article.Targets)
                {
                    if (rel.Target == null)
                    {
                        continue;
                    }
                    if (rel.Target.Id > 0)
                    {
                        if (!ca.Article.TargetArticles.IsEmpty())
                        {
                            var art = ca.Article.TargetArticles.FirstOrDefault(ta => ta.Id == rel.Target.Id);
                            if (art == null)
                            {
                                throw new JException("在TargetArticles集合中出现的对象也必须在Targets集合中");
                            }
                            rel.Target = art;
                            rel.TargetId = art.Id;
                            _context.Entry(rel.Target).State = EntityState.Modified;
                        }
                        else
                        {
                            rel.TargetId = rel.Target.Id;
                            _context.Entry(rel.Target).State = EntityState.Unchanged;
                        }
                    }
                }
            }
            _context.Entry<Base_CatalogArticle>(ca).State = EntityState.Added;
            var r = _context.SaveChanges();
            return r;
        }

        private Base_CatalogArticle Detach(Base_CatalogArticle catArt)
        {
            var oldArt = catArt.Article;
            var oldExts = oldArt.Exts.ToList();
            var oldTargets = oldArt.Targets.ToList();
            var oldArticleText = oldArt.ArticleText;
            if (oldArticleText != null)
            {
                _context.Entry(oldArticleText).State = EntityState.Detached;
            }
            oldExts.Each(oldExt => _context.Entry(oldExt).State = EntityState.Detached);
            oldTargets.Each(oldTarget =>
            {
                var oldtt = oldTarget.Target;
                if (oldtt != null)
                {
                    var exts = oldtt.Exts.ToList();
                    foreach (var oldext in exts)
                    {
                        _context.Entry(oldext).State = EntityState.Detached;
                    }
                    _context.Entry(oldtt).State = EntityState.Detached;
                    oldtt.Exts = exts;
                }
                _context.Entry(oldTarget).State = EntityState.Detached;
                oldTarget.Target = oldtt;
            });
            _context.Entry(oldArt).State = EntityState.Detached;
            _context.Entry(catArt).State = EntityState.Detached;

            oldArt.Exts = oldExts;
            oldArt.Targets = oldTargets;
            oldArt.ArticleText = oldArticleText;
            catArt.Article = oldArt;
            return catArt;
        }

        public override int Change(Base_CatalogArticle ca)
        {
            Base_CatalogArticle catArt = GetByKey(ca.Id);
            var oldArt = catArt.Article;
            var newArt = ca.Article;
            //foreach (var ext in oldArt.Exts.ToArray())
            //{
            //    //将所有未在新对象t.Exts中出现的属性设置为删除状态
            //    if (!t.Article.Exts.Any(newExt => newExt.Id == ext.Id))
            //    {
            //        _context.Entry<Base_ArticleExt>(ext).State = EntityState.Deleted;
            //    }
            //}

            foreach (var ext in newArt.Exts)
            {
                //将所有在老对象中出现的新对象设置为修改状态，
                if (oldArt.Exts.Any(oldExt => oldExt.Id == ext.Id && oldExt.Value != ext.Value))
                {
                    _context.Entry(ext).State = EntityState.Modified;
                }
                else if (oldArt.Exts.Any(oldExt => oldExt.Id == ext.Id && oldExt.Value == ext.Value))
                {
                    _context.Entry(ext).State = EntityState.Unchanged;
                }
                else  //否则是新增
                {
                    ext.ArticleId = newArt.Id;
                    _context.Entry(ext).State = EntityState.Added;
                }
            }

            //以下保存Text数据。Text由于是大字段，所以分表在Base_ArticleText存储
            if (oldArt.Text != newArt.Text)
            {
                if (newArt.ArticleText == null)
                {
                    newArt.ArticleText = new Base_ArticleText();
                }
                newArt.ArticleText.Id = newArt.Id;
                //假如Base_ArticleText表中无数据
                if (oldArt.ArticleText == null)
                {
                    _context.Entry(newArt.ArticleText).State = EntityState.Added;
                }
                else
                {
                    _context.Entry(newArt.ArticleText).State = EntityState.Modified;
                }
            }
            else if (newArt.ArticleText != null)
            {
                newArt.ArticleText.Id = newArt.Id;
                _context.Entry(newArt.ArticleText).State = EntityState.Unchanged;
            }

            foreach (var oldRel in oldArt.Targets)
            {
                //将所有未在新对象t.Targets中出现的ID设置为删除状态
                if (!newArt.Targets.Any(newRel => newRel.Id == oldRel.Id))
                {
                    _context.Entry(oldRel).State = EntityState.Deleted;
                }
            }

            foreach (var rel in newArt.Targets)
            {
                rel.SourceId = newArt.Id;
                //所有在老对象oldExts中出现的新对象
                if (oldArt.Targets.Any(oldExt => oldExt.Id == rel.Id))
                {
                    //已有的目标文章如果在TargetArticles集合里出现，则需要更新
                    if (rel.Target != null && rel.Target.Id > 0 &&
                        !newArt.TargetArticles.IsEmpty())
                    {
                        var art = newArt.TargetArticles.FirstOrDefault(ta => ta.Id == rel.Target.Id);
                        if (art == null)
                        {
                            // throw new JException("在TargetArticles集合中出现的对象也必须在Targets集合中");
                            continue;
                        }
                        rel.Target = art;
  
                        //光下面一句还不能保证art.Exts被保存，因为层次太深
                         _context.Entry(art).State = EntityState.Modified;

                         //所以还要依次遍历
                        foreach (var ext in art.Exts)
                        {
                            if (ext.Id == 0)
                            {
                                _context.Entry(ext).State = EntityState.Added;
                            }
                            else
                            {
                                _context.Entry(ext).State = EntityState.Modified;
                            }
                        }
                    }
                    else
                    {
                        var tar = rel.Target;
                        rel.Target = null;
                        _context.Entry(rel).State = EntityState.Unchanged;
                        rel.Target = tar;
                    }
                }
                else  //否则是新增
                {
                    _context.Entry(rel).State = EntityState.Added;
                }
            }

            newArt.CreateTime = oldArt.CreateTime == default(DateTime) ? DateTime.Now : oldArt.CreateTime;
            if (newArt.EditTime == default(DateTime))
            {
                newArt.EditTime = DateTime.Now;
            }
            _context.Entry(newArt).State = EntityState.Modified;
            ca.ArticleId = newArt.Id;
            _context.Entry(ca).State = EntityState.Modified;
            var r = _context.SaveChanges();
            Detach(ca); //解除与原_context的关联，免得麻烦
            return r;
        }

        /// <summary>
        /// 删除文件，当文件被多次引用的时候只删除CatalogArticle中的一条记录，
        /// 只被引用一次的时候就是删除记录并将Article表中的记录标记为Delete
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override int Delete(Base_CatalogArticle t)
        {
            //CatalogArticle中包括的此ArticleId的数据条数
            var count = GetQuery().Count(art => art.ArticleId == t.ArticleId);
            Base_Article article = t.Article;

            //删除主表中的记录
            _context.Entry(t).State = EntityState.Deleted;

            if (count == 1)
            {
                //将从表的标示设为删除（因为从表中的数据还有其他表与之对应，故软删除）
                article.IsDeleted = true;
                _context.Entry(article).State = EntityState.Modified;

            }
            _context.SaveChanges();
            return count;
        }

        public override Base_CatalogArticle GetByKey(object oid)
        {
            int id = CommOp.ToInt(oid);
            Base_CatalogArticle catArt = GetQuery()
                .Include(ca=>ca.Article.ArticleText)
                .FirstOrDefault(c => c.Id == id);
            if (catArt != null)
            {
                Detach(catArt);
            }
            return catArt;
        }

        void IDisposable.Dispose()
        {
            _context.Dispose();
        }
    }
}
