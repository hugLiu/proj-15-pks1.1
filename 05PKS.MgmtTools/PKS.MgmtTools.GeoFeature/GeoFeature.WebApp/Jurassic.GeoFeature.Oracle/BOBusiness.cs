using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Data;

namespace Jurassic.GeoFeature.Oracle
{
    /// <summary>
    /// 基于Oracle的数据访问层
    /// </summary>
    public class BOBusiness : IBO
    {
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Exist(BOModel bo)
        {
            return GetBoListByName(bo.Name, "").Count() >= 1 ? true : false;
        }
        ///// <summary>
        ///// 添加对象
        ///// </summary>
        ///// <param name="feature"></param>
        ///// <returns></returns>
        public int Insert(BOModel model)
        {
            StringBuilder strAddSql = new StringBuilder();
            strAddSql.Append(" INSERT INTO BO(  ");
            strAddSql.Append(" BOID,NAME,BOTID,ISUSE)");
            strAddSql.Append(" VALUES (:BOID,:NAME,:BOTID,:ISUSE)");

            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),                               
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Varchar2,50),                             
                                new OracleParameter("ISUSE", OracleDbType.Char,1)
                                               };
            parameters[0].Value = System.Guid.NewGuid().ToString();
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Botid;
            parameters[3].Value = model.Isuse;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strAddSql.ToString(), parameters);
        }

        ///// <summary>
        ///// 修改对象
        ///// </summary>
        ///// <param name="feature"></param>
        ///// <returns></returns>
        public int Update(BOModel model)
        {
            StringBuilder strUpdateSql = new StringBuilder();
            strUpdateSql.Append(" UPDATE BO SET ");
            strUpdateSql.Append(" NAME=:NAME,BOTID= :BOTID,ISUSE=:ISUSE");
            strUpdateSql.Append(" WHERE BOID=:BOID");

            OracleParameter[] parameters = {                               
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Varchar2,50), 
                                new OracleParameter("ISUSE", OracleDbType.Char,1),
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Botid;
            parameters[2].Value = model.Isuse;
            parameters[3].Value = model.Boid;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(strUpdateSql.ToString(), parameters);
        }



        ///// <summary>
        ///// 删除对象
        ///// </summary>
        ///// <param name="fId"></param>
        ///// <returns></returns>
        public int Delete(BOModel model)
        {
            List<string> SqlList = new List<string>();
            string Sql = "delete  from relation where BOID1='" + model.Boid + "' or BOID2='" + model.Boid + "'";
            SqlList.Add(Sql);//删除对象关系表

            Sql = "delete from Property where BOID='" + model.Boid + "'";
            SqlList.Add(Sql);//删除对象参数表

            Sql = "delete from ALIASNAME where BOID='" + model.Boid + "'";
            SqlList.Add(Sql);//删除对象别名表

            Sql = "delete from GEOMETRY where BOID='" + model.Boid + "'";
            SqlList.Add(Sql);//删除对象空间信息表

            Sql = "delete from BO where BOID='" + model.Boid + "'";
            SqlList.Add(Sql);//删除对象表
            string[] ExecSql = SqlList.ToArray();
            bool Rtn = DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(ExecSql);
            if (Rtn == true)
                return 1;
            else
                return 0;
        }

        /// <summary>
        ///根据ID获取对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IList<BOModel> GetListByID(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  boid,name,botid,decode( isuse,1,'是','否' ) isuse FROM BO");
            strSql.Append(" WHERE BOID =:BOID ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = ID;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据名称获取对象
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public IList<BOModel> GetListByName(string Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  boid,name,botid,decode( isuse,1,'是','否' ) isuse FROM BO");
            strSql.Append(" WHERE NAME =:NAME ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = Name;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        ///获取全部对象
        /// </summary>
        /// <returns></returns>
        public IList<BOModel> GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  boid,name,botid,decode( isuse,1,'是','否' ) isuse FROM BO");
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString());
        }

        /// <summary>
        /// 根据名称和对象类型获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public List<BOModel> GetBoListByName(string name, string bot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  boid,name,botid,decode( isuse,1,'是','否' ) isuse FROM BO");
            strSql.Append(" WHERE NAME =:NAME AND BOTID=:BOTID ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                 new OracleParameter("BOTID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = bot;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }

        public DataTable GetBoListByID(string BOID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  boid,name,botid,decode( isuse,1,'是','否' ) isuse FROM BO ");
            strSql.Append(" WHERE  BOID=:BOID ");
            OracleParameter[] parameters = {                               
                                 new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = BOID;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
        }


        public List<BOModel> GetBoListByBOTID(string botID)
        {
            StringBuilder strSql = new StringBuilder();
            List<BOModel> List = new List<BOModel>();
            DataSet ds = new DataSet();

            strSql.Append(" SELECT  boid,BO.name,BO.botid,decode( isuse,1,'是','否' ) isuse,objecttype.BOT FROM BO");
            strSql.Append(" inner join objecttype");
            strSql.Append(" on BO.botid=objecttype.botid");
            if (botID != "")
            {
                strSql.Append(" WHERE BO.BOTID=:BOTID order by  BO.botid");
                OracleParameter[] parameters = {                             
                                 new OracleParameter("BOTID", OracleDbType.Varchar2,36)
                                                };
                parameters[0].Value = botID;
                ds = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters);
            }
            else
            {
                strSql.Append("  order by  BO.botid");
                ds = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString());
            }
            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BOModel bo = new BOModel();
                    bo.Boid = dr.ItemArray[0].ToString();
                    bo.Name = dr.ItemArray[1].ToString();
                    bo.Botid = dr.ItemArray[2].ToString();
                    bo.Isuse = dr.ItemArray[3].ToString();
                    bo.Boc = dr.ItemArray[4].ToString();
                    List.Add(bo);
                }

            }
            return List;

        }

        /// <summary>
        /// 根据别名获取对象
        /// </summary>
        /// <param name="alisName"></param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public List<BOModel> GetBOByAlisName(string alisName, string appDomain)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM BO B ");
            strSql.Append(" JOIN ALIASNAME A ON B.BOID=A.BOID ");
            strSql.Append(" WHERE A.NAME=:NAME AND A.APPDOMAIN=:APPDOMAIN ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                 new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = alisName;
            parameters[1].Value = appDomain;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据对象ID查询指定范围内对象,可选对象类型和对象类别
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="distance"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        public List<BOModel> GetNearBOByID(string boid, double distance, string bot = null, string boc = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT A.*,O.BOT FROM TABLE(GETBOBYBO( ");
            strSql.Append(":BOID , :DISTANCE,:BOTID,:BOC)) A JOIN OBJECTTYPE O ON A.BOTID=O.BOTID");

            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("BOID", OracleDbType.Varchar2) { Value = boid });
            oraParams.Add(new OracleParameter("DISTANCE", OracleDbType.Varchar2) { Value = distance });
            oraParams.Add(new OracleParameter("BOTID", OracleDbType.Varchar2) { Value = bot });
            oraParams.Add(new OracleParameter("BOC", OracleDbType.Varchar2) { Value = boc });


            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), oraParams.ToArray());
        }

        /// <summary>
        /// 根据WKT坐标、类型、类别查询指定范围内对象
        /// </summary>
        /// <param name="wkt"></param>
        /// <param name="distance"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        public DataSet GetNearBOByLocation(string wkt, double distance, string bot = null, string boc = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM TABLE(GETBOBYWKT(:WKT , :DISTANCE,:BOTID,:BOC))");
            OracleParameter[] parameters = {
                                new OracleParameter("WKT", OracleDbType.Varchar2,50),
                                new OracleParameter("DISTANCE", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Varchar2,50),
                                new OracleParameter("BOC", OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = wkt;
            parameters[1].Value = distance;
            parameters[2].Value = bot;
            parameters[3].Value = boc;

            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据对象名称、类型、类别查询指定范围内对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot0"></param>
        /// <param name="distance"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        public DataSet GetNearBOByName(string name, string bot0, double distance, string bot = null, string boc = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT A.*,O.BOT FROM TABLE(GETBOBYBO( ");
            strSql.Append("(SELECT BOID FROM BO WHERE BO.NAME=:NAME AND BOTID=:BOTID AND ROWNUM=1) , :DISTANCE,:BOTID,:BOC)) A JOIN OBJECTTYPE O ON A.BOTID=O.BOTID");

            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("NAME", OracleDbType.Varchar2) { Value = name });
            oraParams.Add(new OracleParameter("BOTID", OracleDbType.Varchar2) { Value = bot0 });
            oraParams.Add(new OracleParameter("DISTANCE", OracleDbType.Varchar2) { Value = distance });
            oraParams.Add(new OracleParameter("BOTID", OracleDbType.Varchar2) { Value = bot });
            oraParams.Add(new OracleParameter("BOC", OracleDbType.Varchar2) { Value = boc });

            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), oraParams.ToArray());
        }

        /// <summary>
        /// 根据对象类型ID获取对象列表
        /// </summary>
        /// <param name="botID"></param>
        /// <returns></returns>
        public IList<BOModel> GetBoByBotID(string botID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT BO.*,O.BOT FROM BO,OBJECTTYPE O WHERE O.BOTID=BO.BOTID AND  BO.BOTID=:BOTID ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOTID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = botID;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取对象以及对象的空间数据
        /// </summary>
        /// <param name="botID"></param>
        /// <returns></returns>
        public IList<BoExModel> GetBoExByBotID(string botID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT BO.*,O.BOT FROM BO,OBJECTTYPE O WHERE O.BOTID=BO.BOTID AND  BO.BOTID=:BOTID ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOTID", OracleDbType.Varchar2,36)
                                           };
            parameters[0].Value = botID;

            List<BOModel> list = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
            List<BoExModel> boExList = new List<BoExModel>();
            foreach (var item in list)
            {
                BoExModel model = new BoExModel();
                model.Boid = item.Boid;
                model.Boc = item.Boc;
                model.BotName = item.Bot;
                model.Name = item.Name;
                model.GeometryList = new GeometryServer().GetListByID(item.Boid).ToList();
                boExList.Add(model);
            }
            return boExList;
        }
        /// <summary>
        /// SubTree：返回指定BO的子树节点；
        /// Around：返回指定BO的相邻节点，包括父节点、兄弟节点和下级节点；
        /// Parent：返回指定BO的父节点；
        /// Brother：返回指定BO的兄弟节点；
        /// Child：返回指定BO的下级节点；
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<BOModel> GetBOTree(string boid, string category)
        {
            StringBuilder strSql = new StringBuilder();
            /*
            switch (category)
            {
                case "SubTree":
                    strSql.Append("SELECT * FROM BO START WITH BO.BOID=:BOID CONNECT BY BO.BOPID=PRIOR BO.BOID");
                    break;
                case "Around":
                    strSql.Append(" SELECT BO1.* FROM BO BO1 JOIN BO BO2 ON BO1.BOID=BO2.BOPID AND BO2.BOID=:BOID ");
                    strSql.Append(" UNION ");
                    strSql.Append(" SELECT * FROM BO WHERE EXISTS ( SELECT * FROM BO BO2 WHERE BO2.BOPID=BO.BOPID AND BO2.BOID=:BOID ) ");
                    strSql.Append(" UNION ");
                    strSql.Append(" SELECT * FROM BO WHERE BO.BOPID=:BOID");
                    break;
                case "Parent":
                    strSql.Append(" SELECT BO1.* FROM BO BO1 JOIN BO BO2 ON BO1.BOID=BO2.BOPID AND BO2.BOID=:BOID ");
                    break;
                case "Brother":
                    strSql.Append(" SELECT * FROM BO WHERE EXISTS ( SELECT * FROM BO BO2 WHERE BO2.BOPID=BO.BOPID AND BO2.BOID=:BOID ) ");
                    break;
                case "Child":
                    strSql.Append(" SELECT * FROM BO WHERE BO.BOPID=:BOID");
                    break;
                default:
                    break;
            }*/
            switch (category)
            {
                case "SubTree":
                    strSql.Append("SELECT * FROM BO START WITH BO.BOID=:BOID CONNECT BY BO.BOPID=PRIOR BO.BOID");
                    break;
                case "Around":
                    strSql.Append(" SELECT BO1.* FROM BO BO1 JOIN BO BO2 ON BO1.BOID=BO2.BOPID AND BO2.BOID=:BOID ");
                    strSql.Append(" UNION ");
                    strSql.Append(" SELECT * FROM BO WHERE EXISTS ( SELECT * FROM BO BO2 WHERE BO2.BOPID=BO.BOPID AND BO2.BOID=:BOID ) ");
                    strSql.Append(" UNION ");
                    strSql.Append(" SELECT * FROM BO WHERE BO.BOPID=:BOID");
                    break;
                case "Parent":
                    strSql.Append("     select bo1.name,bo1.boc,a.* from geometry a ,geometry b,bo bo1,reltype rt,bo bo2  where b.boid=:BOID ");
                    strSql.Append(" and bo1.boid=a.boid and bo2.boid=b.boid and bo2.botid=rt.botid2 and bo1.botid=rt.botid1 and ");
                    strSql.Append(" SDO_WITHIN_DISTANCE(A.Geometry, B.Geometry, 'distance = 0') = 'TRUE' ");
                    break;
                case "Brother":
                    strSql.Append(" SELECT * FROM BO WHERE EXISTS ( SELECT * FROM BO BO2 WHERE BO2.BOPID=BO.BOPID AND BO2.BOID=:BOID ) ");
                    break;
                case "Child":
                    strSql.Append(" SELECT * FROM BO WHERE BO.BOPID=:BOID");
                    break;
                default:
                    break;
            }
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                                };
            parameters[0].Value = boid;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据业务对象ID查找指定关系对应的对象
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="relation"></param>
        /// <param name="direction">关联方向，包括正向（Forward），反向（Backward）</param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <returns></returns>
        public List<BOModel> GetRelatedBO(string boid, string relation, string direction, string bot = null, string boc = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BO.* FROM BO WHERE BOID= ");
            if (direction == "Forward")
                strSql.Append(" (SELECT R.BOID2 FROM RELATION R JOIN RELTYPE RT ON R.RTID=RT.RTID WHERE RT.RT=:RT AND R.BOID1=:BOID) ");
            else if (direction == "Backward")
                strSql.Append(" (SELECT R.BOID1 FROM RELATION R JOIN RELTYPE RT ON R.RTID=RT.RTID WHERE RT.RT=:RT AND R.BOID2=:BOID) ");
            if (bot != null) strSql.Append(" AND BOT=:BOT ");
            if (boc != null) strSql.Append(" AND BOC=:BOC ");

            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("RT", OracleDbType.Varchar2, 36) { Value = relation });
            oraParams.Add(new OracleParameter("BOID", OracleDbType.Varchar2, 36) { Value = boid });
            if (bot != null) oraParams.Add(new OracleParameter("BOT", OracleDbType.Varchar2, 36) { Value = bot });
            if (boc != null) oraParams.Add(new OracleParameter("BOC", OracleDbType.Varchar2, 36) { Value = boc });
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), oraParams.ToArray());
        }

        /// <summary>
        /// 根据业务类型和名称查找指定关系对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot0"></param>
        /// <param name="relation"></param>
        /// <param name="bot"></param>
        /// <param name="boc"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<BOModel> GetBOByNameAndRelation(string name, string bot0, string relation, string direction, string bot = null, string boc = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BO.* FROM BO  WHERE BOID= ");
            if (direction == "Forward")
                strSql.Append("(SELECT R.BOID2 FROM  RELATION R JOIN RELTYPE RT ON R.RTID=RT.RTID JOIN BO ON R.BOID1=BO.BOID AND BO.NAME=:NAME AND BO.BOTID=:BOT0 WHERE RT.RT=:RT) ");
            else if (direction == "Backward")
                strSql.Append("(SELECT R.BOID1 FROM  RELATION R JOIN RELTYPE RT ON R.RTID=RT.RTID JOIN BO ON R.BOID2=BO.BOID AND BO.NAME=:NAME AND BO.BOTID=:BOT0 WHERE RT.RT=:RT) ");
            if (bot != null) strSql.Append(" AND BOTID=:BOT ");
            if (boc != null) strSql.Append(" AND BOC=:BOC ");

            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("NAME", OracleDbType.Varchar2, 36) { Value = name });
            oraParams.Add(new OracleParameter("BOT0", OracleDbType.Varchar2, 36) { Value = bot0 });
            oraParams.Add(new OracleParameter("RT", OracleDbType.Varchar2, 36) { Value = relation });
            if (bot != null) oraParams.Add(new OracleParameter("BOT", OracleDbType.Varchar2, 36) { Value = bot });
            if (boc != null) oraParams.Add(new OracleParameter("BOC", OracleDbType.Varchar2, 36) { Value = boc });

            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), oraParams.ToArray());
        }

        public List<BOModel> GetBORoot()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM BO WHERE  BO.BOPID IS NULL AND ROWNUM<=40");
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString());
        }

        /// <summary>
        /// 非事务的方式导入3GX数据
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        public int Upload3GX(List<BoExModel> boExModelList, string replaceOrLeave = null)
        {
            int i = 0;
            foreach (BoExModel bo in boExModelList)
            {
                if (bo.Botid == null) continue;
                List<OracleCommand> oracleCommList = new List<OracleCommand>();
                bool isBoExist = false;
                List<BOModel> boModelList = GetBoListByName(bo.Name, bo.Botid);
                if (boModelList.Count() > 0)
                {
                    isBoExist = true;
                    //对象存在则需要把带入库的对象id都换成库中的该对象id
                    bo.Boid = boModelList[0].Boid;
                }

                #region 添加对象
                OracleCommand oracleComm = new OracleCommand();
                StringBuilder strSql = new StringBuilder();
                //存在且保留
                if (isBoExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                //覆盖
                else
                {
                    //对象不存在
                    if (!isBoExist)
                    {
                        strSql.Append(" INSERT INTO BO(  ");
                        strSql.Append(" BOPID,NAME,BOTID,BOC,SOURCEDB,GATHERUSER,GATHERDATE,ISUSE,BOID)");
                        strSql.Append(" VALUES (:BOPID,:NAME,:BOTID,:BOC,:SOURCEDB,:GATHERUSER,:GATHERDATE,:ISUSE,:BOID)");
                    }
                    //对象存在
                    else
                    {
                        strSql.Append(" UPDATE BO SET ");
                        strSql.Append(" BOPID= :BOPID,NAME=:NAME,BOTID= :BOTID,BOC= :BOC,SOURCEDB=:SOURCEDB,GATHERUSER=:GATHERUSER,GATHERDATE=:GATHERDATE,ISUSE=:ISUSE");
                        strSql.Append(" WHERE BOID=:BOID");
                    }
                    OracleParameter[] parameters = {
                                new OracleParameter("BOPID", OracleDbType.Varchar2,36),
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Varchar2,50),
                                new OracleParameter("BOC", OracleDbType.Varchar2,50),
                                new OracleParameter("SOURCEDB", OracleDbType.Varchar2,150),
                                new OracleParameter("GATHERUSER", OracleDbType.Varchar2,50),
                                new OracleParameter("GATHERDATE", OracleDbType.Date),
                                new OracleParameter("ISUSE", OracleDbType.Char,1),
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                               };
                    parameters[0].Value = bo.Bopid;
                    parameters[1].Value = bo.Name;
                    parameters[2].Value = bo.Botid;
                    parameters[3].Value = bo.Boc;
                    parameters[4].Value = bo.Sourcedb;
                    parameters[5].Value = bo.Gatheruser;
                    parameters[6].Value = bo.Gatherdate;
                    parameters[7].Value = bo.Isuse;
                    parameters[8].Value = bo.Boid;
                    oracleComm.CommandText = strSql.ToString();
                    for (int j = 0; j < parameters.Length; j++)
                        oracleComm.Parameters.Add(parameters[j]);
                    oracleCommList.Add(oracleComm);
                }
                #endregion

                #region 添加坐标数据
                bool isGeometryExist = false;
                //如果对象存在，需要判断坐标是否存在，只要该对象存在一组坐标，就认为该对象坐标存在
                if (isBoExist)
                {
                    bo.GeometryList[0].Boid = bo.Boid;
                    isGeometryExist = new GeometryServer().Exist(bo.GeometryList[0]);
                }
                //坐标且保留
                if (isGeometryExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                //覆盖
                else
                {
                    //坐标存在，先删除
                    if (isGeometryExist)
                    {
                        OracleCommand oracleCommDele = new OracleCommand();
                        StringBuilder strDelSql = new StringBuilder();
                        strDelSql.Append(" DELETE FROM  GEOMETRY  ");
                        strDelSql.Append(" WHERE BOID =:BOID ");
                        OracleParameter parameters = new OracleParameter("BOID", OracleDbType.Varchar2, 36);
                        parameters.Value = bo.Boid;
                        oracleCommDele.Parameters.Add(parameters);
                        oracleCommDele.CommandText = strDelSql.ToString();
                        oracleCommList.Add(oracleCommDele);
                    }

                    foreach (GeometryModel geometry in bo.GeometryList)
                    {
                        OracleCommand oracleComm1 = new OracleCommand();

                        StringBuilder strInsertSql = new StringBuilder();
                        strInsertSql.Append(" INSERT INTO GEOMETRY(  ");
                        strInsertSql.Append(" BOID,NAME,GEOMETRY,SOURCEDB)");
                        strInsertSql.Append(" VALUES (:BOID,:NAME,SDO_GEOMETRY(:GEOMETRY,4326),:SOURCEDB)");

                        OracleParameter[] parameters1 = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("GEOMETRY", OracleDbType.Clob),
                            new OracleParameter("SOURCEDB", OracleDbType.Varchar2,50)
                                           };
                        parameters1[0].Value = bo.Boid;
                        parameters1[1].Value = geometry.Name;
                        parameters1[2].Value = geometry.Geometry;
                        parameters1[3].Value = geometry.Sourcedb;

                        oracleComm1.CommandText = strInsertSql.ToString();
                        for (int j = 0; j < parameters1.Length; j++)
                            oracleComm1.Parameters.Add(parameters1[j]);

                        oracleCommList.Add(oracleComm1);
                    }
                }
                #endregion

                #region 添加属性数据
                foreach (PropertyModel property in bo.PropertyList)
                {
                    bool isPropertyExist = false;
                    if (isBoExist)
                    {
                        property.BOId = bo.Boid;
                        isPropertyExist = new PropertyServer().Exist(property);
                    }
                    //该条属性存在且保留，跳过
                    if (isPropertyExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                    else
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        if (!isPropertyExist)
                        {
                            strSql1.Append(" INSERT INTO PROPERTY(  ");
                            strSql1.Append(" MD,MDSOURCE,BOID,NS)");
                            strSql1.Append(" VALUES (:MD,:MDSOURCE,:BOID,:NS)");
                        }
                        else if (isPropertyExist && replaceOrLeave.ToUpper() != "LEAVE")
                        {
                            strSql1.Append(" UPDATE PROPERTY SET ");
                            strSql1.Append(" MD=:MD ,MDSOURCE=:MDSOURCE ");
                            strSql1.Append(" WHERE BOID=:BOID AND NS=:NS");
                        }
                        OracleParameter[] parameters1 = {
                            new OracleParameter("MD", OracleDbType.XmlType),
                            new OracleParameter("MDSOURCE", OracleDbType.Varchar2,50),
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NS", OracleDbType.Varchar2,50)                           
                                                        };
                        parameters1[0].Value = property.MD;
                        parameters1[1].Value = property.MdSource;
                        parameters1[2].Value = property.BOId;
                        parameters1[3].Value = property.NS;

                        OracleCommand oracleComm1 = new OracleCommand();
                        oracleComm1.CommandText = strSql1.ToString();
                        for (int j = 0; j < parameters1.Length; j++)
                            oracleComm1.Parameters.Add(parameters1[j]);
                        oracleCommList.Add(oracleComm1);
                    }
                }
                #endregion

                #region 添加别名
                foreach (AliasNameModel aliasName in bo.AliasNameList)
                {
                    bool isAliasNameExist = false;
                    if (isBoExist)
                    {
                        aliasName.BOId = bo.Boid;
                        isAliasNameExist = new AliasNameServer().Exist(aliasName);
                    }
                    //该条属性存在且保留，跳过
                    if (isAliasNameExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                    else
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        if (!isAliasNameExist)
                        {
                            strSql1.Append(" INSERT INTO ALIASNAME(  ");
                            strSql1.Append(" NAME,BOID,APPDOMAIN)");
                            strSql1.Append(" VALUES (:NAME,:BOID,:APPDOMAIN)");
                        }
                        else if (isAliasNameExist && replaceOrLeave.ToUpper() != "LEAVE")
                        {
                            strSql1.Append(" UPDATE ALIASNAME SET ");
                            strSql1.Append(" NAME=:NAME");
                            strSql1.Append(" WHERE BOID=:BOID AND APPDOMAIN=:APPDOMAIN");
                        }
                        OracleParameter[] parameters1 = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),                               
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,50)
                                               };
                        parameters1[0].Value = aliasName.Name;                       
                        parameters1[1].Value = aliasName.BOId;
                        parameters1[2].Value = aliasName.AppDomain;
                        OracleCommand oracleComm1 = new OracleCommand();
                        oracleComm1.CommandText = strSql1.ToString();
                        for (int j = 0; j < parameters1.Length; j++)
                            oracleComm1.Parameters.Add(parameters1[j]);

                        oracleCommList.Add(oracleComm1);
                    }
                }
                #endregion

                i = i + (DBUtility.OracleDBHelper.OracleHelper.ExecuteCommand(oracleCommList) == true ? 1 : 0);
            }
            return i;
        }

        /// <summary>
        /// 事务方式导入3GX数据
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        public bool Upload3GXTran(List<BoExModel> boExModelList, string replaceOrLeave = null)
        {
            List<OracleCommand> oracleCommList = new List<OracleCommand>();

            foreach (BoExModel bo in boExModelList)
            {
                bool isBoExist = false;
                List<BOModel> boModelList = GetBoListByName(bo.Name, bo.Botid);
                if (boModelList.Count() > 0)
                {
                    isBoExist = true;
                    //对象存在则需要把带入库的对象id都换成库中的该对象id
                    bo.Boid = boModelList[0].Boid;
                }

                #region 添加对象
                OracleCommand oracleComm = new OracleCommand();
                StringBuilder strSql = new StringBuilder();
                //存在且保留
                if (isBoExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                //覆盖
                else
                {
                    //对象不存在
                    if (!isBoExist)
                    {
                        strSql.Append(" INSERT INTO BO(  ");
                        strSql.Append(" BOID,BOPID,NAME,BOTID,BOC,SOURCEDB,GATHERUSER,GATHERDATE,ISUSE)");
                        strSql.Append(" VALUES (:BOID,:BOPID,:NAME,:BOTID,:BOC,:SOURCEDB,:GATHERUSER,:GATHERDATE,:ISUSE)");
                    }
                    //对象存在
                    else
                    {
                        strSql.Append(" UPDATE BO SET ");
                        strSql.Append(" BOPID= :BOPID,NAME=:NAME,BOT= :BOT,BOC= :BOC,SOURCEDB=:SOURCEDB,GATHERUSER=:GATHERUSER,GATHERDATE=:GATHERDATE,ISUSE=:ISUSE");
                        strSql.Append(" WHERE BOID=:BOID");
                    }
                    OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("BOPID", OracleDbType.Varchar2,36),
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOTID", OracleDbType.Varchar2,50),
                                new OracleParameter("BOC", OracleDbType.Varchar2,50),
                                new OracleParameter("SOURCEDB", OracleDbType.Varchar2,150),
                                new OracleParameter("GATHERUSER", OracleDbType.Varchar2,50),
                                new OracleParameter("GATHERDATE", OracleDbType.Date),
                                new OracleParameter("ISUSE", OracleDbType.Char,1)
                                               };
                    parameters[0].Value = bo.Boid;
                    parameters[1].Value = bo.Bopid;
                    parameters[2].Value = bo.Name;
                    parameters[3].Value = bo.Botid;
                    parameters[4].Value = bo.Boc;
                    parameters[5].Value = bo.Sourcedb;
                    parameters[6].Value = bo.Gatheruser;
                    parameters[7].Value = bo.Gatherdate;
                    parameters[8].Value = bo.Isuse;
                    oracleComm.CommandText = strSql.ToString();
                    for (int j = 0; j < parameters.Length; j++)
                        oracleComm.Parameters.Add(parameters[j]);
                    oracleCommList.Add(oracleComm);
                }
                #endregion

                #region 添加坐标数据
                bool isGeometryExist = false;
                //如果对象存在，需要判断坐标是否存在，只要该对象存在一组坐标，就认为该对象坐标存在
                if (isBoExist)
                {
                    bo.GeometryList[0].Boid = bo.Boid;
                    isGeometryExist = new GeometryServer().Exist(bo.GeometryList[0]);
                }
                //坐标且保留
                if (isGeometryExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                //覆盖
                else
                {
                    //坐标存在，先删除
                    if (isGeometryExist)
                    {
                        OracleCommand oracleCommDele = new OracleCommand();
                        StringBuilder strDelSql = new StringBuilder();
                        strDelSql.Append(" DELETE FROM  GEOMETRY  ");
                        strDelSql.Append(" WHERE BOID =:BOID ");
                        OracleParameter[] parameters = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                            };
                        parameters[0].Value = bo.Boid;
                        oracleCommDele.CommandText = strDelSql.ToString();
                        oracleCommList.Add(oracleCommDele);
                    }

                    foreach (GeometryModel geometry in bo.GeometryList)
                    {
                        OracleCommand oracleComm1 = new OracleCommand();

                        StringBuilder strInsertSql = new StringBuilder();
                        strInsertSql.Append(" INSERT INTO GEOMETRY(  ");
                        strInsertSql.Append(" BOID,NAME,GEOMETRY,SOURCEDB)");
                        strInsertSql.Append(" VALUES (:BOID,:NAME,SDO_GEOMETRY(:GEOMETRY,4326),:SOURCEDB)");

                        OracleParameter[] parameters1 = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NAME", OracleDbType.Varchar2,50),
                            new OracleParameter("GEOMETRY", OracleDbType.Clob),
                            new OracleParameter("SOURCEDB", OracleDbType.Varchar2,50)
                                           };
                        parameters1[0].Value = geometry.Boid;
                        parameters1[1].Value = geometry.Name;
                        parameters1[2].Value = geometry.Geometry;
                        parameters1[3].Value = geometry.Sourcedb;

                        oracleComm1.CommandText = strInsertSql.ToString();
                        for (int j = 0; j < parameters1.Length; j++)
                            oracleComm1.Parameters.Add(parameters1[j]);

                        oracleCommList.Add(oracleComm1);
                    }
                }
                #endregion

                #region 添加属性数据
                foreach (PropertyModel property in bo.PropertyList)
                {
                    bool isPropertyExist = false;
                    if (isBoExist)
                    {
                        property.BOId = bo.Boid;
                        isPropertyExist = new PropertyServer().Exist(property);
                    }
                    //该条属性存在且保留，跳过
                    if (isPropertyExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                    else
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        if (!isPropertyExist)
                        {
                            strSql1.Append(" INSERT INTO PROPERTY(  ");
                            strSql1.Append(" BOID,NS,MD,MDSOURCE)");
                            strSql1.Append(" VALUES (:BOID,:NS,:MD,:MDSOURCE)");
                        }
                        else if (isPropertyExist && replaceOrLeave.ToUpper() != "LEAVE")
                        {
                            strSql1.Append(" UPDATE GEOMETRY SET ");
                            strSql1.Append(" SOURCEDB=:SOURCEDB,GEOMETRY=SDO_GEOMETRY(:GEOMETRY,4326))");
                            strSql1.Append(" WHERE BOID=:BOID AND NAME=:NAME");
                        }
                        OracleParameter[] parameters1 = {
                            new OracleParameter("BOID", OracleDbType.Varchar2,36),
                            new OracleParameter("NS", OracleDbType.Varchar2,50),
                            new OracleParameter("MD", OracleDbType.XmlType),
                            new OracleParameter("MDSOURCE", OracleDbType.Varchar2,50)
                            };
                        parameters1[0].Value = property.BOId;
                        parameters1[1].Value = property.NS;
                        parameters1[2].Value = property.MD;
                        parameters1[3].Value = property.MdSource;

                        OracleCommand oracleComm1 = new OracleCommand();
                        oracleComm1.CommandText = strSql1.ToString();
                        for (int j = 0; j < parameters1.Length; j++)
                            oracleComm1.Parameters.Add(parameters1[j]);
                        oracleCommList.Add(oracleComm1);
                    }
                }
                #endregion

                #region 添加别名
                foreach (AliasNameModel aliasName in bo.AliasNameList)
                {
                    bool isAliasNameExist = false;
                    if (isBoExist)
                    {
                        aliasName.BOId = bo.Boid;
                        isAliasNameExist = new AliasNameServer().Exist(aliasName);
                    }
                    //该条属性存在且保留，跳过
                    if (isAliasNameExist && replaceOrLeave.ToUpper() == "LEAVE") ;
                    else
                    {
                        StringBuilder strSql1 = new StringBuilder();
                        if (!isAliasNameExist)
                        {
                            strSql1.Append(" INSERT INTO ALIASNAME(  ");
                            strSql1.Append(" BOID,NAME,APPDOMAIN,CREATUSER,UPLOADDATE)");
                            strSql1.Append(" VALUES (:BOID,:NAME,:APPDOMAIN,:CREATUSER,:UPLOADDATE)");
                        }
                        else if (isAliasNameExist && replaceOrLeave.ToUpper() != "LEAVE")
                        {
                            strSql1.Append(" UPDATE ALIASNAME SET ");
                            strSql1.Append(" NAME=:NAME,CREATUSER=:CREATUSER,UPLOADDATE=:UPLOADDATE");
                            strSql1.Append(" WHERE BOID=:BOID AND APPDOMAIN=:APPDOMAIN");
                        }
                        OracleParameter[] parameters1 = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,50),
                                new OracleParameter("CREATUSER", OracleDbType.Varchar2,50),
                                new OracleParameter("UPLOADDATE", OracleDbType.Date)
                                               };
                        parameters1[0].Value = aliasName.BOId;
                        parameters1[1].Value = aliasName.Name;
                        parameters1[2].Value = aliasName.AppDomain;
                        parameters1[3].Value = aliasName.CreatUser;
                        parameters1[4].Value = aliasName.UploadDate;
                        OracleCommand oracleComm1 = new OracleCommand();
                        oracleComm1.CommandText = strSql1.ToString();
                        for (int j = 0; j < parameters1.Length; j++)
                            oracleComm1.Parameters.Add(parameters1[j]);

                        oracleCommList.Add(oracleComm1);
                    }
                }
                #endregion
            }
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteCommand(oracleCommList);
        }

        public List<string> GetBoc(List<string> botList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT BO.BOC FROM BO WHERE BOTID IN (:BOTID) ");

            var oraParams = new List<OracleParameter>();
            var counter = 0;
            var collectionParams = new StringBuilder(":");
            foreach (var obj in botList)
            {
                var param = "BOTID" + counter;
                collectionParams.Append(param);
                collectionParams.Append(", :");
                oraParams.Add(new OracleParameter(param, OracleDbType.Varchar2) { Value = obj });
                counter++;
            }
            collectionParams.Remove(collectionParams.Length - 3, 3);

            strSql = strSql.Replace(":" + "BOTID", collectionParams.ToString());

            DataSet bocStr = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), oraParams.ToArray());
            List<string> bocList = new List<string>();
            foreach (DataRow dr in bocStr.Tables[0].Rows)
            {
                bocList.Add(dr["BOC"].ToString());
            }
            return bocList;
        }

        /// <summary>
        /// 根据查询条件返回对象集合
        /// </summary>
        /// <param name="filterList">查询条件，请注意参数表名：PROPERTY，参数名：PROTERTYNAME，参数值：PROPERTYVALUE</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<BOModel> GetBoByFilter(List<string> filterList, Dictionary<string, object> parameter, string botid = null, string ns = null)
        {
            StringBuilder strSql = new StringBuilder();
            //'/PropertySet/P[@n=\"目的层位\"][@r=\"eq\"]'
            //'奥陶系'
            //'f50953af-19be-46bc-b8de-9d242a6b97a5'
            strSql.Append(" SELECT BO.*,OBJECTTYPE.BOT,PROPERTY.* FROM BO,OBJECTTYPE, ");
            strSql.Append(" (SELECT EXTRACTVALUE(VALUE(I),'/P','xmlns=\"http://www.jurassic.com.cn/3gx\"') PROPERTYVALUE,EXTRACTVALUE(VALUE(I),'/P/@n','xmlns=\"http://www.jurassic.com.cn/3gx\"') PROPERTYNAME,X.* ");
            strSql.Append(" FROM PROPERTY X,TABLE(XMLSEQUENCE(EXTRACT(X.MD,'/PropertySet/P','xmlns=\"http://www.jurassic.com.cn/3gx\"'))) I) PROPERTY");
            strSql.Append(" WHERE BO.BOTID=OBJECTTYPE.BOTID AND PROPERTY.BOID=BO.BOID ");

            var oraParams = new List<OracleParameter>();
            foreach (string str in filterList)
                strSql.Append(" AND " + str);
            foreach (KeyValuePair<string, object> kvp in parameter)
            {
                oraParams.Add(new OracleParameter(kvp.Key, OracleDbType.Varchar2) { Value = kvp.Value });
            }
            if (botid != null)
            {
                strSql.Append(" AND BO.BOTID=:BOTID ");
                oraParams.Add(new OracleParameter("BOTID", OracleDbType.Varchar2) { Value = botid });
            }
            if (ns != null)
            {
                strSql.Append(" AND PROPERTY.NS=:NS");
                oraParams.Add(new OracleParameter("NS", OracleDbType.Varchar2) { Value = ns });
            }
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), oraParams.ToArray());
        }

        /// <summary>
        /// 获取对象某个参数的数据类型，作为filter中字段的数据类型
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetPropertyType(string boid, string propertyName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT X.BOID, EXTRACTVALUE(VALUE(I),'/P','xmlns=\"http://www.jurassic.com.cn/3gx\"') PROPERTYVALUE,EXTRACTVALUE(VALUE(I),'/P/@n','xmlns=\"http://www.jurassic.com.cn/3gx\"') PROPERTYNAME,EXTRACTVALUE(VALUE(I),'/P/@t','xmlns=\"http://www.jurassic.com.cn/3gx\"') PROPERTYTYPE,X.* ");
            strSql.Append(" FROM PROPERTY X,TABLE(XMLSEQUENCE(EXTRACT(X.MD,'/PropertySet/P','xmlns=\"http://www.jurassic.com.cn/3gx\"'))) I");
            strSql.Append(" WHERE BOID=:BOID AND EXTRACTVALUE(VALUE(I),'/P/@n','xmlns=\"http://www.jurassic.com.cn/3gx\"') =:PROPERTYNAME ");

            var oraParams = new List<OracleParameter>();
            oraParams.Add(new OracleParameter("BOID", OracleDbType.Varchar2) { Value = boid });
            oraParams.Add(new OracleParameter("PROPERTYNAME", OracleDbType.Varchar2) { Value = propertyName });
            DataSet ds = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), oraParams.ToArray());
            return ds == null ? null : ds.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 返回对象名称和别名，用于词库分词
        /// </summary>
        /// <param name="isWithAlias"></param>
        /// <returns></returns>
        public DataSet GetDictionary(bool isWithAlias)
        {
            StringBuilder strSql = new StringBuilder();
            if (isWithAlias)
            {
                strSql.Append(" SELECT BO.NAME||NVL2(LISTAGG(A.NAME,',') WITHIN GROUP (ORDER BY A.NAME),','||LISTAGG(A.NAME,',') WITHIN GROUP (ORDER BY A.NAME),'') NAMES FROM BO");
                strSql.Append(" LEFT JOIN ALIASNAME A ON BO.BOID = A.BOID GROUP BY BO.NAME ");
            }
            else
                strSql.Append("SELECT BO.NAME FROMBO");
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString());
        }

        public DataTable GetALIASNAME(string BOID)
        {
            DataTable dt = new DataTable();
            string Sql = "select BOID,NAME,APPDOMAIN   from ALIASNAME  where BOID='" + BOID + "'";
            try
            {
                dt = DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText(Sql).Tables[0];
            }
            catch
            {

            }
            return dt;
        }

        /// <summary>
        /// 根据对象名称和对象类型获取对象列表
        //////////////////////////////////////////////////////////2016.3.10日 陈雯雯补充
        /// </summary>
        /// <param name="boName"></param>
        /// <param name="botName"></param>
        /// <returns></returns>
        public List<BOModel> GetBoByBoNameAndBot(string boName, string botName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT BO.*,O.BOT FROM BO,OBJECTTYPE O WHERE O.BOTID=BO.BOTID AND  O.BOT=:BOT AND BO.NAME=:BONAME ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOT", OracleDbType.Varchar2,36),
                                new OracleParameter("BONAME", OracleDbType.Varchar2,36)
               };
            parameters[0].Value = botName;
            parameters[1].Value = boName;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BOModel>(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 导入对象属性数据        
        //////////////////////////////////////////////////////////2016.3.13日 陈雯雯补充
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        public bool UploadBoPropertyTran(List<BoExModel> boExModelList)
        {
            List<OracleCommand> oracleCommList = new List<OracleCommand>();

            foreach (BoExModel bo in boExModelList)
            {
                bool isBoExist = false;
                List<BOModel> boModelList = GetBoListByName(bo.Name, bo.Botid);
                if (boModelList.Count() > 0)
                {
                    isBoExist = true;
                    //对象存在则需要把待入库的对象id都换成库中的该对象id
                    bo.Boid = boModelList[0].Boid;
                }
                else
                {
                    #region 添加对象
                    OracleCommand oracleComm = new OracleCommand();
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" INSERT INTO BO(  ");
                    strSql.Append(" BOID,NAME,BOTID,ISUSE)");
                    strSql.Append(" VALUES (:BOID,:NAME,:BOTID,:ISUSE)");
                    var oraParams = new List<OracleParameter>();
                    oraParams.Add(new OracleParameter("BOID", OracleDbType.Varchar2) { Value = bo.Boid });
                    oraParams.Add(new OracleParameter("NAME", OracleDbType.Varchar2) { Value = bo.Name });
                    oraParams.Add(new OracleParameter("BOTID", OracleDbType.Varchar2) { Value = bo.Botid });
                    oraParams.Add(new OracleParameter("ISUSE", OracleDbType.Char) { Value = bo.Isuse });

                    oracleComm.CommandText = strSql.ToString();
                    oraParams.ForEach(x => oracleComm.Parameters.Add(x));
                    oracleCommList.Add(oracleComm);
                    #endregion
                }

                #region 添加属性数据
                foreach (PropertyModel property in bo.PropertyList)
                {
                    bool isPropertyExist = false;
                    if (isBoExist)
                    {
                        property.BOId = bo.Boid;
                        isPropertyExist = new PropertyServer().Exist(property);
                    }
                    StringBuilder strSql1 = new StringBuilder();
                    if (isPropertyExist)
                    {
                        string delet = "DELETE FROM PROPERTY WHERE BOID=:BOID AND NS=:NS";
                        var oraParams = new List<OracleParameter>();
                        oraParams.Add(new OracleParameter("BOID", OracleDbType.Varchar2) { Value = property.BOId });
                        oraParams.Add(new OracleParameter("NS", OracleDbType.Varchar2) { Value = property.NS });
                        OracleCommand oracleComm = new OracleCommand();
                        oracleComm.CommandText = delet;
                        oraParams.ForEach(x => oracleComm.Parameters.Add(x));
                        oracleCommList.Add(oracleComm);
                    }
                    strSql1.Append(" INSERT INTO PROPERTY(  ");
                    strSql1.Append(" BOID,NS,MD,MDSOURCE)");
                    strSql1.Append(" VALUES (:BOID,:NS,:MD,:MDSOURCE)");

                    var oraParams1 = new List<OracleParameter>();
                    oraParams1.Add(new OracleParameter("BOID", OracleDbType.Varchar2) { Value = property.BOId });
                    oraParams1.Add(new OracleParameter("NS", OracleDbType.Varchar2) { Value = property.NS });
                    oraParams1.Add(new OracleParameter("MD", OracleDbType.XmlType) { Value = property.MD });
                    oraParams1.Add(new OracleParameter("MDSOURCE", OracleDbType.Varchar2) { Value = property.MdSource });


                    OracleCommand oracleComm1 = new OracleCommand();
                    oracleComm1.CommandText = strSql1.ToString();
                    oraParams1.ForEach(x => oracleComm1.Parameters.Add(x));
                    oracleCommList.Add(oracleComm1);
                }
                #endregion
            }
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteCommand(oracleCommList);
        }

        public bool InsertBOandPara(List<string> SqlList)
        {
            bool BRtn = true;
            string[] Sql = SqlList.ToArray();
            try
            {
                BRtn = DBUtility.OracleDBHelper.OracleHelper.ExecuteSql(Sql);
            }
            catch
            {
                BRtn = false;
            }
            return BRtn;
        }


    }
}
