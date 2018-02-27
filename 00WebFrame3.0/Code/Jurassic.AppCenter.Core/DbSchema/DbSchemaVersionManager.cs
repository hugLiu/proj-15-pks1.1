using Jurassic.Com.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    public class DbSchemaVersionManager
    {
        private static readonly DbSchemaVersionManager instance = new DbSchemaVersionManager();

        //以后扩展为注入方式获取接口实例
        private ISchemaScriptProvider schemaProvider = new MssqlSchemaScriptProvider();

        private static DBHelper dbHelper;

        private static string dbSchemaVersion = "";//用于记录数据库数据结构版本
        private static Dictionary<string, string> schemaScriptDictionary;
        private static string currentFrameSchemaVersion;

        private DbSchemaVersionManager()
        {
            dbHelper = new DBHelper("DefaultConnection");
            schemaScriptDictionary = schemaProvider.GetSchemaScriptDictionary();
            currentFrameSchemaVersion = schemaProvider.GetCurrentFrameSchemaVersion();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static DbSchemaVersionManager GetInstance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 判断数据库与框架的数据架构版本是否一致
        /// </summary>
        /// <returns></returns>
        public bool ValidateVersion()
        {
            return GetMaxVersionFromDB() == currentFrameSchemaVersion;
        }

        /// <summary>
        /// 从数据库中获取数据库架构最新版本号
        /// </summary>
        /// <returns>-1：version表不存在，0：没有初始版本数据，其他：最新版本号</returns>
        public string GetMaxVersionFromDB()
        {
            if (string.IsNullOrEmpty(dbSchemaVersion))
            {
                string sql = schemaScriptDictionary["GetMaxVersion"];
                var dt = dbHelper.ExecDataTable(sql);
                dbSchemaVersion = dt.Rows.Count == 0 ? "0" : dt.Rows[0][0].ToString();
            }
            return dbSchemaVersion;
        }

        /// <summary>
        /// 获取差异版本更新脚本
        /// </summary>
        /// <returns></returns>
        public string GetDbSchemaScript()
        {
            string execSql = "";

            string dbVersion = GetMaxVersionFromDB();

            if (dbVersion == "-1")
            {
                execSql += schemaScriptDictionary["CreateSchemaVersionTable"];
            }

            foreach (var key in schemaScriptDictionary.Keys)
            {
                float ver = 0;
                if (float.TryParse(key, out ver))
                {
                    if (ver >= float.Parse(dbVersion) && ver <= float.Parse(currentFrameSchemaVersion))
                    {
                        execSql += " " + schemaScriptDictionary[key];
                    }
                }
            }

            execSql += " " + schemaScriptDictionary["UpdateSchemaVersion"];

            execSql = execSql.Replace("@Version", currentFrameSchemaVersion);
            execSql = execSql.Replace("@UpdateTime", DateTime.Now.ToString());
            execSql = execSql.Replace("@Remark", "");

            return execSql;
        }

        /// <summary>
        /// 更新数据库架构到最新版本
        /// </summary>
        /// <returns></returns>
        public int UpdateDbSchemaToMaxVersion()
        {
            string execSql = GetDbSchemaScript();
            int rowCount = dbHelper.ExecNonQuery(execSql);
            return rowCount;
        }
        
    }
}
