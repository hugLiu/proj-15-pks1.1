using Jurassic.AppCenter.Logs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Jurassic.Com.Tools;
using System.Collections;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2015-04-12, 2014-04-12</remarks>
    /// <summary>
    /// 基于EF的数据访问接口实现
    /// </summary>
    /// <typeparam name="T">DTO数据实体类型</typeparam>
    public class EFAuditDataService<T> : IAuditDataService<T>, IDisposable
        where T : class
    {
        protected DbContext _context;

        public JLogInfo LogInfo { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public EFAuditDataService(DbContext context)
        {
            _context = context;
            if (LogInfo == null)
                LogInfo = new JLogInfo();
            OnLog = sql =>
            {
                var lvl = LogInfo.LogType;
                LogInfo.LogType = JLogType.Debug.ToString();
                LogInfo.Message = sql;
                LogInfo.OpTime = DateTime.Now;
                LogHelper.Write(LogInfo);
                LogInfo.LogType = lvl;
            };
        }

        DbContextTransaction _trans;

        /// <summary>
        /// 开启事务
        /// </summary>
        public virtual void BeginTrans()
        {
            _trans = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// 事务提交
        /// 判断:_trans对象不为空前提下
        /// </summary>
        public virtual void EndTrans()
        {
            try
            {
                if (_trans != null)
                {
                    _context.SaveChanges();
                    _trans.Commit();
                    _trans.Dispose();
                    _trans = null;
                }
            }
            catch
            {
                RollbackTrans();
                throw;
            }
        }

        /// <summary>
        /// 事务回滚
        /// 判断:_trans对象不为空前提下
        /// </summary>
        public virtual void RollbackTrans()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans.Dispose();
                _trans = null;
            }
        }

        /// <summary>
        /// 标记一个对象的增删改状态
        /// </summary>
        /// <param name="t"></param>
        /// <param name="state"></param>
        public void MarkState(object t, EntityState state)
        {
            _context.Entry(t).State = state;
        }

        public virtual IQueryable<T> GetQuery()
        {
            return _context.Set<T>();
        }

        public virtual Pager<T> PageQuery(Expression<Func<T, bool>> whereExpression, int page, int pageSize, string sortExpression)
        {

            return new Pager<T>(
               whereExpression == null ? GetQuery() : GetQuery().Where(whereExpression),
               sortExpression, page, pageSize);
        }

        public virtual IEnumerable<T> GetData()
        {
            return _context.Set<T>().ToList();
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Add(T t)
        {
            _context.Entry<T>(t).State = EntityState.Added;
            return ExecComm();
        }

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Change(T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            return ExecComm();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int Delete(T t)
        {
            _context.Entry(t).State = EntityState.Deleted;
            return ExecComm();
        }

        /// <remarks>zjf, 2016-01-15</remarks>
        /// <summary>
        /// 执行增删改
        /// 如在使用事务的情况下出现异常会自动进行回滚 
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        int ExecComm()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                RollbackTrans();
                throw;
            }
        }

        public virtual Action<string> OnLog
        {
            get
            {
                return _context.Database.Log;
            }
            set
            {
                _context.Database.Log = value;
            }
        }

        public virtual T GetByKey(object key)
        {
            return _context.Set<T>().Find(key);
        }

        public virtual int DeleteByKeys(IEnumerable keys)
        {
            foreach (object key in keys)
            {
                T t = GetByKey(key);
                _context.Set<T>().Remove(t);
            }
            return _context.SaveChanges();
        }

        public virtual Pager<T> PageQuery(int page, int pageSize, string whereString, string sortExpression)
        {
            if (whereString.IsEmpty())
            {
                return PageQuery(null, page, pageSize, sortExpression);
            }
            else
            {
                return new Pager<T>(GetQuery().Where(whereString), sortExpression, page, pageSize);
            }
        }

        public void Dispose()
        {
            if (_trans != null)
            {
                _trans.Rollback();
            }
            _context.Dispose();
        }


        public virtual int Add(IEnumerable<T> ts)
        {
            if (!ts.IsEmpty())
            {
                foreach (var item in ts)
                    _context.Entry<T>(item).State = EntityState.Added;
                return ExecComm();
            }
            else
                return 0;
        }

        public virtual int Change(IEnumerable<T> ts)
        {
            if (!ts.IsEmpty())
            {
                foreach (var item in ts)
                    _context.Entry<T>(item).State = EntityState.Modified;
                return ExecComm();
            }
            else
                return 0;
        }

        public DbContext GetContext()
        {
            return  _context;
        }
    }
}
