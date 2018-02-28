using Jurassic.GF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.SqlServer
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
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.MakeValid().STAsText() AS GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            strSql.Append(" WHERE BOID =@BOID");
            SqlParameter[] parameters = {
                            new SqlParameter("BOID",SqlDbType.VarChar,36)
                            };
            parameters[0].Value = Geometry.BOID;

            return SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters).Rows.Count > 0 ? true : false;

        }

        /// <summary>
        /// 根据ID获取对象空间坐标
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public List<GeometryModel> GetGeometryByID(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,GATHERID,NAME,T.GEOMETRY.MakeValid().STAsText() AS GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            strSql.Append(" WHERE BOID =@BOID");
            SqlParameter[] parameters = {
                            new SqlParameter("BOID",SqlDbType.VarChar,36)
                            };
            parameters[0].Value = boid;
            DataTable dt = SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            List<GeometryModel> list = new List<GeometryModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    GeometryModel model = new GeometryModel();
                    model.BOID = item["BOID"].ToString();
                    model.GATHERID = item["GATHERID"].ToString();
                    model.GEOMETRY = item["GEOMETRY"].ToString();
                    model.NAME = item["NAME"].ToString();
                    model.SOURCEDB = item["SOURCEDB"].ToString();
                    list.Add(model);
                }
            }
            return list;
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
            strInsertSql.Append(" VALUES (@BOID,@NAME,GATHERID,GEOGRAPHY::STGeomFromText(@GEOMETRY, 4326),@SOURCEDB)");

            SqlParameter[] parameters = {
                            new SqlParameter("BOID", SqlDbType.VarChar,36),
                            new SqlParameter("NAME", SqlDbType.VarChar,50),
                            new SqlParameter("GATHERID", SqlDbType.VarChar,36),
                            new SqlParameter("GEOMETRY", SqlDbType.Binary),
                            new SqlParameter("SOURCEDB", SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = Geometry.BOID;
            parameters[1].Value = Geometry.NAME;
            parameters[2].Value = Geometry.GATHERID;
            parameters[3].Value = Geometry.SOURCEDB;
            return SqlServerDBHelper.ExecuteCommand(strInsertSql.ToString(), parameters) > 0 ? true : false;
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
            strUpdateSql.Append(" SOURCEDB=@SOURCEDB,GEOMETRY=GEOGRAPHY::STGeomFromText(@GEOMETRY, 4326),NAME=@NAME");
            strUpdateSql.Append(" WHERE BOID=@BOID ");

            SqlParameter[] parameters = {
                            new SqlParameter("GATHERID", SqlDbType.VarChar,50),
                            new SqlParameter("SOURCEDB", SqlDbType.Binary),
                            new SqlParameter("GEOMETRY", SqlDbType.VarChar,36),
                            new SqlParameter("NAME", SqlDbType.VarChar,50),
                            new SqlParameter("BOID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = Geometry.GATHERID;
            parameters[1].Value = Geometry.SOURCEDB;
            parameters[2].Value = Geometry.GEOMETRY;
            parameters[3].Value = Geometry.NAME;
            parameters[3].Value = Geometry.BOID;
            return SqlServerDBHelper.ExecuteCommand(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
