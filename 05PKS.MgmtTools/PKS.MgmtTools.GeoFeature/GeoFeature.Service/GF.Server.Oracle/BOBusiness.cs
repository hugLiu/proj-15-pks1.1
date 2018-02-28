using GF.Server.DBUtility;
using GGGXParse;
using Juarssic.Server.Comm;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.GF;
using Jurassic.PKS.Service.Interfaces.GF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GF.Server.Oracle
{
    public class BOBusiness : IBO
    {
        /// <summary>
        /// 根据对象ID、G|P|B查询对象3GX数据，3GX数据中可包含坐标信息或参数信息或两者都包含
        /// </summary>
        /// <param name="boid">对象ID</param>
        /// <param name="category">枚举值[G|P|B]</param>
        /// <returns></returns>
        public System.Xml.XmlDocument Get3GXById(string boid, GGGXDataCategory category)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.BOID,T.NAME,T1.BOT,T1.FT FROM BO T ");
            strSql.Append(" LEFT JOIN OBJECTTYPE T1 ");
            strSql.Append(" ON T.BOTID=T1.BOTID ");
            strSql.Append(" WHERE T.BOID =:BOID ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                            };
            parameters[0].Value = boid;
            DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
            List<GeoFeature> ftList = new List<GeoFeature>();
            foreach (DataRow row in dt.Rows)
            {
                GeoFeature ft = new GeoFeature();
                ft.BOID = row["BOID"].ToString();
                ft.BOT = row["BOT"].ToString();
                ft.FT = row["FT"].ToString();
                ft.NAME = row["NAME"].ToString();
                ft.CLASS = ft.CLASS = Comm.GetClassByBoid(ft.BOID);
                ft.AliasNameList = Comm.GetAliasNameByBoid(ft.BOID);
                if (category == GGGXDataCategory.B)
                {
                    ft.PropertyList = Comm.GetPropertyByBoid(ft.BOID);
                    ft.GeometryList = Comm.GetGeometryByBoid(ft.BOID);
                }
                else if (category == GGGXDataCategory.P)
                {
                    ft.PropertyList = Comm.GetPropertyByBoid(ft.BOID);
                    ft.GeometryList = null;
                }
                else if (category == GGGXDataCategory.G)
                {
                    ft.PropertyList = null;
                    ft.GeometryList = Comm.GetGeometryByBoid(ft.BOID);
                }

                ftList.Add(ft);
            }
            return GGGXParse.ConvertFT.FeatureToGGGX(ftList);
        }

        /// <summary>
        /// 根据业务对象ID和业务域查询业务对象别名
        /// </summary>
        /// <param name="boid">业务对象ID</param>
        /// <param name="appdomains">业务域</param>
        /// <returns></returns>
        public AliasCollection GetBOAliasByID(string boid, params string[] appdomains)
        {
            AliasCollection aliasColl = new AliasCollection();
            foreach (var item in appdomains)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" SELECT *  FROM ALIASNAME  ");
                strSql.Append(" WHERE  BOID =:BOID AND APPDOMAIN=:APPDOMAIN");

                OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36),
                                new OracleParameter("APPDOMAIN",OracleDbType.Varchar2,50)
                                                };
                parameters[0].Value = boid;
                parameters[1].Value = item;
                List<Alias> list = OracleDBHelper.OracleHelper.ExecuteQueryText<Alias>(strSql.ToString(), parameters);
                aliasColl.AddRange(list);
            }
            return aliasColl;
        }

        /// <summary>
        /// 根据对象别名、应用域查询业务对象。
        /// </summary>
        /// <param name="alias">对象别名</param>
        /// <param name="appdomain">应用域</param>
        /// <returns></returns>
        public BO GetBOByAlias(string alias, string appdomain)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * FROM BO B ");
            strSql.Append(" JOIN ALIASNAME A ON B.BOID=A.BOID ");
            strSql.Append(" WHERE A.NAME=:NAME AND A.APPDOMAIN=:APPDOMAIN ");
            OracleParameter[] parameters = {
                                 new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                 new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = alias;
            parameters[1].Value = appdomain;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BO>(strSql.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 根据对象ID查询业务对象
        /// </summary>
        /// <param name="boid">对象ID</param>
        /// <returns></returns>
        public BO GetBOById(string boid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.BOID,T.NAME,T1.BOT FROM BO T ");
            strSql.Append(" LEFT JOIN OBJECTTYPE T1 ");
            strSql.Append(" ON T.BOTID=T1.BOTID ");
            strSql.Append(" WHERE T.BOID =:BOID ");
            OracleParameter[] parameters = {
                                new OracleParameter("BOID", OracleDbType.Varchar2,36)
                                            };
            parameters[0].Value = boid;
            return OracleDBHelper.OracleHelper.ExecuteQueryText<BO>(strSql.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 根据业务对象名称、对象类型查询对象ID
        /// </summary>
        /// <param name="name">业务对象名称</param>
        /// <param name="bot">对象类型</param>
        /// <returns></returns>
        public BO GetBOByName(string name, string bot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM BO B");
            strSql.Append(" LEFT JOIN OBJECTTYPE T ");
            strSql.Append(" ON B.BOTID=T.BOTID ");
            strSql.Append(" WHERE B.NAME = :NAME ");
            strSql.Append(" AND T.BOT=:BOT ");
            OracleParameter[] parameters = {
                                new OracleParameter("NAME", OracleDbType.Varchar2,50),
                                new OracleParameter("BOT", OracleDbType.Varchar2,50)
                                            };
            parameters[0].Value = name;
            parameters[1].Value = bot;
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<BO>(strSql.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 根据应用场景和过滤条件查询业务对象。通过对象的参数集进行过滤，返回符合条件的对象列表
        /// </summary>
        /// <param name="bot">业务对象类型</param>
        /// <param name="wktBBox">空间范围</param>
        /// <param name="filte">过滤条件</param>
        /// <returns></returns>
        public BOCollection GetBOListByFilter(string bot, string wktBBox, string filte)
        {
            BOCollection list = new BOCollection();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT T.BOID, T.NAME, T1.BOT ");
            strSql.Append(" FROM ");
            if (!string.IsNullOrEmpty(wktBBox))
            {
                strSql.Append(" GEOMETRY   A,");
            }
            strSql.Append(" BO T,");
            strSql.Append(" OBJECTTYPE T1 ");
            //属性过滤条件不为空
            if (!string.IsNullOrEmpty(filte))
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
            }
            strSql.Append(string.Format(" WHERE T1.BOT = '{0}' ", bot));
            if (!string.IsNullOrEmpty(wktBBox))
            {
                //strSql.Append(" GEOMETRY   A,");
                strSql.Append(" AND A.BOID = T.BOID ");
            }
            strSql.Append(" AND T.BOTID = T1.BOTID ");
            //属性过滤条件不为空
            if (!string.IsNullOrEmpty(filte))
            {
                strSql.Append(" AND PROPERTY.BOID = T.BOID ");
            }
            //空间范围不为空
            if (!string.IsNullOrEmpty(wktBBox))
            {
                //左下右上坐标
                //string bbox = string.Empty;
                ////WKTString格式待统一
                //string[] arrStr = wktBBox.Split('(');
                //string strResult = System.Text.RegularExpressions.Regex.Replace(wktBBox, @"(.*\()(.*)(\).*)", "$2");
                //if (arrStr[0].Trim().ToUpper() == "POINT" || arrStr[0].Trim().ToUpper() == "LINESTRING")
                //{
                //    string[] strArr = wktBBox.ToString().Split(',');
                //    string[] point = strArr[0].Split(new char[] { ' ', '(', ')' });
                //    string[] point1 = strArr[1].Split(new char[] { ' ', '(', ')' });
                //    bbox = point[2] + "," + point[3] + "," + point1[2] + "," + point1[3];
                //}
                //else
                //{
                //    strResult = strResult.Substring(0, strResult.Length - 1);
                //    for (int i = 0; i < strResult.Split(',').Length; i++)
                //    {
                //        for (int j = 0; j < strResult.Split(',')[i].Split(' ').Length; j++)
                //        {
                //            bbox += strResult.Split(',')[i].Split(' ')[j] + ",";
                //        }
                //    }
                //    bbox = bbox.Substring(0, bbox.Length - 1);
                //}
                //strSql.Append(" AND SDO_FILTER(A.GEOMETRY, ");
                //strSql.Append(" MDSYS.SDO_GEOMETRY(2003,  4326, NULL,MDSYS.SDO_ELEM_INFO_ARRAY(1, 1003, 3), ");//只有3才表示矩形
                //strSql.Append(string.Format(" MDSYS.SDO_ORDINATE_ARRAY({0})), ", bbox.ToString()));
                //strSql.Append(" 'QUERYTYPE=WINDOW') = 'TRUE' ");

                //完整矩形（多边形）坐标，使用oracle方法转换wkt
                strSql.Append(" AND SDO_FILTER(A.GEOMETRY, ");
                strSql.Append(" SDO_GEOMETRY('" + wktBBox + "',4326), ");
                strSql.Append(" 'QUERYTYPE=WINDOW') = 'TRUE' ");

            }
            if (!string.IsNullOrEmpty(filte))
            {
                strSql.Append(" AND " + "( " + MongoJsonToSql.JsonToSql(filte, bot).ToString() + " )");
            }
            list.AddRange(OracleDBHelper.OracleHelper.ExecuteQueryText<BO>(strSql.ToString()));
            return list;
        }

        /// <summary>
        /// 根据业务对象类型和过滤条件获取对象列表
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public BOCollection GetBOListByType(string bot, string filter)
        {
            BOCollection list = new BOCollection();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT T.BOID, T.NAME, T1.BOT FROM  ");
            strSql.Append(" BO T,");
            strSql.Append(" OBJECTTYPE T1 ");
            //属性过滤条件不为空
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
            }
            strSql.Append(string.Format(" WHERE T1.BOT = '{0}' ", bot));
            strSql.Append(" AND T.BOTID = T1.BOTID ");
            //属性过滤条件不为空
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" AND PROPERTY.BOID = T.BOID ");
            }
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" AND " + "(" + MongoJsonToSql.JsonToSql(filter, bot).ToString() + ")");
            }
            list.AddRange(OracleDBHelper.OracleHelper.ExecuteQueryText<BO>(strSql.ToString()));
            return list;
        }

        /// <summary>
        /// 根据业务对象ID获取指定BO的父节点、下级节点、兄弟节点、相邻节点（父节点、兄弟节点和下级节点）和子树。返回的节点中不包括自己
        /// </summary>
        /// <param name="template">树模板</param>
        /// <returns></returns>
        public TreeBOCollection GetBOTree(BOTreeTemplate template)
        {
            TreeBOCollection list = new TreeBOCollection();
            foreach (var item in template)
            {
                string strBos = string.Empty;
                TreeBO treeBo = new TreeBO();
                if (item.BOs != null && item.BOs.Count() > 0)
                {
                    for (int i = 0; i < item.BOs.Count(); i++)
                    {
                        if (i == item.BOs.Count() - 1)
                        {
                            strBos += "'" + item.BOs[i].Trim() + "'";
                        }
                        else
                        {
                            strBos += "'" + item.BOs[i].Trim() + "',";
                        }
                    }
                }
                StringBuilder strSql = new StringBuilder();
                OracleParameter[] parameters;
                strSql.Append(" SELECT T.BOID, T1.BOT, T.NAME, '' AS ParentBOID ");
                strSql.Append(" FROM BO T, OBJECTTYPE T1 ");

                //过滤条件
                #region
                if (!string.IsNullOrEmpty(item.Filter))
                {
                    strSql.Append(" ,PROPERTY PROPERTY  ");
                    strSql.Append(" WHERE PROPERTY.BOID = T.BOID ");
                    strSql.Append(" AND " + "(" + MongoJsonToSql.JsonToSql(item.Filter, item.BOT).ToString() + ")");
                    strSql.Append(" AND T.BOTID = T1.BOTID ");
                }
                else
                {
                    strSql.Append(" WHERE T.BOTID = T1.BOTID ");
                }
                #endregion
                if (item.BOs != null && item.BOs.Count() > 0)
                {
                    strSql.Append(string.Format(" AND T.NAME IN({0}) ", strBos));
                    strSql.Append(" AND T1.BOT = :BOT ");
                }
                else
                {
                    strSql.Append(" AND T1.BOT = :BOT ");
                }
                parameters = new OracleParameter[] {
                                 new OracleParameter("BOT", OracleDbType.Varchar2,100)
                                                    };
                parameters[0].Value = item.BOT;

                List<TreeBO> treeBOList = new List<TreeBO>();
                //父节点
                treeBOList = OracleDBHelper.OracleHelper.ExecuteQueryText<TreeBO>(strSql.ToString(), parameters);
                list.AddRange(treeBOList);

                if (item.Children != null)
                {
                    foreach (BOTreeNodeTemplate nodeTemplate in item.Children)
                    {
                        list.AddRange(GetTreeBONode(nodeTemplate, treeBOList, item.Relation));
                    }
                }
            }
            return list;
        }
        List<TreeBO> listTemp = new List<TreeBO>();
        /// <summary>
        /// 递归查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<TreeBO> GetTreeBONode(BOTreeNodeTemplate nodeTemplate, List<TreeBO> treeBOList, BORelation relation)
        {
            foreach (var bo in treeBOList)
            {
                StringBuilder strSql = new StringBuilder();
                if (string.IsNullOrEmpty(relation.ToString()) || relation == BORelation.Space)
                {
                    strSql.Append(" SELECT T.BOID,  T.NAME,  T1.BOT,");
                    strSql.Append("'" + bo.BOID + "'" + " AS PARENTBOID ");
                    strSql.Append(" FROM BO T, OBJECTTYPE T1, GEOMETRY T3 ");
                    //过滤条件
                    #region
                    if (!string.IsNullOrEmpty(nodeTemplate.Filter))
                    {
                        strSql.Append(" ,PROPERTY PROPERTY  ");
                        strSql.Append(" WHERE PROPERTY.BOID = T.BOID ");
                        JObject jObj = JObject.Parse(nodeTemplate.Filter);
                        JToken strFilter = jObj["filter"];
                        strSql.Append(" AND " + "(" + MongoJsonToSql.JsonToSql(strFilter.ToString(), bo.BOT).ToString() + ")");
                        strSql.Append(" AND T.BOTID = T1.BOTID ");
                        strSql.Append(" AND T.BOID = T3.BOID ");
                    }
                    else
                    {
                        strSql.Append(" WHERE T.BOTID = T1.BOTID ");
                        strSql.Append(" AND T.BOID = T3.BOID ");
                    }
                    #endregion
                    //空间包含的子对象
                    strSql.Append(" AND SDO_FILTER(t3.GEOMETRY, ");
                    strSql.Append(" (SELECT T2.GEOMETRY ");
                    strSql.Append(" FROM GEOMETRY T2 ");
                    strSql.Append(string.Format(" WHERE T2.BOID ='{0}'), ", bo.BOID));
                    strSql.Append(" 'QUERYTYPE=WINDOW') = 'TRUE' ");
                }
                else
                {
                    strSql.Append(" SELECT T5.BOID, T5.NAME, T6.BOT, ");
                    strSql.Append("'" + bo.BOID + "'" + " AS PARENTBOID ");
                    strSql.Append(" FROM RELATION T4, BO T5, OBJECTTYPE T6 ");

                    //过滤条件
                    #region
                    if (!string.IsNullOrEmpty(nodeTemplate.Filter))
                    {
                        strSql.Append(" ,PROPERTY PROPERTY  ");
                        strSql.Append(" WHERE PROPERTY.BOID = T5.BOID ");
                        strSql.Append(" AND " + "(" + MongoJsonToSql.JsonToSql(nodeTemplate.Filter, bo.BOT).ToString() + ")");
                        strSql.Append(" AND T4.RTID = (SELECT T.RTID ");
                    }
                    else
                    {
                        strSql.Append(" WHERE T4.RTID = (SELECT T.RTID ");
                    }
                    #endregion
                    strSql.Append(" FROM RELTYPE T ");
                    strSql.Append(" JOIN OBJECTTYPE T1 ");
                    strSql.Append(" ON T.BOTID1 = T1.BOTID ");
                    strSql.Append(" JOIN OBJECTTYPE T2 ");
                    strSql.Append(" ON T.BOTID2 = T2.BOTID ");
                    strSql.Append(string.Format(" AND T1.BOT = '{0}' ", bo.BOT));
                    strSql.Append(string.Format(" AND T2.BOT = '{0}') ", nodeTemplate.BOT));
                    strSql.Append(" AND T4.BOID2 = T5.BOID ");
                    strSql.Append(string.Format(" AND T4.BOID1 = '{0}' ", bo.BOID));
                    strSql.Append(" AND T5.BOTID = T6.BOTID ");
                }


                //过滤条件未添加
                if (nodeTemplate.BOs != null && nodeTemplate.BOs.Count() > 0)
                {
                    if (nodeTemplate.BOs != null && nodeTemplate.BOs.Count() > 0)
                    {
                        string strBos = string.Empty;
                        for (int i = 0; i < nodeTemplate.BOs.Count(); i++)
                        {
                            if (i == nodeTemplate.BOs.Count() - 1)
                            {
                                strBos += "'" + nodeTemplate.BOs[i].Trim() + "'";
                            }
                            else
                            {
                                strBos += "'" + nodeTemplate.BOs[i].Trim() + "',";
                            }
                        }

                        strSql.Append(string.Format(" AND T5.NAME IN ({0})", strBos));
                        strSql.Append(string.Format(" AND T6.BOT = '{0}' ", nodeTemplate.BOT));
                    }
                }
                else
                {
                    strSql.Append(string.Format(" AND T6.BOT = '{0}' ", nodeTemplate.BOT));
                }
                List<TreeBO> boList = OracleDBHelper.OracleHelper.ExecuteQueryText<TreeBO>(strSql.ToString());
                listTemp.AddRange(boList);
                if (nodeTemplate.Children != null)
                {
                    foreach (var children in nodeTemplate.Children)
                    {
                        GetTreeBONode(children, boList, nodeTemplate.Relation);
                    }
                }
            }
            return listTemp;
        }
        /// <summary>
        /// 获取BO的叙词分类。主要用于短语分词的时候识别业务对象是什么类型
        /// </summary>
        /// <returns></returns>
        public TermBOCollection GetCCTermOfBO()
        {
            TermBOCollection termBolist = new TermBOCollection();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" WITH TAB AS( ");
            strSql.Append(" SELECT T.BOID,T.NAME,T2.BOT,T1.NAME NAME1 FROM BO T ");
            strSql.Append(" LEFT JOIN ALIASNAME T1 ");
            strSql.Append(" ON T.BOID=T1.BOID ");
            strSql.Append(" LEFT JOIN OBJECTTYPE T2 ");
            strSql.Append(" ON T.BOTID=T2.BOTID )");
            strSql.Append(" SELECT BOID, NAME,BOT,WMSYS.WM_CONCAT(NAME1) AS ALIAS ");
            strSql.Append(" FROM TAB ");
            strSql.Append(" GROUP BY BOID,NAME,BOT ");
            DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString()).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                TermBO termBo = new TermBO();
                termBo.BOID = row["BOID"].ToString();
                termBo.BOT = row["BOT"].ToString();
                termBo.Name = row["NAME"].ToString();
                if (!string.IsNullOrEmpty(row["ALIAS"].ToString()))
                {
                    termBo.Alias = row["ALIAS"].ToString().Split(',').ToList();
                }
                termBolist.Add(termBo);
            }
            return termBolist;
        }

        /// <summary>
        /// 根据业务对象ID查找在指定距离范围内的业务对象。
        /// 根据BO的空间坐标信息，计算出该对象指定范围内的要求的业务对象类型和业务对象类别的临近对象，
        /// 并按照与指定对象的距离进行排序，距离近的排在前面。返回对象包括它自己
        /// </summary>
        /// <param name="boid"></param>
        /// <param name="distince"></param>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public NearBOCollection GetNearBOById(string boid, decimal distince, string bot, string filter)
        {
            NearBOCollection list = new NearBOCollection();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("(SELECT T.GEOMETRY FROM GEOMETRY T WHERE ROWNUM<=1 AND T.BOID='{0}')", boid));

            list = GetNearBOByBo(distince, bot, filter, strSql.ToString());
            return list;
        }

        /// <summary>
        /// 根据WKT格式坐标、对象类型、对象类别查询在该坐标指定距离范围内的业务对象，
        /// 对象类型、对象类别为可选条件，为空则返回所有符合位置关系的对象。返回对象包括它自己
        /// </summary>
        /// <param name="pointWKT"></param>
        /// <param name="distince"></param>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public NearBOCollection GetNearBOByLocation(string pointWKT, decimal distince, string bot, string filter)
        {
            NearBOCollection list = new NearBOCollection();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT B.BOID, T.NAME, O.BOT, TO_CHAR(SUBSTR(B.SPACELOCATION,1,3800)) as SPACELOCATION, B.DIST DISTINCE ");
            strSql.Append(" FROM (SELECT A.BOID, A.NAME, A.GEOMETRY.GET_WKT() AS SPACELOCATION, ");
            strSql.Append(" SDO_GEOM.SDO_DISTANCE(A.GEOMETRY, ");

            strSql.Append(" MDSYS.SDO_GEOMETRY(:WKT,4326)");

            strSql.Append(",1) dist FROM GEOMETRY A ");

            //过滤条件
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
                strSql.Append(" WHERE PROPERTY.BOID = A.BOID ");
                strSql.Append(" AND " + "(" + MongoJsonToSql.JsonToSql(filter, bot).ToString() + ")");
                strSql.Append(" AND SDO_WITHIN_DISTANCE(A.GEOMETRY, ");
            }
            else
            {
                strSql.Append(" WHERE SDO_WITHIN_DISTANCE(A.GEOMETRY, ");
            }

            strSql.Append("MDSYS.SDO_GEOMETRY(:WKT,4326)");

            strSql.Append(string.Format( ",'distance = {0}  unit = M') = 'TRUE' ",distince));
            strSql.Append(" ORDER BY A.NAME) B ");
            strSql.Append(" LEFT JOIN BO T ");
            strSql.Append(" ON B.BOID = T.BOID ");
            strSql.Append(" LEFT JOIN OBJECTTYPE O ");
            strSql.Append(" ON O.BOTID = T.BOTID ");

            //对象类型
            if (!string.IsNullOrEmpty(bot))
            {
                strSql.Append(string.Format("  WHERE O.BOT = '{0}'", bot));
            }
            OracleParameter[] parameters = {
                              new OracleParameter("WKT", OracleDbType.Clob)
                                            };
            parameters[0].Value = pointWKT;
            list.AddRange(OracleDBHelper.OracleHelper.ExecuteQueryText<NearBO>(strSql.ToString(), parameters));
            return list;
        }


        public NearBOCollection GetNearBOByBo(decimal distince, string bot, string filter, string strSqlGetGeo)
        {
            NearBOCollection list = new NearBOCollection();
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" SELECT DISTINCT B.BOID, T.NAME, O.BOT, TO_CHAR(SUBSTR(B.SPACELOCATION,1,3800)) as SPACELOCATION, B.DIST DISTINCE ");
            strSql.Append(" FROM (SELECT A.BOID, A.NAME, A.GEOMETRY.GET_WKT() AS SPACELOCATION, ");
            strSql.Append(" SDO_GEOM.SDO_DISTANCE(A.GEOMETRY, ");

            strSql.Append(strSqlGetGeo);

            strSql.Append(",1) dist FROM GEOMETRY A ");

            //过滤条件
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
                strSql.Append(" WHERE PROPERTY.BOID = A.BOID ");
                strSql.Append(" AND " + "(" + MongoJsonToSql.JsonToSql(filter, bot).ToString() + ")");
                strSql.Append(" AND SDO_WITHIN_DISTANCE(A.GEOMETRY, ");
            }
            else
            {
                strSql.Append(" WHERE SDO_WITHIN_DISTANCE(A.GEOMETRY, ");
            }

            strSql.Append(strSqlGetGeo);

            strSql.Append(string.Format( ",'distance = {0}  unit = M') = 'TRUE' ",distince));
            strSql.Append(" ORDER BY A.NAME) B ");
            strSql.Append(" LEFT JOIN BO T ");
            strSql.Append(" ON B.BOID = T.BOID ");
            strSql.Append(" LEFT JOIN OBJECTTYPE O ");
            strSql.Append(" ON O.BOTID = T.BOTID ");

            //对象类型
            if (!string.IsNullOrEmpty(bot))
            {
                strSql.Append(string.Format("  WHERE O.BOT = '{0}'", bot));
            }
            list.AddRange(OracleDBHelper.OracleHelper.ExecuteQueryText<NearBO>(strSql.ToString()));
            return list;
        }
        
        /// <summary>
        /// 根据对象名称、对象类型查询在该对象指定距离范围内对象，
        /// 对象类型、对象类别为可选条件，为空则返回所有符合位置关系的对象。返回对象包括它自己
        /// </summary>
        /// <param name="boName"></param>
        /// <param name="boType"></param>
        /// <param name="distince"></param>
        /// <param name="bot"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public NearBOCollection GetNearBOByName(string boName, string boType, decimal distince, string bot, string filter)
        {
            NearBOCollection list = new NearBOCollection();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" (SELECT T3.GEOMETRY ");
            strSql.Append(" FROM BO T ");
            strSql.Append(" LEFT JOIN OBJECTTYPE T1 ON T.BOTID = T1.BOTID ");
            strSql.Append(" LEFT JOIN GEOMETRY T3 ON T.BOID = T3.BOID ");
            strSql.Append(string.Format(" WHERE  ROWNUM<=1 AND T.NAME ='{0}' ", boName));
            strSql.Append(string.Format(" AND T1.BOT ='{0}') ", boType));

            list = GetNearBOByBo(distince, bot, filter, strSql.ToString());
            return list;
        }

        /// <summary>
        /// 根据业务对象类型和应用域获取属性定义信息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="appDomain"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public List<PropertySchema> GetPropertySchema(string bot, string appDomain, List<PropertyName> names) 
        {
            List<PropertySchema> list = new List<PropertySchema>();
            StringBuilder strSql = new StringBuilder();
            if (names.Count == 0)
            {
                #region 获取应用场景下的所有的参数以及参数的值域
                strSql.Append(" SELECT DISTINCT EXTRACTVALUE(VALUE(I), '/P/@n') AS NAME, ");
                strSql.Append(" EXTRACTVALUE(VALUE(I), '/P/@t') AS DATATYPE, ");
                strSql.Append(" EXTRACTVALUE(VALUE(I), '/P') AS VS ");
                strSql.Append(" FROM OBJTYPEPROPERTY X, ");
                strSql.Append(" OBJECTTYPE T2, ");
                strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
                strSql.Append(" WHERE X.BOTID = T2.BOTID ");
                strSql.Append(" AND  X.NS =:NS ");
                strSql.Append(" AND  T2.BOT =:BOT ");
                OracleParameter[] parameters = {
                            new OracleParameter("NS", OracleDbType.Varchar2,100),
                            new OracleParameter("BOT", OracleDbType.Varchar2,100)
                                            };
                parameters[0].Value = appDomain;
                parameters[1].Value = bot;
                DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
                foreach (DataRow item in dt.Rows)
                {
                    PropertySchema proSchema = new PropertySchema();
                    proSchema.Name = item["NAME"].ToString();
                    if (PropertyDataType.Date.ToString().ToUpper() == item["DataType"].ToString().ToUpper())
                    {
                        proSchema.DataType = PropertyDataType.Date;
                    }
                    else if (PropertyDataType.Decimal.ToString().ToUpper() == item["DataType"].ToString().ToUpper())
                    {
                        proSchema.DataType = PropertyDataType.Decimal;
                    }
                    else
                    {
                        proSchema.DataType = PropertyDataType.String;
                    }

                    proSchema.Values = item["VS"].ToString().Split(',').ToList();
                    list.Add(proSchema);
                }
                #endregion
            }
            else
            {
                foreach (var item in names)
                {
                    strSql = new StringBuilder();
                    if (item.ValueType == ValueTypes.All)
                    {
                        #region 可能值+实际值
                        strSql.Append(" SELECT DISTINCT EXTRACTVALUE(VALUE(I), '/P/@n') AS NAME, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), '/P/@t') AS DATATYPE, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), '/P') AS VS ");
                        strSql.Append(" FROM OBJTYPEPROPERTY X, ");
                        strSql.Append(" OBJECTTYPE T2, ");
                        strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
                        strSql.Append(" WHERE X.BOTID = T2.BOTID ");
                        strSql.Append(" AND  X.NS =:NS ");
                        strSql.Append(" AND  T2.BOT =:BOT ");
                        strSql.Append(" AND EXTRACTVALUE(VALUE(I), '/P/@n') =:PARANAME ");
                        OracleParameter[] parameters = {
                            new OracleParameter("NS", OracleDbType.Varchar2,100),
                            new OracleParameter("BOT", OracleDbType.Varchar2,100),
                            new OracleParameter("PARANAME", OracleDbType.Varchar2,100)
                                            };
                        parameters[0].Value = appDomain;
                        parameters[1].Value = bot;
                        parameters[2].Value = item.Name;
                        DataTable enumDt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
                        PropertySchema proSchema;
                        if (enumDt.Rows.Count > 0)
                        {
                            proSchema = new PropertySchema();
                            if (enumDt.Rows.Count > 0)
                            {
                                proSchema.Name = enumDt.Rows[0]["NAME"].ToString();
                                if (PropertyDataType.Date.ToString().ToUpper() == enumDt.Rows[0]["DataType"].ToString().ToUpper())
                                {
                                    proSchema.DataType = PropertyDataType.Date;
                                }
                                else if (PropertyDataType.Decimal.ToString().ToUpper() == enumDt.Rows[0]["DataType"].ToString().ToUpper())
                                {
                                    proSchema.DataType = PropertyDataType.Decimal;
                                }
                                else
                                {
                                    proSchema.DataType = PropertyDataType.String;
                                }
                                if (!string.IsNullOrEmpty(enumDt.Rows[0]["VS"].ToString()))
                                {
                                    proSchema.Values = enumDt.Rows[0]["VS"].ToString().Split(',').ToList();
                                    proSchema.Values.Distinct<string>().ToList();
                                }
                            }
                        }
                        else
                        {
                            proSchema = null;
                        }
                        strSql = new StringBuilder();
                        strSql.Append(" WITH TAB AS ");
                        strSql.Append(" (SELECT DISTINCT EXTRACTVALUE(VALUE(I), ");
                        strSql.Append(" '/P/@n') NAME, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I),");
                        strSql.Append(" '/P/@t') DataType, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), ");
                        strSql.Append("  '/P') VS ");
                        strSql.Append(" FROM PROPERTY X,   bo t1,  objecttype t2, ");
                        strSql.Append("  TABLE(XMLSEQUENCE(EXTRACT(X.MD, ");
                        strSql.Append(" '/PropertySet/P'))) I ");
                        strSql.Append(" WHERE X.BOID = T1.BOID");
                        strSql.Append(" AND T1.BOTID = T2.BOTID ");
                        strSql.Append(" AND T2.BOT = :BOT  ");
                        strSql.Append(" AND X.NS = :APPDOMAIN ");
                        strSql.Append(" AND EXTRACTVALUE(VALUE(I), ");
                        strSql.Append(" '/P/@n') =:PARANAME) ");
                        strSql.Append(" SELECT A.NAME, A.DataType, LTRIM(MAX(SYS_CONNECT_BY_PATH(A.VS, ',')), ',') VS ");
                        strSql.Append(" FROM (SELECT ROW_NUMBER() OVER(PARTITION BY A.NAME, A.NAME, A.DataType ORDER BY A.NAME, A.NAME, A.DataType) RN,");
                        strSql.Append(" A.*   FROM TAB A) A  ");
                        strSql.Append(" START WITH RN = 1 CONNECT BY PRIOR RN = RN - 1  ");
                        strSql.Append(" AND A.NAME = PRIOR A.NAME  ");
                        strSql.Append(" AND A.DataType = PRIOR A.DataType  ");
                        strSql.Append(" GROUP BY A.NAME, A.NAME, A.DataType  ");

                        OracleParameter[] factParameters = {
                            new OracleParameter("BOT", OracleDbType.Varchar2,100),
                            new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,100),
                            new OracleParameter("PARANAME", OracleDbType.Varchar2,100)
                                            };
                        factParameters[0].Value = bot;
                        factParameters[1].Value = appDomain;
                        factParameters[2].Value = item.Name;
                        DataTable factDt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), factParameters).Tables[0];
                        if (factDt.Rows.Count > 0)
                        {
                            proSchema.Values.AddRange(factDt.Rows[0]["VS"].ToString().Split(',').ToList());
                            proSchema.Values = proSchema.Values.Distinct<string>().ToList();
                        }
                        if (proSchema != null)
                        {
                            list.Add(proSchema);
                        }
                        #endregion
                    }
                    else if (item.ValueType == ValueTypes.Enum)//取枚举值
                    {
                        #region 获取当前应用场景下参数以及参数的枚举值
                        strSql.Append(" SELECT DISTINCT EXTRACTVALUE(VALUE(I), '/P/@n') AS NAME, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), '/P/@t') AS DATATYPE, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), '/P') AS VS ");
                        strSql.Append(" FROM OBJTYPEPROPERTY X, ");
                        strSql.Append(" OBJECTTYPE T2, ");
                        strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
                        strSql.Append(" WHERE X.BOTID = T2.BOTID ");
                        strSql.Append(" AND  X.NS =:NS ");
                        strSql.Append(" AND  T2.BOT =:BOT ");
                        strSql.Append(" AND EXTRACTVALUE(VALUE(I), '/P/@n') =:PARANAME ");

                        OracleParameter[] parameters = {
                            new OracleParameter("NS", OracleDbType.Varchar2,100),
                            new OracleParameter("BOT", OracleDbType.Varchar2,100),
                            new OracleParameter("PARANAME", OracleDbType.Varchar2,100)
                                            };
                        parameters[0].Value = appDomain;
                        parameters[1].Value = bot;
                        parameters[2].Value = item.Name;
                        DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
                        foreach (DataRow row in dt.Rows)
                        {
                            PropertySchema proSchema = new PropertySchema();
                            proSchema.Name = row["NAME"].ToString();
                            if (PropertyDataType.Date.ToString().ToUpper() == row["DataType"].ToString().ToUpper())
                            {
                                proSchema.DataType = PropertyDataType.Date;
                            }
                            else if (PropertyDataType.Decimal.ToString().ToUpper() == row["DataType"].ToString().ToUpper())
                            {
                                proSchema.DataType = PropertyDataType.Decimal;
                            }
                            else
                            {
                                proSchema.DataType = PropertyDataType.String;
                            }
                            proSchema.Values = row["VS"].ToString().Split(',').ToList();
                            list.Add(proSchema);
                        }
                        #endregion
                    }
                    else if (item.ValueType == ValueTypes.Fact)//取实际值
                    {
                        #region 取实际值
                        strSql.Append(" WITH TAB AS ");
                        strSql.Append(" (SELECT DISTINCT EXTRACTVALUE(VALUE(I), ");
                        strSql.Append(" '/P/@n') NAME, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I),");
                        strSql.Append(" '/P/@t') DataType, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), ");
                        strSql.Append("  '/P') VS ");
                        strSql.Append(" FROM PROPERTY X,   bo t1,  objecttype t2, ");
                        strSql.Append("  TABLE(XMLSEQUENCE(EXTRACT(X.MD, ");
                        strSql.Append(" '/PropertySet/P'))) I ");
                        strSql.Append(" WHERE X.BOID = T1.BOID");
                        strSql.Append(" AND T1.BOTID = T2.BOTID ");
                        strSql.Append(" AND T2.BOT = :BOT  ");
                        strSql.Append(" AND X.NS = :APPDOMAIN ");
                        strSql.Append(" AND EXTRACTVALUE(VALUE(I), ");
                        strSql.Append(" '/P/@n') =:PARANAME) ");
                        strSql.Append(" SELECT A.NAME, A.DataType, LTRIM(MAX(SYS_CONNECT_BY_PATH(A.VS, ',')), ',') VS ");
                        strSql.Append(" FROM (SELECT ROW_NUMBER() OVER(PARTITION BY A.NAME, A.NAME, A.DataType ORDER BY A.NAME, A.NAME, A.DataType) RN,");
                        strSql.Append(" A.*   FROM TAB A) A  ");
                        strSql.Append(" START WITH RN = 1 CONNECT BY PRIOR RN = RN - 1  ");
                        strSql.Append(" AND A.NAME = PRIOR A.NAME  ");
                        strSql.Append(" AND A.DataType = PRIOR A.DataType  ");
                        strSql.Append(" GROUP BY A.NAME, A.NAME, A.DataType  ");

                        OracleParameter[] parameters = {
                            new OracleParameter("BOT", OracleDbType.Varchar2,100),
                            new OracleParameter("APPDOMAIN", OracleDbType.Varchar2,100),
                            new OracleParameter("PARANAME", OracleDbType.Varchar2,100)
                                            };
                        parameters[0].Value = bot;
                        parameters[1].Value = appDomain;
                        parameters[2].Value = item.Name;
                        DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
                        foreach (DataRow row in dt.Rows)
                        {
                            PropertySchema proSchema = new PropertySchema();
                            proSchema.Name = row["NAME"].ToString();
                            if (PropertyDataType.Date.ToString().ToUpper() == row["DataType"].ToString().ToUpper())
                            {
                                proSchema.DataType = PropertyDataType.Date;
                            }
                            else if (PropertyDataType.Decimal.ToString().ToUpper() == row["DataType"].ToString().ToUpper())
                            {
                                proSchema.DataType = PropertyDataType.Decimal;
                            }
                            else
                            {
                                proSchema.DataType = PropertyDataType.String;
                            }
                            proSchema.Values = row["VS"].ToString().Split(',').ToList();
                            list.Add(proSchema);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 获取参数的数据类型
                        strSql.Append(" SELECT DISTINCT EXTRACTVALUE(VALUE(I), '/P/@n') AS NAME, ");
                        strSql.Append(" EXTRACTVALUE(VALUE(I), '/P/@t') AS DATATYPE ");
                        strSql.Append(" FROM OBJTYPEPROPERTY X, ");
                        strSql.Append(" OBJECTTYPE T2, ");
                        strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
                        strSql.Append(" WHERE X.BOTID = T2.BOTID ");
                        strSql.Append(" AND  X.NS =:NS ");
                        strSql.Append(" AND  T2.BOT =:BOT ");
                        strSql.Append(" AND  EXTRACTVALUE(VALUE(I), '/P/@n') =:NAME ");
                        OracleParameter[] parameters = {
                            new OracleParameter("NS", OracleDbType.Varchar2,100),
                            new OracleParameter("BOT", OracleDbType.Varchar2,100),
                            new OracleParameter("NAME", OracleDbType.Varchar2,100)
                                            };
                        parameters[0].Value = appDomain;
                        parameters[1].Value = bot;
                        parameters[2].Value = item.Name;
                        DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
                        foreach (DataRow row in dt.Rows)
                        {
                            PropertySchema proSchema = new PropertySchema();
                            proSchema.Name = row["NAME"].ToString();
                            if (PropertyDataType.Date.ToString().ToUpper() == row["DataType"].ToString().ToUpper())
                            {
                                proSchema.DataType = PropertyDataType.Date;
                            }
                            else if (PropertyDataType.Decimal.ToString().ToUpper() == row["DataType"].ToString().ToUpper())
                            {
                                proSchema.DataType = PropertyDataType.Decimal;
                            }
                            else
                            {
                                proSchema.DataType = PropertyDataType.String;
                            }
                            list.Add(proSchema);
                        }

                        #endregion
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取DBINFO基本信息
        /// </summary>
        /// <returns></returns>
        public DbInfo GetDbInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DBSERNAME,CRS,CSPARAM FROM DBInfo ");
            return DBUtility.OracleDBHelper.OracleHelper.ExecuteQueryText<DbInfo>(strSql.ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 根据查询条件获取GGGX数据
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="bos"></param>
        /// <param name="filter"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public XmlDocument Get3GXByFilter(string bot, List<string> bos, string filter, GGGXDataCategory category)
        {
            string sqlWhere = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DISTINCT BO.*,OBJECTTYPE.BOT,OBJECTTYPE.FT  FROM BO,OBJECTTYPE");
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
                strSql.Append(" WHERE BO.BOTID = OBJECTTYPE.BOTID  ");
                strSql.Append(" AND PROPERTY.BOID = BO.BOID  ");
                strSql.Append(string.Format(" AND  OBJECTTYPE.BOT='{0}' ", bot));
                strSql.Append(" AND  BO.BOTID=OBJECTTYPE.BOTID  ");
            }
            else
            {
                strSql.Append(string.Format(" WHERE  OBJECTTYPE.BOT='{0}' ", bot));
                strSql.Append(" AND  BO.BOTID=OBJECTTYPE.BOTID  ");
            }
            //对象名称
            if (bos != null && bos.Count > 0)
            {
                string strBos = string.Empty;
                for (int i = 0; i < bos.Count; i++)
                {
                    if (i == bos.Count - 1)
                    {
                        strBos += "'" + bos[i].Trim() + "'";
                    }
                    else
                    {
                        strBos += "'" + bos[i].Trim() + "',";
                    }
                }
                strSql.Append(string.Format(" AND  BO.NAME in ({0}) ", strBos));
            }
            //属性
            if (!string.IsNullOrEmpty(filter))
            {
                strSql.Append(" AND " + "( " + MongoJsonToSql.JsonToSql(filter, bot).ToString() + " )");
            }
            DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString()).Tables[0];
            List<GeoFeature> ftList = new List<GeoFeature>();
            foreach (DataRow row in dt.Rows)
            {
                GeoFeature ft = new GeoFeature();
                ft.BOID = row["Boid"].ToString();
                ft.BOT = row["BOT"].ToString();
                ft.FT = row["FT"].ToString();
                ft.NAME = row["Name"].ToString();
                ft.CLASS = Comm.GetClassByBoid(ft.BOID);
                if (category == GGGXDataCategory.B)
                {
                    ft.PropertyList = Comm.GetPropertyByBoid(ft.BOID);
                    ft.GeometryList = Comm.GetGeometryByBoid(ft.BOID);
                }
                else if (category == GGGXDataCategory.P)
                {
                    ft.PropertyList = Comm.GetPropertyByBoid(ft.BOID);
                    ft.GeometryList = null;
                }
                else if (category == GGGXDataCategory.G)
                {
                    ft.PropertyList = null;
                    ft.GeometryList = Comm.GetGeometryByBoid(ft.BOID);
                }
                ftList.Add(ft);
            }
            return GGGXParse.ConvertFT.FeatureToGGGX(ftList);
        }

        /// <summary>
        /// 获取应用场景已经参数
        /// </summary>
        /// <returns></returns>
        public AppDomainCollection GetAppDomains()
        {
            AppDomainCollection coll = new AppDomainCollection();
            List<Jurassic.PKS.Service.GF.AppDomain> list = new List<Jurassic.PKS.Service.GF.AppDomain>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" WITH TAB AS( ");
            strSql.Append(" SELECT T.BOT, T1.NS ");
            strSql.Append(" FROM OBJECTTYPE T ");
            strSql.Append(" LEFT JOIN OBJTYPEPROPERTY T1 ");
            strSql.Append(" ON T.BOTID = T1.BOTID) ");
            strSql.Append(" SELECT A.BOT, ");
            strSql.Append(" LTRIM(MAX(SYS_CONNECT_BY_PATH(NS, ',')), ',') NSS ");
            strSql.Append(" FROM (SELECT ROW_NUMBER() OVER(PARTITION BY A.BOT ORDER BY A.BOT) RN, A.* ");
            strSql.Append(" FROM TAB A) A ");
            strSql.Append(" START WITH RN = 1 ");
            strSql.Append(" CONNECT BY PRIOR RN = RN - 1 ");
            strSql.Append(" AND A.BOT = PRIOR A.BOT  ");
            strSql.Append(" GROUP BY A.BOT ");
            DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString()).Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                Jurassic.PKS.Service.GF.AppDomain appdomain = new Jurassic.PKS.Service.GF.AppDomain();
                appdomain.BOT = row["BOT"].ToString();
                if (!string.IsNullOrEmpty(row["NSS"].ToString()))
                {
                    appdomain.Appdomain = row["NSS"].ToString().Split(',').ToList();
                }
                list.Add(appdomain);
            }
            coll.AddRange(list);
            return coll;
        }
    }
}

