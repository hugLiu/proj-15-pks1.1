using GF.Server.DBUtility;
using GGGXParse;
using Juarssic.Server.Comm;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.GF;
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
    public class GGGXBusinesscs : IGGGX
    {
        /// <summary>
        /// 获取对象类型集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetBOTList()
        {
            return OracleDBHelper.OracleHelper.ExecuteQueryText<string>("SELECT BOT FROM OBJECTTYPE ");
        }

        /// <summary>
        /// 获取指定查询条件的取值
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<string> GetDomain(string ft, string parameter)
        {
            StringBuilder strSql = new StringBuilder();
            List<string> list = new List<string>();
            strSql.Append(" SELECT DISTINCT  ");
            strSql.Append(" EXTRACTVALUE(VALUE(I), '/P') AS VS ");
            strSql.Append(" FROM OBJTYPEPROPERTY X, ");
            strSql.Append(" OBJECTTYPE T2, ");
            strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
            strSql.Append(" WHERE X.BOTID = T2.BOTID ");
            strSql.Append(" AND  X.NS =:NS ");
            strSql.Append(" AND  T2.FT =:FT ");
            strSql.Append(" AND EXTRACTVALUE(VALUE(I), '/P/@n') =:PARANAME ");
            OracleParameter[] parameters = {
                            new OracleParameter("NS", OracleDbType.Varchar2,100),
                            new OracleParameter("FT", OracleDbType.Varchar2,100),
                            new OracleParameter("PARANAME", OracleDbType.Varchar2,100)
                                            };
            string[] strArr = parameter.Split('.').ToArray();
            parameters[0].Value = strArr[0];
            parameters[1].Value = ft;
            parameters[2].Value = strArr[1];
            DataTable dt = OracleDBHelper.OracleHelper.ExecuteQueryText(strSql.ToString(), parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = dt.Rows[0]["VS"].ToString().Split(',').ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取FT集合
        /// </summary>
        /// <returns></returns>
        public FTCCollection GetFTCList()
        {
            FTCCollection list = new FTCCollection();
            FTC ftc = new FTC("平面");
            ftc.Name = "平面";
            //获取全部对象类型
            DataTable objTypeDt = OracleDBHelper.OracleHelper.ExecuteQueryText("SELECT BOTID,FT FROM OBJECTTYPE ").Tables[0];
            //DataTable dt;
            foreach (DataRow row in objTypeDt.Rows)
            {
                FT ft = new FT(row["FT"].ToString());
                ft.Name = row["FT"].ToString();
                OracleParameter[] parameters = {
                                     new OracleParameter("BOTID",OracleDbType.Varchar2,36)
                                                };
                parameters[0].Value = row["BOTID"].ToString();
                DataTable dt = new DataTable();
                //MD不截取发布服务出错
                dt = OracleDBHelper.OracleHelper.ExecuteQueryText("SELECT  TO_CHAR(SUBSTR(MD, 1, 3900)) AS MD,NS FROM OBJTYPEPROPERTY WHERE BOTID = :BOTID ", parameters).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["MD"].ToString()))
                    {
                        NS ns = new NS(dt.Rows[i]["NS"].ToString());
                        ns.Name = dt.Rows[i]["NS"].ToString();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(dt.Rows[i]["MD"].ToString());
                        for (int j = 0; j < xmlDoc.SelectNodes("PropertySet/P").Count; j++)
                        {
                            FeatureParameter para = new FeatureParameter(xmlDoc.SelectNodes("PropertySet/P").Item(j).Attributes[0].Value);
                            para.Name = xmlDoc.SelectNodes("PropertySet/P").Item(j).Attributes[0].Value;
                            if (xmlDoc.SelectNodes("PropertySet/P").Item(j).Attributes[1].Value.ToUpper() == Jurassic.PKS.Service.PropertyDataType.String.ToString().ToUpper())
                            {
                                para.DataType = PropertyDataType.String;
                            }
                            else if (xmlDoc.SelectNodes("PropertySet/P").Item(j).Attributes[1].Value.ToUpper() == Jurassic.PKS.Service.PropertyDataType.Decimal.ToString().ToUpper())
                            {
                                para.DataType = PropertyDataType.Decimal;
                            }
                            else
                            {
                                para.DataType = PropertyDataType.Date;
                            }
                            ns.Parameters.Add(para);
                        }
                        ft.NSs.Add(ns);
                    }
                }
                ftc.FTs.Add(ft);
            }
            list.Add(ftc);
            return list;
        }

        /// <summary>
        /// 根据条件获取3GX数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public XmlDocument GetFeatures(FeatureFilter filter)
        {
            string sqlWhere = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DISTINCT BO.*,OBJECTTYPE.BOT,OBJECTTYPE.FT  FROM BO,OBJECTTYPE,Geometry");
            if (filter.Filter != null)
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
                strSql.Append(" WHERE BO.BOTID = OBJECTTYPE.BOTID  ");
                strSql.Append(" AND PROPERTY.BOID = BO.BOID  ");
                strSql.Append(" AND GEOMETRY.BOID=BO.BOID  ");
                strSql.Append(string.Format(" AND  OBJECTTYPE.FT='{0}' ", filter.FT));
                strSql.Append(" AND  BO.BOTID=OBJECTTYPE.BOTID  AND BO.BOID=GEOMETRY.BOID  ");
            }
            else
            {
                strSql.Append(string.Format(" WHERE  OBJECTTYPE.FT='{0}' ", filter.FT));
                strSql.Append(" AND  BO.BOTID=OBJECTTYPE.BOTID  AND BO.BOID=GEOMETRY.BOID  ");
            }
            //对象名称
            if (filter.BOs != null && filter.BOs.Count > 0)
            {
                string bos = string.Empty;
                for (int i = 0; i < filter.BOs.Count; i++)
                {
                    if (i == filter.BOs.Count - 1)
                    {
                        bos += "'" + filter.BOs[i].Trim() + "'";
                    }
                    else
                    {
                        bos += "'" + filter.BOs[i].Trim() + "',";
                    }
                }
                strSql.Append(string.Format(" AND  BO.NAME in {0} ", bos));
            }
            //空间范围和crs  未启用坐标范围
            if (!string.IsNullOrEmpty(filter.BBox))
            {
                strSql.Append(" AND SDO_FILTER(GEOMETRY.GEOMETRY, ");
                strSql.Append(" SDO_GEOMETRY('" + filter.BBox + "',4326), ");
                strSql.Append(" 'QUERYTYPE=WINDOW') = 'TRUE' ");
            }
            //属性
            if (filter.Filter != null)
            {
                JObject jObj = JObject.Parse(filter.Filter.ToString());
                string bot = OracleDBHelper.OracleHelper.ExecuteQueryText<string>(string.Format("SELECT BOT FROM OBJECTTYPE WHERE FT='{0}'", filter.FT)).FirstOrDefault();
                strSql.Append(" AND " + "( " + MongoJsonToSql.JsonToSql(jObj.ToString(), bot).ToString() + " )");
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
                ft.CLASS = GF.Server.Oracle.Comm.GetClassByBoid(ft.BOID);
                ft.AliasNameList = Comm.GetAliasNameByBoid(ft.BOID);
                ft.PropertyList = Comm.GetPropertyByBoid(ft.BOID);
                ft.GeometryList = Comm.GetGeometryByBoid(ft.BOID);
                ftList.Add(ft);
            }
            return GGGXParse.ConvertFT.FeatureToGGGX(ftList);
        }

        /// <summary>
        /// 获取全部对象类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllBOT()
        {
            return OracleDBHelper.OracleHelper.ExecuteQueryText<string>("SELECT BOT FROM OBJECTTYPE ");
        }
    }
}
