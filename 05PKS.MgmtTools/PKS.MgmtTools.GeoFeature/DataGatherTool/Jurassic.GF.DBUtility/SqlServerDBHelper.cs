using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Server.DBUtility
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
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static bool ExecuteSqlTran(List<SQLEntity> ListSqlEntity)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (SQLEntity myDE in ListSqlEntity)
                        {
                            string cmdText = myDE.Sqlstr.ToString();
                            SqlParameter[] cmdParms = myDE.SqlServerParameter;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)//如果数据库连接为关闭状态
                conn.Open();//打开数据库连接
            cmd.Connection = conn;//设置命令连接
            cmd.CommandText = cmdText;//设置执行命令的sql语句
            if (trans != null)//如果事务不为空
                cmd.Transaction = trans;//设置执行命令的事务
            cmd.CommandType = CommandType.Text;//设置解释sql语句的类型为“文本”类型（也是就说该函数不适用于存储过程）
            if (cmdParms != null)//如果参数数组不为空
            {


                foreach (SqlParameter parameter in cmdParms) //循环传入的参数数组
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value; //获取参数的值
                    }
                    cmd.Parameters.Add(parameter);//添加参数
                }
            }
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

        public SqlParameter[] SqlServerParameter
        {
            get { return _sqlparameter; }
            set { _sqlparameter = value; }
        }
    }
}
