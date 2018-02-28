using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Jurassic.GF.Interface.Model;
using GGGXParse;

namespace Jurassic.GF.Server.SqlServer
{
    internal class Comm
    {
        /// <summary>
        /// 根据对象ID获取对象属性信息
        /// </summary>
        /// <param name="bOID"></param>
        /// <returns></returns>
        internal static List<Property> GetPropertyByBoid(string boid)
        {
            List<Property> list = new List<Property>();
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(string.Format("SELECT NS,T.MD.GETCLOBVAL() MD  FROM PROPERTY T WHERE T.BOID = '{0}'", boid));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Property model = new Property();
                    // model.BOID = item["BOID"].ToString();
                    // model.ISUSERDEFINE = item["ISUSERDEFINE"].ToString();
                    model.MD = item["MD"].ToString();
                    //model.MdSource = item["MdSource"].ToString();
                    model.NS = item["NS"].ToString();
                    list.Add(model);
                }

            }
            return list;
        }

        /// <summary>
        ///  根据对象ID获取对象空间坐标信息
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        internal static List<Geometry> GetGeometryByBoid(string boid)
        {
            List<Geometry> list = new List<Geometry>();
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(string.Format("SELECT NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T WHERE T.BOID = '{0}'", boid));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Geometry model = new Geometry();
                    model.NAME = item["NAME"].ToString();
                    model.GEOMETRY = item["GEOMETRY"].ToString();
                    model.SOURCEDB = item["SOURCEDB"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据对象名称对象类型专业名称获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ft"></param>
        /// <returns></returns>
        internal BoMode GetBoListByName(string name, string ft)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM BO T");
            strSql.Append(" WHERE T.NAME =@NAME ");
            strSql.Append(" AND T.BOTID = (SELECT BOTID FROM OBJECTTYPE T1 WHERE T1.FT =@FT ) ");
            SqlParameter[] parameters = {
                                new SqlParameter("NAME", SqlDbType.VarChar,50),
                                new SqlParameter("FT", SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = ft;
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            BoMode model = new BoMode();
            if (dt != null && dt.Rows.Count > 0)
            {
                model.BOID = dt.Rows[0]["BOID"].ToString();
                model.BOTID = dt.Rows[0]["BOTID"].ToString();
                model.NAME = dt.Rows[0]["NAME"].ToString();
                model.ISUSE = dt.Rows[0]["ISUSE"].ToString();
            }
            return model;
        }
    }
}
