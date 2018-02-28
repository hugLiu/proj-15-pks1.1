using GF.Server.DBUtility;
using GGGXParse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Server.Oracle
{
    public static class Comm
    {
        /// <summary>
        /// 根据对象ID获取对象的CLASS
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static string GetClassByBoid(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT EXTRACTVALUE(VALUE(I), '/P' ");
            strSql.Append(" ) value ");
            strSql.Append(" FROM PROPERTY X, ");
            strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P' ");
            strSql.Append(" ))) I ");
            strSql.Append(string.Format(" WHERE x.boid='{0}' ", boid));
            strSql.Append(" and EXTRACTVALUE(VALUE(I), '/P/@n' ");
            strSql.Append(string.Format(" ) = '{0}' ", "井型"));
            return OracleDBHelper.OracleHelper.ExecuteQueryText<string>(strSql.ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 根据对象ID获取别名
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static List<AliasName> GetAliasNameByBoid(string boid)
        {
            return OracleDBHelper.OracleHelper.ExecuteQueryText<AliasName>(string.Format("SELECT APPDOMAIN,NAME FROM ALIASNAME WHERE BOID = '{0}'", boid));
        }

        /// <summary>
        /// 根据对象ID获取空间坐标数据
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static List<Geometry> GetGeometryByBoid(string boid)
        {
            return OracleDBHelper.OracleHelper.ExecuteQueryText<Geometry>(string.Format("SELECT NAME,T.GEOMETRY.GET_WKT() GEOMETRY,SOURCEDB  FROM GEOMETRY T WHERE T.BOID = '{0}'", boid));
        }
        /// <summary>
        /// 根据对象ID获取属性数据
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static List<Property> GetPropertyByBoid(string boid)
        {
            return OracleDBHelper.OracleHelper.ExecuteQueryText<Property>(string.Format("SELECT NS,T.MD.GETCLOBVAL() MD  FROM PROPERTY T WHERE T.BOID = '{0}'", boid));
        }
    }
}
