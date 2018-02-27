using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data.Common;
using System.Data;
using System.Configuration;
using Jurassic.Com.Tools;

namespace Jurassic.Com.DB
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// ACCESS数据库的数据接口实现
    /// </summary>
    public class DBCommOleDb : IDBComm
    {
        /// <summary>
        /// 通用数据访问类
        /// </summary>
        public DBHelper Helper { get; set; }
        /// <summary>
        /// 参照接口定义
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            return new OleDbConnection(Helper.ConnStr);
        }

        /// <summary>
        /// 参照接口定义
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter(DbCommand command)
        {
            return new OleDbDataAdapter((OleDbCommand) command);
        }

        /// <summary>
        /// 参照接口定义
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string parameterName, object value)
        {
            if (value is byte[])
            {
                return CreateImageParameter(parameterName, (byte[])value);
            }
            OleDbParameter param = new OleDbParameter(parameterName, CommOp.TestNull(value));
            return param;
        }

        /// <summary>
        /// 生成二进制参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDataParameter CreateImageParameter(string parameterName, byte[] value)
        {
            OleDbParameter param = new OleDbParameter(parameterName, OleDbType.Binary);
            param.Value = CommOp.TestNull(value);
            return param;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetDBSize()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShrinkDB()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public IDataReader ExecPageReader(DBPagerInfo pager, params IDataParameter[] sp)
        {
            throw new NotImplementedException();
        }

        public void CreateTable(DataTable dt, string tableName)
        {
            if (tableName.IsEmpty())
            {
                tableName = dt.TableName;
            }

            string strSql = string.Format("create table {0}(", tableName);
            foreach (DataColumn c in dt.Columns)
            {
                strSql += string.Format("[{0}] {1},", c.ColumnName, TypeMappingName(c.DataType, c.MaxLength < 0 ? 255 : c.MaxLength));
            }
            strSql = strSql.Trim(',') + ")";

            Helper.ExecNonQuery(strSql);
        }

        public string TypeMappingName(Type type, int maxLength)
        {
            switch (type.Name.ToLower())
            {
                case "int32": return "int";
                case "int64": return "bigint";
                case "bool": return "bit";
                case "byte[]": return "image";
                case "float": return "real";
                case "double": return "float";
                case "decimal": return "numeric(18,4)";
            }
            return "nvarchar(" + maxLength + ")";
        }

        public string ParamPrefix
        {
            get { return "@"; }
        }


        public bool TableExists(string tableName)
        {
            //string sql = "select * from MSysObjects where Name=@tableName";

            //object r = Helper.ExecGetObject(sql, Helper.CreateParameter("tableName", tableName));
            //return CommOp.ToInt(r) == 1;
            try
            {
                Helper.ExecGetObject("SELECT COUNT(*) FROM [" + tableName + "]");
            }
            catch (OleDbException ex)
            {
                return false;
            }
            return true;
        }


        public DbCommandBuilder CreateCommandBuilder(DbDataAdapter sda)
        {
            return new OleDbCommandBuilder(((OleDbDataAdapter)sda));
        }


        public string FieldPrefix
        {
            get { return "["; }
        }

        public string FieldSuffix
        {
            get { return "]"; }
        }
    }
}
