using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Jurassic.Com.Tools;
using Jurassic.WebQuery.Models;
using Jurassic.WebFrame;
using Jurassic.CommonModels.ModelBase;
using System.Drawing;
using System.Reflection;
using Jurassic.AppCenter.Resources;
using Jurassic.CommonModels;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 高级查询的MVC筛选器，一般用于获取列表数据的GetData方法上
    /// GetData方法只需返回未经筛选和分页的原始数据集
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=true)]
    public class AdvQueryAttribute : ActionFilterAttribute
    {
        Type _queryType { get; set; }

        IResultFilter _innerFilter;

        /// <summary>
        /// ctor
        /// </summary>
        public AdvQueryAttribute()
        {
        }

        private IResultFilter CreaterInnerFilter(Type _queryType)
        {
            string typeName = _queryType.AssemblyQualifiedName;
            //RefHelper.LoadType(
            //    "Jurassic.CommonModels.EFProvider.OrgEFAuditDataService`1[[Jurassic.CommonModels.Organization.Dep_Department, Jurassic.CommonModels]],Jurassic.CommonModels.EFProvider"));
            return RefHelper.LoadClass("Jurassic.WebQuery.AdvQueryFilterWrapper`1[[" + typeName + "]], Jurassic.WebQuery")
                as IResultFilter;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            var jsonResult = filterContext.Result as JsonResult;
            if (jsonResult == null)
            {
                return;
            }
            _queryType = jsonResult.Data.GetType().GetGenericArguments().LastOrDefault();
            if (_queryType == null)
            {
                return;
            }

            _innerFilter = CreaterInnerFilter(_queryType);
            _innerFilter.OnResultExecuting(filterContext);
        }
    }

    /// <summary>
    /// 高级查询的MVC筛选器的内部包装类
    /// 因为MVC筛选器用于打标记时，不支持泛型
    /// </summary>
    public class AdvQueryFilterWrapper<T> : IResultFilter where T : class
    {
        protected PageModel _pageModel;
        ResultExecutingContext _filterContext;
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            _filterContext = filterContext;
            var jsonResult = filterContext.Result as JsonResult;

            _pageModel = new PageModel
            {
                AdvQuery = filterContext.HttpContext.Request["AdvQuery"],
                Key = CommOp.ToStr(filterContext.HttpContext.Request["Key"]),
                SortOrder = filterContext.HttpContext.Request["SortOrder"],
                SortField = filterContext.HttpContext.Request["SortField"],
                PageSize = CommOp.ToInt(filterContext.HttpContext.Request["PageSize"]),
                PageIndex = CommOp.ToInt(filterContext.HttpContext.Request["PageIndex"]),
            };
            WrapData(jsonResult);
        }

        /// 非高级查询，简单输入一个关键字时的查询条件
        /// </summary>
        Expression<Func<T, bool>> _keyFilter;

        /// <summary>
        /// 根据查询条件返回数据
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private void WrapData(JsonResult result)
        {
            var baseQuery = result.Data as IQueryable<T>;
            var queryManager = SiteManager.Get<AdvQueryManager>();
            if (String.IsNullOrWhiteSpace(_pageModel.SortExpression))
            {
                _pageModel.SortField = "Id";
                _pageModel.SortOrder = "desc";
            }
            if (!_pageModel.Key.IsEmpty())
            {
                IKeyFilter<T> keyController = _filterContext.Controller as IKeyFilter<T>;
                Func<string, Expression<Func<T, bool>>> func = BuildDefaultKeyFilter;
                if (keyController != null)
                {
                    func = keyController.GetKeyFilter;
                }

                string[] keys = _pageModel.Key.Split(' ');
                _keyFilter = null;
                foreach (string key in keys)
                {
                    if (!String.IsNullOrWhiteSpace(key))
                    {
                        _keyFilter = _keyFilter.And(func(key));
                    }
                }

                baseQuery = baseQuery.Where(_keyFilter);
            }
            else if (!_pageModel.AdvQuery.IsEmpty())
            {
                int queryType = CommOp.ToInt(_filterContext.HttpContext.Request["QueryType"]);
                //调用服务层业务逻辑获取分页数据
                baseQuery = queryManager.Query(baseQuery, _pageModel.AdvQuery, queryType);
            }

            if (_pageModel.PageSize == 0) //不分页的情况
            {
                result.Data = baseQuery;
            }
            else
            {
                Pager<T> pageData = null;
                _pageModel.PageIndex++;//MiniUI的分页数字是从0开始，实际的分页是从1开始的，这里+1同步。
                pageData = new Pager<T>(baseQuery.OrderBy(_pageModel.SortExpression), _pageModel.PageIndex, _pageModel.PageSize);

                //让用户有机会在数据返回前作一些处理
                var pagedController = _filterContext.Controller as IPagedDataController<T>;
                if (pagedController !=null)
                {
                    pagedController.BeforeShowingPage(pageData);
                }

                //根据mini-UI的mini-grid控件的约定返回数据
                result.Data = new
                {
                    data = pageData,
                    total = pageData.RecordCount
                };

            }
            queryManager.Dispose();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        private Expression<Func<T, bool>> BuildDefaultKeyFilter(string key)
        {
            Expression<Func<T, bool>> expr = null;

            ModelRule rule = ModelRule.Get<T>();

            foreach (var attr in rule.SingleRules)
            {
                if (!attr.Browsable)
                {
                    continue;
                }
                if ( attr.Property.PropertyType == typeof(string))
                {
                    string exprStr = String.Format("{0}.Contains(@0)", attr.Name);
                    expr = expr.Or(Jurassic.Com.Tools.DynamicExpression.ParseLambda<T, bool>(exprStr, key));
                }
                else if (attr.Property.PropertyType.IsEnum && !attr.Property.PropertyType.IsNullableType())
                {
                    var enumValue = GetEnumValue(attr.Property.PropertyType, key);
                    if (enumValue == null) continue;
                    string exprStr = String.Format("{0} = @0", attr.Name);
                    expr = expr.Or(Jurassic.Com.Tools.DynamicExpression.ParseLambda<T, bool>(exprStr, enumValue));
                }
                else if (attr.Property.PropertyType.IsNullableType())
                {
                    string exprStr = String.Format("{0} !=null &&{0}.Value.ToString().Contains(@0)", attr.Name);
                    expr = expr.Or(Jurassic.Com.Tools.DynamicExpression.ParseLambda<T, bool>(exprStr, key));
                }
                //else if (attr.Property.PropertyType == typeof(DateTime))
                //{
                //    DateTime dt = CommOp.ToDateTime(key);
                //    if (dt == default(DateTime)) continue;
                //    string exprStr = String.Format("{0} == @0", attr.Name);
                //    expr = expr.Or(Jurassic.Com.Tools.DynamicExpression.ParseLambda<T, bool>(exprStr, dt));
                //}
                else
                {
                    string exprStr = String.Format("{0}.ToString().Contains(@0)", attr.Name);
                    expr = expr.Or(Jurassic.Com.Tools.DynamicExpression.ParseLambda<T, bool>(exprStr, key));
                }
            }

            return expr;
        }

        //private Expression<Func<T, bool>> GetEnumExpression<T>(PropertyInfo prop, string key)
        //{
        //    object enumValue = GetEnumValue(prop.PropertyType, key);
        //    if (enumValue == null)
        //    {
        //        return null;
        //    }
        //    var keyValue = Expression.Constant(enumValue);
        //    var property = Expression.Property(Expression.Parameter(typeof(T)), prop.Name);
        //    var value = Expression.Convert(keyValue, Enum.GetUnderlyingType(prop.PropertyType));
        //    var propertyValue = Expression.Convert(property, Enum.GetUnderlyingType(prop.PropertyType));
        //}

        private object GetEnumValue(Type enumType, string key)
        {
            foreach (var val in Enum.GetValues(enumType))
            {

                if (enumType.GetEnumResKey(val.ToString()).Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return val;
                }
            }
            return null;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}