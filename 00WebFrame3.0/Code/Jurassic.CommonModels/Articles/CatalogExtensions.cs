using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 关于Catalog的扩展
    /// </summary>
    public static class CatalogExtensions
    {
        /// <summary>
        /// 根据栏目获取它的某个指定名称的标签
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="extName"></param>
        /// <returns></returns>
        public static Base_CatalogExt GetExtByName(this Base_Catalog cat, string extName)
        {
            return SiteManager.Catalog.GetAllExts(cat.Id)
                .FirstOrDefault(ext => ext.Name.Equals(extName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取上级Static的栏目
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static Base_Catalog GetStaticParent(this Base_Catalog cat)
        {
            while (cat.ParentId != null)
            {
                var parent = SiteManager.Catalog.GetById(cat.ParentId.Value);
                if (parent == null) break;
                cat = parent;
                if (cat.State == ArticleState.Static || cat.ParentId == 0)
                {
                    break;
                }
            }
            return cat;
        }

        /// <summary>
        /// 根据栏目获取子结点
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static IEnumerable<Base_Catalog> GetChildrenByLang(this Base_Catalog cat)
        {
            return SiteManager.Catalog.GetChildrenByLang(cat.Id);
        }

        /// <summary>
        /// 根据栏目获取所有下级子结点
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static IEnumerable<Base_Catalog> GetDescendantByLang(this Base_Catalog cat)
        {
            return SiteManager.Catalog.GetDescendantByLang(cat.Id);
        }

        //public static bool IsPictureCatalog(this Base_Catalog cat)
        //{
        //    var root = cat.GetStaticParent();
        //    return root != null && root.Id == CatalogManager.CAT_PICTURES;
        //}

        /// <summary>
        /// 将CatalogExtAttribute转换为Base_CatalogExt
        /// </summary>
        /// <param name="extAttr"></param>
        /// <returns></returns>
        //public static Base_CatalogExt ConvertToCatalogExt(this CatalogExtAttribute extAttr)
        //{
        //    Base_CatalogExt catExt = new Base_CatalogExt
        //    {
        //        AllowNull = extAttr.AllowNull,
        //        DataSource = extAttr.DataSource,
        //        DataSourceType = extAttr.DataSourceType,
        //        DataType = extAttr.DataType,
        //        DefaultValue = extAttr.DefaultValue,
        //        MaxLength = extAttr.MaxLength,
        //        MaxValue = extAttr.MaxValue,
        //        MinValue = extAttr.MinValue,
        //        MinLength = extAttr.MinLength,
        //        Name = extAttr.Name,
        //        Ord = extAttr.Ord,
        //        RegExpr = extAttr.RegExpr,
        //    };
        //    return catExt;
        //}

    }
}
