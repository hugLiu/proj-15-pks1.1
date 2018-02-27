using Jurassic.Com.Tools;
//using Jurassic.CommonModels.EntityExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 关于文章属性的扩展
    /// </summary>
    public static class ArticleExtensions
    {
        /// <summary>
        /// 根据文章的标签名称, 获取文章的某个标签值，返回根据该文章的标签属性定义的数据类型
        /// </summary>
        /// <param name="ca">栏目文章对象</param>
        /// <param name="name">标签名称</param>
        /// <returns>标签值</returns>
        public static object GetExt(this Base_CatalogArticle ca, string name)
        {
            Base_CatalogExt catExt = null;
            if (ca != null)
            {
                catExt = SiteManager.Catalog.GetAllExts(ca.CatalogId)
               .FirstOrDefault(ext => ext.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return GetExt(ca.Article, catExt);
            }
            return null;
        }

        /// <summary>
        /// 获取日期时间扩展属性
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DateTime GetExtDateTime(this Base_CatalogArticle ca, string name)
        {
            return CommOp.ToDateTime(ca.GetExt(name));
        }

        /// <summary>
        /// 获取整数的扩展属性
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetExtInt(this Base_CatalogArticle ca, string name)
        {
            return CommOp.ToInt(ca.GetExt(name));
        }

        /// <summary>
        /// 获取字符串扩展属性
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetExtStr(this Base_CatalogArticle ca, string name)
        {
            return CommOp.ToStr(ca.GetExt(name));
        }

        /// <summary>
        /// 获取文章扩展展性的显示值（来自键值对的下拉列表中的显示内容）
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetExtDisplay(this Base_CatalogArticle ca, Base_CatalogExt catExt)
        {
            if (catExt.DataSource.IsEmpty())
            {
                return GetExt(ca, catExt);
            }
            else
            {
                var obj = GetExt(ca, catExt);
                string str = CommOp.ToStr(obj);
                StringSpliter ss = new StringSpliter(catExt.DataSource, ";", "=");

                foreach (var key in ss.Keys)
                {
                    if (ss[key] == str)
                    {
                       return key;
                    }
                }
            }

            return GetExt(ca, catExt);
        }

        public static object GetExt(this Base_Article article, int catExtId)
        {
            var catExt = SiteManager.Catalog.GetExtById(catExtId);
            return GetExt(article, catExt);
        }

        public static object GetExt(this Base_CatalogArticle ca, Base_CatalogExt catExt)
        {
            return GetExt(ca.Article, catExt);
        }

        public static object GetExt(this Base_Article article, Base_CatalogExt catExt)
        {
            if (catExt == null) return null;
            var artExt = article.Exts.FirstOrDefault(ext => ext.CatlogExtId == catExt.Id);
            if (artExt == null) return null;

            switch ((ExtDataType)catExt.DataType)
            {
                case ExtDataType.Currency:
                    return CommOp.ToDecimalNull(artExt.Value);
                case ExtDataType.DateAndTime:
                case ExtDataType.Date:
                    return CommOp.ToDateTimeNull(artExt.Value);
                case ExtDataType.SingleNumber:
                    return CommOp.ToIntNull(artExt.Value);
                case ExtDataType.Bool:
                    return CommOp.ToBool(artExt.Value);
                default:
                    return artExt.Value;
            }
        }

        /// <summary>
        /// 根据文章所在栏目的扩展属性ID，获取文章的某个标签值，返回根据该文章的标签属性定义的数据类型
        /// </summary>
        /// <param name="ca">栏目文章对象</param>
        /// <param name="catExtId">栏目的扩展属性ID</param>
        /// <returns>标签值</returns>
        public static object GetExt(this Base_CatalogArticle ca, int catExtId)
        {
            Base_CatalogExt catExt = null;
            if (ca != null)
            {
                catExt = SiteManager.Catalog.GetAllExts(ca.CatalogId)
               .FirstOrDefault(ext => ext.Id == catExtId);
            }

            return GetExt(ca.Article, catExt);
        }

        /// <summary>
        /// 设置文章的某个标签值
        /// </summary>
        /// <param name="ca"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object SetExt(this Base_CatalogArticle ca, string name, object value)
        {
            var catExt = SiteManager.Catalog.GetAllExts(ca.CatalogId)
                .FirstOrDefault(ext => ext.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return SetExt(ca.Article, catExt, value);
        }

        public static object SetExt(this Base_Article article, int catExtId, object value)
        {
            var catExt = SiteManager.Catalog.GetExtById(catExtId);
            return SetExt(article, catExt, value);
        }

        public static object SetExt(this Base_CatalogArticle ca, Base_CatalogExt catExt, object value)
        {
            return SetExt(ca.Article, catExt, value);
        }

        public static object SetExt(this Base_Article article, Base_CatalogExt catExt, object value)
        {
            if (catExt == null) return null;
            var artExt = article.Exts.FirstOrDefault(ext => ext.CatlogExtId == catExt.Id);
            if (artExt == null)
            {
                artExt = new Base_ArticleExt
                {
                    CatlogExtId = catExt.Id,
                };
                article.Exts.Add(artExt);
            };

            string strValue = null;
            switch ((ExtDataType)catExt.DataType)
            {
                case ExtDataType.Currency:
                    Decimal? dec = CommOp.ToDecimalNull(value);
                    if (dec != null)
                    {
                        strValue = dec.Value.ToString();
                    }
                    break;
                case ExtDataType.FloatNumber:
                    double? flt = CommOp.ToDoubleNull(value);
                    if (flt != null)
                    {
                        strValue = flt.Value.ToString();
                    }
                    break;
                case ExtDataType.DateAndTime:
                case ExtDataType.Date:
                    DateTime? dt = CommOp.ToDateTimeNull(value);
                    if (dt != null)
                    {
                        strValue = dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    break;
                case ExtDataType.SingleNumber:
                    long? lng = CommOp.ToLongNull(value);
                    if (lng != null)
                    {
                        strValue = lng.Value.ToString();
                    }
                    break;
                case ExtDataType.Bool:
                    value = CommOp.ToBool(value);
                    strValue = value.ToString();
                    break;
                default:
                    strValue = CommOp.ToStr(value);
                    if (strValue.IsEmpty())
                    {
                        strValue = null;
                    }
                    break;
            }
            artExt.Value = strValue;
            return value;
        }

        /// <summary>
        /// 检查文章的扩展属性，凡是栏目中没有的属性就去掉，凡是栏目中有的属性则加上。
        /// </summary>
        /// <param name="ca">栏目文章对象</param>
        public static void CheckExts(this Base_CatalogArticle ca)
        {
            var catExtIds = SiteManager.Catalog.GetAllExts(ca.CatalogId).Select(ext => ext.Id);
            var artExtIds = ca.Article.Exts.Select(ext => ext.CatlogExtId);
            var newIds = catExtIds.Except(artExtIds); //文章中没有的属性ID
            var oldIds = artExtIds.Except(catExtIds); //栏目中没有的属性ID
            foreach (var newId in newIds)
            {
                var ext = SiteManager.Catalog.GetExtById(newId);
                //文章没有这个属性则新增
                ca.SetExt(ext.Name, ext.DefaultValue);
            }
            foreach (var oldId in oldIds)
            {
                var ext = ca.Article.Exts.First(a => a.CatlogExtId == oldId);
                //栏目没有这个属性则删除
                ca.Article.Exts.Remove(ext);
            }

        }


        ///// <summary>
        ///// 取得MD5值
        ///// </summary>
        ///// <param name="article"></param>
        ///// <returns></returns>
        //public static string GetMD5Code(this Base_Article article)
        //{
        //    if (article == null)
        //    {
        //        return null;
        //    }

        //    return (string)article.GetExt(ResourceManager.Category_EXT_MD5_NAME);
        //}
        ///// <summary>
        ///// 取得Key值
        ///// </summary>
        ///// <param name="article"></param>
        ///// <returns></returns>
        //public static string GetKeyCode(this Base_Article article)
        //{
        //    if (article == null)
        //    {
        //        return null;
        //    }

        //    return (string)article.GetExt(ResourceManager.Category_EXT_KEY_NAME);
        //}

    }
}
