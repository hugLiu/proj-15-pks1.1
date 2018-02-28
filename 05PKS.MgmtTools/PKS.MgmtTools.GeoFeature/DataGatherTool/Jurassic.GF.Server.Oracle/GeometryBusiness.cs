using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Jurassic.GF.Interface.Model;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.Oracle
{
    public class GeometryBusiness : IGeometry
    {
        public bool DelGeometry(string boid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 空间坐标是否存在
        /// </summary>
        /// <param name="Geometry"></param>
        /// <returns></returns>
        public bool ExistGeometry(GeometryModel Geometry)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            strSql.Append(" WHERE BOID =:BOID");
            OracleParameter[] parameters = {
                            new OracleParameter("BOID",OracleDbType.Varchar2,36)
                            };
            parameters[0].Value = Geometry.BOID;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<GeometryModel>(strSql.ToString(), parameters).Count > 0 ? true : false;

        }

        /// <summary>
        /// 根据ID获取对象空间坐标
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public List<GeometryModel> GetGeometryByID(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            strSql.Append(" WHERE BOID =:BOID");
            OracleParameter[] parameters = {
                            new OracleParameter("BOID",OracleDbType.Varchar2,36)
                            };
            parameters[0].Value = boid;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<GeometryModel>(strSql.ToString(), parameters);
        }

        public List<GeometryModel> GetGeometryByID(string boid, string ns = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加空间坐标
        /// </summary>
        /// <param name="Geometry"></param>
        /// <returns></returns>
        public bool InsertGeometry(GeometryModel Geometry)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO GEOMETRY(  ");
            strInsertSql.Append(" BOID,NAME,GATHERID,GEOMETRY,SOURCEDB)");
            strInsertSql.Append(" VALUES (:BOID,:NAME,GATHERID,SDO_GEOMETRY(:GEOMETRY,4326),:SOURCEDB)");

            OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,36),
                            new OracleParameter("GEOMETRY", OracleDbType.Clob),
                            new OracleParameter("SOURCEDB", OracleDbType.Varchar2,50)
                                           };
            parameters[0].Value = Geometry.BOID;
            parameters[1].Value = Geometry.NAME;
            parameters[2].Value = Geometry.GATHERID;
            parameters[3].Value = Geometry.SOURCEDB;
            return OracleDBHelper.OracleHelper.ExecuteSql(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 修改空间坐标
        /// </summary>
        /// <param name="Geometry"></param>
        /// <returns></returns>
        public bool UpdateGeometry(GeometryModel Geometry)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE GEOMETRY SET GATHERID=:GATHERID");
            strUpdateSql.Append(" SOURCEDB=:SOURCEDB,GEOMETRY=SDO_GEOMETRY(:GEOMETRY,4326)),NAME=:NAME");
            strUpdateSql.Append(" WHERE BOID=:BOID ");

            OracleParameter[] parameters = {
                            new OracleParameter("GATHERID", OracleDbType.Varchar2,50),
                            new OracleParameter("SOURCEDB", OracleDbType.Clob),
                            new OracleParameter("GEOMETRY", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                            };
            parameters[0].Value = Geometry.GATHERID;
            parameters[1].Value = Geometry.SOURCEDB;
            parameters[2].Value = Geometry.GEOMETRY;
            parameters[3].Value = Geometry.NAME;
            parameters[3].Value = Geometry.BOID;
            return OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
