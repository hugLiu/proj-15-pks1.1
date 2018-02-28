using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Jurassic.GF.Interface.Model;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.Oracle
{
    public class HisGeometryBusiness : IHisGeometry
    {
        public bool DelHisGeometry(string boid)
        {
            throw new NotImplementedException();
        }

        public bool ExistHisGeometry(HisGeometryModel HisGeometry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据对象ID和版本ID获取对象历史空间坐标数据
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="gatherid"></param>
        /// <returns></returns>
        public List<HisGeometryModel> GetHisGeometryByID(string boid, string gatherid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM HISGEOMETRY T ");
            strSql.Append(" WHERE BOID =:BOID AND GATHERID=:GATHERID ");
            OracleParameter[] parameters = {
                            new OracleParameter("BOID",OracleDbType.Varchar2,36),
                            new OracleParameter("GATHERID",OracleDbType.Varchar2,36)
                            };
            parameters[0].Value = boid;
            parameters[1].Value = gatherid;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<HisGeometryModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 添加历史空间数据
        /// </summary>
        /// <param name="HisGeometry"></param>
        /// <returns></returns>
        public bool InsertHisGeometry(HisGeometryModel HisGeometry)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO HISGEOMETRY(  ");
            strInsertSql.Append(" BOID,NAME,GATHERID,GEOMETRY,SOURCEDB)");
            strInsertSql.Append(" VALUES (:BOID,:NAME,GATHERID,SDO_GEOMETRY(:GEOMETRY,4326),:SOURCEDB)");

            OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,36),
                            new OracleParameter("GEOMETRY", OracleDbType.Clob),
                            new OracleParameter("SOURCEDB", OracleDbType.Varchar2,50)
                                           };
            parameters[0].Value = HisGeometry.BOID;
            parameters[1].Value = HisGeometry.NAME;
            parameters[2].Value = HisGeometry.GATHERID;
            parameters[3].Value = HisGeometry.SOURCEDB;
            return OracleDBHelper.OracleHelper.ExecuteSql(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        public bool UpdateHisGeometry(HisGeometryModel HisGeometry)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE HISGEOMETRY SET GATHERID=:GATHERID");
            strUpdateSql.Append(" SOURCEDB=:SOURCEDB,GEOMETRY=SDO_GEOMETRY(:GEOMETRY,4326)),NAME=:NAME");
            strUpdateSql.Append(" WHERE BOID=:BOID ");

            OracleParameter[] parameters = {
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,50),
                            new OracleParameter("SOURCEDB", OracleDbType.Clob),
                            new OracleParameter("GEOMETRY", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                            };
            parameters[0].Value = HisGeometry.GATHERID;
            parameters[1].Value = HisGeometry.SOURCEDB;
            parameters[2].Value = HisGeometry.GEOMETRY;
            parameters[3].Value = HisGeometry.NAME;
            parameters[3].Value = HisGeometry.BOID;
            return OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
