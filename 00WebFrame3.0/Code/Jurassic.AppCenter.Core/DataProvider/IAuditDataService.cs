using Jurassic.AppCenter.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2015-04-12, 2014-04-12</remarks>
    /// <summary>
    /// 通用的数据服务提供实现接口，
    /// 应用系统通过实现此接口来实现自己的基础数据存取、事务处理，
    /// 并记录访问日志
    /// </summary>
    public interface IAuditDataService<T> : ILinqDataProvider<T>, IDataBatchCUD<T>, IDisposable
        where T : class
    {
        /// <summary>
        /// 开始一个事务
        /// </summary>
        void BeginTrans();

        /// <summary>
        /// 完成当前事务
        /// </summary>
        void EndTrans();

        /// <summary>
        /// 回滚当前事务
        /// </summary>
        void RollbackTrans();

        /// <summary>
        /// 写日志的委托
        /// </summary>
        Action<string> OnLog { get; set; }

        /// <summary>
        /// 用于跟踪的日志对象
        /// </summary>
        JLogInfo LogInfo { get; set; }
    }
}
