using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Jurassic.GeoFeature.DBUtility;
namespace Jurassic.GeoFeature.DBUtility
{
    /// <summary>
    /// SqlServer数据操作公共类
    /// </summary>
    public class SqlServerDBHelper
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnStr
        {
            get
            {
                return SqlConnString.ReturnConnString();
            }
        }

        private static SqlConnection _conn;
        /// <summary>
        /// SqlConnection
        /// </summary>
        public static SqlConnection Conn
        {
            get
            {
                SqlConnection conn = new SqlConnection(ConnStr);
                conn.Open();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                return conn;
            }
            set { _conn = value; }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void ConnClose()
        {
            if (Conn.State != ConnectionState.Closed)
                Conn.Close();
        }
        /// <summary>
        /// 执行带参数的PL-SQL语句
        /// </summary>
        /// <param name="p_strSQL">PL-SQL语句</param>
        /// <param name="p_parmOracleCmdParms">SQL语句的OracleParameter</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSql(string p_strSQL, params SqlParameter[] p_parmOracleCmdParms)
        {
            try
            {
                //定义OracleCommand
                SqlCommand cmdOracleCommand = new SqlCommand();

                //通过using语句创建数据库连接,使用后自动释放资源

                using (SqlConnection conOracleConnection = new SqlConnection(ConnStr))
                {
                    //调用OracleCommand预处理函数

                    PrepareCommand(cmdOracleCommand, conOracleConnection, null, CommandType.Text, p_strSQL, p_parmOracleCmdParms);

                    //执行SQL语句并记录受影响的行数

                    int intResult = cmdOracleCommand.ExecuteNonQuery();

                    //清空参数集合
                    cmdOracleCommand.Parameters.Clear();

                    return intResult;
                }
            }
            catch
            {
                //异常处理
                throw;
            }
        }
        /// <summary>  
        /// 执行多条SQL语句，实现数据库事务。  
        /// </summary>  
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>  
        public static bool ExecuteSql(List<SQLEntity> ListSqlEntity)
        {
            //创建数据库连接
            SqlConnection conOracleConnection = new SqlConnection(ConnStr);

            //定义OracleCommand
            SqlCommand cmdOracleCommand = new SqlCommand();

            //判断数据库连接是否打开,未打开则立刻打开
            if (conOracleConnection.State != ConnectionState.Open)
            {
                conOracleConnection.Open();
            }
            //创建Oracle事务
            SqlTransaction tranOracleTransaction = conOracleConnection.BeginTransaction();
            try
            {
                foreach (SQLEntity myDE in ListSqlEntity)//循环哈希表（本例中 即，循环执行添加在哈希表中的sql语句  
                {
                    string cmdText = myDE.Sqlstr;//获取键值（本例中 即，sql语句）  
                    SqlParameter[] cmdParms = (SqlParameter[])myDE.Sqlparameter;//获取键值（本例中 即，sql语句对应的参数）  
                    PrepareCommand(cmdOracleCommand, conOracleConnection, tranOracleTransaction, CommandType.Text, cmdText, cmdParms != null ? cmdParms : null
                        ); //调用PrepareCommand()函数，添加参数  
                    //调用增删改函数ExcuteNoQuery()，执行哈希表中添加的sql语句  
                    cmdOracleCommand.ExecuteNonQuery();
                    cmdOracleCommand.Parameters.Clear();
                }
                //提交事务
                tranOracleTransaction.Commit();
                return true;
            }
            catch
            {
                //遇到异常则回滚事务
                tranOracleTransaction.Rollback();
                //异常抛出给调用代码
                throw;
            }
            finally
            {
                //关闭数据库连接,释放资源
                conOracleConnection.Close();
            }
        }
        /// <summary>
        /// 调用事务执行PL-SQL语句数组
        /// </summary>
        /// <param name="p_strSQL">PL-SQL语句</param>
        /// <returns>是否成功执行</returns>
        public static bool ExecuteSql(string[] p_strSQL)
        {
            //创建数据库连接

            SqlConnection conOracleConnection = new SqlConnection(ConnStr);

            //定义OracleCommand
            SqlCommand cmdOracleCommand = new SqlCommand();

            //判断数据库连接是否打开,未打开则立刻打开
            if (conOracleConnection.State != ConnectionState.Open)
            {
                conOracleConnection.Open();
            }

            //创建Oracle事务
            SqlTransaction tranOracleTransaction = conOracleConnection.BeginTransaction();

            try
            {
                //循环执行SQL语句
                for (int intCount = 0; intCount < p_strSQL.Length; intCount++)
                {
                    //调用OracleCommand预处理函数

                    PrepareCommand(cmdOracleCommand, conOracleConnection, tranOracleTransaction, CommandType.Text, p_strSQL[intCount], null);

                    //执行SQL语句
                    cmdOracleCommand.ExecuteNonQuery();
                }

                //提交事务
                tranOracleTransaction.Commit();

                return true;
            }
            catch
            {
                //遇到异常则回滚事务

                tranOracleTransaction.Rollback();

                //异常抛出给调用代码

                throw;
            }
            finally
            {
                //关闭数据库连接,释放资源
                conOracleConnection.Close();
            }
        }
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static int ExecuteSql(string strSql)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            int i = cmd.ExecuteNonQuery();
            return i;
        }

        public static bool ExecuteCommand(List<SqlCommand> commandList)
        {
            try
            {
                SqlConnection conOracleConnection = new SqlConnection(ConnStr);
                //判断数据库连接是否打开,未打开则立刻打开
                if (conOracleConnection.State != ConnectionState.Open)
                {
                    conOracleConnection.Open();
                }
                //创建Oracle事务
                SqlTransaction tranOracleTransaction = conOracleConnection.BeginTransaction();
                try
                {
                    //定义OracleCommand
                    foreach (SqlCommand command in commandList)
                    {

                        if (command.Parameters != null)
                        {
                            //循环检查SQL命名的参数是否有null
                            for (int i=0;i<command.Parameters.Count;i++)
                            {
                                if (command.Parameters[i].Value == null)
                                    command.Parameters[i].Value = DBNull.Value;
                            }
                        }
                        command.Connection = conOracleConnection;
                        command.Transaction = tranOracleTransaction;
                        command.ExecuteNonQuery();
                    }
                    ////提交事务
                    tranOracleTransaction.Commit();
                    return true;
                }
                catch(Exception e)
                {
                    //遇到异常则回滚事务
                    conOracleConnection.Open();
                    tranOracleTransaction.Rollback();
                    //异常抛出给调用代码
                    throw e;
                }
                finally
                {
                    //关闭数据库连接,释放资源
                    conOracleConnection.Close();
                }
            }
            catch
            {

                throw;
            }

        }

        /// <summary>
        /// 带参数的执行SQL
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string strSql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            cmd.Parameters.AddRange(paras);
            int i = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return i;
        }
        /// <summary>
        /// 返回一行记录
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static int GetScalar(string strSql)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;
        }
        /// <summary>
        /// 返回一行记录
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int GetScalar(string strSql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            cmd.Parameters.AddRange(paras);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Parameters.Clear();
            return i;
        }
        /// <summary>
        /// 返回SqlDataReader
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string strSql)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }
        /// <summary>
        /// 带参数返回SqlDataReader
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string strSql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            cmd.Parameters.AddRange(paras);
            SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return dr;
        }
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strSql)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ds.Clear();
            da.Fill(ds);
            return ds.Tables[0];
        }
        /// <summary>
        /// 带参数返回DataTable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string strSql, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            cmd.Parameters.AddRange(paras);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            return ds.Tables[0];
        }


        /// <summary>
        /// DataReader转泛型List
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="p_drSqlDataReader">OracleDataReader</param>
        /// <returns>泛型List</returns>
        public static List<T> Translate<T>(SqlDataReader p_drSqlDataReader)
        {
            try
            {
                //定义/初始化T类型集合
                List<T> objReturnList = new List<T>();

                //循环读取OracleDataReader中的数据
                while (p_drSqlDataReader.Read())
                {
                    //动态创建T类型实例
                    T objRowInstance = default(T);

                    //判断类型是否为.Net基础类型
                    if (typeof(T).Namespace == "System")
                    {
                        try
                        {
                            //是.Net基础类型时,取第一列,判断是否为DBNull
                            if (p_drSqlDataReader.GetValue(0) != DBNull.Value)
                            {
                                //将数据经过类型转换后,赋值给该类型实例

                                objRowInstance = (T)Convert.ChangeType(p_drSqlDataReader.GetValue(0), typeof(T));
                            }
                        }
                        catch
                        {
                            //异常处理
                        }
                    }
                    else
                    {
                        //非.Net基础类型时,先通过CreateInstance实例化

                        objRowInstance = Activator.CreateInstance<T>();

                        //遍历该类型的属性

                        foreach (PropertyInfo Property in typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public
                                                                                | BindingFlags.Instance | BindingFlags.Static))
                        {
                            try
                            {
                                //通过属性名称查找OracleDataReader中的对应列索引

                                int intOrdinal = p_drSqlDataReader.GetOrdinal(Property.Name);

                                //判断该列是否为DBNull
                                if (p_drSqlDataReader.GetValue(intOrdinal) != DBNull.Value)
                                {
                                    //获得该属性类型

                                    Type objType = Property.PropertyType;

                                    //可空类型的类型获取

                                    if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        objType = objType.GetGenericArguments()[0];
                                    }

                                    //将数据经过类型转换后,赋值给该类型实例

                                    Property.SetValue(objRowInstance, Convert.ChangeType(p_drSqlDataReader.GetValue(intOrdinal), objType), null);
                                }
                            }
                            catch
                            {
                                //异常处理
                            }
                        }
                    }

                    //将赋值后的对象实例添加到集合中

                    objReturnList.Add(objRowInstance);
                }

                return objReturnList;
            }
            catch
            {
                //异常抛出给调用代码

                throw;
            }
            finally
            {
                //关闭OracleDataReader,释放资源,数据库连接会随OracleDataReader一同关闭

                p_drSqlDataReader.Close();
            }
        }



        /// <summary>
        /// 根据PL-SQL语句返回泛型List
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="p_strSQL">PL-SQL语句</param>
        /// <returns>返回的泛型结果集</returns>
        public static List<T> ExecuteQueryText<T>(string p_strSQL)
        {
            try
            {
                //执行ExecuteReader方法得到OracleDataReader
                SqlDataReader drSqlDataReader = GetReader(p_strSQL);

                //调用DataReader转泛型List的方法

                return Translate<T>(drSqlDataReader);
            }
            catch
            {
                //异常抛出给调用代码

                throw;
            }
        }

        public static DataSet ExecuteQueryText(string p_strSQL)
        {
            SqlCommand cmd = new SqlCommand(p_strSQL, Conn);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ds.Clear();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// 根据带参数的PL-SQL语句返回DataSet数据集
        /// </summary>
        /// <param name="p_strSQL">PL-SQL语句</param>
        /// <param name="p_parmOracleCmdParms">SQL语句的OracleParameter</param>
        /// <returns>返回的DataSet类型的结果集</returns>
        public static DataSet ExecuteQueryText(string p_strSQL, params SqlParameter[] p_parmOracleCmdParms)
        {
            //定义返回的DataSet/OracleDataAdapter 
            DataSet dsReturnDataSet = new DataSet();
            SqlDataAdapter daOracleDataAdapter = new SqlDataAdapter();

            //初始化OracleDataAdapter的SelectCommand
            daOracleDataAdapter.SelectCommand = new SqlCommand();

            //通过using语句创建数据库连接,使用后自动释放资源

            using (SqlConnection conOracleConnection = new SqlConnection(ConnStr))
            {
                //调用OracleCommand预处理函数

                PrepareCommand(daOracleDataAdapter.SelectCommand, conOracleConnection, null, CommandType.Text, p_strSQL, p_parmOracleCmdParms);

                //将结果填充到DataSet
                daOracleDataAdapter.Fill(dsReturnDataSet);

                //清空参数集合
                daOracleDataAdapter.SelectCommand.Parameters.Clear();

                return dsReturnDataSet;
            }
        }


        /// <summary>
        /// 根据带参数的PL-SQL语句返回泛型List
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="p_strSQL">PL-SQL语句</param>
        /// <param name="p_parmOracleCmdParms">SQL语句的OracleParameter</param>
        /// <returns>返回的泛型结果集</returns>
        public static List<T> ExecuteQueryText<T>(string p_strSQL, params SqlParameter[] p_parmSqlCmdParms)
        {
            try
            {
                //执行ExecuteReader方法得到OracleDataReader
                SqlDataReader drSqlDataReader = GetReader(p_strSQL, p_parmSqlCmdParms);

                //调用DataReader转泛型List的方法

                return Translate<T>(drSqlDataReader);
            }
            catch
            {
                //异常抛出给调用代码

                throw;
            }
        }

        #region 基础操作
        /// <summary>
        /// 预处理OracleCommand
        /// </summary>
        /// <param name="p_cmdOracleCommand">OracleCommand</param>
        /// <param name="p_conOracleConnection">OracleConnection</param>
        /// <param name="p_tranOracleTransaction">OracleTransaction</param>
        /// <param name="p_objCmdType">OracleCommand类型</param>
        /// <param name="p_strCmdText">PL-SQL语句</param>
        /// <param name="p_parmOracleCmdParms">OracleParameters</param>
        private static void PrepareCommand(SqlCommand p_cmdOracleCommand, SqlConnection p_conOracleConnection, SqlTransaction p_tranOracleTransaction,
                                           CommandType p_objCmdType, string p_strCmdText, SqlParameter[] p_parmOracleCmdParms)
        {
            //判断数据库连接是否打开,未打开则立刻打开
            if (p_conOracleConnection.State != ConnectionState.Open)
            {
                p_conOracleConnection.Open();
            }

            //设置SQL命令的数据库连接/超时时间/SQL语句
            p_cmdOracleCommand.Connection = p_conOracleConnection;
            p_cmdOracleCommand.CommandTimeout = 300;
            p_cmdOracleCommand.CommandText = p_strCmdText;

            //判断及设置SQL命令的调用事务

            if (p_tranOracleTransaction != null)
            {
                p_cmdOracleCommand.Transaction = p_tranOracleTransaction;
            }

            //设置SQL命令种类
            p_cmdOracleCommand.CommandType = p_objCmdType;

            if (p_parmOracleCmdParms != null)
            {
                //循环为SQL命名的参数赋值

                foreach (SqlParameter parm in p_parmOracleCmdParms)
                {

                    if (parm.Value == null)
                        parm.Value = DBNull.Value;

                    p_cmdOracleCommand.Parameters.Add(parm);

                }
            }
        }
        #endregion

        public class SQLEntity
        {

            private string _sqlstr;

            public string Sqlstr
            {
                get { return _sqlstr; }
                set { _sqlstr = value; }
            }


            private SqlParameter[] _Sqlparameter;

            public SqlParameter[] Sqlparameter
            {
                get { return _Sqlparameter; }
                set { _Sqlparameter = value; }
            }


        }
    }

    
}
