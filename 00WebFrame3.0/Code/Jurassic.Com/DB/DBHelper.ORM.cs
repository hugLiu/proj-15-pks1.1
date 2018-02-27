using Jurassic.Com.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jurassic.Com.DB
{
    /// <summary>
    /// 此处主要放置ORM相关的逻辑
    /// </summary>
    partial class DBHelper
    {        
        #region  T

        /// <summary>
        /// 根据唯一ID获取对象,返回实体，实体为数据表
        /// </summary>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(string pkName, string pkVal) where T:new()
        {
            if (string.IsNullOrEmpty(pkVal))
            {
                return default(T);
            }

            T model = new T();
            Type type = model.GetType();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ").Append(type.Name).Append(" Where ").Append(pkName).Append("=@ID");
            List<IDataParameter> list = new List<IDataParameter>();
            DataTable dt = this.ExecDataTable(sb.ToString(), CreateParameter("ID", pkVal));
            if (dt.Rows.Count > 0)
            {
                return DataHelper.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }
        /// <summary>
        /// 根据查询参数获取对象,返回实体,实体为数据表
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(Hashtable ht)
        {

            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + AddFix(type.Name) + " WHERE 1=1");
            foreach (string key in ht.Keys)
            {
                strSql.Append(" AND " + AddFix(key) + " =@" + key + "");
            }
            DataTable dt = this.ExecDataTable(strSql.ToString(), this.GetParameter(ht).ToArray());
            if (dt.Rows.Count > 0)
            {
                return DataHelper.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }

        /// <summary>
        /// 根据查询条件获取对象,返回实体，实体为数据表
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="param">参数化</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(StringBuilder where, params IDataParameter[] param )
        {

            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " +AddFix(type.Name) + " WHERE 1=1");
            strSql.Append(where);
            DataTable dt = this.ExecDataTable(strSql.ToString(), param);
            if (dt.Rows.Count > 0)
            {
                return DataHelper.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }

        /// <summary>
        /// 根据查询条件获取对象,返回实体，实体可为业务Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T GetModel<T>(string sql, params IDataParameter[] param)
        {

            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            DataTable dt = this.ExecDataTable(sql, param);
            if (dt.Rows.Count > 0)
            {
                return DataHelper.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }
        #endregion

        #region 对象参数转换SqlParam
        /// <summary>
        /// Hashtable对象参数转换,跟进Hashtable获得List
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IEnumerable<IDataParameter> GetParameter(Hashtable ht)
        {
            foreach (string key in ht.Keys)
            {
                yield return CreateParameter(key, ht[key]);
            }
        }

        /// <summary>
        /// 实体类对象参数转换
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<IDataParameter> GetParameter<T>(T entity)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            List<IDataParameter> list = new List<IDataParameter>();
            foreach (PropertyInfo prop in props)
            {
                    list.Add(CreateParameter(prop.Name, prop.GetValue(entity, null)));
            }
            return list;
        }

        /// <summary>
        /// 字符串转换为参数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<IDataParameter> GetParameter(string text, string value)
        {
            List<IDataParameter> list = new List<IDataParameter>();
            list.Add(CreateParameter(text, value));
            return list;
        }

        #endregion

        #region 拼接 查询
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string PrepareQuerySqlString<T>(T entity)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" select * from  ");
            sb.Append(type.Name);
            sb.Append("where ");
            StringBuilder sp = new StringBuilder();

            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    sp.Append("," + prop.Name + "=@" + prop.Name);
                }
            }

            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb.ToString();
        }

        #endregion
        #region 拼接 新增 SQL语句
        /// <summary>
        /// 哈希表生成InsertSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public string PrepareInsertSqlString(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    sb_prame.Append("," + AddFix(key));
                    sp.Append("," + DBComm.ParamPrefix + key);
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb.ToString();
        }
        /// <summary>
        /// 泛型方法，反射生成InsertSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>int</returns>
        public string PrepareInsertSqlString<T>(T entity)
        {

            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(type.Name);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    sb_prame.Append("," + AddFix(prop.Name));
                    sp.Append("," + DBComm.ParamPrefix + prop.Name);
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb.ToString();
        }
        #endregion

        #region 拼接 修改 SQL语句
        /// <summary>
        /// 哈希表生成UpdateSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">主键</param>
        /// <param name="ht">Hashtable</param>
        /// <returns></returns>
        public string PrepareUpdateSqlString(string tableName, string pkName, Hashtable ht)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(key);
                        sb.Append("=");
                        sb.Append(DBComm.ParamPrefix + key);
                    }
                    else
                    {
                        sb.Append("," + key);
                        sb.Append("=");
                        sb.Append(DBComm.ParamPrefix + key);
                    }
                }
            }
            sb.Append(" Where ").Append(AddFix(pkName)).Append("=").Append(DBComm.ParamPrefix + pkName);
            return sb.ToString();
        }

        /// <summary>
        /// 泛型方法，反射生成UpdateSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="pkName">主键</param>
        /// <returns>int</returns>
        public string PrepareUpdateSqlString<T>(T entity, string pkName)
        {

            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(prop.Name);
                        sb.Append("=");
                        sb.Append(DBComm.ParamPrefix + prop.Name);
                    }
                    else
                    {
                        sb.Append("," + prop.Name);
                        sb.Append("=");
                        sb.Append(DBComm.ParamPrefix + prop.Name);
                    }
                }
            }
            sb.Append(" Where ").Append(AddFix(pkName)).Append("=").Append(DBComm.ParamPrefix + pkName);

            return sb.ToString();
        }
        #endregion

        #region 拼接 删除 SQL语句
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="pkName"></param>
        /// <returns></returns>
        public string PrepareDeleteSqlString<T>(string tableName, string pkName)
        {

            StringBuilder sb = new StringBuilder("Delete From " + AddFix(tableName) + " Where " + AddFix(pkName) + " = @" + pkName + "");
            return sb.ToString();
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <returns></returns>
        public string PrepareDeleteSqlString(string tableName, string pkName)
        {

            StringBuilder sb = new StringBuilder("Delete From " + AddFix(tableName) + " Where " + AddFix(pkName) + " = @" + pkName + "");
            return sb.ToString();
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">多参数</param>
        /// <returns></returns>
        public string PrepareDeleteSqlString(string tableName, Hashtable ht)
        {

            StringBuilder sb = new StringBuilder("Delete From " + tableName + " Where 1=1");
            foreach (string key in ht.Keys)
            {
                sb.Append(" AND " + AddFix(key) + " =@" + key + "");
            }
            return sb.ToString();
        }
        #endregion

        string AddFix(string fieldName)
        {
            if (fieldName.StartsWith(DBComm.FieldPrefix))
            {
                return fieldName;
            }
            return DBComm.FieldPrefix + fieldName + DBComm.FieldSuffix;
        }
    }
}
