using Jurassic.GF.Interface;
using System.Linq;
using System.Text;
using Jurassic.GF.Interface.Model;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.Oracle
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
            strSql.Append(" WHERE BOID =:BOID ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = boid;
            return OracleDBHelper.OracleHelper.ExecuteSql(strSql.ToString()) > 0 ? true : false;
        }

        public bool ExistBO(string name, string bot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM BO T");
            strSql.Append(" WHERE T.NAME =:NAME ");
            strSql.Append(" AND T.BOTID = (SELECT BOTID FROM OBJECTTYPE T1 WHERE T1.BOT =:BOT ) ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOT", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = bot;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<BoMode>(strSql.ToString(), parameters).Count > 0 ? true : false;
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
            strSql.Append(" WHERE T.NAME =:NAME ");
            strSql.Append(" AND T.BOTID = (SELECT BOTID FROM OBJECTTYPE T1 WHERE T1.BOT =:BOT ) ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOT", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = bot;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<BoMode>(strSql.ToString(), parameters).FirstOrDefault();
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
            strAddSql.Append(" VALUES (:BOID,:NAME,:BOTID,:ISUSE)");

            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Varchar2,36),
                                new OracleParameter("ISUSE", OracleDbType.Char,1)
                                               };
            parameters[0].Value = System.Guid.NewGuid().ToString();
            parameters[1].Value = model.NAME;
            parameters[2].Value = model.BOTID;
            parameters[3].Value = model.ISUSE;
            return OracleDBHelper.OracleHelper.ExecuteSql(strAddSql.ToString(), parameters) > 0 ? true : false;
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
            strUpdateSql.Append(" NAME=:NAME,BOTID= :BOTID,ISUSE=:ISUSE");
            strUpdateSql.Append(" WHERE BOID=:BOID");

            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Char,1),
                                new OracleParameter("ISUSE", OracleDbType.Varchar2,36),
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = model.NAME;
            parameters[1].Value = model.BOTID;
            parameters[2].Value = model.ISUSE;
            parameters[3].Value = model.BOID;
            return OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
