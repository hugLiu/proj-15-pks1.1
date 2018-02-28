using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Jurassic.GF.Interface.Model;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.Oracle
{
    public class BOPropertyBusiness : IBOProperty
    {
        public bool DelProperty(string boid, string ns = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  PROPERTY  ");
            strSql.Append(" WHERE BOID =:BOID");
            OracleParameter[] parameters;
            if (!string.IsNullOrEmpty(ns))
            {
                strSql.Append(" AND NS=:NS");
                parameters = new OracleParameter[]{
                                             new OracleParameter("BOID",OracleDbType.Varchar2,36),
                                             new OracleParameter("NS",OracleDbType.Varchar2,50)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = ns;
            }
            else
            {
                parameters = new OracleParameter[]{
                                    new OracleParameter("BOID",OracleDbType.Varchar2,36)
                                           };
                parameters[0].Value = boid;
            }
            return OracleDBHelper.OracleHelper.ExecuteSql(strSql.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 判断对象属性是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool ExistProperty(PropertyModel property)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select boid, ns, t.md.getclobval() md, mdsource, gatherid from PROPERTY t ");
            strSql.Append(" WHERE BOID=:BOID AND NS=:NS ");
            OracleParameter[] parameters = { new OracleParameter("BOID",OracleDbType.Varchar2,36),
                                             new OracleParameter("NS",OracleDbType.Varchar2,50)
                                           };
            parameters[0].Value = property.BOID;
            parameters[1].Value = property.NS;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<PropertyModel>(strSql.ToString(), parameters).Count > 0 ? true : false;
        }

        /// <summary>
        /// 根据对象ID和NS获取对象属性
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public List<PropertyModel> GetListByID(string boid, string ns)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select boid, ns, t.md.getclobval() md, mdsource, gatherid from PROPERTY t ");
            strSql.Append(" WHERE BOID=:BOID  ");
            OracleParameter[] parameters;
            if (!string.IsNullOrEmpty(ns))
            {
                strSql.Append(" AND NS=:NS ");
                parameters = new OracleParameter[]{
                                             new OracleParameter("BOID",OracleDbType.Varchar2,36),
                                             new OracleParameter("NS",OracleDbType.Varchar2,50)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = ns;
            }
            else
            {
                parameters = new OracleParameter[]{
                                    new OracleParameter("BOID",OracleDbType.Varchar2,36)
                                           };
                parameters[0].Value = boid;
            }
            return OracleDBHelper.OracleHelper.ExecuteQueryText<PropertyModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        ///添加属性数据
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool InsertProperty(PropertyModel property)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO PROPERTY(  ");
            strInsertSql.Append(" BOID,GATHERID,NS,MD,MDSOURCE)");
            strInsertSql.Append(" VALUES (:BOID,:GATHERID,:NS,:MD,:MDSOURCE)");

            OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,50),
                            new OracleParameter("NS", OracleDbType.Varchar2,50),
                            new OracleParameter("MD", OracleDbType.XmlType),
                            new OracleParameter("MDSOURCE", OracleDbType.Varchar2,50)
                            };
            parameters[0].Value = property.BOID;
            parameters[1].Value = property.GATHERID;
            parameters[2].Value = property.NS;
            parameters[2].Value = property.MD;
            parameters[4].Value = property.MDSOURCE;
            return OracleDBHelper.OracleHelper.ExecuteSql(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 修改属性数据
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool UpdateProperty(PropertyModel property)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE PROPERTY SET ");
            strUpdateSql.Append(" GATHERID=:GATHERID, MD=:MD ,MDSOURCE=:MDSOURCE ");
            strUpdateSql.Append(" WHERE BOID=:BOID AND NS=:NS");

            OracleParameter[] parameters = {
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,36),
                            new OracleParameter("MD", OracleDbType.XmlType),
                            new OracleParameter("MDSOURCE", OracleDbType.Varchar2,50),
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NS", OracleDbType.Varchar2,50)
                                            };
            parameters[0].Value = property.GATHERID;
            parameters[1].Value = property.MD;
            parameters[2].Value = property.MDSOURCE;
            parameters[3].Value = property.BOID;
            parameters[4].Value = property.NS;
            return OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
