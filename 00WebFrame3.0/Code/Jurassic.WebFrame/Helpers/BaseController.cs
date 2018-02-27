using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using System.Diagnostics;
using Jurassic.AppCenter.Logs;
using System.Threading;
using Jurassic.AppCenter.Resources;
using Jurassic.WebFrame.Models;
using Jurassic.AppCenter.Config;
using Ninject;
using Jurassic.CommonModels;

namespace Jurassic.WebFrame
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 所有Controller的基类，用于日志记录、提示信息和用户权限
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 错误的请求
        /// </summary>
        protected static HttpStatusCodeResult BadRequest = new HttpStatusCodeResult(400);
        ReturnValueWithTips mTips;

        /// <summary>
        /// 本页的提示信息对象，它可能跨页传递给下一个页面。
        /// </summary>
        ReturnValueWithTips Tips
        {
            get
            {
                if (mTips == null)
                {
                    mTips = new ReturnValueWithTips();
                }
                return mTips;
            }
            set
            {
                mTips = value;
            }
        }

        /// <summary>
        /// 数据ID
        /// </summary>
        public virtual int Id { get; private set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        public virtual int CatalogId { get; private set; }

        /// <summary>
        /// 当前会话的ID, 之所以不直接用Session.SessionID,是因为
        /// 不在Session里放东西，它每次就会生成一个不同的ID,放了一个东西它就固定
        /// </summary>
        protected string CurrentSessionId
        {
            get
            {
                string currentSessionId = CommOp.ToStr(Session["CurrentSessionId"]);
                if (currentSessionId.IsEmpty())
                {
                    currentSessionId = Session.SessionID;
                    Session["CurrentSessionId"] = currentSessionId;
                }
                return currentSessionId;
            }
        }

        /// <summary>
        /// 当前执行的功能描述实体对象
        /// </summary>
        public AppFunction Function { get; set; }

        /// <summary>
        /// 面包屑导航
        /// </summary>
        public List<AppFunction> BreadCrumbList { get; set; }

        /// <summary>
        /// 当前控制器的日志数据实体对象
        /// </summary>
        public JLogInfo LogInfo { get; set; }

        /// <summary>
        /// 是否是回发操作
        /// </summary>
        protected bool IsPostBack
        {
            get
            {
                return Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase);
            }
        }

        private AppUser _currentUser;

        /// <summary>
        /// 代表当前用户的AppUser对象
        /// </summary>
        public AppUser CurrentUser
        {
            get
            {
                if (_currentUser == null && !CurrentUserId.IsEmpty())
                {
                    _currentUser = AppManager.Instance.UserManager.GetById(CurrentUserId);
                }
                return _currentUser;
            }
        }

        /// <summary>
        /// 代表当前用户的AppUser对象的ID
        /// </summary>
        protected string CurrentUserId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        public UserConfig UserConfig { get; set; }

        public BaseController()
        {
            this.LogInfo = new JLogInfo();
            sw.Start();
        }

        /// <summary>
        /// 判断用户是否有某URL的访问权限
        /// </summary>
        /// <param name="location">URL地址(不带http)</param>
        /// <param name="method"></param>
        /// <returns>有/无</returns>
        public bool HasRight(string location, WebMethod method = WebMethod.GET)
        {
            return User.Identity.HasRight(location, method);
        }

        /// <summary>
        /// 判断当前用户是否有某功能ID的访问权限
        /// </summary>
        /// <param name="functionId">功能ID</param>
        /// <returns>有/无</returns>
        public bool HasRightId(string functionId)
        {
            return User.Identity.HasRightId(functionId);
        }
        //protected override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    base.OnAuthorization(filterContext);
        //    if (CurrentUserId.IsEmpty())
        //    {
        //        var user = _usrMgr.FindByName("wangleyi");
        //        var identity = _usrMgr.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        //        HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
        //        CurrentUserId = user.Id;
        //    }
        //}

        Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 在服务端数据验证错误时的操作,如果不想检查错误，则重写此方法并返回空值
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual ActionResult OnModelError(IEnumerable<ModelError> error)
        {
            if (Request.IsAjaxRequest())
            {
                //当数据验证不通过时终止请求，并返回错误信息
                return JsonTips();
            }
            else
            {
                return null; ///非ajax请求时正常返回带Tips提示信息的页面
            }
        }

        /// <summary>
        /// 重写以处理Tips提示信息对象
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!ModelState.IsValid)
            {
                OnModelError(CheckModelError());
            }

            if (CurrentUser != null)
            {
                UserConfig = CurrentUser as UserConfig;
            }
            else
            {
                UserConfig = SiteManager.Get<UserConfig>();
            }

            if (UserConfig.Theme.IsEmpty())
            {
                var theme = WebHelper.GetCookie("theme");
                UserConfig.Theme = theme;
            }

            var tempTips = TempData["Tips"] as ReturnValueWithTips;
            if (tempTips != null)
            {
                Tips = tempTips;
            }

            BreadCrumbList = new List<AppFunction>();

            if (Function == null)
            {
                Function = AppManager.Instance.FunctionManager.Create();
            }

            //循环构造面包屑导航
            var func = Function;
            while (func != null)
            {
                if (func.Method == WebMethod.GET)
                {
                    BreadCrumbList.Insert(0, func);
                }
                func = AppManager.Instance.FunctionManager.GetById(func.ParentId);
            }

            if (Id == 0) Id = CommOp.ToInt(filterContext.RequestContext.GetParamsValue("id"));
            if (CatalogId == 0) CatalogId = CommOp.ToInt(filterContext.RequestContext.GetParamsValue("CatalogId"));

            LogInfo.ActionName = filterContext.ActionDescriptor.ActionName;
            LogInfo.Browser = Request.Browser.Browser;
            LogInfo.BrowserVersion = CommOp.ToDecimal(Request.Browser.Version);
            LogInfo.ClientIP = GetIp();
            LogInfo.CatalogId = CatalogId;
            LogInfo.LogType = Function.LogType.ToString();
            LogInfo.Message = Function.Name;
            LogInfo.ModuleName = this.GetType().Name.Replace("Controller", "");
            LogInfo.ObjectId = Id;
            LogInfo.OpTime = DateTime.Now;
            LogInfo.Platform = LogHelper.GetOSNameByUserAgent(Request.UserAgent);
            LogInfo.Request = (this.Request.IsAjaxRequest() ? "Ajax" : "") + this.Request.HttpMethod + " " + Request.Url.ToString();
            LogInfo.UserName = User.Identity.Name;
        }

        private string GetIp()
        {
            if (Request.ServerVariables["HTTP_VIA"] != null)
                return Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return Request.ServerVariables["REMOTE_ADDR"];
        }
        /// <summary>
        /// 当页面数据验证有错误时，找到错误信息
        /// </summary>
        protected virtual IEnumerable<ModelError> CheckModelError()
        {
            IEnumerable<ModelError> errors = null;
            if (!ModelState.IsValid)
            {
                errors = ModelState.Values.SelectMany(v => v.Errors);

                Tips.Type = "error";
                Tips.Message = String.Join(";", errors.Select(err => err.ErrorMessage.IsEmpty() ? (err.Exception == null ? "Data Validate Error" : err.Exception.Message) : err.ErrorMessage));
            }
            return errors;
        }

        /// <summary>
        /// 返回一个带提示信息的Json对象。这个通常在验证不通过时
        /// 提示验证错误
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        protected JsonResult JsonTips(object returnValue = null)
        {
            CheckModelError();
            Tips.ReturnValue = returnValue;
            return Json(Tips);
        }


        /// <summary>
        /// 返回一个带提示信息的Json对象。这个通常在验证不通过时
        /// 提示验证错误
        /// </summary>
        /// <param name="returnValue">返回的对象</param>
        /// <param name="method">提交方法的限制</param>
        /// <returns>带提示的返回对象</returns>
        protected JsonResult JsonTips(object returnValue, JsonRequestBehavior method = JsonRequestBehavior.DenyGet)
        {
            CheckModelError();
            Tips.ReturnValue = returnValue;
            return Json(Tips, method);
        }

        /// <summary>
        /// 返回一个带提示信息的Json对象
        /// </summary>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        protected ActionResult ViewTips(object returnValue = null)
        {
            var view = View(returnValue);
            Tips.Type = "";
            return view;
        }

        /// <summary>
        /// 返回一个带提示信息的Json对象
        /// </summary>
        /// <param name="type">error,warning,success中的一个</param>
        /// <param name="title">信息标题</param>
        /// <param name="message">信息内容</param>
        /// <param name="returnValue">附加的对象数据</param>
        /// <param name="formatArgs">以message作为格式化字符串的格式化参数</param>
        /// <returns>Json序列化后的ReturnValueWithTips对象</returns>
        protected JsonResult JsonTips(string type, string title, string message, object returnValue = null, params object[] formatArgs)
        {
            Tips.Type = type;
            Tips.Title = title;
            Tips.Message += String.Format(message, formatArgs);
            Tips.ReturnValue = returnValue;
            return Json(Tips);
        }

        /// <summary>
        /// 使用多语言版本显示信息
        /// </summary>
        /// <param name="type">error,warning,success中的一个</param>
        /// <param name="titleKey">信息标题的多语言Key</param>
        /// <param name="messageKey">信息内容的多语言Key</param>
        /// <param name="returnValue">附加的对象数据</param>
        /// <param name="formatArgs">以message作为格式化字符串的格式化参数</param>
        /// <returns>Json序列化后的ReturnValueWithTips对象</returns>
        protected JsonResult JsonTipsLang(string type, string titleKey, string messageKey, object returnValue = null, params object[] formatArgs)
        {
            Tips.Type = type;
            Tips.Title = ResHelper.GetStr(titleKey);
            Tips.Message += String.Format(ResHelper.GetStr(messageKey), formatArgs);
            Tips.ReturnValue = returnValue;
            return Json(Tips);
        }

        /// <summary>
        /// 返回一个带提示信息的Json对象
        /// </summary>
        /// <param name="type">error,warning,success中的一个</param>
        /// <param name="message">信息内容</param>
        /// <param name="returnValue">附加的对象数据</param>
        /// <returns>Json序列化后的ReturnValueWithTips对象</returns>
        protected JsonResult JsonTips(string type, string message, object returnValue = null, params string[] formatArgs)
        {
            Tips.Type = type;
            Tips.Message += String.Format(message, formatArgs);
            Tips.ReturnValue = returnValue;
            return Json(Tips);
        }

        /// <summary>
        /// 使用多语言版本显示信息
        /// </summary>
        /// <param name="type">error,warning,success中的一个</param>
        /// <param name="message">信息内容的多语言Key</param>
        /// <param name="returnValue">附加的对象数据</param>
        /// <param name="formatArgs">以messageKey作为格式化字符串的格式化参数</param>
        /// <returns>Json序列化后的ReturnValueWithTips对象</returns>
        protected JsonResult JsonTipsLang(string type, string messageKey, object returnValue = null, params string[] formatArgs)
        {
            Tips.Type = type;
            Tips.Message += String.Format(ResHelper.GetStr(messageKey), formatArgs);
            Tips.ReturnValue = returnValue;
            return Json(Tips);
        }

        /// <summary>
        /// Json()方法的替换版本，用NewtonSoft.Json来序列化对象
        /// 这对于序列化DataTable很有用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ActionResult JsonNT(object obj)
        {
            return Content(JsonHelper.ToJson(obj));
        }

        /// <summary>
        /// 重写以处理Tips提示信息以及记录日志
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SaveTips();
            if (filterContext.Result is JsonResult)
            {
                TempData.Remove("Tips");
            }
            LogInfo.Costs = sw.ElapsedMilliseconds;
            LogHelper.Write(LogInfo);
        }

        /// <summary>
        /// 供视图显示调用，以显示本页执行的秒数
        /// 用在视图末尾，可以较准确地估算视图实际运行的时间
        /// </summary>
        /// <returns></returns>
        public virtual double OnFinish()
        {
            sw.Stop();
            LogInfo.Costs = sw.ElapsedMilliseconds;
            LogHelper.Write(LogInfo);
            return LogInfo.Costs;
        }

        /// <summary>
        /// 保存提示信息以使前台能获取到
        /// </summary>
        internal void SaveTips()
        {
            TempData["Tips"] = Tips;
            ViewBag.Tips = Tips;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var exp = filterContext.Exception;
            JException myExp = exp as JException;
            sw.Stop();
            LogInfo.Costs = sw.ElapsedMilliseconds;
            LogInfo.LogType = JLogType.Error.ToString();
            LogHelper.Write(LogInfo, exp);
            if (myExp != null)
            {
                if (Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new ReturnValueWithTips("error", exp.Source, exp.Message, null), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw myExp;
                }
            }
            else if (exp is HttpAntiForgeryException)
            {
                filterContext.Result = Json(new ReturnValueWithTips("warning", "HTTP异常", "正在重新刷新页面", new { Url = Request.UrlReferrer.ToString() }), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 当返回的是HTML页面时，要在页面上显示的提示信息
        /// 通常用于跨页面显示提示信息
        /// </summary>
        /// <param name="type">error,warning,success中的一个</param>
        /// <param name="title">信息标题</param>
        /// <param name="message">信息内容</param>
        /// <param name="returnValue">附加的对象数据</param>
        public void SetTips(string type, string title, string message, object returnValue = null, params string[] formatArgs)
        {
            Tips.Title = title;
            Tips.Type = type;
            Tips.Message = message;
            Tips.ReturnValue = returnValue;
        }

        /// <summary>
        /// 当返回的是HTML页面时，要在页面上显示的提示信息
        /// </summary>
        /// <param name="obj">附加的对象数据</param>
        protected void SetTips(object obj)
        {
            Tips.ReturnValue = obj;
        }

        /// <summary>
        /// 释放控制器
        /// </summary>
        /// <param name="disposing">是否释放托管资源，false表示只释放非找管资源</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// 公开RedirectToAction方法,这个一般只在筛选器中用。
        /// </summary>
        /// <param name="actionName">Action名称</param>
        /// <param name="controllerName">控制器名称</param>
        /// <returns>ActionResult</returns>
        internal ActionResult RedirectAction(string actionName, string controllerName)
        {
            return RedirectToAction(actionName, controllerName);
        }

        /// <summary>
        /// 设置Cookie的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minutes">过期时间，以分钟记</param>
        public void SetCookie(string key, string value, int minutes = 0)
        {
            var cookie = new HttpCookie(key, value);
            if (minutes != 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(minutes);
            }
            Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获取指定Key的Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCookie(string key)
        {
            if (Request.Cookies.AllKeys.Contains(key))
            {
                return Request.Cookies[key].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除COOKIE
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCookie(string key)
        {
            var cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddDays(-10);
            Response.AppendCookie(cookie);
        }
    }

    /// <summary>
    /// 为前台的操作定义既要返回提示信息又要返回对象
    /// </summary>
    public class ReturnValueWithTips
    {
        /// <summary>
        /// 创建一个提示信息对象
        /// </summary>
        internal ReturnValueWithTips() { }

        /// <summary>
        /// 提示信息的内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 提示信息的类型,可以是success,warning或error三种
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 提示信息的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 附加的对象信息
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// 提示信息是否有Type
        /// </summary>
        public bool IsEmpty
        {
            get { return String.IsNullOrWhiteSpace(Type); }
        }

        /// <summary>
        /// 根据一个对象创建提示信息对象
        /// </summary>
        /// <param name="value"></param>
        internal ReturnValueWithTips(object value)
        {
            ReturnValue = value;
        }

        /// <summary>
        /// 根据提示信息和对象创建提示对象
        /// </summary>
        /// <param name="type">提示信息的类型,可以是success,warning或error三种</param>
        /// <param name="title">提示信息的标题</param>
        /// <param name="message">提示信息的内容</param>
        /// <param name="value">附加的对象信息</param>
        public ReturnValueWithTips(string type, string title, string message, object value)
        {
            this.Message = message;
            this.Type = type;
            this.Title = title;
            ReturnValue = value;
        }

    }
}