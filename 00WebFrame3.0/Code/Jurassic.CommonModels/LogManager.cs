using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.Com.DB;

namespace Jurassic.CommonModels
{
    /// <summary>
    /// 日志管理类，该类应该通过依赖注入来初始化
    /// </summary>
    /// <example>
    /// LogManager _logManager = SiteManager.Kernel.Get&lt;LogManager&gt;();
    /// </example>
    public class LogManager
    {
        private DBHelper _dbHelper;
        public LogManager(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public IQueryable<JLogInfo> GetQuery()
        {
            return SiteManager.Get<IAuditDataService<JLogInfo>>().GetQuery();
        }

        /// <summary>
        /// 日志的分页查询
        /// </summary>
        /// <param name="page">分页号，从1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="key">查询关键字</param>
        /// <param name="sortExpression">排序表达式，如'ID DESC'</param>
        /// <returns></returns>
        public virtual Pager<JLogInfo> GetPage(int page, int pageSize, string key, string sortExpression = null)
        {
            var dataProivder = SiteManager.Get<IAuditDataService<JLogInfo>>();
            if (key.IsEmpty())
                return dataProivder.PageQuery(page, pageSize, null, sortExpression);
            else
                return dataProivder.PageQuery((log) => log.ActionName.Contains(key)
                || log.Message.Contains(key) || log.ModuleName.Contains(key)
                || log.Request.Contains(key) || log.UserName.Contains(key)
                || log.Browser.Contains(key), page, pageSize, sortExpression);
        }

        /// <summary>
        /// 清除所有日志
        /// </summary>
        public virtual void Clear()
        {
            _dbHelper.ExecNonQuery("TRUNCATE TABLE SYS_LOG");
        }

        /// <summary>
        /// 批量删除指定ID的日志记录
        /// </summary>
        /// <param name="idArr"></param>
        public virtual void DeleteByKeys(int[] idArr)
        {
            SiteManager.Get<IAuditDataService<JLogInfo>>().DeleteByKeys(idArr);
        }

        /// <summary>
        /// 查询指定用户ID的登录历史记录,按ID排降序
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Pager<JLogInfo> GetUserLoginLogs(string userName, int page, int pageSize)
        {
            return SiteManager.Get<IAuditDataService<JLogInfo>>().PageQuery(log => log.ActionName == "Login"
            && log.ModuleName == "Account" && log.UserName.Equals(userName), page, pageSize, "ID DESC");
        }
    }
}
