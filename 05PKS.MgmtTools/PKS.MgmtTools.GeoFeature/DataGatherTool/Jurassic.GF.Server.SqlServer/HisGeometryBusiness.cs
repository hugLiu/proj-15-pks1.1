using Jurassic.GF.Interface;
using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Server.SqlServer
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
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY.MakeValid().STAsText() AS GEOMETRY,SOURCEDB  FROM HISGEOMETRY T ");
            strSql.Append(" WHERE BOID =@BOID AND GATHERID=@GATHERID ");
            SqlParameter[] parameters = {
                            new SqlParameter("BOID",SqlDbType.VarChar,36),
                            new SqlParameter("GATHERID",SqlDbType.VarChar,36)
                            };
            parameters[0].Value = boid;
            parameters[1].Value = gatherid;
            DataTable dt = SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            List<HisGeometryModel> list = new List<HisGeometryModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    HisGeometryModel model = new HisGeometryModel();
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
            strInsertSql.Append(" VALUES (@BOID,@NAME,GATHERID,GEOGRAPHY::STGeomFromText(@GEOMETRY, 4326),@SOURCEDB)");

            SqlParameter[] parameters = {
                            new SqlParameter("BOID", SqlDbType.VarChar,36),
                            new SqlParameter("NAME", SqlDbType.VarChar,50),
                            new SqlParameter("GATHERID", SqlDbType.VarChar,36),
                            new SqlParameter("GEOMETRY", SqlDbType.Binary),
                            new SqlParameter("SOURCEDB", SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = HisGeometry.BOID;
            parameters[1].Value = HisGeometry.NAME;
            parameters[2].Value = HisGeometry.GATHERID;
            parameters[3].Value = HisGeometry.SOURCEDB;
            return SqlServerDBHelper.ExecuteCommand(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        public bool UpdateHisGeometry(HisGeometryModel HisGeometry)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE HISGEOMETRY SET GATHERID=:GATHERID");
            strUpdateSql.Append(" SOURCEDB=@SOURCEDB,GEOMETRY=GEOGRAPHY::STGeomFromText(@GEOMETRY, 4326),NAME=@NAME");
            strUpdateSql.Append(" WHERE BOID=@BOID ");

            SqlParameter[] parameters = {
                            new SqlParameter("GATHERID", SqlDbType.VarChar,50),
                            new SqlParameter("SOURCEDB", SqlDbType.Binary),
                            new SqlParameter("GEOMETRY", SqlDbType.VarChar,36),
                            new SqlParameter("NAME", SqlDbType.VarChar,50),
                            new SqlParameter("BOID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = HisGeometry.GATHERID;
            parameters[1].Value = HisGeometry.SOURCEDB;
            parameters[2].Value = HisGeometry.GEOMETRY;
            parameters[3].Value = HisGeometry.NAME;
            parameters[3].Value = HisGeometry.BOID;
            return SqlServerDBHelper.ExecuteCommand(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
