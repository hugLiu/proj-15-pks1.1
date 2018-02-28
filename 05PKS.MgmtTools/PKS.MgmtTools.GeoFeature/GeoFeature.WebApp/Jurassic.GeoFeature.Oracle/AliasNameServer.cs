using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace Jurassic.GeoFeature.Oracle
{
    public class AliasNameServer : IAlisaName
    {
        /// <summary>
        /// 判断别名是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool Exist(AliasNameModel aliasName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ALIASNAME T ");
            strSql.Append(" WHERE BOID=:BOID AND  APPDOMAIN=:APPDOMAIN AND NAME=:NAME ");
            OracleParameter[] parameters = { new OracleParameter("BOID",OracleDbType.Varchar2,36),
                                           new OracleParameter("APPDOMAIN",OracleDbType.Varchar2,50),
                                           new OracleParameter("NAME",OracleDbType.Varchar2,50)
                                           };
            parameters[0].Value = aliasName.BOId;
            parameters[1].Value = aliasName.AppDomain;
            parameters[2].Value = aliasName.Name;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters).Count() >= 1 ? true : false;
        }
        /// <summary>
        /// 添加别名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(AliasNameModel model)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO ALIASNAME(  ");
            strInsertSql.Append(" BOID,NAME,APPDOMAIN,CREATUSER,UPLOADDATE)");
            strInsertSql.Append(" VALUES (:BOID,:NAME,:APPDOMAIN,:CREATUSER,:UPLOADDATE)");

            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,50),
                                new OracleParameter("CREATUSER", OracleDbType.Varchar2,50),
                                new OracleParameter("UPLOADDATE", OracleDbType.Date)
                                               };
            parameters[0].Value = model.BOId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.AppDomain;
            parameters[3].Value = model.CreatUser;
            parameters[4].Value = model.UploadDate;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strInsertSql.ToString(), parameters);
        }

        /// <summary>
        /// 修改别名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(AliasNameModel model)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE ALIASNAME SET ");
            strUpdateSql.Append(" NAME=:NAME,CREATUSER=:CREATUSER,UPLOADDATE=:UPLOADDATE");
            strUpdateSql.Append(" WHERE BOID=:BOID AND APPDOMAIN=:APPDOMAIN");

            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("CREATUSER", OracleDbType.Varchar2,50),
                                new OracleParameter("UPLOADDATE", OracleDbType.Date),
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,50)
                                };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.CreatUser;
            parameters[2].Value = model.UploadDate;
            parameters[3].Value = model.BOId;
            parameters[4].Value = model.AppDomain;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除别名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Delete(AliasNameModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  ALIASNAME  ");
            strSql.Append(" WHERE BOID=:BOID AND APPDOMAIN=:APPDOMAIN");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,136)
                                                };
            parameters[0].Value = model.BOId;
            parameters[1].Value = model.AppDomain;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据对象ID获取别名列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AliasNameModel> GetListByID(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM ALIASNAME  ");
            strSql.Append(" WHERE  BOID =:BOID");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = id;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据对象名称获取别名列表,有什么实际意义？还应该有Bot吧
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<AliasNameModel> GetListByName(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM ALIASNAME  ");
            strSql.Append(" WHERE  BOID =(SELECT T.BOID FROM BO T WHERE T.NAME=:NAME)");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = name;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取全部别名，没有实际意义
        /// </summary>
        /// <returns></returns>
        public IList<AliasNameModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM ALIASNAME  ");
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString());
        }

        /// <summary>
        /// 根据对象ID和应用域确定对象别名
        /// </summary>
        /// <param name="boId"></param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public IList<AliasNameModel> GetAlisaNameByIDAndAppDomain(string boId, string appDomain)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM ALIASNAME  ");
            strSql.Append(" WHERE  BOID =:BOID AND APPDOMAIN=:APPDOMAIN");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("APPDOMAIN",OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = boId;
            parameters[1].Value = appDomain;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters);
        }
    }
}
