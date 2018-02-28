using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGatherTool
{
    public sealed class DataBaseSetup
    {
        #region 委托和事件声明
        public delegate void SetupStatusHandle(object sender, SetupStatusEventArgs e);
        public delegate void SetupStepChangingHandle(object sender, SetupStepEventArgs e);

        #endregion

        #region 私有成员
        private string username;
        private string password;
        private string servername;
        private string databasename;
        private DataBaseType dbType;
        private string connStr;
        #endregion

        #region 构造函数
        public DataBaseSetup()
        {

        }

        public DataBaseSetup(string sname, string dataname, string uname, string pwd, DataBaseType dtype)
            : base()
        {
            servername = sname;
            databasename = dataname;
            username = uname;
            password = pwd;
            DbType = dtype;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 数据库用户
        /// </summary>
        public string UserName
        {
            get
            {
                if (username == null)
                {
                    username = "";
                }
                return username;
            }
            set
            {
                username = value;
            }
        }

        /// <summary>
        /// 数据库用户密码
        /// </summary>
        public string PassWord
        {
            get
            {
                if (password == null)
                {
                    password = "";
                }
                return password;
            }
            set
            {
                password = value;
            }
        }
        public string DataBaseName
        {
            get
            {
                if (databasename == null)
                {
                    databasename = "";
                }
                return databasename;
            }
            set
            {
                databasename = value;
            }
        }
        /// <summary>
        /// 服务器名
        /// </summary>
        public string ServerName
        {
            get
            {
                if (servername == null)
                    return "";
                return servername;
            }
            set
            {
                servername = value;
            }
        }

        public DataBaseType DbType
        {
            get
            {
                return dbType;
            }
            set
            {
                dbType = value;
            }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 测试数据库联接
        /// </summary>
        public bool TestConn(out string conn)
        {
            bool result = false;
            switch (this.dbType)
            {
                case DataBaseType.Oracle:
                    result = TestConnOracle();
                    conn = connStr;
                    break;
                case DataBaseType.SqlServer:
                    result = TestConnSqlServer();
                    conn = connStr;
                    break;
            }
            conn = connStr;
            return result;
        }
        #endregion

        #region 私有方法
        private bool TestConnSqlServer()
        {
            bool result = false;
            SqlConnection sqlConn = GetSqlServerConnection();
            try
            {
                sqlConn.Open();
                result = true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }
            }
            return result;
        }

        private bool TestConnOracle()
        {
            bool result = false;
            OracleConnection sqlConn = GetOracleConnection();
            try
            {
                sqlConn.Open();
                result = true;
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }
            }

            return result;
        }

        private SqlConnection GetSqlServerConnection()
        {
            SqlConnection sqlConn = new SqlConnection();
            sqlConn.ConnectionString = "Data Source= " + servername + ";"
                               + "Initial Catalog =" + databasename + ";"
                               + "User id=" + username + ";"
                               + "Password=" + password + ";";
            connStr = "Data Source= " + servername + ";"
                           + "Initial Catalog =" + databasename + ";"
                           + "User id=" + username + ";"
                           + "Password=" + password + ";"
                           + "Packet Size=8192" + ";"
                           + "Max Pool Size=1000";
            return sqlConn;
        }

        private OracleConnection GetOracleConnection()
        {
            OracleConnection sqlConn = new OracleConnection();
            sqlConn.ConnectionString = "Data Source=" + servername + "/" + databasename + ";Persist Security Info=True;User ID=" + username + ";Password=" + password + "";
            connStr = "Data Source=" + servername + "/" + databasename + ";Persist Security Info=True;User ID=" + username + ";Password=" + password + "";
            return sqlConn;
        }


        #endregion
    }
}
