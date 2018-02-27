using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

namespace Jurassic.CommonModels.EFProvider
{
    public class CatalogProvider : IDataProvider<Base_Catalog>
    {
        ModelContext mc;
        public CatalogProvider(ModelContext context)
        {
            mc = context;
        }

        public IEnumerable<Base_Catalog> GetData()
        {
            //关闭ModelContext的延迟加载后，需要Include集合
            var list = mc.Catalogs.Include("Exts").ToList();
            //foreach (var cat in list)
            //{
            //    cat.Exts = cat.Exts.Where(ext => (ext.State & ArticleState.Deleted) != ArticleState.Deleted).ToList();
            //}
            return list;
        }

        public int Add(Base_Catalog t)
        {
            mc.Entry<Base_Catalog>(t).State = EntityState.Added;
            return mc.SaveChanges();
        }

        private void Detach(Base_Catalog t)
        {
            //显式加载Exts集合
            mc.Entry(t).Collection(cat => cat.Exts).Load();
            var exts = t.Exts.ToList();
            exts.Each(ext => mc.Entry(ext).State = EntityState.Detached);
            mc.Entry(t).State = EntityState.Detached;
            t.Exts = exts;
        }

        public int Change(Base_Catalog t)
        {
            Base_Catalog oldCat = SiteManager.Catalog.GetById(t.Id);
            Detach(oldCat);
            foreach (var ext in t.Exts.ToArray())
            {
                //将所有在老对象oldExts中出现的新对象设置为修改状态，
                if (ext.Id > 0)
                {
                    mc.Entry<Base_CatalogExt>(ext).State = EntityState.Modified;
                }
                else  //否则是新增
                {
                    mc.Entry<Base_CatalogExt>(ext).State = EntityState.Added;
                }
            }

            mc.Entry<Base_Catalog>(t).State = EntityState.Modified;
            return mc.SaveChanges();
        }

        /// <summary>
        /// 删除目录
        /// 修改：[2017-01-16/汪敏]
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Delete(Base_Catalog t)
        {
            //逻辑删除
            //t.State |= ArticleState.Deleted;
            //mc.Entry<Base_Catalog>(t).State = EntityState.Modified;
            //物理删除
            mc.Entry<Base_Catalog>(t).State = EntityState.Deleted;

            return mc.SaveChanges();
        }
    }
}
