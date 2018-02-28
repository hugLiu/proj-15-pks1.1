using Jurassic.GF.Interface;
using System.Collections.Generic;
using System.Text;
using Jurassic.GF.Interface.Model;
using Oracle.ManagedDataAccess.Client;
using Jurassic.GF.Server.DBUtility;

namespace Jurassic.GF.Server.Oracle
{
    public class ObjTypePropertyBusiness : IObjTypeProperty
    {
        /// <summary>
        /// 根据对象类型ID获取对象类型的参数集信息
        /// </summary>
        /// <returns></returns>
        public List<ObjTypePropertyModel> GetObjPropertyByBOTID(string BOTID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select botid, ns, t.md.getclobval() md, isuserdefine, rowid from OBJTYPEPROPERTY t  ");
            strSql.Append(" WHERE  BOTID =:BOTID ");

            OracleParameter[] parameters = {
                                new OracleParameter("BOTID", OracleDbType.Varchar2,36)
                                           };
            parameters[0].Value = BOTID;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<ObjTypePropertyModel>(strSql.ToString(), parameters);
        }
    }
}
