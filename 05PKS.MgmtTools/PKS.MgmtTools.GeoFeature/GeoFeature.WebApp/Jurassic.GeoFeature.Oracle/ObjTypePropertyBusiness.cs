using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using Jurassic.GeoFeature.DBUtility;
using Oracle.ManagedDataAccess.Client;
using System.Xml;

namespace Jurassic.GeoFeature.Oracle
{
    public class ObjTypePropertyBusiness : IObjectTypeProperty
    {
        public IList<ObjTypePropertyModel> GetListByID(string BotId)
        {
            List<ObjTypePropertyModel> list = new List<ObjTypePropertyModel>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM OBJTYPEPROPERTY ");
            strSql.Append(" WHERE BOTID =:BOTID   order by ISUSERDEFINE desc");
            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("BOTID", OracleDbType.Varchar2) { Value = BotId });
            list.AddRange(DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<ObjTypePropertyModel>(strSql.ToString(), oraParams.ToArray()));
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
            strSql.Append(" WHERE T.BOTID=:BOTID AND T.NS=:NS");
            OracleParameter[] parameters = {
                            new OracleParameter("BOTID", OracleDbType.Varchar2,36),
                            new OracleParameter("NS", OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = BOTID;
            parameters[1].Value = Name;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<ObjTypePropertyModel>(strSql.ToString(), parameters);
        }

    }
}
