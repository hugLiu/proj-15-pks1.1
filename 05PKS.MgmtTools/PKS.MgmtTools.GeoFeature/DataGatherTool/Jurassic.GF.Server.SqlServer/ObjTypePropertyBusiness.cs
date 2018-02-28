using Jurassic.GF.Interface;
using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Server.SqlServer
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
            strSql.Append(" SELECT *  FROM OBJTYPEPROPERTY  ");
            strSql.Append(" WHERE  BOTID =@BOTID ");

            SqlParameter[] parameters = {
                                new SqlParameter("BOTID", System.Data.SqlDbType.VarChar,36)
                                           };
            parameters[0].Value = BOTID;
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            List<ObjTypePropertyModel> list = new List<ObjTypePropertyModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ObjTypePropertyModel model = new ObjTypePropertyModel();
                    model.BOTID = item["BOTID"].ToString();
                    model.ISUSERDEFINE = item["ISUSERDEFINE"].ToString();
                    model.MD = item["MD"].ToString();
                    model.NS = item["NS"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
