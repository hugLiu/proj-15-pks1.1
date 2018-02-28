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
    public class HisPropertyBuiness : IHisBOProperty
    {
        public bool DelHisProperty(string boid, string ns)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  HISPROPERTY  ");
            strSql.Append(" WHERE BOID =@BOID AND NS=@NS");
            SqlParameter[] parameters = {
                            new SqlParameter("BOID", SqlDbType.VarChar,36),
                            new SqlParameter("NS", SqlDbType.VarChar,50)
                                            };
            parameters[0].Value = boid;
            parameters[1].Value = ns;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strSql.ToString()) > 0 ? true : false;
        }

        public bool ExistHisProperty(HisPropertyModel HisProperty)
        {
            throw new NotImplementedException();
        }

        public List<HisPropertyModel> GetHisPropertyByID(string boid)
        {
            throw new NotImplementedException();
        }

        public List<HisPropertyModel> GetHisPropertyByID(string boid, string ns, string gatherid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM HISPROPERTY T ");
            strSql.Append(" WHERE BOID=@BOID AND  GATHERID=@GATHERID ");
            SqlParameter[] parameters;
            if (!string.IsNullOrEmpty(ns))
            {
                strSql.Append(" AND NS=:NS ");
                parameters = new SqlParameter[]{
                                             new SqlParameter("BOID",SqlDbType.VarChar,36),
                                             new SqlParameter("GATHERID",SqlDbType.VarChar,50),
                                             new SqlParameter("NS",SqlDbType.VarChar,50)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = gatherid;
                parameters[2].Value = ns;
            }
            else
            {
                parameters = new SqlParameter[]{
                                    new SqlParameter("BOID",SqlDbType.VarChar,36),
                                    new SqlParameter("GATHERID",SqlDbType.VarChar,36)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = gatherid;
            }
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            List<HisPropertyModel> list = new List<HisPropertyModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    HisPropertyModel model = new HisPropertyModel();
                    model.BOID = item["BOID"].ToString();
                    model.GATHERID = item["GATHERID"].ToString();
                    model.MD = item["MD"].ToString();
                    model.MDSOURCE = item["MDSOURCE"].ToString();
                    model.NS = item["NS"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }

        public bool InsertHisProperty(HisPropertyModel HisProperty)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO HISPROPERTY(  ");
            strInsertSql.Append(" BOID,GATHERID,NS,MD,MDSOURCE)");
            strInsertSql.Append(" VALUES (@BOID,@GATHERID,@NS,@MD,@MDSOURCE)");

            SqlParameter[] parameters = {
                            new SqlParameter("BOID", SqlDbType.VarChar,36),
                            new SqlParameter("GATHERID", SqlDbType.VarChar,50),
                            new SqlParameter("NS", SqlDbType.VarChar,50),
                            new SqlParameter("MD", SqlDbType.Xml),
                            new SqlParameter("MDSOURCE", SqlDbType.VarChar,50)
                            };
            parameters[0].Value = HisProperty.BOID;
            parameters[1].Value = HisProperty.GATHERID;
            parameters[2].Value = HisProperty.NS;
            parameters[2].Value = HisProperty.MD;
            parameters[4].Value = HisProperty.MDSOURCE;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        public bool UpdateHisProperty(HisPropertyModel HisProperty)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE HISPROPERTY SET ");
            strUpdateSql.Append(" GATHERID=@GATHERID, MD=@MD ,MDSOURCE=@MDSOURCE ");
            strUpdateSql.Append(" WHERE BOID=@BOID AND NS=@NS");

            SqlParameter[] parameters = {
                            new SqlParameter("GATHERID", SqlDbType.VarChar,36),
                            new SqlParameter("MD", SqlDbType.Xml),
                            new SqlParameter("MDSOURCE", SqlDbType.VarChar,50),
                            new SqlParameter("BOID", SqlDbType.VarChar,36),
                            new SqlParameter("NS", SqlDbType.VarChar,50)
                                            };
            parameters[0].Value = HisProperty.GATHERID;
            parameters[1].Value = HisProperty.MD;
            parameters[2].Value = HisProperty.MDSOURCE;
            parameters[3].Value = HisProperty.BOID;
            parameters[4].Value = HisProperty.NS;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
