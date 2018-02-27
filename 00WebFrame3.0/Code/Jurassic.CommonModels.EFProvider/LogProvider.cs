using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EFProvider
{
    /// <summary>
    /// 日志的数据访问提供类
    /// </summary>
    public class LogProvider : EFAuditDataService<JLogInfo>
    {
        ModelContext mc;
        public LogProvider(ModelContext context)
            : base(context)
        {
            this.mc = context;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //public override Pager<JLogInfo> PageQuery(int page, int pageSize)
        //{
        //    return new Pager<JLogInfo>(mc.Set<JLogInfo>()
        //        .OrderByDescending(log => log.Id),
        //        page, pageSize);
        //}

        //public override Pager<JLogInfo> PageQuery(System.Linq.Expressions.Expression<Func<JLogInfo, bool>> whereExpression, int page, int pageSize)
        //{
        //    return new Pager<JLogInfo>(mc.Set<JLogInfo>()
        //        .Where(whereExpression)
        //        .OrderByDescending(log => log.Id), page, pageSize);
        //}
    }
}
