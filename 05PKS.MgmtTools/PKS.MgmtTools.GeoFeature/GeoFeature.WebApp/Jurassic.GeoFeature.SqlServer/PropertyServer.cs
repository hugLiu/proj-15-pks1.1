using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GeoFeature.SqlServer
{
    public class PropertyServer : IInterface<PropertyModel>
    {
        /// <summary>
        /// 判断参数是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool Exist(PropertyModel property)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM PROPERTY T ");
            strSql.Append(" WHERE BOID=@BOID AND NS=@NS ");
            SqlParameter[] parameters = { new SqlParameter("@BOID",SqlDbType.VarChar,36),
                                             new SqlParameter("@NS",SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = property.BOId;
            parameters[1].Value = property.NS;
            List<PropertyModel> propertyList= DBUtility.SqlServerDBHelper.ExecuteQueryText<PropertyModel>(strSql.ToString(), parameters);
            return propertyList.Count() >= 1 ? true : false;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int Insert(PropertyModel property)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO PROPERTY(  ");
            strInsertSql.Append(" BOID,NS,MD,MDSOURCE)");
            strInsertSql.Append(" VALUES (@BOID,@NS,@MD,@MDSOURCE)");

            SqlParameter[] parameters = {
                            new SqlParameter("@BOID", SqlDbType.VarChar,36),
                            new SqlParameter("@NS", SqlDbType.VarChar,50),
                            new SqlParameter("@MD", SqlDbType.Xml),
                            new SqlParameter("@MDSOURCE", SqlDbType.VarChar,50)
                            };
            parameters[0].Value = property.BOId;
            parameters[1].Value = property.NS;
            parameters[2].Value = property.MD;
            parameters[3].Value = property.MdSource;

            return DBUtility.SqlServerDBHelper.ExecuteSql(strInsertSql.ToString(), parameters);
        }
        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int Update(PropertyModel property)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE PROPERTY SET ");
            strUpdateSql.Append(" MD=@MD ,MDSOURCE=@MDSOURCE ");
            strUpdateSql.Append(" WHERE BOID=@BOID AND NS=@NS");

            SqlParameter[] parameters = {
                            new SqlParameter("@MD", SqlDbType.Xml),
                            new SqlParameter("@MDSOURCE", SqlDbType.VarChar,50),
                            new SqlParameter("@BOID", SqlDbType.VarChar,36),
                            new SqlParameter("@NS", SqlDbType.VarChar,50)
                                            };
            parameters[0].Value = property.MD;
            parameters[1].Value = property.MdSource;
            parameters[2].Value = property.BOId;
            parameters[3].Value = property.NS;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int Delete(PropertyModel property)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  PROPERTY  ");
            strSql.Append(" WHERE BOID =@BOID AND NS=@NS");
            SqlParameter[] parameters = {
                            new SqlParameter("@BOID", SqlDbType.VarChar,36),
                            new SqlParameter("@NS", SqlDbType.VarChar,50)
                                            };
            parameters[0].Value = property.BOId;
            parameters[1].Value = property.NS;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 获取某对象参数
        /// </summary>
        /// <returns></returns>
        public IList<PropertyModel> GetListByID(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  t.BOID,t.NS,T.MD,a.BOTID,b.md MdSource FROM PROPERTY t");
            strSql.Append(" join BO a on t.BOID=a.BOID");
            strSql.Append(" join OBJTYPEPROPERTY b on a.BOTID=b.BOTID and t.ns=b.ns");
            strSql.Append(" where t.BOID=@BOID");
          
            SqlParameter[] parameters = {
                            new SqlParameter("@BOID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = boid;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<PropertyModel>(strSql.ToString(), parameters);
        }

        public IList<PropertyModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NS,T.MD  FROM PROPERTY  T");
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<PropertyModel>(strSql.ToString());
        }

        public IList<PropertyModel> GetListByName(string Name)
        {
            throw new NotImplementedException();
        }
    }
}
