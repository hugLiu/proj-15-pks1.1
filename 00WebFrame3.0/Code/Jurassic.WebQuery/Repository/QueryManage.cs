using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic.Com.DB;
using Jurassic.WebQuery.Models;

namespace Jurassic.WebQuery.Repository
{
    public class QueryManage
    {
        private static DBHelper helper;

        public QueryManage()
        {
            helper = new DBHelper("DefaultConnection");
            //helper.DBComm = new DBCommSql();
        }

        public IEnumerable<TableModel> GetAllTables()
        {
            //表和字段的Id按拿取是的次序，要是严格按照Id去东西可以考虑吧数据放静态数据。

            var tables = helper.ExecDataTable("select name from sysobjects where type='U' ");
            var data = new List<TableModel>();
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                string tableName = tables.Rows[i]["name"].ToString();
                var model = new TableModel
                {
                    Id = i.ToString(),
                    //PId = "0",
                    CHName = tableName,
                    ENName = tableName,
                    Order = i,
                    Fields = new List<FieldModel>()
                };
                //获取表结构没有按表原有的次序去出来，会造成显示和数据的结构不同。
                //var fields = helper.ExecDataTable("Select name from syscolumns Where ID=OBJECT_ID('" + tableName + "')");
                var fields = helper.ExecDataTable("select * from " + tableName + " where 0=1;");
                for (int j = 0; j < fields.Columns.Count; j++)
                {
                    string fieldName = fields.Columns[j].ToString();
                    model.Fields.Add(new FieldModel
                    {
                        Id = j.ToString(),
                        TableId = i.ToString(),
                        TableENName = tableName,
                        CHName = fieldName,
                        IsKey = true,
                        ENName = fieldName,
                        Order = j
                    });
                }
                data.Add(model);
            }

            return data;
        }

        public string GetSql(QueriesAndReportingModel qrModel)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetData(string sql)
        {
            return helper.ExecDataTable(sql);
        }

        public void SaveUserSettings(string userId, QueriesAndReportingModel qrModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QueriesAndReportingModel> GetAllUserSettings(string userId)
        {
            throw new NotImplementedException();
        }
    }
}