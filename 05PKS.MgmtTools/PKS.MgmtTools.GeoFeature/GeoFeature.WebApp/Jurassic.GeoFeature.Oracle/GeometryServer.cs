using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace Jurassic.GeoFeature.Oracle
{
    public class GeometryServer : IInterface<GeometryModel>
    {
        /// <summary>
        /// 判断坐标是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Exist(GeometryModel geometry)
        {
            return GetListByID(geometry.Boid).Count>0 ? true : false;
        }
        /// <summary>
        /// 添加对象空间坐标
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public int Insert(GeometryModel model)
        {
            int result = 0;
            /*测试代码*/
            string selectSQL = string.Format("select * from BO where boid='{0}'", model.Boid);
            GeometryModel mdoel1 = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<GeometryModel>(selectSQL).FirstOrDefault();
            /*测试代码*/
            if (mdoel1 != null)
            {
                StringBuilder strInsertSql = new StringBuilder();
                strInsertSql.Append(" INSERT INTO GEOMETRY(  ");
                strInsertSql.Append(" BOID,NAME,GEOMETRY,SOURCEDB)");
                strInsertSql.Append(" VALUES (:BOID,:NAME,SDO_GEOMETRY(:GEOMETRY,4326),:SOURCEDB)");

                OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("GEOMETRY", OracleDbType.Clob),
                            new OracleParameter("SOURCEDB", OracleDbType.Varchar2,50)
                                           };
                parameters[0].Value = model.Boid;
                parameters[1].Value = model.Name;
                parameters[2].Value = model.Geometry;
                parameters[3].Value = model.Sourcedb;
                result = DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strInsertSql.ToString(), parameters);
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 修改对象空间坐标
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public int Update(GeometryModel model)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE GEOMETRY SET ");
            strUpdateSql.Append(" SOURCEDB=:SOURCEDB,GEOMETRY=SDO_GEOMETRY(:GEOMETRY,4326))");
            strUpdateSql.Append(" WHERE BOID=:BOID AND NAME=:NAME");

            OracleParameter[] parameters = {
                            new OracleParameter("SOURCEDB", OracleDbType.Varchar2,50),
                            new OracleParameter("GEOMETRY", OracleDbType.Clob),
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50)
                                            };
            parameters[0].Value = model.Sourcedb;
            parameters[1].Value = model.Geometry;
            parameters[2].Value = model.Boid;
            parameters[3].Value = model.Name;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除对象空间坐标
        /// </summary>
        /// <param name="fId"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public int Delete(GeometryModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  GEOMETRY  ");
            strSql.Append(" WHERE BOID =:BOID AND NAME=:NAME");
            OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50)
                                            };
            parameters[0].Value = model.Boid;
            parameters[1].Value = model.Name;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 获取全部坐标
        /// </summary>
        /// <returns></returns>
        public IList<GeometryModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<GeometryModel>(strSql.ToString());
        }

        public IList<GeometryModel> GetListByID(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            strSql.Append(" WHERE BOID =:BOID");
            OracleParameter[] parameters = {
                            new OracleParameter("BOID",OracleDbType.Varchar2,36)
                            };
            parameters[0].Value = boid;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<GeometryModel>(strSql.ToString(), parameters);
        }

        public IList<GeometryModel> GetListByName(string Name)
        {
            throw new NotImplementedException();
        }
    }
}
