using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System.Data.SqlClient;
using System.Data;
namespace Jurassic.GeoFeature.SqlServer
{
    public class RelTypeBusiness : IRelType
    {
        public bool Exist(RelTypeModel reltype)
        {
            return false;
        }
        /// <summary>
        /// 判断对象关系类型是否存在
        /// </summary>
        /// <param name="reltype"></param>
        /// <returns></returns>
        public bool Exist(RelTypeModel reltype, ref string RTID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM RELTYPE T ");
            strSql.Append(" WHERE (REMARK=@REMARK) or (BOTID1=@BOTID1 and BOTID2=@BOTID2)");
            SqlParameter[] parameters = {
                              new SqlParameter("@REMARK",SqlDbType.VarChar,50),
                              new SqlParameter("@BOTID1",SqlDbType.VarChar,36),
                              new SqlParameter("@BOTID2",SqlDbType.VarChar,36)
                                           };
            parameters[0].Value = reltype.Title;
            parameters[1].Value = reltype.Botid1;
            parameters[2].Value = reltype.Botid2;
            RelTypeModel model = DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(strSql.ToString(), parameters).FirstOrDefault();
            RTID = model == null ? System.Guid.NewGuid().ToString() : model.Rtid;
            return model == null ? false : true;
        }
        /// <summary>
        /// 添加对象关系类型
        /// </summary>
        /// <param name="reltype"></param>
        /// <returns></returns>
        public int Insert(RelTypeModel reltype)
        {
            StringBuilder strInsertSql = new StringBuilder();
            strInsertSql.Append(" INSERT INTO RELTYPE(  ");
            strInsertSql.Append(" RT,BOT1,BOT2) ");
            strInsertSql.Append(" VALUES (@RT,@REMARK,@BOT1,@BOT2 )");

            SqlParameter[] parameters = {
                            new SqlParameter("@RT", SqlDbType.VarChar,36),     
                            new SqlParameter("@REMARK", SqlDbType.VarChar,100),     
                            new SqlParameter("@BOT1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOT2", SqlDbType.VarChar,36)
                            };
            parameters[0].Value = reltype.RT;
            parameters[1].Value = reltype.Title;
            parameters[2].Value = reltype.Bot1;
            parameters[3].Value = reltype.Bot2;

            return DBUtility.SqlServerDBHelper.ExecuteSql(strInsertSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新对象关系类型
        /// </summary>
        /// <param name="reltype"></param>
        /// <returns></returns>
        public int Update(RelTypeModel reltype)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE RELTYPE SET ");
            strUpdateSql.Append(" TITLE=@TITLE,BOT1=@BOT1,BOT2=@BOT2");
            strUpdateSql.Append(" WHERE RT=@RT");

            SqlParameter[] parameters = {
                            new SqlParameter("@TITLE", SqlDbType.VarChar,100),
                            new SqlParameter("@BOT1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOT2", SqlDbType.VarChar,36),
                            new SqlParameter("@RT", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = reltype.Title;
            parameters[1].Value = reltype.Bot1;
            parameters[2].Value = reltype.Bot2;
            parameters[3].Value = reltype.RT;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除对象关系类型
        /// </summary>
        /// <param name="reltype"></param>
        /// <returns></returns>
        public int Delete(RelTypeModel reltype)
        {
            List<Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity> sqlList = new List<Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity>();
            StringBuilder DelRelationSql = new StringBuilder();
            DelRelationSql.Append(" DELETE   RELATION  ");
            DelRelationSql.Append(" WHERE RTID IN ");
            DelRelationSql.Append(" (SELECT RTID FROM RELTYPE ");
            DelRelationSql.Append(" WHERE RT=@RT) ");


            StringBuilder DelRelTypeSql = new StringBuilder();
            DelRelTypeSql.Append(" DELETE   RELTYPE  ");
            DelRelTypeSql.Append(" WHERE RT=@RT");
            SqlParameter[] parameters = {
                            new SqlParameter("@RT", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = reltype.RT;

            DBUtility.SqlServerDBHelper.SQLEntity DelRelationEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
            DelRelationEntity.Sqlstr = DelRelationSql.ToString();
            DelRelationEntity.Sqlparameter = parameters;
            DBUtility.SqlServerDBHelper.SQLEntity DelRelTypeEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
            DelRelTypeEntity.Sqlstr = DelRelTypeSql.ToString();
            DelRelTypeEntity.Sqlparameter = parameters;
            sqlList.Add(DelRelationEntity);
            sqlList.Add(DelRelTypeEntity);
            return DBUtility.SqlServerDBHelper.ExecuteSql(sqlList) == true ? 1 : 0;
        }


        public List<string> GetBOTbyName(string BOTName1, string BOTName2)
        {
            string strSql = "select  USEGEOMETRY=case USEGEOMETRY   " +
	                         "  when 1 then '1' " +
	                         "  when 0 then BOTID " +
	                         "  end " +
	                         "   from objecttype t  where t.bot='" + BOTName1 + "'    " +
	                         "  union all   " +  
	                         "   select  USEGEOMETRY=case USEGEOMETRY  " +
	                         "  when 1 then '1' " +
	                         "  when 0 then BOTID " +
	                         "  end " +
	                         "   from objecttype t  where t.bot='" + BOTName2 + "' ";           
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<string>(strSql.ToString()); ;
        }

        public List<string> GetBOTRelByName(string BOTName1, string BOTName2)
        {
            string strSql = " select  USEGEOMETRY=case USEGEOMETRY  " +
                            "    when 1 then '1' " +
	                        "    when 0 then BOTID " +
	                        "    end " +
                            "     from objecttype t  where t.bot='" + BOTName1 + "' " +   
	                        "    union all    " + 
	                        "     select  USEGEOMETRY=case USEGEOMETRY  " +
                            "    when 1 then '1' " +
	                        "    when 0 then BOTID " +
	                        "    end " +
                            "     from objecttype t  where t.bot='" + BOTName2 + "' ";         
                         
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<string>(strSql.ToString()); ;
        }

        /// <summary>
        /// 根据对象类型获取对象关系类型
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="direction">关联方向，包括正向（Forward），反向（Backward），如果为空则两个方向都查询</param>
        /// <returns></returns>
        public IList<RelTypeModel> GetListByBot(string bot, string direction = null)
        {
            StringBuilder strSql = new StringBuilder();
            if (direction == "Forward")
                strSql.Append("SELECT * FROM RELTYPE R WHERE R.BOT1=@BOT");
            else if (direction == "Backward")
                strSql.Append("SELECT * FROM RELTYPE R WHERE R.BOT2=@BOT");
            else
            {
                strSql.Append("SELECT * FROM RELTYPE R WHERE R.BOT1=@BOT");
                strSql.Append(" UNION ");
                strSql.Append("SELECT * FROM RELTYPE R WHERE R.BOT2=@BOT");
            }
            SqlParameter[] parameters = {
                            new SqlParameter("@BOT", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = bot;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(strSql.ToString());
        }

        /// <summary>
        /// 根据id查找对象关系类型
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        public IList<RelTypeModel> GetListByID(string rtid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM RELTYPE R WHERE RT=@RT");
            SqlParameter[] parameters = {
                            new SqlParameter("@RT", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = rtid;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(strSql.ToString());
        }

        /// <summary>
        /// 根据名称查找对象关系类型
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public IList<RelTypeModel> GetListByName(string title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM RELTYPE R WHERE TITLE=@TITLE");
            SqlParameter[] parameters = {
                            new SqlParameter("@TITLE", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = title;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(strSql.ToString());
        }

        public IList<RelTypeModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.RTID,T.BOTID1,T.BOTID2,T.RT AS RT,T1.BOT AS BOT1,T2.BOT AS BOT2,t.REMARK as Title  FROM RELTYPE T, ");
            strSql.Append(" OBJECTTYPE T1, OBJECTTYPE T2 ");
            strSql.Append(" WHERE T.BOTID1 = T1.BOTID ");
            strSql.Append(" AND T.BOTID2 = T2.BOTID ");
            strSql.Append(" ORDER BY RT ");
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(strSql.ToString());
        }


        public List<RelTypeModel> GetRelTypeNameByID(string BOTID)
        {
            string Sql = "select  RTID,remark  as Title   from RELTYPE where BOTID1='" + BOTID + "' " +
                       " or BOTID2='" + BOTID + "'";
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(Sql);
        }

        /// <summary>
        /// 添加对象类型关系,包括实例的关系
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddRelationModel(List<RelTypeModel> list)
        {
            List<Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity> sqlList = new List<Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity>();

            foreach (var item in list)
            {
                Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity sqlEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                StringBuilder strInsertSql = new StringBuilder();
                strInsertSql.Append(" INSERT INTO RELTYPE( ");
                strInsertSql.Append(" RTID,RT,REMARK,BOTID1,BOTID2) ");
                strInsertSql.Append(" VALUES (@RTID,@RT,@REMARK,@BOTID1,@BOTID2) ");

                SqlParameter[] parameters = {
                            new SqlParameter("@RTID", SqlDbType.VarChar,36),
                            new SqlParameter("@RT", SqlDbType.VarChar,36),    
                            new SqlParameter("@REMARK", SqlDbType.VarChar,100),    
                            new SqlParameter("@BOTID1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOTID2", SqlDbType.VarChar,36)
                            };
                parameters[0].Value = item.Rtid;
                parameters[1].Value = item.RT;
                parameters[2].Value = item.Title;
                parameters[3].Value = item.Botid1;
                parameters[4].Value = item.Botid2;
                sqlEntity.Sqlstr = strInsertSql.ToString();
                sqlEntity.Sqlparameter = parameters;
                sqlList.Add(sqlEntity);
            }
            return DBUtility.SqlServerDBHelper.ExecuteSql(sqlList) == true ? 1 : 0;
        }

        /// <summary>
        /// 获取全部关系类型
        /// </summary>
        /// <returns></returns>
        public IList<RelTypeModel> GetAllRelationRT()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT A.RTID,A.RT ");
            strSql.Append(" FROM RELTYPE A,");
            strSql.Append("(SELECT MAX(RTID) RTID ");
            strSql.Append(" FROM RELTYPE");
            strSql.Append(" GROUP BY RT) B ");
            strSql.Append(" WHERE A.RTID  = B.RTID ");
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(strSql.ToString());
        }

        /// <summary>
        /// 根据对象类型关系rt获取对象类型关系实例
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        public IList<ObjTypeRelationModel> GetRelationListByrt(string rt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.RTID,T.RT,T.BOTID1,T.BOTID2, T1.BOT AS BOT1, T2.BOT AS BOT2");
            strSql.Append(" FROM RELTYPE T, OBJECTTYPE T1, OBJECTTYPE T2 ");
            strSql.Append(" WHERE T.BOTID1 = T1.BOTID ");
            strSql.Append(" AND T.BOTID2 = T2.BOTID ");
            strSql.Append(" AND T.RT = @RT ");
            SqlParameter[] parameters = {
                            new SqlParameter("@RT", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = rt;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<ObjTypeRelationModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据对象关系实例ID删除对象关系
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        public int DeleteRelTypeByRtID(string rtid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM  RELTYPE  ");
            strSql.Append(" WHERE RTID=@RTID");
            SqlParameter[] parameters = {
                            new SqlParameter("@RTID", SqlDbType.VarChar,36)
                                            };
            parameters[0].Value = rtid;
            return DBUtility.SqlServerDBHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 修改对象关系类型
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oldRT"></param>
        /// <returns></returns>
        public int UpdateRelTypeByRt(List<RelTypeModel> list, string oldRT)
        {
            //1.根据RT查询系统中所有的对象类型 2.对比修改、添加、删除
            IList<ObjTypeRelationModel> RelationList = GetRelationListByrt(oldRT);
            List<Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity> sqlList = new List<Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity>();

            //修改关系类型的实例
            foreach (var item in list)
            {
                ObjTypeRelationModel seachModel = (from p in RelationList where p.Botid1 == item.Botid1 && p.Botid2 == item.Botid2 && p.Rt == oldRT select p).FirstOrDefault();
                if (seachModel != null)
                {
                    #region 修改部分
                    Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity sqlUpdateEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                    StringBuilder UpdateRelTypeSql = new StringBuilder();
                    UpdateRelTypeSql.Append(" UPDATE RELTYPE ");
                    UpdateRelTypeSql.Append(" SET TITLE=@TITLE,RT=@RT ");
                    UpdateRelTypeSql.Append(" WHERE RTID=@RTID ");
                    SqlParameter[] parameters = {
                            new SqlParameter("@TITLE", SqlDbType.VarChar,36),
                            new SqlParameter("@RT", SqlDbType.VarChar,36),
                            new SqlParameter("@RTID", SqlDbType.VarChar,36)
                                            };
                    parameters[0].Value = item.Title;
                    parameters[1].Value = item.RT;
                    parameters[2].Value = item.Rtid;
                    sqlUpdateEntity.Sqlstr = UpdateRelTypeSql.ToString();
                    sqlUpdateEntity.Sqlparameter= parameters;
                    sqlList.Add(sqlUpdateEntity);
                    #endregion
                }
                else
                {
                    #region 新增部分
                    Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity sqlAddEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                    StringBuilder strInsertSql = new StringBuilder();
                    strInsertSql.Append(" INSERT INTO RELTYPE( ");
                    strInsertSql.Append(" RTID,RT,TITLE,BOTID1,BOTID2) ");
                    strInsertSql.Append(" VALUES (@RTID,@RT,@TITLE,@BOTID1,@BOTID2) ");

                    SqlParameter[] parameters = {
                            new SqlParameter("@RTID", SqlDbType.VarChar,36),
                            new SqlParameter("@RT", SqlDbType.VarChar,36),
                            new SqlParameter("@TITLE", SqlDbType.VarChar,100),
                            new SqlParameter("@BOTID1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOTID2", SqlDbType.VarChar,36)
                            };
                    parameters[0].Value = System.Guid.NewGuid().ToString();
                    parameters[1].Value = item.RT;
                    parameters[2].Value = item.Title;
                    parameters[3].Value = item.Botid1;
                    parameters[4].Value = item.Botid2;
                    sqlAddEntity.Sqlstr = strInsertSql.ToString();
                    sqlAddEntity.Sqlparameter = parameters;
                    sqlList.Add(sqlAddEntity);
                    #endregion
                }
            }

            #region 删除部分
            foreach (var item in RelationList)
            {
                RelTypeModel seachModel = (from p in list where p.Botid1 == item.Botid1 && p.Botid2 == item.Botid2 && item.Rt == oldRT select p).FirstOrDefault();
                if (seachModel == null)
                {
                    //删除对象关系实例
                    Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity sqlDelEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                    StringBuilder DelRelationSql = new StringBuilder();
                    DelRelationSql.Append(" DELETE RELATION ");
                    DelRelationSql.Append(" WHERE RTID =(SELECT RTID FROM RELTYPE WHERE BOTID1 =@BOTID1 AND BOTID2=@BOTID2 AND RT=@RT) ");
                    SqlParameter[] parameters = {
                            new SqlParameter("@BOTID1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOTID2", SqlDbType.VarChar,36),
                            new SqlParameter("@RT", SqlDbType.VarChar,50)
                                                        };
                    parameters[0].Value = item.Botid1;
                    parameters[1].Value = item.Botid2;
                    parameters[2].Value = oldRT;
                    sqlDelEntity.Sqlstr = DelRelationSql.ToString();
                    sqlDelEntity.Sqlparameter = parameters;
                    sqlList.Add(sqlDelEntity);

                    //删除类型关系实例
                    Jurassic.GeoFeature.DBUtility.SqlServerDBHelper.SQLEntity Entity = new DBUtility.SqlServerDBHelper.SQLEntity();
                    StringBuilder delRelTypeSql = new StringBuilder();
                    delRelTypeSql.Append(" DELETE RELTYPE ");
                    delRelTypeSql.Append(" WHERE RTID =(SELECT RTID FROM RELTYPE WHERE BOTID1 =@BOTID1 AND BOTID2=@BOTID2 AND RT=@RT) ");
                    SqlParameter[] parameters1 = {
                            new SqlParameter("@BOTID1", SqlDbType.VarChar,36),
                            new SqlParameter("@BOTID2", SqlDbType.VarChar,36),
                            new SqlParameter("@RT", SqlDbType.VarChar,50)
                                                        };
                    parameters1[0].Value = item.Botid1;
                    parameters1[1].Value = item.Botid2;
                    parameters1[2].Value = oldRT;
                    Entity.Sqlstr = delRelTypeSql.ToString();
                    Entity.Sqlparameter = parameters1;
                    sqlList.Add(Entity);
                }
            }
            #endregion
            return DBUtility.SqlServerDBHelper.ExecuteSql(sqlList) == true ? 1 : 0;
        }

        /// <summary>
        /// 获取全部对象类型关系
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllRelation()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            dt1 = DBUtility.SqlServerDBHelper.ExecuteQueryText(" select DISTINCT RT from reltype ").Tables[0];
            dt1.TableName = "reltypeRT";
            DataTable dt = new DataTable();
            dt = DBUtility.SqlServerDBHelper.ExecuteQueryText(" select *  from reltype ").Tables[0];
            dt.TableName = "reltypeAll";
            ds.Tables.Add(dt1.Copy());
            ds.Tables.Add(dt.Copy());
            return ds;
        }

        /// <summary>
        /// 获取所有关系类型树，或者根据节点取得子树
        /// </summary>
        /// <param name="rootList"></param>
        /// <returns></returns>
        public IList<RelTypeModel> GetRelTree(List<string> rootList = null)
        {
            StringBuilder sql = new StringBuilder();
            var oraParams = new List<SqlParameter>();
            sql.Append("SELECT * FROM RELTYPE R START WITH R.BOTID1 IN");
            if (rootList == null)
            {
                sql.Append("(SELECT A.BOTID1 FROM RELTYPE A WHERE A.BOTID1 NOT IN (SELECT B.BOTID2 FROM RELTYPE B))");
            }
            else
            {
                sql.Append("(@BOTID1)");
                var counter = 0;
                var collectionParams = new StringBuilder(":");
                foreach (var obj in rootList)
                {
                    var param = "@BOTID1" + counter;
                    collectionParams.Append(param);
                    collectionParams.Append(", :");
                    oraParams.Add(new SqlParameter(param, SqlDbType.VarChar) { Value = obj });
                    counter++;
                }
                collectionParams.Remove(collectionParams.Length - 3, 3);

                sql = sql.Replace("@BOTID1", collectionParams.ToString());
            }
            sql.Append("CONNECT BY R.BOTID1 = PRIOR R.BOTID2");

            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(sql.ToString(), oraParams.ToArray());
        }

        public IList<RelTypeModel> GetRelTreeRoot()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT A.* FROM RELTYPE A WHERE A.BOTID1 NOT IN (SELECT B.BOTID2 FROM RELTYPE B)");
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(sql.ToString());
        }

        public IList<RelTypeModel> GetRelTreeSubNode(string botid1)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT A.* FROM RELTYPE A WHERE A.BOTID1 =@BOTID1");
            var oraParams = new List<SqlParameter>();
            oraParams.Add(new SqlParameter("@BOTID1", SqlDbType.VarChar) { Value = botid1 });

            return DBUtility.SqlServerDBHelper.ExecuteQueryText<RelTypeModel>(sql.ToString(), oraParams.ToArray());
        }

        public DataTable GetRelTable(string RTID)
        {
            DataSet ds = new DataSet();
            string Sql = "select RTID,关系名称,关系类型,对象类型1,对象类型2 from( " +
                           " select  '0' SortID,  RTID , REMARK as 关系名称,RT as 关系类型, b.bot as 对象类型1,c1.bot as 对象类型2  from " +
                           " reltype  a ,objecttype b,objecttype c1 " +
                           "  where  RTID='" + RTID + "'  " +
                           "  and a.botid1=b.botid and a.botid2=c1.botid " +
                           "  union all " +
                           " select '1' SortID,RELATIONID RTID ,'' 关系名称,'' 关系类型,c.name 对象类型1,d.name 对象类型1   " +
                           " from relation b ,bo c ,bo d " +
                           "  where RTID='" + RTID + "' and  " +
                           " b.boid1=c.boid and b.boid2=d.boid " +
                           " ) ss order by SortID asc";

            ds = DBUtility.SqlServerDBHelper.ExecuteQueryText(Sql);
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
        }

        public bool DelBOTRel(List<string> RTID, string BOTorBO)
        {
            List<string> ListSql = new List<string>();
            string Sql = "";
            if (BOTorBO == "BOT")//删除对象类型及该类型下对象实例
            {
                Sql = "delete from Relation where RELATIONID='" + RTID[0] + "'";
                ListSql.Add(Sql);

                Sql = "delete from RelType where RTID='" + RTID[0] + "'";
                ListSql.Add(Sql);
            }
            else//只删除对象实例
            {
                foreach (string tmplist in RTID)
                {
                    Sql = "delete from Relation where RELATIONID='" + tmplist + "'";
                    ListSql.Add(Sql);
                }
            }
            string[] Sqls = ListSql.ToArray();
            if (Sqls.Length > 0)
                return DBUtility.SqlServerDBHelper.ExecuteSql(Sqls);
            else
                return false;
        }
    }
}
