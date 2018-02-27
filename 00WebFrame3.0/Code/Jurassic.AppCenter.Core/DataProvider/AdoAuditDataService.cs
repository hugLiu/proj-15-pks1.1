using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using Jurassic.Com.DB;
using Jurassic.Com.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 基于ADO.net的数据访问接口实现
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class AdoAuditDataService<T> : IAuditDataService<T>
        where T : class,new()
    {
        protected DBHelper DBHelper;

        public JLogInfo LogInfo { get; set; }

        /// <summary>
        /// 根据DBHelper创建一个
        /// </summary>
        /// <param name="helper"></param>
        public AdoAuditDataService(DBHelper helper)
        {
            DBHelper = helper;
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

        public virtual Pager<T> PageQuery(int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetData()
        {
            throw new NotImplementedException();
        }

        public virtual int Add(T t)
        {
            string sqlString = this.DBHelper.PrepareInsertSqlString(t);
            List<IDataParameter> list = this.DBHelper.GetParameter(t);
            int count = _intrans ?
                this.DBHelper.TransNonQuery(sqlString, list.ToArray()) :
                this.DBHelper.ExecNonQuery(sqlString, list.ToArray());

            return count;
        }

        public virtual int Change(T t)
        {
            string key = ModelHelper.GetKeyField(typeof(T));

            string sqlString = this.DBHelper.PrepareUpdateSqlString(t, key);
            List<IDataParameter> list = this.DBHelper.GetParameter(t);
            int count = _intrans ?
                this.DBHelper.TransNonQuery(sqlString, list.ToArray()) :
                this.DBHelper.ExecNonQuery(sqlString, list.ToArray());
            return count;
        }

        public virtual int Delete(T t)
        {
            string key = ModelHelper.GetKeyField(typeof(T));
            object keyValue = ModelHelper.GetKeyFieldValue(t);

            string sqlString = this.DBHelper.PrepareDeleteSqlString(typeof(T).Name, key);
            int count = _intrans ?
                this.DBHelper.TransNonQuery(sqlString, DBHelper.CreateParameter(key, keyValue)) :
               this.DBHelper.ExecNonQuery(sqlString, DBHelper.CreateParameter(key, keyValue));
            return count;
        }


        public virtual Action<string> OnLog
        {
            get
            {
                return DBHelper.Log;
            }
            set
            {
                DBHelper.Log = value;
            }
        }

        public virtual IEnumerable<T> GetData(string whereString)
        {
            throw new NotImplementedException();
        }

        public virtual T GetByKey(object key)
        {
            string keyName = ModelHelper.GetKeyField(typeof(T));
            return this.DBHelper.GetModelById<T>(keyName, CommOp.ToStr(key));
        }

        public virtual int DeleteByKeys(IEnumerable keys)
        {
            string key = ModelHelper.GetKeyField(typeof(T));

            string sqlString = this.DBHelper.PrepareDeleteSqlString(typeof(T).Name, key);
            IDataParameter p = DBHelper.CreateParameter(key, null);
            int count = 0;
            foreach (var k in keys)
            {
                p.Value = k;
                count += (_intrans ? this.DBHelper.TransNonQuery(sqlString, p) :
                    this.DBHelper.ExecNonQuery(sqlString, p));
            }
            return count;
        }


        public IQueryable<T> GetQuery()
        {
            throw new NotImplementedException();
        }
 
        public virtual Pager<T> PageQuery(Expression<Func<T, bool>> whereExpression, int page, int pageSize, string sortExpression)
        {
            throw new NotImplementedException();
        }

        bool _intrans = false;
        public void BeginTrans()
        {
            _intrans = true;
            DBHelper.BeginTrans();
        }

        public void EndTrans()
        {
            try
            {
                DBHelper.EndTrans();
                _intrans = false;
            }
            catch
            {
                DBHelper.RollbackTrans();
                _intrans = false;
                throw;
            }
        }

        public void RollbackTrans()
        {
            DBHelper.RollbackTrans();
            _intrans = false;
        }

        public virtual Pager<T> PageQuery(int page, int pageSize, string whereString, string sortExpression)
        {
            throw new NotImplementedException();
        }


        public int Add(IEnumerable<T> ts)
        {
            throw new NotImplementedException();
        }

        public int Change(IEnumerable<T> ts)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
