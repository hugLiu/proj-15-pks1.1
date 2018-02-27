using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Text.RegularExpressions;
using Jurassic.Com.Tools;
using System.Reflection;
using System.Linq;

namespace Jurassic.Com.DB
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 通用数据访问类
    /// </summary>
    public partial class DBHelper
    {
        Dictionary<string, string> versions;
        IDBComm dBComm;
        DbTransaction _trans;
        DbConnection _transConn;
        ConnectionStringSettings _connStrSetting;

        /// <summary>
        /// 用于记录SQL日志的委托
        /// </summary>
        public Action<string> Log { get; set; }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public String ConnStr { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 数据库对象的公共接口
        /// </summary>
        public virtual IDBComm DBComm
        {
            get
            {
                if (dBComm == null)
                {
                    dBComm = CreateDBComm(_connStrSetting);
                    dBComm.Helper = this;
                }
                return dBComm;
            }
            set
            {
                dBComm = value;
                dBComm.Helper = this;
            }
        }

        IDBComm CreateDBComm(ConnectionStringSettings connStrSetting)
        {
            switch (connStrSetting.ProviderName)
            {
                case "System.Data.SqlClient": return new DBCommSql();
                //case "IBM.Data.DB2":return new DBCommDB2(); 
                case "System.Data.Oledb": return new DBCommOleDb();
                default:
                    {
                        if (connStrSetting.ConnectionString.Contains("Microsoft.Jet.OLEDB"))
                        {
                            return new DBCommOleDb();
                        }
                        throw new Exception(@"无法识别的ProviderName, 默认只支持System.Data.SqlClient,System.Data.OracleClient和System.Data.Oledb。
如果使用其他数据驱动，您需要用该驱动实现IDBComm接口，再在使用前手动初始化DBComm属性。");
                    }
            }
        }


        /// <summary>
        /// 根据连接字符串(或其在配置节中的名称)创建数据库帮助类
        /// </summary>
        /// <param name="connStrOrName">连接字符串,或在config配置文件中的名称</param>
        public DBHelper(string connStrOrName)
        {
            if (connStrOrName.Contains('=')) //表示传递过来的是完整的连接串
            {
                ConnStr = connStrOrName;
                _connStrSetting = new ConnectionStringSettings();
                _connStrSetting.ConnectionString = connStrOrName;
                _connStrSetting.ProviderName = "System.Data.SqlClient";
            }
            else //表示传递过来的是配置节中的连接串名称
            {
                _connStrSetting = ConfigurationManager.ConnectionStrings[connStrOrName];
                ConnStr = _connStrSetting.ConnectionString;
            }
        }

        /// <summary>
        /// 根据连接字符串(或其在配置节中的名称)和指定的数据接口创建数据库帮助类
        /// </summary>
        /// <param name="connStrOrName">连接字符串(或其在配置节中的名称)</param>
        /// <param name="dbComm">数据接口</param>
        public DBHelper(string connStrOrName, IDBComm dbComm)
            : this(connStrOrName)
        {
            DBComm = dbComm;
        }

        /// <summary>
        /// 根据连接字符串配置节信息创建数据库帮助类
        /// </summary>
        /// <param name="settings">连接字符串配置节信息</param>
        public DBHelper(ConnectionStringSettings settings)
        {
            _connStrSetting = settings;
            ConnStr = _connStrSetting.ConnectionString;
        }

        /// <summary>
        /// 创建一个默认的数据库帮助类
        /// </summary>
        public DBHelper()
        {
        }

        /// <summary>
        /// 获取数据库系统版本
        /// </summary>
        /// <returns>系统版本号</returns>
        public virtual String GetServerVersion()
        {
            if (versions == null)
                TestConnetion();
            return versions["ServerVersion"];
        }

        /// <summary>
        /// 获取数据库名
        /// </summary>
        /// <returns>数据库名称</returns>
        public virtual String GetDBName()
        {
            if (versions == null)
                TestConnetion();
            return versions["Database"];
        }

        /// <summary>
        /// 测试Connection
        /// </summary>
        /// <returns>测试连接成功与否</returns>
        public virtual bool TestConnetion()
        {
            try
            {
                versions = new Dictionary<string, string>();
                _transConn = DBComm.CreateConnection();
                _transConn.Open();
                versions["ServerVersion"] = _transConn.ServerVersion;
                versions["Database"] = _transConn.Database;
                versions["DataSource"] = _transConn.DataSource;
            }
            catch
            {
                return false;
            }
            finally
            {
                _transConn.Close();
            }
            return true;
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        public virtual DbTransaction BeginTrans()
        {
            _transConn = DBComm.CreateConnection();
            _transConn.Open();
            _trans = _transConn.BeginTransaction();
            return _trans;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public virtual void RollbackTrans()
        {
            _trans.Rollback();
            _transConn.Close();
        }

        /// <summary>
        /// 结束事务
        /// </summary>
        /// <returns>结束事务出错时返回的错误信息</returns>
        public virtual String EndTrans()
        {
            String s = "";
            try
            {
                _trans.Commit();
            }
            catch (DbException ex)
            {
                _trans.Rollback();
                s = ex.Message;
            }

            _transConn.Close();
            return s;
        }

        /// <summary>
        /// 用指定连接串执行非查询语句
        /// </summary>
        /// <param name="sql">非查询语句</param>
        /// <param name="sp">可选参数数组</param>
        /// <returns>影响的行数</returns>
        public virtual int ExecNonQuery(string sql, params IDataParameter[] sp)
        {
            using (DbConnection conn =  DBComm.CreateConnection())
            {
                DbCommand sc = conn.CreateCommand();
                sc.CommandText = sql;
                PrepareCommand(sc, sp);
                int r = sc.ExecuteNonQuery();
                conn.Close();
                sc.Parameters.Clear();
                return r;
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">可选参数数组</param>
        /// <returns>影响的行数</returns>
        public virtual int RunProcedure(string procname, params IDataParameter[] sp)
        {
            using (DbConnection conn = DBComm.CreateConnection())
            {
                DbCommand sc = conn.CreateCommand();
                sc.CommandText = procname;
                sc.CommandType = CommandType.StoredProcedure;
                PrepareCommand(sc, sp);
                int r = sc.ExecuteNonQuery();
                sc.Parameters.Clear();
                return r;
            }
        }

        /// <summary>
        /// 执行存储过程(返回DataSet)
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="sp">可选参数数组</param>
        /// <returns>DataSet</returns>
        public virtual DataSet RunProcedureDs(string procname, params IDataParameter[] sp)
        {
            using (DbConnection conn = DBComm.CreateConnection())
            {
                DbCommand sc = conn.CreateCommand();
                sc.CommandText = procname;
                sc.CommandType = CommandType.StoredProcedure;
                PrepareCommand(sc, sp);

                DbDataAdapter da = DBComm.CreateDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);
                sc.Parameters.Clear();
                return ds;
            }
        }

        /// <summary>
        /// 执行事务中的非查询语句
        /// </summary>
        /// <param name="sql">非查询语句</param>
        /// <param name="sp">可选参数数组</param>
        /// <returns>影响的行数</returns>
        public virtual int TransNonQuery(string sql, params IDataParameter[] sp)
        {
            DbCommand sc = _transConn.CreateCommand();
            sc.CommandText = sql;
            PrepareCommand(sc, sp);
            sc.Transaction = _trans;
            int i = sc.ExecuteNonQuery();
            sc.Parameters.Clear();
            return i;
        }

        /// <summary>
        /// 在事务中获取单个对象
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="sp">可选参数数组</param>
        /// <returns>返回的单个值</returns>
        public virtual object TransGetObject(string sql, params IDataParameter[] sp)
        {
            object o = null;
            DbCommand sc = _transConn.CreateCommand();
            sc.CommandText = sql;
            PrepareCommand(sc, sp);
            sc.Transaction = _trans;
            o = sc.ExecuteScalar();
            sc.Parameters.Clear();
            return o;
        }


        /// <summary>
        /// 得到单一对象
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sp">参数数组</param>
        /// <returns>返回的单个值</returns>
        public virtual object ExecGetObject(String sql, params IDataParameter[] sp)
        {
            object o = 0;
            using (DbConnection conn = DBComm.CreateConnection())
            {
                DbCommand sc = conn.CreateCommand();
                sc.CommandText = sql;
                PrepareCommand(sc, sp);
                o = sc.ExecuteScalar();
                sc.Parameters.Clear();
                return o;
            }
        }

        private void PrepareCommand(DbCommand sc, IDataParameter[] sp)
        {
            if (sc.Connection.State != ConnectionState.Open)
                sc.Connection.Open();

            sc.CommandTimeout = Timeout;
            if (sp != null)
            {
                foreach (IDataParameter parm in sp)
                {
                    if (parm.ParameterName.StartsWith("@"))
                    {
                        parm.ParameterName = parm.ParameterName.Substring(1);
                    }
                    sc.Parameters.Add(parm);

                    if (sc.CommandText.Contains("@" + parm.ParameterName))
                    {
                        sc.CommandText = sc.CommandText.Replace("@" + parm.ParameterName, DBComm.ParamPrefix + parm.ParameterName);
                    }
                }

                if (Log != null)
                {
                    Log(sc.CommandText);
                }
            }
        }

        /// <summary>
        /// 根据指定Sql返回一个DataReader
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sp">参数数组</param>
        /// <returns>IDataReader</returns>
        public virtual IDataReader ExecReader(string sql, params IDataParameter[] sp)
        {
            DbConnection conn = DBComm.CreateConnection();
            DbCommand sc = conn.CreateCommand();
            sc.CommandText = sql;

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(sc, sp);
                IDataReader rdr = sc.ExecuteReader(CommandBehavior.CloseConnection);
                sc.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// 执行SQL查询语句，返回DataTable
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="sp">参数数组</param>
        /// <returns>DataTable</returns>
        public virtual DataTable ExecDataTable(string sql, params IDataParameter[] sp)
        {
            using (DbConnection conn = DBComm.CreateConnection())
            {
                DbCommand sc = conn.CreateCommand();
                sc.CommandText = sql;
                PrepareCommand(sc, sp);
                DbDataAdapter da = DBComm.CreateDataAdapter(sc);
                DataTable dt = new DataTable();
                da.Fill(dt);

                sc.Parameters.Clear();
                return dt;
            }
        }

        /// <summary>
        /// 执行SQL查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="sp">参数数组</param>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecDataSet(string sql, params IDataParameter[] sp)
        {
            using (DbConnection conn = DBComm.CreateConnection())
            {
                DbCommand sc = conn.CreateCommand();
                sc.CommandText = sql;
                PrepareCommand(sc, sp);
                DbDataAdapter da = DBComm.CreateDataAdapter(sc);
                DataSet ds = new DataSet();
                da.Fill(ds);
                sc.Parameters.Clear();
                return ds;
            }
        }

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数对象</returns>
        public virtual IDataParameter CreateParameter(string parameterName, object value)
        {
            return DBComm.CreateParameter(parameterName, CommOp.TestNull(value));
        }

        /// <summary>
        /// 获取数据库大小
        /// </summary>
        /// <returns>数据库大小</returns>
        public virtual double GetDBSize()
        {
            return DBComm.GetDBSize();
        }

        /// <summary>
        /// 压缩数据库
        /// </summary>
        public virtual void ShrinkDB()
        {
            DBComm.ShrinkDB();
        }

        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="ps">可选参数数组</param>
        /// <returns></returns>
        public virtual IDataReader ExecPageReader(DBPagerInfo pager, params IDataParameter[] ps)
        {
            return DBComm.ExecPageReader(pager, ps);
        }

        /// <summary>
        /// 导入DataTable到数据库
        /// </summary>
        /// <param name="dt">内存中的数据表</param>
        /// <param name="tableName">表名,如果为空，则以传入的DataTable的TableName作为表名</param>
        /// <param name="buckCopy">是否使用批量导入, 对应的DBComm必须实现ISupportBuckCopy的接口</param>
        /// <param name="notifyAfter">发生提示时导入的行数</param>
        /// <param name="onRowsCopied">发生提示时执行的委托</param>
        /// <returns>成功导入的行数</returns>
        public virtual int Import(DataTable dt, string tableName = null, bool buckCopy = true, int notifyAfter = 10, Action<int> onRowsCopied = null)
        {
            if (dt == null)
            {
                throw new ArgumentNullException("dt");
            }
            if (onRowsCopied == null) onRowsCopied = r => { };

            if (tableName.IsEmpty())
            {
                tableName = dt.TableName;
            }
            if (notifyAfter <= 0)
            {
                throw new ArgumentException("notifyAfter<=0");
            }

            int rowsCopied = 0;
            int rowCount = dt.Rows.Count;

            //如果目标表不存在则创建
            if (!DBComm.TableExists(tableName))
            {
                DBComm.CreateTable(dt, tableName);
            }

            //用bcp导入数据
            if (buckCopy && DBComm is ISupportBuckCopy)
            {
                return ((ISupportBuckCopy)DBComm).BuckCopy(dt, tableName, notifyAfter, onRowsCopied);
            }
            else //用Sql Insert 导入数据
            {
                BeginTrans();
                string sqlFields = "";
                string sqlValues = "";
                foreach (DataColumn f in dt.Columns)
                {
                    if (!f.AutoIncrement)
                    {
                        sqlFields += String.Format(",{1}{0}{2}", f.ColumnName, DBComm.FieldPrefix, DBComm.FieldSuffix);
                        sqlValues += String.Format(",@{0}", f.ColumnName);
                    }
                }
                sqlFields = sqlFields.Substring(1);
                sqlValues = sqlValues.Substring(1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int j = 0;

                    IDataParameter[] sp = new IDataParameter[dt.Columns.Count];
                    foreach (DataColumn f in dt.Columns)
                    {
                        if (!f.AutoIncrement)
                        {
                            IDataParameter p = CreateParameter(f.ColumnName, CommOp.TestNull(dt.Rows[i][j]));
                            sp[j] = p;
                        }
                        j++;
                    }
                    string sql = String.Format("INSERT INTO {3}{0}{4}({1}) VALUES({2})", tableName, sqlFields, sqlValues, DBComm.FieldPrefix, DBComm.FieldSuffix);
                    try
                    {
                        TransNonQuery(sql, sp);
                    }
                    catch (Exception ex)
                    {
                        RollbackTrans();
                        throw new TableImportException(ex, i + 1, 0);
                    }
                    if (i % notifyAfter == 0 || i == rowCount)
                        onRowsCopied(i);
                }
                EndTrans();
            }
            rowsCopied = dt.Rows.Count;
            return rowsCopied;
        }

        /// <summary>
        /// 通过执行sql语句返回一个泛型T的列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">查询语句</param>
        /// <param name="sp">参数列表</param>
        /// <returns>T的泛型列表</returns>
        public virtual List<T> GetList<T>(string sql, params IDataParameter[] sp)
            where T : new()
        {
            using (IDataReader reader = ExecReader(sql, sp))
            {
                DataTable schemaTable = reader.GetSchemaTable();
                PropertyInfo[] infos = typeof(T).GetProperties();
                var readerCols = schemaTable.Rows.Cast<DataRow>().Select(dr => dr["ColumnName"].ToString().ToLower());
                List<T> listT = new List<T>();
                while (reader.Read())
                {
                    T t = new T();
                    foreach (PropertyInfo info in infos)
                    {
                        if (readerCols.Contains(info.Name.ToLower()))
                        {
                            SetValue(t, info, reader[info.Name]);
                        }
                    }
                    listT.Add(t);
                }

                return listT;
            }
        }

        /// <summary>
        /// 将value中的值赋给对象的属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="pi">对象的属性信息</param>
        /// <param name="value">值</param>
        static void SetValue(object obj, PropertyInfo pi, object value)
        {
            pi.SetValue(obj, DataHelper.HackType(value, pi.PropertyType), null);
        }
    }
}
