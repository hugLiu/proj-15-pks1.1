using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace Jurassic.Com.DB
{
    /// <summary>
    /// 错误处理的委托
    /// </summary>
    public delegate void ErrorsHander(Exception ex);

    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 将Excel作为数据库操作的类
    /// </summary>
    public class ExcelDB
    {
        /// <summary>
        /// 一般性错误处理委托
        /// </summary>
        public static ErrorsHander ErrorHander = defaultErrorHander;

        /// <summary>
        /// 获取指定Excel表的架构信息
        /// </summary>
        /// <param name="excelFile">Excel文件路径</param>
        /// <returns>Excel所有工作表名称的数组</returns>
        public static String[] GetSheets(string excelFile)
        {
            List<String> sheets = new List<String>();
            DataTable table = new DataTable();
            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + excelFile + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                conn.Open();
                table = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorHander(new Exception(ex.Message));
            }
            foreach (DataRow drow in table.Rows)
            {
                string tableName = drow["Table_Name"].ToString();
                sheets.Add(tableName);

                /*DataTable tableColumns = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[]{ null, null, TableName ,null});
      
                foreach (DataRow drowColumns in tableColumns.Rows)
                {
                    string ColumnName = drowColumns["Column_Name"].ToString();
                    //columns.Add(ColumnName);
                }*/
            }
            return sheets.ToArray();
        }

        /// <summary>
        /// 默认错误处理
        /// </summary>
        /// <param name="ex"></param>
        private static void defaultErrorHander(Exception ex)
        {
            //do nothing
            //需要上级调用来改写
        }

        /// <summary>
        /// 默认进度条处理
        /// </summary>
        /// <param name="rowCount">总行数</param>
        /// <param name="rowsCopied">已拷贝的行数</param>
        private static void defaultImporting(long rowCount, long rowsCopied)
        {
            //do nothing
            //需要上级调用来改写
        }

        static string getExcelConnStr(string excelFile)
        {
            if (excelFile.EndsWith("xls", StringComparison.OrdinalIgnoreCase))
            {
                //获取全部数据
                return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
            }
            else
            {
                return "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + excelFile + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
            }
        }
        /// <summary>
        /// 读出指定Excel表中的全部数据
        /// </summary>
        /// <param name="excelFile">Excel文件名</param>
        /// <param name="sheetName">工作表名</param>
        /// <param name="columns">要访问的字段列表，默认是全部返回(*)</param>
        public static DataTable GetExcelTable(string excelFile, string sheetName, string columns = "*")
        {
            DataTable dt = new DataTable();
            string strConn = getExcelConnStr(excelFile);

            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            strExcel = string.Format("select {0} from [{1}]", columns, sheetName);
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            myCommand.Fill(dt);
            conn.Close();

            //清除空列
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (String.IsNullOrEmpty(dt.Columns[i].ColumnName)
                    || dt.Columns[i].ColumnName.Trim() == ""
                    || Regex.IsMatch(dt.Columns[i].ColumnName, @"^F\d{1,3}$")
                    && (dt.Rows.Count == 0 || dt.Rows[0][i].ToString().Trim() == "")
                    )
                    dt.Columns.Remove(dt.Columns[i]);
                else
                    break;
            }

            //清除空行
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                bool isEmptyRow = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString().Trim() != "")
                    {
                        isEmptyRow = false;
                        break;
                    }
                }
                if (isEmptyRow)
                {
                    dt.Rows.Remove(dt.Rows[i]);
                }
                else
                {
                    break;
                }
            }

            //将系统自动产生的#号替换回.号
            foreach (DataColumn dc in dt.Columns)
            {
                dc.ColumnName = dc.Caption = dc.Caption.Replace('#', '.').Trim();
            }

            return dt;
        }

        /// <summary>
        /// 将数据表导出到Excel中
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="sheetName"></param>
        /// <param name="columns"></param>
        /// <returns>操作成功的Excel行数</returns>
        public static int SaveExcelTable(DataTable dt, string excelFile, string sheetName)
        {
            int i = 0;
            string strConn = getExcelConnStr(excelFile);
            string values = "";
            String fields = "";
            foreach (DataColumn dc in dt.Columns)
            {
                values += ",?";
                fields += ",[" + dc.ColumnName + "]";
            }
            if (values != "") values = values.Substring(1);
            if (values != "") fields = fields.Substring(1);

            string sql = String.Format("INSERT INTO [{0}] ({1}) VALUES({2})", sheetName, fields, values);
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            OleDbCommand myCommand = new OleDbCommand(sql, conn);
            myCommand.CommandText = sql;
            myCommand.CommandType = CommandType.Text;
            foreach (DataRow dr in dt.Rows)
            {
                myCommand.Parameters.Clear();
                foreach (DataColumn dc in dt.Columns)
                {
                    myCommand.Parameters.AddWithValue("@" + dc.ColumnName, dr[dc.ColumnName]);
                }
                i += myCommand.ExecuteNonQuery();
            }
            conn.Close();
            return i;
        }
    }
}
