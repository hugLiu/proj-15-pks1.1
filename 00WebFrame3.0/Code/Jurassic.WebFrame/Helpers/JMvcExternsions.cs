using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Jurassic.AppCenter.Resources;
using System.Web.Routing;
using Jurassic.Com.Tools;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Specialized;
using Jurassic.CommonModels.Articles;
using Jurassic.Com.DB;
using System.Data;
using Jurassic.CommonModels;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Jurassic.CommonModels.ModelBase;
using Jurassic.WebFrame.Models;
using Jurassic.AppCenter.Models;
using Jurassic.WebFrame.Controllers;

namespace Jurassic.WebFrame
{
    public static class JMvcExtensions
    {
        /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
        /// <summary>
        /// 根据用户所拥有的权限，决定链接显示还是不显示
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="linkText">链接文本</param>
        /// <param name="actionName">Action名称</param>
        /// <param name="controllerName">Controller名称</param>
        /// <param name="routeValues">路由对象包</param>
        /// <param name="htmlAttributes">Html属性包</param>
        /// <returns>HtmlString,没有权限返回空串</returns>
        public static MvcHtmlString JAuthLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null)
        {
            MvcHtmlString link = MvcHtmlString.Empty;
            string linkUrl = ((WebViewPage)htmlHelper.ViewDataContainer).Url.Action(actionName, controllerName, routeValues);
            if (htmlHelper.ViewContext.HttpContext.User.Identity.HasRight(linkUrl))
            {
                link = htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return link;
        }

        //public static MvcHtmlString JAuthLink(this HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
        //{
        //    return JAuthLink(helper, linkText, actionName, null, routeValues, htmlAttributes);
        //}

        //public static MvcHtmlString JAuthLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        //{
        //    return JAuthLink(htmlHelper, linkText, actionName, null, null, null);
        //}

        //public static MvcHtmlString JAuthLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        //{
        //    return JAuthLink(htmlHelper, linkText, actionName, controllerName, null, null);
        //}

        /// <summary>
        /// 根据用户是否有权限生成一个链接
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="linkText">链接文本</param>
        /// <param name="actionName">Action名称</param>
        /// <param name="routeValues">路由对象包</param>
        /// <returns>HtmlString,没有权限返回空串</returns>
        public static MvcHtmlString JAuthLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return JAuthLink(htmlHelper, linkText, actionName, null, routeValues, null);
        }

        /// <summary>
        /// 输出有HTML标记的资源字符串
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="name">资源名称</param>
        /// <param name="defaultValue">没有此资源的默认值</param>
        /// <returns>带HTML标记的资源字符串</returns>
        public static MvcHtmlString RawStr(this HtmlHelper htmlHelper, string name, string defaultValue = null)
        {
            return new MvcHtmlString(ResHelper.GetStr(name, defaultValue));
        }

        /// <summary>
        /// 编码输出资源字符串
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="name">资源名称</param>
        /// <param name="defaultValue">没有此资源的默认值</param>
        /// <returns>资源字符串</returns>
        public static String Str(this HtmlHelper htmlHelper, string name, string defaultValue = null)
        {
            return ResHelper.GetStr(name, defaultValue);
        }

        /// <summary>
        /// 获取某个操作的ID，用于前台权限判断
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="actionName">Action名称</param>
        /// <param name="method">提交方法</param>
        /// <returns>链接的ID</returns>
        public static String GetId(this HtmlHelper htmlHelper, string actionName, WebMethod method = WebMethod.POST)
        {
            return GetId(htmlHelper, actionName, null, null, method);
        }

        /// <summary>
        /// 获取某个操作的ID，用于前台权限判断
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="actionName">Action名称</param>
        /// <param name="controllerName">Controller名称</param>
        /// <param name="method">提交方法</param>
        /// <returns>链接的ID</returns>
        public static String GetId(this HtmlHelper htmlHelper, string actionName, string controllerName, WebMethod method = WebMethod.POST)
        {
            return GetId(htmlHelper, actionName, controllerName, null, method);
        }

        /// <summary>
        /// 获取某个操作的ID，用于前台权限判断
        /// </summary>
        /// <param name="htmlHelper">HTMLhelper 帮助类实例.</param>
        /// <param name="actionName">Action名称</param>
        /// <param name="controllerName">Controller名称</param>
        /// <param name="routeValues">路由对象包</param>
        /// <param name="method">提交方法</param>
        /// <returns>链接的ID</returns>
        public static String GetId(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, WebMethod method = WebMethod.POST)
        {
            string linkUrl = ((WebViewPage)htmlHelper.ViewDataContainer).Url.Action(actionName, controllerName, routeValues);
            return AppManager.Instance.GetFunctionId(linkUrl, method);
        }

        /// <summary>
        /// 根据指定的功能生成链接地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string FunctionAction(this UrlHelper url, AppFunction func)
        {
            if (func.ControllerName.IsEmpty())
            {
                return "#";
            }
            string controller = func.ControllerName.Split('.').Last().Replace("Controller", "");
            RouteValueDictionary dict = new RouteValueDictionary();

            foreach (var p in func.Parameters)
            {
                if (p.ValuePattern.IsEmpty()) continue;
                if (Regex.IsMatch(p.ValuePattern, "^[\\w|\\d]+$")) //valuepattern不是正则表达式
                {
                    dict[p.Name] = p.ValuePattern;
                }
            }
            dict["area"] = func.AreaName;
            return url.Action(func.ActionName, controller, dict);
        }

        /// <summary>
        /// 通过RouteValues或QueryString获取指定的值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParamsValue(this RequestContext context, string name)
        {
            string r = CommOp.ToStr(context.RouteData.Values[name]);

            if (r.IsEmpty())
            {
                r = CommOp.ToStr(context.HttpContext.Request[name]);
            }
            return r;
        }

        /// <summary>
        /// 使用Jquery的jqGrid扩展来渲染出一个JqGrid的表格UI界面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="pagerSource"></param>
        /// <returns></returns>
        public static MvcHtmlString JGrid<T>(this HtmlHelper htmlHelper, IEnumerable<T> dataSource)
        {
            var colNames = string.Join(",", typeof(T).GetProperties()
                .Select(p => "'" + p.Name + "'"));
            var colModel = string.Join(",", typeof(T).GetProperties()
                .Select(p => String.Format("{{name:'{0}',index:'{0}'}}", p.Name)));

            var gridJs = @"
<table id=""grid_{0}""></table>
<div id=""pager_{0}""></div>
<script type=""text/javascript"">
$(document).ready(function(){{
    $(""#grid_{0}"").jqGrid({{
        caption: ""jqGrid MVC组件"",
        url:""{1}"",
        editurl:""{1}"",
       datatype:""json"", 
        mtype:""POST"",
        height:420,
       
        autowidth:true,
        colNames:[{2}],
        colModel:[
{3}
        ],
        rownumbers:true,//添加左侧行号
        //altRows:true,//设置为交替行表格,默认为false
        //sortname:'createDate',
        //sortorder:'asc',
        viewrecords: true,//是否在浏览导航栏显示记录总数
        rowNum:15,//每页显示记录数
        rowList:[15,20,25],//用于改变显示行数的下拉列表框的元素数组。
        jsonReader:{{
            id: ""Id"",//设置返回参数中，表格ID的名字为blackId
            
            repeatitems : false
        }},
        pager:$('#pager_{0}')
    }});
}});
</script>";
            string getDataUrl = htmlHelper.ViewContext.HttpContext.Request.Url.ToString();
            gridJs = String.Format(gridJs, typeof(T).Name, getDataUrl, colNames, colModel);
            return new MvcHtmlString(gridJs);
        }

        /// <summary>
        /// 根据现有请求信息重新构造一个请求字典
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RouteValueDictionary ReBuildRouteDict(this ViewContext context)
        {
            var routeValues = new RouteValueDictionary(); // ViewContext.RouteData.Values;
            var request = context.RequestContext.HttpContext.Request;
            foreach (string key in request.QueryString.Keys)
            {
                if (!key.IsEmpty())
                {
                    routeValues[key] = request.QueryString[key];
                }
            }

            foreach (KeyValuePair<string, object> so in context.RouteData.Values)
            {
                if (so.Value.GetType() == typeof(String) || so.Value.GetType().IsValueType)
                {
                    routeValues[so.Key] = so.Value;
                }
            }
            return routeValues;
        }

        /// <summary>
        /// 将Form表单中的数据导入实体对象中的同名属性
        /// </summary>
        /// <param name="form">表单数据</param>
        /// <param name="model">实体对象</param>
        public static void AssignFormValues(this NameValueCollection form, object model)
        {
            //添加其他程序子类中的特殊属性
            foreach (string key in form.Keys)
            {
                var prop = model.GetType().GetProperty(key);
                if (prop != null)
                {
                    if (prop.PropertyType == typeof(bool))
                    {
                        //在复选框时，勾选会传回两个值： true,false; 所以要分辨
                        RefHelper.SetValue(model, key, CommOp.ToStr(form[key]).Split(',').First());
                    }
                    else
                    {
                        RefHelper.SetValue(model, key, CommOp.ToStr(form[key]));
                    }
                }
            }
        }

        /// <summary>
        /// 提供一个所有用户的下拉列表选项
        /// </summary>
        /// <param name="helper">HTMLHelper</param>
        /// <param name="allowNull">是否在前面加个空值</param>
        /// <returns>所有用户的下拉列表</returns>
        public static IEnumerable<SelectListItem> GetUsersSelectList(this HtmlHelper helper, bool allowNull = true)
        {
            var usersList = AppManager.Instance.UserManager.GetAll()
            .OrderBy(u => u.Name)
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id,
            }).ToList();
            if (allowNull)
            {
                usersList.Insert(0, new SelectListItem { Text = "无", Value = "0" });
            }
            return usersList;
        }

        /// <summary>
        /// 根据栏目扩展属性生成下拉列表项
        /// </summary>
        /// <param name="catExt"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList(this Base_CatalogExt catExt)
        {
            var datas = catExt.DataSource.Split(';').ToList();
            var items = datas.Select(d =>
            {
                var txts = d.Split('=');
                var txt = txts[0];
                var val = txt;
                if (txts.Length > 1)
                {
                    val = txts[1];
                }
                return new SelectListItem
                {
                    Text = txt,
                    Value = val
                };
            });

            return items;
        }

        /// <summary>
        /// 根据栏目扩展属性标签生成下拉列表项
        /// </summary>
        /// <param name="catExt"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList(this CatalogExtAttribute catExt)
        {
            return GetSelectList(catExt.DataSource);
        }

        static IEnumerable<SelectListItem> GetSelectList(string s)
        {
            var datas = s.Split(';').ToList();
            var items = datas.Select(d =>
            {
                var txts = d.Split('=');
                var txt = txts[0];
                var val = txt;
                if (txts.Length > 1)
                {
                    val = txts[1];
                }
                return new SelectListItem
                {
                    Text = txt,
                    Value = val
                };
            });

            return items;
        }

        /// <summary>
        /// 根据sql语句返回下拉列表所需数据
        /// </summary>
        /// <param name="catExt"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSqlList(this Base_CatalogExt catExt)
        {
            string sql = catExt.DataSource;
            var dbHelper = SiteManager.Get<DBHelper>();
            List<IDataParameter> ps = new List<IDataParameter>();
            if (sql.Contains("@UserId"))
            {
                ps.Add(dbHelper.CreateParameter("@UserId", AppManager.Instance.GetCurrentUserId()));
            }
            DataTable dt = dbHelper.ExecDataTable(sql, ps.ToArray());
            int valIndex = dt.Columns.Count > 0 ? 1 : 0;
            var items = dt.Rows.Cast<DataRow>()
                .Select(dr => new SelectListItem
                {
                    Text = CommOp.ToStr(dr[0]),
                    Value = CommOp.ToStr(dr[valIndex])
                });
            return items;
        }

        /// <summary>
        /// 根据sql语句返回下拉列表所需数据
        /// </summary>
        /// <param name="catExt"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSqlList(this CatalogExtAttribute catExt)
        {
            string sql = catExt.DataSource;
            var dbHelper = SiteManager.Get<DBHelper>();
            List<IDataParameter> ps = new List<IDataParameter>();
            if (sql.Contains("@UserId"))
            {
                ps.Add(dbHelper.CreateParameter("@UserId", AppManager.Instance.GetCurrentUserId()));
            }
            DataTable dt = dbHelper.ExecDataTable(sql, ps.ToArray());
            return GetSelectList(dt);
        }

        static IEnumerable<SelectListItem> GetSelectList(DataTable dt)
        {
            int valIndex = dt.Columns.Count > 0 ? 1 : 0;
            var items = dt.Rows.Cast<DataRow>()
                .Select(dr => new SelectListItem
                {
                    Text = CommOp.ToStr(dr[0]),
                    Value = CommOp.ToStr(dr[valIndex])
                });
            return items;
        }

        /// <summary>
        /// 根据Model中定义的方法来获取下拉列表所需数据
        /// 方法可以返回一个字符串或一个DataTable或一个System.Web.MVC.SelectListItem集合
        /// </summary>
        /// <param name="catExt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetUserDefineList(this CatalogExtAttribute catExt, object model)
        {
            string funcName = catExt.DataSource;

            MethodInfo m = model.GetType().GetMethod(funcName);

            if (m == null)
            {
                return null;
            }

            object o = m.Invoke(model, null);
            if (o is string)
            {
                return GetSelectList((string)o);
            }
            else if (o is DataTable)
            {
                return GetSelectList((DataTable)o);
            }

            return o as IEnumerable<SelectListItem>;
        }

        /// <summary>
        /// 标该标签的控件不会显示，但是会用隐藏域传值
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static bool IsHidden(this CatalogExtAttribute attr)
        {
            return attr.DataSourceType == ExtDataSourceType.Hidden;
        }


        /// <summary>
        /// 获取某个枚举类型的枚举名称对应的多语言Key值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="valueName">该类型上的枚举名称</param>
        /// <returns>Key值: Enum.枚举类型名称.值名称</returns>
        public static string GetEnumResKey(this Type enumType, string valueName)
        {
            return ResHelper.GetStr("Enum." + enumType.Name + "." + valueName, valueName);
        }


        /// <summary>
        /// 根据枚举类型，返回一个下拉列表
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSelectList(this Type type)
        {
            return type.GetEnumNames()
              .Select(name => new SelectListItem
              {
                  Text = type.GetEnumResKey(name),
                  Value = ((int)Enum.Parse(type, name)).ToString(),
              });
        }

        #region 用于登录页逻辑的扩展方法
        internal const string Cookie_LoginName = "_Ln_";
        internal const string Cookie_Password = "_Pw_";
        internal const string Cookie_RememberMe = "_Rb_";
        internal const string Cookie_StartPage = "startPage";

        internal static LoginModel GetLoginModel(this BaseController controller)
        {
            var model = new LoginModel();
            model.UserName = Encryption.Decrypt(controller.GetCookie(Cookie_LoginName));
            model.Password = Encryption.Decrypt(controller.GetCookie(Cookie_Password));
            model.RememberMe = CommOp.ToBool(controller.GetCookie(Cookie_RememberMe));
            return model;
        }

        internal static void SetLoginModel(this BaseController controller, LoginModel model)
        {
            controller. SetCookie(Cookie_RememberMe, "True");
            controller.SetCookie(Cookie_LoginName, Encryption.Encrypt(model.UserName), 30 * 24 * 60);
            controller.SetCookie(Cookie_Password, Encryption.Encrypt(model.Password), 30 * 24 * 60);
        }

        internal static void ClearLoginModel(this BaseController controller)
        {
            controller.RemoveCookie(Cookie_RememberMe);
            controller.RemoveCookie(Cookie_LoginName);
            controller.RemoveCookie(Cookie_Password);
        }

        internal static void SetStartPageCookie(this AccountController controller, string returnUrl)
        {
            if (!returnUrl.IsEmpty() && !returnUrl.Equals(controller.Url.Action("Index", "Home")))
            {
                var function = AppManager.Instance.GetFunctionByLocation(returnUrl);
                controller.SetCookie(Cookie_StartPage, JsonHelper.ToJson(new
                {
                    Id = function == null ? "shortcut_startpage" : function.Id,
                    Name = function == null ? null : function.Name,
                    Location = returnUrl
                }));
            }
            else
            {
               controller.RemoveCookie(Cookie_StartPage);
            }
        }
        #endregion
    }
}
