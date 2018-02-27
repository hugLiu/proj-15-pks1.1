using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using System.Diagnostics;
using System.Collections;
using EntityFramework.Extensions;

namespace Jurassic.CommonModels.Schedule
{
    /// <summary>
    /// 日程表和消息提醒的数据管理类
    /// </summary>
    public class ScheduleManager
    {
        ArticleManager _article;
        public ScheduleManager(ArticleManager article)
        {
            _article = article;
        }

        public ArticleManager Article
        {
            get { return _article; }
        }

        /// <summary>
        /// 获取用户所有落在某个时间段的日程表事件，只要是起始时间或结束时间在指定范围都算
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public IEnumerable<EventModel> GetEvents(int userId, DateTime startTime, DateTime endTime)
        {
            //转为yyyy-MM-dd HH:mm:ss标准格式以便在数据库中以字符串数据规则进行比对
            string starts = CommOp.ToTimeStr(startTime);
            string ends = CommOp.ToTimeStr(endTime);
            int startTimeId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.StartTime);
            int endTimeId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.EndTime);
            int alertBeforeId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.AlertBefore);
            int urlId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.ProcessUrl);

            return _article.GetAllInCatalog(ScheduleEvent.Root.Id)
                           .Where(ca => ca.Article.EditorId == userId
                               //不是属于通知
                               && ca.Article.Options < ScheduleEvent.OptionNotice)
                    .Select(ca => new EventModel
                    {
                        start = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == startTimeId).Value,
                        end = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == endTimeId).Value,
                        caId = ca.Id,
                        title = ca.Article.Title,
                        alertBefore = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == alertBeforeId).Value,
                        editable = ca.Article.Options == ScheduleEvent.OptionReadOnly,
                        finished = ca.Article.State == ScheduleEvent.EventFinished,
                        url = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == urlId).Value,
                    })
                    .Where(r => (r.start != null && starts.CompareTo(r.start) <= 0 && ends.CompareTo(r.start) >= 0)
                    || (r.end != null && starts.CompareTo(r.end) <= 0 && ends.CompareTo(r.end) >= 0))
                    .ToList();
        }

        /// <summary>
        /// 生成循环事件
        /// </summary>
        public void CreateLoopEvents()
        {
            int loopExtId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.LoopType);
            var loops = SiteManager.Get<ArticleManager>().GetAllInCatalog(ScheduleEvent.Root.Id)
                  .Where(ca => (ca.Article.Exts.FirstOrDefault(ext => (ext.CatlogExtId == loopExtId)).Value != "0"));
        }

        /// <summary>
        /// 获取指定用户的当前时间点需要提醒的项
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IQueryable<EventModel> GetAlerts(int userId)
        {
            var now1 = DateTime.Now.ToTimeStr();
            var query = InnerGetAllAlerts(userId)
                .Where(r => r.alertTime != null
                && now1.CompareTo(r.alertTime) >= 0
                && (r.read == false))
                  .OrderByDescending(r => r.caId);

            return query;
        }

        /// <summary>
        /// 清空指定用户的消息 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ClearAlerts(int userId)
        {
            int alertTimeId = SiteManager.Catalog.GetExtByName(ScheduleEvent.Root.Id, ScheduleEvent.Root.AlertTime).Id;
            var query = _article.GetAllAtCatalog(ScheduleEvent.Root.Id)
                .Where(ca => ca.Article.Options == ScheduleEvent.OptionNotice && ca.Article.EditorId == userId);
            return query.Delete();
        }

        /// <summary>
        /// 将所有消息设置成已读
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ReadAllAlerts(int userId)
        {
            var now1 = DateTime.Now.ToTimeStr();
            int alertTimeId = SiteManager.Catalog.GetExtByName(ScheduleEvent.Root.Id, ScheduleEvent.Root.AlertTime).Id;
            var query = _article.GetAllAtCatalog(ScheduleEvent.Root.Id)
                .Where(ca => (ca.Article.State == 0
                && ca.Article.EditorId == userId && ca.Article.Exts.Any(ext => ext.CatlogExtId == alertTimeId)));
            return query.Select(ca => ca.Article).Update(a => new Base_Article { State = ScheduleEvent.EventRead });
        }

        /// <summary>
        /// 获取当前时间点需要提醒的所有用户ID
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetUserIdsToAlert()
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            var now1 = DateTime.Now.ToTimeStr();
            int alertTimeId = SiteManager.Catalog.GetExtByName(ScheduleEvent.Root.Id, ScheduleEvent.Root.AlertTime).Id;
            var list = _article.GetAllAtCatalog(ScheduleEvent.Root.Id)
                .Where(ca => (ca.Article.State == 0))
                .Select(ca => new
                {
                    AlertToId = ca.Article.EditorId,
                    AlertTime = ca.Article.Exts.FirstOrDefault(e => e.CatlogExtId == alertTimeId && e.ArticleId == ca.Article.Id).Value,
                })
                .Where(a => now1.CompareTo(a.AlertTime) >= 0)
                .Select(a => a.AlertToId)
                .Distinct()
                .ToArray();

            //筛选当前时间小于事件发生时间的记录
            //sw.Stop();
            //Console.WriteLine("GetUserIdsToAlert Elapsed=" + sw.ElapsedMilliseconds);
            return list; // 由于主要是后台处理程序自动调用，所以不需要考虑分页

            //return new List<int>{1,2,3};
        }

        /// <summary>
        /// 根据用户设置的提醒分钟数,设定事件的提醒时间
        /// </summary>
        /// <param name="ca"></param>
        public static void AdjustAlertTime(Base_CatalogArticle ca)
        {
            var startTime = ca.GetExtDateTime(ScheduleEvent.Root.StartTime);
            var alertBefore = ca.GetExtStr(ScheduleEvent.Root.AlertBefore);
            if (!alertBefore.IsEmpty())
                ca.SetExt(ScheduleEvent.Root.AlertTime, startTime.AddMinutes(-CommOp.ToInt(alertBefore)));
        }

        /// <summary>
        /// 删除指定的消息 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="idArr"></param>
        /// <returns></returns>
        public int DeleteAlerts(int userId, int[] idArr)
        {
            var query = _article.GetAllAtCatalog(ScheduleEvent.Root.Id)
                .Where(ca => ca.Article.EditorId == userId);
            return query.Where(ca => idArr.Contains(ca.Id)).Delete();
        }

        /// <summary>
        /// 获取用户的所有消息提醒事项用于管理
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IQueryable<EventModel> GetAllAlerts(int userId)
        {
            return InnerGetAllAlerts(userId).OrderByDescending(e => e.caId);
        }

        IQueryable<EventModel> InnerGetAllAlerts(int userId)
        {
            var now1 = DateTime.Now.ToTimeStr();
            int alertTimeId = SiteManager.Catalog.GetExtByName(ScheduleEvent.Root.Id, ScheduleEvent.Root.AlertTime).Id;
            int startTimeId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.StartTime);
            int endTimeId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.EndTime);
            int alertBeforeId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.AlertBefore);
            int urlId = SiteManager.Catalog.GetExtId(ScheduleEvent.Root.Id, ScheduleEvent.Root.ProcessUrl);
            var query = _article.GetAllAtCatalog(ScheduleEvent.Root.Id)
                .Where(ca => ca.Article.EditorId == userId && ca.Article.Exts.Any(ext => ext.CatlogExtId == alertTimeId))
                .Select(ca => new EventModel
                {
                    start = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == startTimeId).Value,
                    end = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == endTimeId).Value,
                    caId = ca.Id,
                    title = ca.Article.Title,
                    alertBefore = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == alertBeforeId).Value,
                    alertTime = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == alertTimeId).Value,
                    editable = ca.Article.Options == ScheduleEvent.OptionReadOnly,
                    finished = ca.Article.State == ScheduleEvent.EventFinished,
                    url = ca.Article.Exts.FirstOrDefault(ext => ext.CatlogExtId == urlId).Value,
                    read = ca.Article.State > 0
                });

            return query;
        }
    }
}
