using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace Jurassic.Com.DB
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// DbHelper调用的产生数据库对象的公共接口
    /// </summary>
    public interface IDBComm
    {
        DBHelper Helper { get; set; }

        /// <summary>
        /// 产生数据库连接
        /// </summary>
        /// <returns></returns>
        DbConnection CreateConnection();

        /// <summary>
        /// 产生DataAdapter
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        DbDataAdapter CreateDataAdapter(DbCommand command);

        /// <summary>
        /// 产生DbCommandBuilder
        /// </summary>
        /// <param name="sda"></param>
        /// <returns></returns>
        DbCommandBuilder CreateCommandBuilder(DbDataAdapter sda);

        /// <summary>
        /// 产生参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IDataParameter CreateParameter(string parameterName, object value);

        /// <summary>
        /// 执行分页查询获取一个DataReader
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="sp">参数列表</param>
        /// <returns>分页的DataReader</returns>
        IDataReader ExecPageReader(DBPagerInfo pager, params IDataParameter[] sp);

        /// <summary>
        /// 获取数据库大小
        /// </summary>
        /// <returns></returns>
        double GetDBSize();

        /// <summary>
        /// 压缩数据库
        /// </summary>
        void ShrinkDB();

        /// <summary>
        /// 根据DataTable的架构生成一个表
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="tableName">要生成的表名</param>
        void CreateTable(DataTable dt, string tableName);

        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool TableExists(string tableName);

        /// <summary>
        /// 参数前缀，比如sqlserver是@，oracle是：
        /// </summary>
        string ParamPrefix { get; }

        /// <summary>
        /// 表示字段名开头的符号,sqlserver是‘[’,oracle是‘"’
        /// </summary>
        string FieldPrefix { get; }

        /// <summary>
        /// 表示字段名结束的符号,sqlserver是‘]’,oracle是‘"’
        /// </summary>
        string FieldSuffix { get; }
    }
}

