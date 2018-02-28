using System;
using System.Collections.Generic;
using GGGXParse;
using Jurassic.GF.Server.DBUtility;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Interface.Model;
using System.Linq;

namespace Jurassic.GF.Server.Oracle
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
            return OracleDBHelper.OracleHelper.ExecuteQueryText<Property>(string.Format("SELECT NS,T.MD.GETCLOBVAL() MD  FROM PROPERTY T WHERE T.BOID = '{0}'", boid));
        }

        /// <summary>
        ///  根据对象ID获取对象空间坐标信息
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        internal static List<Geometry> GetGeometryByBoid(string boid)
        {
            return OracleDBHelper.OracleHelper.ExecuteQueryText<Geometry>(string.Format("SELECT NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T WHERE T.BOID = '{0}'", boid));
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
            strSql.Append(" WHERE T.NAME =:NAME ");
            strSql.Append(" AND T.BOTID = (SELECT BOTID FROM OBJECTTYPE T1 WHERE T1.FT =:FT ) ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("FT", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = ft;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<BoMode>(strSql.ToString(), parameters).FirstOrDefault();
        }

    }
}