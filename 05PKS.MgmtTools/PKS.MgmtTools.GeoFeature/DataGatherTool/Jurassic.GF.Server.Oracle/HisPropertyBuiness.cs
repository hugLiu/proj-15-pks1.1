using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Jurassic.GF.Interface.Model;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.Oracle
{
    public class HisPropertyBuiness : IHisBOProperty
    {
        public bool DelHisProperty(string boid, string ns)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  HISPROPERTY  ");
            strSql.Append(" WHERE BOID =:BOID AND NS=:NS");
            OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NS", OracleDbType.Varchar2,50)
                                            };
            parameters[0].Value = boid;
            parameters[1].Value = ns;
            return OracleDBHelper.OracleHelper.ExecuteSql(strSql.ToString()) > 0 ? true : false;
        }

        public bool ExistHisProperty(HisPropertyModel HisProperty)
        {
            throw new NotImplementedException();
        }

        public List<HisPropertyModel> GetHisPropertyByID(string boid)
        {
            throw new NotImplementedException();
        }

        public List<HisPropertyModel> GetHisPropertyByID(string boid, string ns, string gatherid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select boid, ns, t.md.getclobval() md, gatherid, mdsource from HISPROPERTY t ");
            strSql.Append(" WHERE BOID=:BOID AND  GATHERID=:GATHERID ");
            OracleParameter[] parameters;
            if (!string.IsNullOrEmpty(ns))
            {
                strSql.Append(" AND NS=:NS ");
                parameters = new OracleParameter[]{
                                             new OracleParameter("BOID",OracleDbType.Varchar2,36),
                                             new OracleParameter("GATHERID",OracleDbType.Varchar2,50),
                                             new OracleParameter("NS",OracleDbType.Varchar2,50)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = gatherid;
                parameters[2].Value = ns;
            }
            else
            {
                parameters = new OracleParameter[]{
                                    new OracleParameter("BOID",OracleDbType.Varchar2,36),
                                    new OracleParameter("GATHERID",OracleDbType.Varchar2,36)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = gatherid;
            }
            return OracleDBHelper.OracleHelper.ExecuteQueryText<HisPropertyModel>(strSql.ToString(), parameters);
        }

        public bool InsertHisProperty(HisPropertyModel HisProperty)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO HISPROPERTY(  ");
            strInsertSql.Append(" BOID,GATHERID,NS,MD,MDSOURCE)");
            strInsertSql.Append(" VALUES (:BOID,:GATHERID,:NS,:MD,:MDSOURCE)");

            OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,50),
                            new OracleParameter("NS", OracleDbType.Varchar2,50),
                            new OracleParameter("MD", OracleDbType.XmlType),
                            new OracleParameter("MDSOURCE", OracleDbType.Varchar2,50)
                            };
            parameters[0].Value = HisProperty.BOID;
            parameters[1].Value = HisProperty.GATHERID;
            parameters[2].Value = HisProperty.NS;
            parameters[2].Value = HisProperty.MD;
            parameters[4].Value = HisProperty.MDSOURCE;
            return OracleDBHelper.OracleHelper.ExecuteSql(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        public bool UpdateHisProperty(HisPropertyModel HisProperty)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE HISPROPERTY SET ");
            strUpdateSql.Append(" GATHERID=:GATHERID, MD=:MD ,MDSOURCE=:MDSOURCE ");
            strUpdateSql.Append(" WHERE BOID=:BOID AND NS=:NS");

            OracleParameter[] parameters = {
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,36),
                            new OracleParameter("MD", OracleDbType.XmlType),
                            new OracleParameter("MDSOURCE", OracleDbType.Varchar2,50),
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NS", OracleDbType.Varchar2,50)
                                            };
            parameters[0].Value = HisProperty.GATHERID;
            parameters[1].Value = HisProperty.MD;
            parameters[2].Value = HisProperty.MDSOURCE;
            parameters[3].Value = HisProperty.BOID;
            parameters[4].Value = HisProperty.NS;
            return OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
