using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.Schedule;
using Jurassic.WebFrame;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using Jurassic.AppCenter.Resources;

namespace Jurassic.WebSchedule.Controllers
{
    /// <summary>
    /// 用户日程表的控制器
    /// </summary>
    [JAuth(JAuthType.AllUsers)]
    public class ScheduleController : BaseController
    {
        ScheduleManager _schedule;
        //
        // GET: /Calendar/
        public ScheduleController(ScheduleManager schedule)
        {
            _schedule = schedule;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有用户事件
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult GetEvents(DateTime start, DateTime end)
        {
            var events = _schedule.GetEvents(User.Identity.GetUserId().ToInt(), start, end);
            return Json(GetUrl(events), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MessageAlert()
        {
            ViewBag.ShowSearchBox = false;
            ViewBag.ShowBreadCrumb = false;
            return View();
        }

        private IEnumerable<EventModel> GetUrl(IEnumerable<EventModel> events)
        {
            foreach (var evt in events)
            {
                var url = evt.url;
                if (!url.IsEmpty())
                {
                    evt.url = Url.Content(url);
                }
            }
            return events;
        }

        private string GetUrl(Base_CatalogArticle ca)
        {
            string processUrl = CommOp.ToStr(ca.GetExt(ScheduleEvent.Root.ProcessUrl));
            if (processUrl.IsEmpty())
            {
                return null;
            }
            else
            {
                return Url.Content(processUrl);
            }
        }


        /// <summary>
        /// 点击或拖动某个空白区(id=0)，或点击某个日程(id>0)， 显示弹窗界面
        /// </summary>
        /// <param name="start"></param>
        /// <param name="caId"></param>
        /// <param name="end"></param>
        /// <param name="allDay"></param>
        /// <returns></returns>
        public ActionResult Edit(bool allDay, DateTime? start, DateTime? end, int caId = 0)
        {
            Base_CatalogArticle ca;
            if (caId == 0)
            {
                if (start != null && start.Value.TimeOfDay.TotalSeconds > 0.1)
                {
                    allDay = false;
                }
                ca = _schedule.Article.CreateByCatalog(ScheduleEvent.Root.Id);
                ca.SetExt(ScheduleEvent.Root.StartTime, start);
                ca.SetExt(ScheduleEvent.Root.EndTime, end);
                ca.SetExt(ScheduleEvent.Root.AllDay, allDay);
            }
            else
            {
                ca = _schedule.Article.GetById(caId);
            }
            return View(ca.Article);
        }

        /// <summary>
        /// 读取消息 
        /// </summary>
        /// <param name="caId"></param>
        /// <returns></returns>
        public ActionResult Read(int caId)
        {
            Base_CatalogArticle ca = _schedule.Article.GetById(caId);
            //读过以后加上标记
            var state = ScheduleEvent.EventRead;
            _schedule.Article.ChangeState(new int[] { ca.Article.Id }, state);
            return View(ca.Article);
        }

        /// <summary>
        /// 接受弹窗编辑界面的提交数据
        /// </summary>
        /// <param name="a"></param>
        /// <param name="caId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Base_Article a, int caId = 0)
        {
            Base_CatalogArticle ca;
            if (caId > 0)
            {
                ca = _schedule.Article.GetById(caId);
                if (ca.Article.State == ScheduleEvent.OptionReadOnly)
                {
                    return JsonTips("error", SStr.CantChangeReadonlySchedule);
                }
            }
            else
            {
                a.State = ArticleState.Published;
                ca = new Base_CatalogArticle
                {
                    CatalogId = ScheduleEvent.Root.Id,
                };
            }
            ca.Article = a;
            ca.Article.EditorId = CurrentUserId.ToInt();
            ScheduleManager.AdjustAlertTime(ca);
            _schedule.Article.Save(ca);
            // Manager.Save(evt);
            return JsonTips("success", JStr.SuccessSaved, ca.Article.Title);
        }

        // $.newPOST(editUrl,{id:event.id,daydiff:dayDelta,minudiff:minuteDelta,allday:allDay},function(data){

        /// <summary>
        /// 接受界面的拖动产生的修改
        /// </summary>
        /// <param name="caId">日程信息ID</param>
        /// <param name="delta">拖动产生的时间差(分钟数），用于事件整个拖动</param>
        /// <param name="end">是只拖动下端产生的新时间</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Drag(int caId, int delta, DateTime? end)
        {
            Base_CatalogArticle ca = _schedule.Article.GetById(caId);

            if (ca.Article.State == ScheduleEvent.OptionReadOnly)
            {
                return JsonTips("error", SStr.CantChangeReadonlySchedule);
            }
            DateTime startTime = CommOp.ToDateTime(ca.GetExt(ScheduleEvent.Root.StartTime));
            DateTime endTime = CommOp.ToDateTime(ca.GetExt(ScheduleEvent.Root.EndTime));
            if (end.IsEmpty())
            {
                //表示是拖动整个日程，同时改变起止时间
                startTime = startTime.AddMinutes(delta);
                ca.SetExt(ScheduleEvent.Root.StartTime, startTime);
                if (endTime != default(DateTime))
                {
                    endTime = endTime.AddMinutes(delta);
                    ca.SetExt(ScheduleEvent.Root.EndTime, endTime);
                }
            }
            else
            {
                ca.SetExt(ScheduleEvent.Root.EndTime, end);
            }
            ca.Article.EditorId = CurrentUserId.ToInt();
            ca.Article.State = ArticleState.Published;
            _schedule.Article.Save(ca);
            return JsonTips();
        }

        /// <summary>
        /// 删除一项日程
        /// </summary>
        /// <param name="id">Base_Article的ID</param>
        /// <returns>结果提示</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return JsonTips();
            }
            Base_CatalogArticle ca = _schedule.Article.GetByArticleId(id, ScheduleEvent.Root.Id);

            if (ca.Article.State == ScheduleEvent.OptionReadOnly)
            {
                return JsonTips("error", SStr.CantDeleteReadonlySchedule);
            }

            //  Manager.Delete(id);
            _schedule.Article.Delete(ca.Id);
            return JsonTips();
        }

        [HttpPost]
        public ActionResult DeleteAlerts(string ids)
        {
            int[] idArr = CommOp.ToIntArray(ids, ',');
            //  Manager.Delete(id);
            _schedule.DeleteAlerts(CurrentUserId.ToInt(), idArr);
            return JsonTips("success", JStr.SuccessDeleted);
        }

        /// <summary>
        /// 获取用户的到期提醒事项Json数据
        /// </summary>
        /// <param name="pm">页面传递的分页对象</param>
        /// <returns></returns>
        public ActionResult GetAlerts(PageModel pm)
        {
            pm.PageIndex++;
            var alerts = _schedule.GetAlerts(User.Identity.GetUserId().ToInt());
            var pager = new Pager<EventModel>(alerts, pm.PageIndex, pm.PageSize);

            return Json(new
            {
                data = GetUrl(pager),
                total = pager.RecordCount,
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户的到期提醒事项Json数据,在管理页
        /// </summary>
        /// <param name="pm">页面传递的分页对象</param>
        /// <returns></returns>
        public ActionResult GetAllAlerts(PageModel pm)
        {
            pm.PageIndex++;
            var alerts = _schedule.GetAllAlerts(User.Identity.GetUserId().ToInt());
            var pager = new Pager<EventModel>(alerts, pm.PageIndex, pm.PageSize);

            return Json(new
            {
                data = GetUrl(pager),
                total = pager.RecordCount,
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取扩展菜单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExtInfos()
        {
            var extInfoService = SiteManager.Get<IMenuExtInfoService>();
            IEnumerable<MenuExtInfo> extInfos = extInfoService == null ? null : extInfoService.GetMenuExtInfos(CurrentUserId.ToInt());
            return Json(extInfos, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清空当前用户的所有消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Clear()
        {
            _schedule.ClearAlerts(CurrentUserId.ToInt());
            return JsonTips("success", SStr.AllMessageCleared);
        }

        /// <summary>
        /// 打开消息管理界面
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageManager()
        {
            return View();
        }

        /// <summary>
        /// 将所有消息设置成只读
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReadAll()
        {
            _schedule.ReadAllAlerts(CurrentUserId.ToInt());
            return JsonTips("success", SStr.AllMessageRead);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _schedule.Article.Dispose();
        }
    }
}
