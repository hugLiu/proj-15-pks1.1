using Jurassic.GF.Interface;
using Jurassic.GF.Interface.Model;
using Jurassic.GF.Server.DBUtility;
using System.Collections.Generic;
using System.Text;

namespace Jurassic.GF.Server.Oracle
{
    public class ObjectTypeBusiness : IObjectType
    {
        /// <summary>
        /// 获取全部对象类型
        /// </summary>
        /// <returns></returns>
        public List<ObjectTypeModel> GetAllObjectType()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM OBJECTTYPE  ");
            return OracleDBHelper.OracleHelper.ExecuteQueryText<ObjectTypeModel>(strSql.ToString());
        }
    }
}
