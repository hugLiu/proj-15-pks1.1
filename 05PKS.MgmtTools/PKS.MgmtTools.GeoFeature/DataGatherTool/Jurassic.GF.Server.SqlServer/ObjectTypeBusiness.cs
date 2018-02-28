using Jurassic.GF.Interface;
using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Server.SqlServer
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
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString());
            List<ObjectTypeModel> list = new List<ObjectTypeModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    ObjectTypeModel model = new ObjectTypeModel();
                    model.BOT = item["BOT"].ToString();
                    model.BOTID = item["BOTID"].ToString();
                    model.FT = item["FT"].ToString();
                    model.ID = item["ID"].ToString();
                    model.NAME = item["NAME"].ToString();
                    model.SHAPE = item["SHAPE"].ToString();
                    model.ISUSERDEFINEE = item["ISUSERDEFINE"].ToString();
                    model.USEGEOMETRY = item["USEGEOMETRY"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
