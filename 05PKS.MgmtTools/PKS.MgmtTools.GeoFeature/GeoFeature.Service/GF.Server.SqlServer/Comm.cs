using GF.Server.DBUtility;
using GGGXParse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GF.Server.SqlServer
{
    public static class Comm
    {
        /// <summary>
        /// 根据对象ID获取别名
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static List<AliasName> GetAliasNameByBoid(string boid)
        {
            return SqlServerDBHelper.ExecuteQueryText<AliasName>(string.Format("SELECT APPDOMAIN,NAME FROM ALIASNAME WHERE BOID = '{0}'", boid));
        }

        /// <summary>
        /// 根据对象ID获取空间坐标数据
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static List<Geometry> GetGeometryByBoid(string boid)
        {
            return SqlServerDBHelper.ExecuteQueryText<Geometry>(string.Format("SELECT NAME,T.GEOMETRY.STAsText() GEOMETRY,SOURCEDB  FROM GEOMETRY T WHERE T.BOID = '{0}'", boid));
        }
        /// <summary>
        /// 根据对象ID获取属性数据
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public static List<Property> GetPropertyByBoid(string boid)
        {
            return SqlServerDBHelper.ExecuteQueryText<Property>(string.Format("SELECT NS,T.MD MD  FROM PROPERTY T WHERE T.BOID = '{0}'", boid));
        }
    }
}
