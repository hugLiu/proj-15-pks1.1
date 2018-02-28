using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using Jurassic.GeoFeature.DBUtility;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Jurassic.GeoFeature.Oracle
{
    public class ObjectTypeBusiness : IObjectType
    {

        public IList<ObjectTypeModel> GetListByTypeClass(string ClassId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOTID,BOT,FEATURETYPE,ISUSERDEFINE,SHAPE,NVL2(SHAPE,1,0) AS HASGEOMETRY  FROM OBJECTTYPE ");
            strSql.Append(" WHERE CLASSID =:CLASSID ");
            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("CLASSID", OracleDbType.Varchar2) { Value = ClassId });

            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<ObjectTypeModel>(strSql.ToString(),oraParams.ToArray());

        }

        public bool Exist(ObjectTypeModel model)
        {
            return false;
        }
        public int Insert(ObjectTypeModel model)
        {
            return 0;
        }
        public int Update(ObjectTypeModel model)
        {
            return 0;
        }
        public int Delete(ObjectTypeModel model)
        {
            return 0;
        }
        public IList<ObjectTypeModel> GetList()
        {
            List<ObjectTypeModel> List = new List<ObjectTypeModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM OBJECTTYPE");
            
            DataSet ds = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString());
            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ObjectTypeModel OBM = new ObjectTypeModel();
                    OBM.Botid = dr.ItemArray[0].ToString();
                    OBM.Bot = dr.ItemArray[1].ToString();

                    List.Add(OBM);
                }
            }
            return List;
        }
        public IList<ObjectTypeModel> GetListByID(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM OBJECTTYPE");
            strSql.Append(" WHERE BOTID = :BOTID");
            OracleParameter[] parameters = {
                                new OracleParameter("BOT", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = ID;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<ObjectTypeModel>(strSql.ToString(), parameters);
        }
        public IList<ObjectTypeModel> GetListByName(string bot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM OBJECTTYPE");
            strSql.Append(" WHERE BOT = :BOT");
            OracleParameter[] parameters = {
                                new OracleParameter("BOT", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = bot;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<ObjectTypeModel>(strSql.ToString(), parameters);
        }


    }
}
