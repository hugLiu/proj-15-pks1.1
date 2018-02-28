using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using Jurassic.GeoFeature.DBUtility;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
namespace Jurassic.GeoFeature.SqlServer
{
    public  class ObjTypePropertyBusiness : IObjectTypeProperty
    {
        public IList<ObjTypePropertyModel> GetListByID(string BotId)
        {
            List<ObjTypePropertyModel> list = new List<ObjTypePropertyModel>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM OBJTYPEPROPERTY ");
            strSql.Append(" WHERE BOTID =@BOTID  order by ISUSERDEFINE desc");
               
            System.Data.SqlClient.SqlParameter[] oraParams = new SqlParameter[]
             {
                 new SqlParameter("@BOTID",SqlDbType.VarChar)
             };

            oraParams[0].Value = BotId;
           

            list.AddRange(DBUtility.SqlServerDBHelper.ExecuteQueryText<ObjTypePropertyModel>(strSql.ToString(), oraParams.ToArray()));
            return list;

        }

        public bool Exist(ObjTypePropertyModel model)
        {
            return false;
        }
        public int Insert(ObjTypePropertyModel model)
        {
            return 0;
        }
        public int Update(ObjTypePropertyModel model)
        {
            return 0;
        }
        public int Delete(ObjTypePropertyModel model)
        {
            return 0;
        }
        public IList<ObjTypePropertyModel> GetList()
        {
            return null;
        }
        public IList<ObjTypePropertyModel> GetListByName(string Name)
        { return null; }
        public List<ObjTypePropertyModel> GetBOTProp(string BOTID, string Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.*,O.BOT AS BOTNAME FROM OBJTYPEPROPERTY T");
            strSql.Append(" JOIN OBJECTTYPE O ON T.BOTID = O.BOTID ");
            strSql.Append(" WHERE T.BOTID=@BOTID AND T.NS=@NS");
            SqlParameter[] parameters = {
                            new SqlParameter("@BOTID",  SqlDbType.VarChar,36),
                            new SqlParameter("@NS",  SqlDbType.VarChar,50)
                                                };
            parameters[0].Value = BOTID;
            parameters[1].Value = Name;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<ObjTypePropertyModel>(strSql.ToString(), parameters);
        }

    }
}
