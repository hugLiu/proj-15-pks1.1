using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Jurassic.GeoFeature.SqlServer
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
            strSql.Append(" WHERE BOID=@BOID AND  APPDOMAIN=@APPDOMAIN AND NAME=@NAME ");
            SqlParameter[] parameters = { new SqlParameter("@BOID",SqlDbType.VarChar,36),
                                           new SqlParameter("@APPDOMAIN",SqlDbType.VarChar,50),
                                           new SqlParameter("@NAME",SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = aliasName.BOId;
            parameters[1].Value = aliasName.AppDomain;
            parameters[2].Value = aliasName.Name;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters).Count() >= 1 ? true : false;
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
            strInsertSql.Append(" BOID,NAME,APPDOMAIN)");
            strInsertSql.Append(" VALUES (@BOID,@NAME,@APPDOMAIN)");

            SqlParameter[] parameters = {
                                new SqlParameter("@BOID", SqlDbType.VarChar,36),
                                new SqlParameter("@NAME", SqlDbType.VarChar,50),
                                new SqlParameter("@APPDOMAIN", SqlDbType.VarChar,50)                              
                                               };
            parameters[0].Value = model.BOId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.AppDomain;        
            return DBUtility.SqlServerDBHelper.ExecuteSql(strInsertSql.ToString(), parameters);
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
            strUpdateSql.Append(" NAME=@NAME");
            strUpdateSql.Append(" WHERE BOID=@BOID AND APPDOMAIN=@APPDOMAIN");

            SqlParameter[] parameters = {
                                new SqlParameter("@NAME", SqlDbType.VarChar,50),                              
                                new SqlParameter("@BOID", SqlDbType.VarChar,36),
                                new SqlParameter("@APPDOMAIN", SqlDbType.VarChar,50)
                                };
            parameters[0].Value = model.Name;          
            parameters[1].Value = model.BOId;
            parameters[2].Value = model.AppDomain;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
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
            strSql.Append(" WHERE BOID=@BOID AND APPDOMAIN=@APPDOMAIN");
            SqlParameter[] parameters = {
                                new SqlParameter("@BOID", SqlDbType.VarChar,36),
                                new SqlParameter("@APPDOMAIN", SqlDbType.VarChar,136)
                                                };
            parameters[0].Value = model.BOId;
            parameters[1].Value = model.AppDomain;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append(" WHERE  BOID =@BOID");
            SqlParameter[] parameters = {
                                new SqlParameter("@BOID", SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = id;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters);
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
            strSql.Append(" WHERE  BOID =(SELECT T.BOID FROM BO T WHERE T.NAME=@NAME)");
            SqlParameter[] parameters = {
                                new SqlParameter("@NAME", SqlDbType.VarChar,50)
                                                };
            parameters[0].Value = name;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取全部别名，没有实际意义
        /// </summary>
        /// <returns></returns>
        public IList<AliasNameModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM ALIASNAME  ");
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString());
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
            strSql.Append(" WHERE  BOID =@BOID AND APPDOMAIN=@APPDOMAIN");
            SqlParameter[] parameters = {
                                new SqlParameter("@BOID", SqlDbType.VarChar,36),
                                new SqlParameter("@APPDOMAIN",SqlDbType.VarChar,50)
                                                };
            parameters[0].Value = boId;
            parameters[1].Value = appDomain;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<AliasNameModel>(strSql.ToString(), parameters);
        }
    }
}
