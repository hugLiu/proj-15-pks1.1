using Jurassic.AppCenter;
using Jurassic.Com.DB;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.Messages;
using Jurassic.WebFrame;
using Jurassic.WebSchedule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Jurassic.WebSchedule
{
    public static class ScheduleExtensions
    {
        /// <summary>
        /// 根据初始化对象创建一个日程表
        /// </summary>
        /// <param name="htmlHelper">HTML帮助类</param>
        /// <param name="formData">初始化参数类</param>
        /// <returns>日程表</returns>
        public static MvcHtmlString Schedule(this HtmlHelper htmlHelper,
           ScheduleFormData formData)
        {
            return htmlHelper.Partial("_Schedule", formData);
        }

        /// <summary>
        /// 根据指定元素ID创建一个日程表
        /// </summary>
        /// <param name="htmlHelper">HTML帮助类</param>
        /// <param name="elementId">HTML元素的ID</param>
        /// <returns>日程表</returns>
        public static MvcHtmlString Schedule(this HtmlHelper htmlHelper,
         string elementId)
        {
            return htmlHelper.Partial("_Schedule", new ScheduleFormData { ElementId = elementId });
        }

        /// <summary>
        /// 根据默认初始化参数创建一个日程表
        /// </summary>
        /// <param name="htmlHelper">HTML帮助类</param>
        /// <returns>日程表</returns>
        public static MvcHtmlString Schedule(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("_Schedule", new ScheduleFormData());
        }

        /// <summary>
        /// 主要是为了及时提醒用户，作为原有的轮询消息的补充，因为原有的轮询是半分钟一次，
        /// 如果有重要消息要提醒，可以使用此方法强制用户刷一下最新消息，或者指定消息提醒
        /// </summary>
        /// <param name="manager">消息管理器</param>
        /// <param name="userId">用户ID</param>
        /// <param name="alertMessage">如果为空，则强制用户刷一下消息，以更新左侧菜单和顶部消息图标中的数字，如果有值，则用此值通知用户，并在框架子页面处理具体消息</param>
        public static void AlertFast(this MessageManager manager, int userId, object alertMessage = null)
        {
            SignalRProcesserFactory.Instance.Group(userId.ToString()).Alert(alertMessage);
        }
    }
}