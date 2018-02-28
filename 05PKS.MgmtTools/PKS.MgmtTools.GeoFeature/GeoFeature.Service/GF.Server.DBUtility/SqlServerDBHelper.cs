using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GF.Server.DBUtility
{
    /// <summary>
    /// SqlServer数据操作公共类
    /// </summary>
    public static class SqlServerDBHelper
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
        /// 执行SQL
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string strSql)
        {
            SqlCommand cmd = new SqlCommand(strSql, Conn);
            int i = cmd.ExecuteNonQuery();
            return i;
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

        /// <summary>  
        /// 执行多条SQL语句，实现数据库事务。  
        /// </summary>  
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>  
        public static bool ExecuteSql(List<SQLEntity> ListSqlEntity)
        {
            SqlConnection connt = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand();

            //判断数据库连接是否打开,未打开则立刻打开
            if (connt.State != ConnectionState.Open)
            {
                connt.Open();
            }
            SqlTransaction tranSqlTransaction = connt.BeginTransaction();
            try
            {
                foreach (SQLEntity myDE in ListSqlEntity)//循环哈希表（本例中即，循环执行添加在哈希表中的sql语句  
                {
                    string cmdText = myDE.Sqlstr;//获取键值（本例中 即，sql语句）  
                    SqlParameter[] cmdParms = (SqlParameter[])myDE.Sqlparameter;//获取键值（本例中 即，sql语句对应的参数）  
                    //PrepareCommand(cmdOracleCommand, conOracleConnection, tranOracleTransaction, CommandType.Text, cmdText, cmdParms != null ? cmdParms : null);
                    cmd.Connection = connt;
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = cmdText;
                    cmd.Transaction = tranSqlTransaction;
                    cmd.CommandType = CommandType.Text;
                    if (cmdParms != null)
                    {
                        //循环为SQL命名的参数赋值

                        foreach (SqlParameter parm in cmdParms)
                        {

                            if (parm.Value == null)
                                parm.Value = DBNull.Value;

                            cmd.Parameters.Add(parm);

                        }
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                //提交事务
                tranSqlTransaction.Commit();
                return true;
            }
            catch
            {
                //遇到异常则回滚事务
                tranSqlTransaction.Rollback();
                //异常抛出给调用代码
                throw;
            }
            finally
            {
                //关闭数据库连接,释放资源
                tranSqlTransaction.Dispose();
            }
        }



        public class SQLEntity
        {

            private string _sqlstr;

            public string Sqlstr
            {
                get { return _sqlstr; }
                set { _sqlstr = value; }
            }


            private SqlParameter[] _sqlparameter;

            public SqlParameter[] Sqlparameter
            {
                get { return _sqlparameter; }
                set { _sqlparameter = value; }
            }


        }

    }
}
