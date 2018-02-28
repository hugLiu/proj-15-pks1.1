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
    public class BOPropertyBusiness : IBOProperty
    {
        public bool DelProperty(string boid, string ns = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  PROPERTY  ");
            strSql.Append(" WHERE BOID =@BOID");
            SqlParameter[] parameters;
            if (!string.IsNullOrEmpty(ns))
            {
                strSql.Append(" AND NS=@NS");
                parameters = new SqlParameter[]{
                                             new SqlParameter("BOID",SqlDbType.VarChar,36),
                                             new SqlParameter("NS",SqlDbType.VarChar,50)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = ns;
            }
            else
            {
                parameters = new SqlParameter[]{
                                    new SqlParameter("BOID",SqlDbType.VarChar,36)
                                           };
                parameters[0].Value = boid;
            }
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strSql.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 判断对象属性是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool ExistProperty(PropertyModel property)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM PROPERTY T ");
            strSql.Append(" WHERE BOID=@BOID AND NS=@NS ");
            SqlParameter[] parameters = { new SqlParameter("BOID",SqlDbType.VarChar,36),
                                             new SqlParameter("NS",SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = property.BOID;
            parameters[1].Value = property.NS;
            return DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters).Rows.Count > 0 ? true : false;
        }

        /// <summary>
        /// 根据对象ID和NS获取对象属性
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public List<PropertyModel> GetListByID(string boid, string ns)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM PROPERTY T ");
            strSql.Append(" WHERE BOID=@BOID  ");
            SqlParameter[] parameters;
            if (!string.IsNullOrEmpty(ns))
            {
                strSql.Append(" AND NS=@NS ");
                parameters = new SqlParameter[]{
                                             new SqlParameter("BOID",SqlDbType.VarChar,36),
                                             new SqlParameter("NS",SqlDbType.VarChar,50)
                                           };
                parameters[0].Value = boid;
                parameters[1].Value = ns;
            }
            else
            {
                parameters = new SqlParameter[]{
                                    new SqlParameter("BOID",SqlDbType.VarChar,36)
                                           };
                parameters[0].Value = boid;
            }
            List<PropertyModel> list = new List<PropertyModel>();
            DataTable dt = DBUtility.SqlServerDBHelper.GetDataTable(strSql.ToString(), parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    PropertyModel model = new PropertyModel();
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

        /// <summary>
        ///添加属性数据
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool InsertProperty(PropertyModel property)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO PROPERTY(  ");
            strInsertSql.Append(" BOID,GATHERID,NS,MD,MDSOURCE)");
            strInsertSql.Append(" VALUES (@BOID,@GATHERID,@NS,@MD,@MDSOURCE)");

            SqlParameter[] parameters = {
                            new SqlParameter("BOID", SqlDbType.VarChar,36),
                            new SqlParameter("GATHERID", SqlDbType.VarChar,50),
                            new SqlParameter("NS", SqlDbType.VarChar,50),
                            new SqlParameter("MD", SqlDbType.Xml),
                            new SqlParameter("MDSOURCE", SqlDbType.VarChar,50)
                            };
            parameters[0].Value = property.BOID;
            parameters[1].Value = property.GATHERID;
            parameters[2].Value = property.NS;
            parameters[2].Value = property.MD;
            parameters[4].Value = property.MDSOURCE;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strInsertSql.ToString(), parameters) > 0 ? true : false;
        }

        /// <summary>
        /// 修改属性数据
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool UpdateProperty(PropertyModel property)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE PROPERTY SET ");
            strUpdateSql.Append(" GATHERID=@GATHERID, MD=@MD ,MDSOURCE=@MDSOURCE ");
            strUpdateSql.Append(" WHERE BOID=@BOID AND NS=@NS");

            SqlParameter[] parameters = {
                new SqlParameter("GATHERID", SqlDbType.VarChar, 36),
                            new SqlParameter("MD", SqlDbType.Xml),
                            new SqlParameter("MDSOURCE", SqlDbType.VarChar, 50),
                            new SqlParameter("BOID", SqlDbType.VarChar, 36),
                            new SqlParameter("NS", SqlDbType.VarChar, 50)
                                             };
            parameters[0].Value = property.GATHERID;
            parameters[1].Value = property.MD;
            parameters[2].Value = property.MDSOURCE;
            parameters[3].Value = property.BOID;
            parameters[4].Value = property.NS;
            return DBUtility.SqlServerDBHelper.ExecuteCommand(strUpdateSql.ToString(), parameters) > 0 ? true : false;
        }
    }
}
