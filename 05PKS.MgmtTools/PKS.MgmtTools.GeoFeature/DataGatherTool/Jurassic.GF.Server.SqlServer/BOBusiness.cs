using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using Jurassic.GF.Interface.Model;

namespace Jurassic.GF.Server.SqlServer
{
    public class BOBusiness : IBO
    {
        /// <summary>
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public bool DelBO(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  BO  ");
            strSql.Append(" WHERE BOID =@BOID ");
            SqlParameter[] parameters = {
                                new SqlParameter("BOID", SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = boid;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strSql.ToString(), parameters) > 0 ? true : false;
        }

        public bool ExistBO(string name, string bot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM BO T");
            strSql.Append(" WHERE T.NAME =@NAME ");
            strSql.Append(" AND T.BOTID = (SELECT BOTID FROM OBJECTTYPE T1 WHERE T1.BOT =@BOT ) ");
            SqlParameter[] parameters = {
                                new SqlParameter("NAME", SqlDbType.VarChar,50),
                                new SqlParameter("BOT", SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = bot;
            return DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters).Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// 根据对象名称和对象类型名称获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public BoMode GetBoListByName(string name, string bot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM BO T");
            strSql.Append(" WHERE T.NAME =@NAME ");
            strSql.Append(" AND T.BOTID = (SELECT BOTID FROM OBJECTTYPE T1 WHERE T1.BOT =@BOT ) ");
            SqlParameter[] parameters = {
                                new SqlParameter("NAME", SqlDbType.VarChar,50),
                                new SqlParameter("BOT", SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = bot;
            BoMode model = new BoMode();
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                model.BOID = dt.Rows[0]["BOID"].ToString();
                model.BOTID = dt.Rows[0]["BOTID"].ToString();
                model.NAME = dt.Rows[0]["NAME"].ToString();
                model.ISUSE = dt.Rows[0]["ISUSE"].ToString();
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertBO(BoMode model)
        {
            StringBuilder strAddSql = new StringBuilder();
            strAddSql.Append(" INSERT INTO BO(  ");
            strAddSql.Append(" BOID,NAME,BOTID,ISUSE)");
            strAddSql.Append(" VALUES (@BOID,@NAME,@BOTID,@ISUSE)");

            SqlParameter[] parameters = {
                                new SqlParameter("BOID", SqlDbType.VarChar,36),
                                new SqlParameter("NAME", SqlDbType.VarChar,50),
                                new SqlParameter("BOTID", SqlDbType.VarChar,36),
                                new SqlParameter("ISUSE", SqlDbType.Char,1)
                                               };
            parameters[0].Value = System.Guid.NewGuid().ToString();
            parameters[1].Value = model.NAME;
            parameters[2].Value = model.BOTID;
            parameters[3].Value = model.ISUSE;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strAddSql.ToString(), parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateBO(BoMode model)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE BO SET ");
            strUpdateSql.Append(" NAME=@NAME,BOTID= @BOTID,ISUSE=@ISUSE");
            strUpdateSql.Append(" WHERE BOID=@BOID");

            SqlParameter[] parameters = {
                                new SqlParameter("NAME", SqlDbType.VarChar,50),
                                new SqlParameter("BOTID", SqlDbType.Char,1),
                                new SqlParameter("ISUSE", SqlDbType.VarChar,36),
                                new SqlParameter("BOID", SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = model.NAME;
            parameters[1].Value = model.BOTID;
            parameters[2].Value = model.ISUSE;
            parameters[3].Value = model.BOID;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
