using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System.Data.SqlClient;
using Jurassic.GeoFeature.DBUtility;
namespace Jurassic.GeoFeature.SqlServer
{
    public class RelationBusiness : IRelation
    {
        /// <summary>
        /// 判断对象关系是否存在
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public bool Exist(RelationModel relation)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM RELATION T ");
            strSql.Append(" WHERE BOID1=@BOID1 AND BOID2=@BOID2 AND RTID=@RTID ");
            SqlParameter[] parameters = {
                                               new SqlParameter("@BOID1",SqlDbType.VarChar,36),
                                               new SqlParameter("@BOID2",SqlDbType.VarChar,36),
                                               new SqlParameter("@RTID",SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = relation.BOId1;
            parameters[1].Value = relation.BOId2;
            parameters[2].Value = relation.RTID;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelationModel>(strSql.ToString(), parameters).Count >= 1 ? true : false;
        }

        /// <summary>
        /// 添加对象关系
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public int Insert(RelationModel relation)
        {
            int irtn = 0;
            bool Exist = true;
            StringBuilder strInsertSql = new StringBuilder();
            string Sql = "select * from RELATION where RTID='" + relation.RTID + "' and BOID1='" + relation.BOId1 + "' and BOID2='" + relation.BOId2 + "'";
            RelationModel model = DBUtility.SqlServerDBHelper.ExecuteQueryText<RelationModel>(Sql).FirstOrDefault();
            Exist = model == null ? false : true;
            if (!Exist)  //去重处理，只添加不存在的对象关系          
            {
                strInsertSql.Append(" INSERT INTO RELATION(  ");
                strInsertSql.Append(" RELATIONID,RTID,BOID1,BOID2) ");
                strInsertSql.Append(" VALUES (@RELATIONID,@RTID,@BOID1,@BOID2 )");

                SqlParameter[] parameters = {
                            new SqlParameter("@RELATIONID", SqlDbType.VarChar,36),
                            new SqlParameter("@RTID", SqlDbType.VarChar,36),
                            new SqlParameter("@BOID1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOID2", SqlDbType.VarChar,50)
                            };
                parameters[0].Value = System.Guid.NewGuid().ToString();
                parameters[1].Value = relation.RTID;
                parameters[2].Value = relation.BOId1;
                parameters[3].Value = relation.BOId2;

                irtn = DBUtility.SqlServerDBHelper.ExecuteSql(strInsertSql.ToString(), parameters);
            }
            return irtn;
        }

        /// <summary>
        /// 修改对象关系
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public int Update(RelationModel relation)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE RELATION SET ");
            strUpdateSql.Append(" BOID1=@BOID1,BOID2=@BOID2 ");
            strUpdateSql.Append(" WHERE RELATIONID=@RELATIONID");

            SqlParameter[] parameters = {
                            new SqlParameter("@BOID1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOID2", SqlDbType.VarChar,36),
                            new SqlParameter("@RELATIONID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = relation.BOId1;
            parameters[1].Value = relation.BOId2;
            parameters[2].Value = relation.RelationID;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除对象关系
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public int Delete(RelationModel relation)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  RELATION  ");
            strSql.Append(" WHERE RELATIONID=@RELATIONID ");
            SqlParameter[] parameters = {
                            new SqlParameter("@RELATIONID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = relation.RelationID;

            return DBUtility.SqlServerDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public List<string> GetBObyName(string BOName1, string BOName2)
        {
            string strSql=" select BOID=case count(*)  " +
		                 "  when 0 then '0' " +
		                 "  else t.BOID " +
                         "  end from bo t where t.name='" + BOName1 + "' group by BOID " +
		                 "  union all   " +
		                 "  select BOID=case count(*)  " +
		                 "  when 0 then '0' " +
		                 "  else t.BOID " +
                         "  end from bo t where t.name='" + BOName2 + "'   group by BOID ";                     
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<string>(strSql.ToString()); ;
        }
        public IList<RelationModel> GetListByID(string boid)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT * FROM RELATION R WHERE R.BOID1=@BOID ");
            strSql.Append("UNION ");
            strSql.Append("SELECT * FROM RELATION R WHERE R.BOID2=@BOID ");
            SqlParameter[] parameters = {
                            new SqlParameter("@BOID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = boid;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelationModel>(strSql.ToString());
        }

        /// <summary>
        /// 根据对象ID查询对象关系
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="direction">查询方向，包括正向（Forward），反向（Backward），为空则两个方向都查</param>
        /// <returns></returns>
        public IList<RelationModel> GetListByID(string boid, string direction = null)
        {
            StringBuilder strSql = new StringBuilder();
            if (direction == "Forward")
                strSql.Append("SELECT * FROM RELATION R WHERE R.BOID1=@BOID");
            else if (direction == "Backward")
                strSql.Append("SELECT * FROM RELATION R WHERE R.BOID2=@BOID");
            else
            {
                strSql.Append("SELECT * FROM RELATION R WHERE R.BOID1=@BOID ");
                strSql.Append("UNION ");
                strSql.Append("SELECT * FROM RELATION R WHERE R.BOID2=@BOID ");
            }
            SqlParameter[] parameters = {
                            new SqlParameter("@BOID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = boid;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelationModel>(strSql.ToString());
        }

        public IList<RelationModel> GetListByName(string Name)
        {
            throw new NotImplementedException();
        }
        public IList<RelationModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM RELATION R ");
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelationModel>(strSql.ToString());
        }


        /// <summary>
        /// 根据类型关系ID获取对象关系的实例
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        public IList<RelationModel> GetBoRelTypeListByRTID(string rtid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.*,T1.NAME AS NAME1, T2.NAME AS NAME2,T3.RT FROM RELATION T,BO T1,BO T2 ,RelType t3");
            strSql.Append(" WHERE T.BOID1=T1.BOID ");
            strSql.Append(" AND T.BOID2=T2.BOID ");
            strSql.Append(" AND T.RTID=T3.RTID ");
            strSql.Append(" AND T.RTID=@RTID ");
            SqlParameter[] parameters = {
                            new SqlParameter("@RTID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = rtid;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelationModel>(strSql.ToString(), parameters);
        }
    }
}
