using GF.Server.DBUtility;
using GGGXParse;
using Juarssic.Server.Comm;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.GF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;

namespace GF.Server.SqlServer
{
    public class GGGXBusinesscs : IGGGX
    {
        /// <summary>
        /// 获取对象类型集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetBOTList()
        {
            return SqlServerDBHelper.ExecuteQueryText<string>("SELECT BOT FROM OBJECTTYPE ");
        }

        /// <summary>
        /// 获取指定查询条件的取值
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<string> GetDomain(string ft, string parameter)
        {
            string[] strArr = parameter.Split('.').ToArray();
            StringBuilder strSql = new StringBuilder();
            List<string> list = new List<string>();
            strSql.Append(" SELECT VS=COL.value('text()[1]','VARCHAR(MAX)') ");
            strSql.Append(" FROM OBJECTTYPE T2,OBJTYPEPROPERTY X CROSS APPLY MD.nodes('/PropertySet/P') AS TBL(COL) ");
            strSql.Append( string.Format(" WHERE X.BOTID=T2.BOTID AND X.NS='{0}' AND T2.FT='{1}' AND COL.value('@n','VARCHAR(MAX)')='{2}' ", strArr[0], ft, strArr[1]));

            DataTable dt = SqlServerDBHelper.GetDataTable(strSql.ToString());
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
            DataTable objTypeDt = SqlServerDBHelper.GetDataTable("SELECT BOTID,FT FROM OBJECTTYPE ");
            //DataTable dt;
            foreach (DataRow row in objTypeDt.Rows)
            {
                FT ft = new FT(row["FT"].ToString());
                ft.Name = row["FT"].ToString();
                SqlParameter[] parameters = {
                                     new SqlParameter("BOTID",SqlDbType.VarChar,36)
                                                };
                parameters[0].Value = row["BOTID"].ToString();
                DataTable dt = new DataTable();
                //MD不截取发布服务出错
                dt = SqlServerDBHelper.GetDataTable("SELECT  MD AS MD,NS FROM OBJTYPEPROPERTY WHERE BOTID = @BOTID ", parameters);
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
            strSql.Append("SELECT DISTINCT BO.*,OBJECTTYPE.BOT,OBJECTTYPE.FT  FROM BO,OBJECTTYPE");
            if (!string.IsNullOrEmpty(filter.BBox))
            {
                strSql.Append(",v_Geometry ");
            }
            if (filter.Filter != null)
            {
                strSql.Append(" ,PROPERTY PROPERTY  ");
                strSql.Append(" WHERE BO.BOTID = OBJECTTYPE.BOTID  ");
                strSql.Append(" AND PROPERTY.BOID = BO.BOID  ");

                strSql.Append(string.Format(" AND  OBJECTTYPE.FT='{0}' ", filter.FT));
                strSql.Append(" AND  BO.BOTID=OBJECTTYPE.BOTID ");
            }
            else
            {
                strSql.Append(string.Format(" WHERE  OBJECTTYPE.FT='{0}' ", filter.FT));
                strSql.Append(" AND  BO.BOTID=OBJECTTYPE.BOTID ");
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
                strSql.Append(" AND v_Geometry.BOID=BO.BOID  ");
                strSql.Append(string.Format(" AND ( v_Geometry.GEOMETRY.STWithin(GEOGRAPHY::STGeomFromText('{0}', 4326))=1 or ", filter.BBox));
                strSql.Append(string.Format("  v_Geometry.GEOMETRY.STIntersects(GEOGRAPHY::STGeomFromText('{0}', 4326))=1  )  ", filter.BBox));
            }
            //属性
            if (filter.Filter != null)
            {
                JObject jObj = JObject.Parse(filter.Filter.ToString());
                string bot = SqlServerDBHelper.ExecuteQueryText<string>(string.Format("SELECT BOT FROM OBJECTTYPE WHERE FT='{0}'", filter.FT)).FirstOrDefault();
                strSql.Append(" AND " + "( " + MongoJsonToSql.JsonToSql(jObj.ToString(), bot).ToString() + " )");
            }
            DataTable dt = SqlServerDBHelper.GetDataTable(strSql.ToString());
            List<GeoFeature> ftList = new List<GeoFeature>();
            foreach (DataRow row in dt.Rows)
            {
                GeoFeature ft = new GeoFeature();
                ft.BOID = row["Boid"].ToString();
                ft.BOT = row["BOT"].ToString();
                ft.FT = row["FT"].ToString();
                ft.NAME = row["Name"].ToString();
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
            return SqlServerDBHelper.ExecuteQueryText<string>("SELECT BOT FROM OBJECTTYPE ");
        }
    }
}
