using Jurassic.PKS.Service.GF;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GGGXParse;
using System.IO;
using GF.Server.DBUtility;
using System.Data.SqlClient;

namespace GF.Server.SqlServer
{
    public class SummissionBusiness : ISubmission
    {
        string xmldata;
        string option; //操作覆盖或是保留
        SubmissionResult result = null;
        SubmissionError subErr = null;
        List<SubmissionError> ListsubErr = null;
        /// <summary>
        /// 提交3GX数据到主数据库中
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public SubmissionResult Submit(SubmissionInfo info)
        {
            result = new SubmissionResult();
            ListsubErr = new List<SubmissionError>();
            xmldata = Xml2Str(info.GGGXData);//得到传入的3GX数据
            option = Enum.GetName(typeof(SubmissionOption), info.Option);//得到传入的枚举值
            List<GeoFeature> ftList = ConvertFT.ConvertToFTListByXML(xmldata);//将3GX文件转换为对象集合
            if (ftList != null)
                Save3GX(ftList, option);//保存数据      
            return result;
        }

        public string Xml2Str(XmlDocument XmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            XmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlstring = sr.ReadToEnd();
            sr.Close();
            return xmlstring;
        }
        /// <summary>
        /// 保存3GX数据到数据库中
        /// </summary>
        /// <param name="boExModelList"></param>
        /// <param name="replaceOrLeave"></param>
        /// <returns></returns>
        private int Save3GX(List<GeoFeature> ftList, string replaceOrLeave = null)
        {
            int i = 0;
            int iInsertCount = 0;//记录添加条数
            int iUpdateCount = 0;//记录修改条数
            bool IsInsert = true;
            SqlServerDBHelper.SQLEntity sqlEntity;
            SqlParameter[] parameters;
            StringBuilder strSql = new StringBuilder();
            foreach (GeoFeature bo in ftList)
            {
                subErr = new SubmissionError();
                List<SqlServerDBHelper.SQLEntity> sqlList = new List<SqlServerDBHelper.SQLEntity>();
                bool isBoExist = false;
                GeoFeature seachFt = GetBoListByName(bo.NAME, bo.FT);
                if (seachFt != null)
                {
                    isBoExist = true;
                    //对象存在则需要把带入库的对象id都换成库中的该对象id
                    bo.BOID = seachFt.BOID;
                }
                else
                {
                    bo.BOID = System.Guid.NewGuid().ToString();
                }

                #region 添加对象
                //存在且保留
                if (isBoExist && replaceOrLeave.ToUpper() == "UNCHANGE") ;
                //覆盖
                else
                {
                    strSql = new StringBuilder();
                    //对象不存在
                    if (!isBoExist)
                    {
                        strSql.Append(" INSERT INTO BO(  ");
                        strSql.Append(" NAME,BOID,BOTID) ");
                        strSql.Append(" VALUES (@NAME,@BOID,@BOTID)");
                        IsInsert = true;
                    }
                    //对象存在
                    else
                    {
                        strSql.Append(" UPDATE BO SET ");
                        strSql.Append(" NAME=@NAME ");
                        strSql.Append(" WHERE BOID=@BOID  AND BOTID= @BOTID ");
                        IsInsert = false;
                    }
                    parameters = new SqlParameter[] {                              
                                 new SqlParameter("NAME",SqlDbType.VarChar,50),
                                 new SqlParameter("BOID",SqlDbType.VarChar,36),
                                 new SqlParameter("BOTID",SqlDbType.VarChar,36)                
                                                        };
                    parameters[0].Value = bo.NAME;
                    parameters[1].Value = bo.BOID == null ? System.Guid.NewGuid().ToString() : bo.BOID;
                    //根据FT查找对象的BOTID没用找到时，添加会出现异常
                    parameters[2].Value = DBUtility.SqlServerDBHelper.ExecuteQueryText<string>(string.Format(" SELECT BOTID FROM OBJECTTYPE WHERE FT='{0}'", bo.FT)).FirstOrDefault();
                    if (parameters[2].Value == null)
                    {
                        subErr.BOName = bo.NAME;
                        subErr.Error = bo.NAME + "对应的对象类型不存在";
                        result.TotalBO = ftList.Count;
                        result.UpdatedBO = iUpdateCount;
                        result.FailedBO = ListsubErr.Count + 1;
                        result.InsertedBO = iInsertCount;
                        result.Errors.Add(subErr);
                        continue;
                    }
                    sqlEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                    sqlEntity.Sqlstr = strSql.ToString();
                    sqlEntity.Sqlparameter = parameters;
                    sqlList.Add(sqlEntity);
                }
                #endregion

                #region 添加坐标数据
                bool isGeometryExist = false;
                //如果对象存在，需要判断坐标是否存在，只要该对象存在一组坐标，就认为该对象坐标存在
                if (isBoExist)
                {
                    bo.GeometryList[0].BOID = bo.BOID;
                    isGeometryExist = GeometryeExist(bo.GeometryList[0]);
                }
                //坐标且保留
                if (isGeometryExist && replaceOrLeave.ToUpper() == "UNCHANGE") ;
                //覆盖
                else
                {
                    //坐标存在，先删除
                    if (isGeometryExist)
                    {
                        strSql = new StringBuilder();
                        strSql.Append(" DELETE FROM  GEOMETRY  ");
                        strSql.Append(" WHERE BOID =@BOID ");

                        parameters = new SqlParameter[] {                
                         new SqlParameter("BOID",SqlDbType.VarChar, 36)
                                   };
                        parameters[0].Value = bo.BOID;

                        sqlEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                        sqlEntity.Sqlstr = strSql.ToString();
                        sqlEntity.Sqlparameter = parameters;
                        sqlList.Add(sqlEntity);
                    }

                    foreach (Geometry geometry in bo.GeometryList)
                    {
                        strSql = new StringBuilder();
                        strSql.Append(" INSERT INTO GEOMETRY(  ");
                        strSql.Append(" BOID,NAME,GEOMETRY,SOURCEDB)");
                        strSql.Append(" VALUES (@BOID,@NAME,GEOGRAPHY::STGeomFromText(@GEOMETRY, 4326),@SOURCEDB)");

                        parameters = new SqlParameter[]{
                            new SqlParameter("BOID",SqlDbType.VarChar,36),
                            new SqlParameter("NAME",SqlDbType.VarChar,100),
                            new SqlParameter("GEOMETRY", SqlDbType.VarChar),
                            new SqlParameter("SOURCEDB",SqlDbType.VarChar,100)
                                           };
                        parameters[0].Value = bo.BOID == null ? System.Guid.NewGuid().ToString() : bo.BOID;
                        parameters[1].Value = geometry.NAME == null ? geometry.GEOMETRY.Split('(')[0] : geometry.NAME;
                        parameters[2].Value = geometry.GEOMETRY;
                        parameters[3].Value = geometry.SOURCEDB;

                        sqlEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                        sqlEntity.Sqlstr = strSql.ToString();
                        sqlEntity.Sqlparameter = parameters;
                        sqlList.Add(sqlEntity);
                    }
                }
                #endregion

                #region 添加属性数据
                foreach (Property property in bo.PropertyList)
                {
                    strSql = new StringBuilder();
                    bool isPropertyExist = false;
                    if (isBoExist)
                    {
                        property.BOID = bo.BOID;
                        isPropertyExist = PropertyExist(property);
                    }
                    //该条属性存在且保留，跳过
                    if (isPropertyExist && replaceOrLeave.ToUpper() == "UNCHANGE") ;
                    else
                    {
                        if (!isPropertyExist)
                        {
                            strSql.Append(" INSERT INTO PROPERTY(  ");
                            strSql.Append(" MD,MDSOURCE,BOID,NS)");
                            strSql.Append(" VALUES (@MD,@MDSOURCE,@BOID,@NS)");
                        }
                        else if (isPropertyExist && replaceOrLeave.ToUpper() != "UNCHANGE")
                        {
                            strSql.Append(" UPDATE PROPERTY SET ");
                            strSql.Append(" MD=@MD ,MDSOURCE=@MDSOURCE ");
                            strSql.Append(" WHERE BOID=@BOID AND NS=@NS");
                        }

                        parameters = new SqlParameter[] {
                            new SqlParameter("MD", SqlDbType.Xml),
                            new SqlParameter("MDSOURCE",SqlDbType.VarChar,50),
                            new SqlParameter("BOID",SqlDbType.VarChar,36),
                            new SqlParameter("NS",SqlDbType.VarChar,50)                           
                                                        };
                        parameters[0].Value = property.MD;
                        parameters[1].Value = property.MdSource;
                        parameters[2].Value = bo.BOID;
                        parameters[3].Value = property.NS;

                        sqlEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                        sqlEntity.Sqlstr = strSql.ToString();
                        sqlEntity.Sqlparameter = parameters;
                        sqlList.Add(sqlEntity);
                    }
                }
                #endregion

                #region 添加别名
                foreach (AliasName aliasName in bo.AliasNameList)
                {
                    strSql = new StringBuilder();
                    bool isAliasNameExist = false;
                    if (isBoExist)
                    {
                        aliasName.BOID = bo.BOID;
                        isAliasNameExist = AliasNameExist(aliasName);
                    }
                    //该条属性存在且保留，跳过
                    if (isAliasNameExist && replaceOrLeave.ToUpper() == "UNCHANGE") ;
                    else
                    {
                        strSql = new StringBuilder();
                        if (!isAliasNameExist)
                        {
                            strSql.Append(" INSERT INTO ALIASNAME(  ");
                            strSql.Append(" NAME,BOID,APPDOMAIN)");
                            strSql.Append(" VALUES (@NAME,@BOID,@APPDOMAIN)");
                        }
                        else if (isAliasNameExist && replaceOrLeave.ToUpper() != "LEAVE")
                        {
                            strSql.Append(" UPDATE ALIASNAME SET ");
                            strSql.Append(" NAME=@NAME ");
                            strSql.Append(" WHERE BOID=@BOID AND APPDOMAIN=@APPDOMAIN");
                        }
                        parameters = new SqlParameter[]{
                                new SqlParameter("NAME",SqlDbType.VarChar,50),
                                new SqlParameter("BOID",SqlDbType.VarChar,36),
                                new SqlParameter("APPDOMAIN",SqlDbType.VarChar,50)
                                                           };
                        parameters[0].Value = aliasName.NAME;
                        parameters[1].Value = aliasName.BOID;
                        parameters[2].Value = aliasName.APPDOMAIN;

                        sqlEntity = new DBUtility.SqlServerDBHelper.SQLEntity();
                        sqlEntity.Sqlstr = strSql.ToString();
                        sqlEntity.Sqlparameter = parameters;
                        sqlList.Add(sqlEntity);
                    }
                }
                #endregion

                try
                {
                    //存在需要执行的sql语句
                    if (sqlList.Count > 0)
                    {
                        i = i + (DBUtility.SqlServerDBHelper.ExecuteSql(sqlList) == true ? 1 : 0);
                        if (IsInsert == true)
                        {
                            iInsertCount++;
                        }
                        else
                        {
                            iUpdateCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //记录入库错误信息
                    subErr.BOName = bo.NAME;
                    subErr.Error = "空间数据有误！";
                    result.Errors.Add(subErr);
                    //continue;
                    //记录完毕
                    throw ex;
                }

                //构造需要返回的SubmissionResult
                result.TotalBO = ftList.Count;
                result.UpdatedBO = iUpdateCount;
                result.FailedBO = ListsubErr.Count;
                result.InsertedBO = iInsertCount;
                //result.Errors.AddRange(ListsubErr);
                //结束
            }
            return i;
        }

        /// <summary>
        /// 根据名称和对象特征类型获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        private GeoFeature GetBoListByName(string name, string ft)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T.BOID,T1.BOTID  FROM BO T");
            strSql.Append(" LEFT JOIN OBJECTTYPE T1");
            strSql.Append(" ON T.BOTID=T1.BOTID");
            strSql.Append(" WHERE T.NAME =@NAME AND T1.FT=@FT ");
            SqlParameter[] parameters = {
                                 new SqlParameter("NAME",SqlDbType.VarChar,50),
                                 new SqlParameter("FT",SqlDbType.VarChar,36)
                                                };
            parameters[0].Value = name;
            parameters[1].Value = ft;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<GeoFeature>(strSql.ToString(), parameters).FirstOrDefault();
        }


        /// <summary>
        /// 判断坐标是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool GeometryeExist(Geometry geometry)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT BOID,NAME,T.GEOMETRY GEOMETRY,SOURCEDB  FROM GEOMETRY T ");
            strSql.Append(" WHERE BOID =@BOID");
            SqlParameter[] parameters = {
                            new SqlParameter("BOID",SqlDbType.VarChar,36)
                            };
            parameters[0].Value = geometry.BOID;
            List<Geometry> GeometryList = DBUtility.SqlServerDBHelper.ExecuteQueryText<Geometry>(strSql.ToString(), parameters);
            return GeometryList.Count() >= 1 ? true : false;
        }

        /// <summary>
        /// 判断参数是否存在
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool PropertyExist(Property property)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM PROPERTY T ");
            strSql.Append(" WHERE BOID=@BOID AND NS=@NS ");
            SqlParameter[] parameters = { new SqlParameter("BOID",SqlDbType.VarChar,36),
                                             new SqlParameter("NS",SqlDbType.VarChar,50)
                                           };
            parameters[0].Value = property.BOID;
            parameters[1].Value = property.NS;
            List<Property> propertyList = DBUtility.SqlServerDBHelper.ExecuteQueryText<Property>(strSql.ToString(), parameters);
            return propertyList.Count() >= 1 ? true : false;
        }

        /// <summary>
        /// 判断别名是否存在
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private bool AliasNameExist(AliasName aliasName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ALIASNAME T ");
            strSql.Append(" WHERE BOID=@BOID AND  APPDOMAIN=@APPDOMAIN ");
            SqlParameter[] parameters = { new SqlParameter("BOID",SqlDbType.VarChar,36),
                                           new SqlParameter("APPDOMAIN",SqlDbType.VarChar,50)};
            parameters[0].Value = aliasName.BOID;
            parameters[1].Value = aliasName.APPDOMAIN;
            return DBUtility.SqlServerDBHelper.ExecuteQueryText<AliasName>(strSql.ToString(), parameters).Count() >= 1 ? true : false;
        }
    }
}
