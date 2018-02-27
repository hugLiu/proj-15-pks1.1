using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using PKS.Models;
using PKS.Utils;
using PKS.Web.MVC;
using PKS.Web.MVC.Filter;
using PKS.WebAPI.Models;
using System.Security.Principal;

namespace PKS.Web.Controllers
{
    /// <summary>PKS Web MVC基类控制器</summary>
    public abstract class PKSBaseController : Controller
    {
        #region 授权方法
        /// <summary>认证令牌</summary>
        protected string Token
        {
            get { return this.HttpContext.GetTokenFromSession(); }
        }
        /// <summary>获得登录用户</summary>
        protected IPKSPrincipal PKSUser
        {
            get { return this.HttpContext.GetPrincipal(); }
        }
        /// <summary>获得登录用户</summary>
        [AllowAnonymous]
        public ActionResult GetPrincipals()
        {
            var principals = this.BuildPrincipals();
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return NewtonJson(principals, JsonRequestBehavior.AllowGet, settings);
        }
        #endregion

        #region JSON序列化方法
        /// <summary>使用Newton库序列化</summary>
        protected NewtonJsonResult DefaultNewtonJson<T>(T data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return NewtonJson(data, behavior, new JsonSerializerSettings());
        }
        /// <summary>使用Newton库序列化</summary>
        protected NewtonJsonResult NewtonJson<T>(T data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet, JsonSerializerSettings settings = null)
        {
            return new NewtonJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                Settings = settings ?? JsonUtil.CamelCaseJsonSerializerSettings
            };
        }

        #endregion

        #region 依赖服务解析方法

        /// <summary>服务提供者</summary>
        /// <remarks>不要使用MVC控制器提供的Resolver,内部使用的是缓存未使用Scope</remarks>
        protected IDependencyResolver ServiceProvider => DependencyResolver.Current;

        /// <summary>获得注入服务</summary>
        protected TService GetService<TService>()
        {
            return (TService)ServiceProvider.GetService(typeof(TService));
        }
        #endregion
    }
}