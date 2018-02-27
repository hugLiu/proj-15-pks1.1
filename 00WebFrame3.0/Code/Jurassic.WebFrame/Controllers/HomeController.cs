using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.AppCenter;
using Jurassic.WebFrame;
using Jurassic.AppCenter.Config;
using Jurassic.WebFrame.Models;
using Ninject;
using Jurassic.Com.Tools;
using System.IO;
using System.Text.RegularExpressions;
using Jurassic.AppCenter.Caches;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Organization;

namespace Jurassic.WebFrame.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 框架主页
        /// </summary>。
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            //  ConfigData.ShowTab = true;
            return View(UserConfig);
        }

        /// <summary>
        /// 
        /// </summary>。
        /// <returns></returns>
        public virtual ActionResult HomeSetting()
        {
            return View();
        }

        /// <summary>
        /// 主页中内框架mainframe中的起始页
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult StartPage()
        {
            string userid = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userid))
            {
                return RedirectAction("Login", "Account");
            }
            var userStartPageConfig = UserConfig.UserStartPageConfig;
            if (userStartPageConfig == null)
            {
                var cookieModel = CreateCookieModel(userid, false);
                ViewData["userCookie"] = cookieModel;
                UserConfig.UserStartPageConfig = cookieModel;

                var tUser = AppManager.Instance.UserManager.GetById(userid);

                if (tUser != null)
                {
                    //添加其他程序AppUser子类中的特殊属性
                    Request.Form.AssignFormValues(tUser);

                    AppManager.Instance.UserManager.Change(tUser);
                }
                else
                {
                    return RedirectAction("Logout", "Account");
                }
            }
            else
            {
                SyncUserCookieWidgetData(ref userStartPageConfig);
                ViewData["userCookie"] = userStartPageConfig;
            }

            return View();
        }

        /// <summary>
        /// mainframe起始页的配置页面
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult StartConfigPage()
        {
            #region old
            //string userid = User.Identity.GetUserId();

            ////初始化用户的初始主页显示，应该提出成为一个方法在控制器外。
            //string cookieName = "homePage_" + userid;

            //ViewData["cookieId"] = cookieName;
            //var jscookie = Com.Tools.WebHelper.GetCookie(cookieName);

            //if (string.IsNullOrWhiteSpace(jscookie))
            //{
            //    var cookieModel = CreateCookieModel(userid);
            //    ViewData["userCookie"] = cookieModel;
            //    // Com.Tools.WebHelper.SetCookie(cookieName, HttpUtility.UrlEncode(JsonHelper.ToJson(cookieModel, false)), 365);
            //    ViewBag.ChkWidgetIds = GetChkWidgetIds(cookieModel);
            //}
            //else
            //{
            //    var cookie = HttpUtility.UrlDecode(jscookie);
            //    var userCookieModel = JsonHelper.FormJson(cookie, typeof(UserCookieModel)) as UserCookieModel;
            //    SyncUserCookieWidgetData(ref userCookieModel);
            //    ViewData["userCookie"] = userCookieModel;
            //    ViewBag.ChkWidgetIds = GetChkWidgetIds(userCookieModel);
            //}
            #endregion

            #region new
            string userid = User.Identity.GetUserId();

            var userStartPageConfig = UserConfig.UserStartPageConfig;
            if (userStartPageConfig == null)
            {
                var cookieModel = CreateCookieModel(userid);
                ViewData["userCookie"] = cookieModel;
                ViewBag.ChkWidgetIds = GetChkWidgetIds(cookieModel);
            }
            else
            {
                SyncUserCookieWidgetData(ref userStartPageConfig);
                ViewData["userCookie"] = userStartPageConfig;
                ViewBag.ChkWidgetIds = GetChkWidgetIds(userStartPageConfig);
            }
            #endregion

            ViewBag.WidgetList = GetWidgetList();

            return View();
        }

        [HttpPost]
        public virtual bool SaveStartConfig(string widgetsJson, string colLayout)
        {
            IList<WidgetModel> widgets = JsonHelper.FromJson<IList<WidgetModel>>(widgetsJson);

            UserConfig.UserStartPageConfig.PanelIncision = colLayout;
            UserConfig.UserStartPageConfig.Widgets = widgets;

            string userid = User.Identity.GetUserId();
            var tUser = AppManager.Instance.UserManager.GetById(userid);

            //添加其他程序AppUser子类中的特殊属性
            Request.Form.AssignFormValues(tUser);

            var result = AppManager.Instance.UserManager.Change(tUser);

            return result;
        }

        private void SyncUserCookieWidgetData(ref UserCookieModel userCookieModel)
        {
            var widgets = AppManager.Instance.GetWidgetFunctions();

            for (int i = userCookieModel.Widgets.Count - 1; i >= 0; i--)
            {
                var userWidget = userCookieModel.Widgets[i];
                var widgetItem = widgets.Where(w => w.Id == userWidget.WidgetId).FirstOrDefault();

                if (widgetItem != null)
                {
                    userWidget.WidgetTitle = widgetItem.Name;
                    userWidget.RenderUrl = widgetItem.Location;
                    userWidget.RenderAction = widgetItem.ActionName;
                    userWidget.RenderController = widgetItem.ControllerName;
                }
                else
                {
                    userCookieModel.Widgets.Remove(userWidget);
                }
            }
        }

        private List<string> GetChkWidgetIds(UserCookieModel cookieModel)
        {
            var list = cookieModel.Widgets.Select(w => w.WidgetId).ToList();
            return list;
        }

        private List<dynamic> GetWidgetList()
        {
            var widgets = base.Function.GetChildren()
               .Where(f => (f.Visible & VisibleType.Widget) == VisibleType.Widget)
               .OrderBy(f => f.Ord)
               .ThenBy(f => f.Id)
               .ToArray();

            List<dynamic> widgetList = widgets.Select(w => new WidgetModel
            {
                WidgetId = w.Id,
                WidgetTitle = w.Name,
                Order = w.Ord.ToString(),
                WidgetBody = "",
                WidgetColumn = WidgetPlace.Left,
                WidgetHeight = "250",
                WidgetShowCloseButton = "false",
                RenderUrl = w.Location,
                RenderAction = w.ActionName,
                RenderController = w.ControllerName
            }).ToList<dynamic>();

            return widgetList;
        }

        private UserCookieModel CreateCookieModel(string userId, bool isConfigPage = true)
        {
            var widgets = isConfigPage ? base.Function.GetChildren()
                .Where(f => (f.Visible & VisibleType.Widget) == VisibleType.Widget)
                .OrderBy(f => f.Ord)
                .ThenBy(f => f.Id)
                .ToArray() : AppManager.Instance.GetWidgetFunctions();

            var widgetModel = widgets.Select(w => new WidgetModel
            {
                WidgetId = w.Id,
                WidgetTitle = w.Name,
                Order = w.Ord.ToString(),
                WidgetBody = "",
                WidgetColumn = WidgetPlace.Left,
                WidgetHeight = "250",
                WidgetShowCloseButton = "false",
                RenderUrl = w.Location,
                RenderAction = w.ActionName,
                RenderController = w.ControllerName
            }).ToList();

            var cookieModel = new UserCookieModel
            {
                UserId = userId,
                //UserName = "xujg",
                PanelHeight = "100",
                PanelWidth = "100",
                PanelIncision = "['50%','50%']",
                Widgets = widgetModel
            };

            return cookieModel;
        }
        public virtual ActionResult SelfServiceIndex()
        {
            string themePath = Server.MapPath("~/content/theme");
            ViewData["ThemeList"] = Directory.GetDirectories(themePath)
                .Select(dir => new DirectoryInfo(dir).Name);
            ViewBag.ShowToolBar = false;
            return View(UserConfig);
        }

        [HttpPost]
        public virtual ActionResult SelfServiceIndex(UserConfig user)
        {
            if (!ModelState.IsValid) return JsonTips(user);

            var tUser = AppManager.Instance.UserManager.GetById(user.Id);

            //添加其他程序AppUser子类中的特殊属性
            Request.Form.AssignFormValues(tUser);

            AppManager.Instance.UserManager.Change(tUser);
            return JsonTips("success", FStr.UpdateUserInfoSucceed, FStr.UpdateUserInfoSucceed0, user, user.Name);
        }
    }
}
