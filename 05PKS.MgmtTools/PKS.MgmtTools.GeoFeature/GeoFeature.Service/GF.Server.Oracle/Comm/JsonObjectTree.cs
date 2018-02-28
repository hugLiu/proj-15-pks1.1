using GF.Server.DBUtility;
using Json_Sql;
using Juarssic.Server.Comm;
using Jurassic.PKS.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Juarssic.Server.Comm
{
    public enum JsonType { Array, Object, Value };

    class JsonParseError : ApplicationException
    {
        public JsonParseError() : base() { }
        public JsonParseError(string message) : base(message) { }
        protected JsonParseError(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public JsonParseError(string message, Exception innerException) : base(message, innerException) { }

    }
    /// <summary>
    /// Json树
    /// </summary>
    public class JsonObjectTree
    {
        public List<ObjModel> ListMode = new List<ObjModel>();
        private ObjModel ParentModel;
        static string FT = string.Empty;

        /// <summary>
        /// json转换成树
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JsonObjectTree Parse(string json)
        {
            //FT = ft;
            JObject jsonObject;
            try
            {
                jsonObject = (JObject)JsonConvert.DeserializeObject(json.ToString());
            }
            catch (Exception e)
            {
                throw new JsonParseError(e.Message, e);
            }
            return new JsonObjectTree(jsonObject);
        }

        /// <summary>
        /// json组装成ListMode
        /// </summary>
        /// <param name="rootObject"></param>
        public JsonObjectTree(object rootObject)
        {
            JObject javaScriptObject = rootObject as JObject;

            if (javaScriptObject != null)
            {
                foreach (KeyValuePair<string, JToken> pair in javaScriptObject)
                {
                    ObjModel model = ConvertToObject(pair.Key, pair.Value);
                    ListMode.Add(model);
                }
            }
        }

        /// <summary>
        /// 转换jsonObject
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        private ObjModel ConvertToObject(string name, object jsonObject)
        {
            ObjModel obj = CreateJsonObject(jsonObject);
            if (obj.Leaives)
            {
                obj.FieldN = name;
                obj.Relations = "";
                obj.Parent = ParentModel;

                JObject javaScriptObjectChild = obj.Value as JObject;
                if (javaScriptObjectChild == null)
                {
                    obj.Opt = "=";
                    obj.Value = obj.Value;
                }
                else
                {
                    foreach (KeyValuePair<string, JToken> pair in javaScriptObjectChild)
                    {
                        foreach (KeyValuePair<string, string> kvp in MongoCode.m_MongoCodeDic)
                        {
                            if (pair.Key == kvp.Key)
                            {
                                obj.Opt = kvp.Value;
                                break;
                            }
                            if (obj.Opt == null)
                            {
                                obj.Opt = "=";
                            }
                        }
                        obj.Value = pair.Value;
                    }

                }
            }
            else
            {
                obj.Parent = ParentModel;
                obj.Relations = name.Replace('$', ' ');
            }
            AddChildren(obj, jsonObject);
            return obj;
        }

        /// <summary>
        /// 创建jsonObject
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        private ObjModel CreateJsonObject(object jsonObject)
        {
            ObjModel obj = new ObjModel();
            if (jsonObject is JArray)
            {
                obj.FieldN = "";
                obj.Opt = "";
                obj.Value = "";
                obj.Leaives = false;
                obj.JsonType = JsonType.Array;
            }
            else if (jsonObject is JObject)
            {
                obj.FieldN = "";
                obj.Opt = "";
                obj.Leaives = true;
                obj.JsonType = JsonType.Value;
                obj.Value = jsonObject;
            }
            else
            {
                obj.Leaives = true;
                obj.JsonType = JsonType.Value;
                obj.Value = jsonObject;
            }
            return obj;
        }

        /// <summary>
        /// 添加树节点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonObject"></param>
        private void AddChildren(ObjModel obj, object jsonObject)
        {
            JObject javaScriptObject = jsonObject as JObject;

            if (javaScriptObject != null)
            {
                if (javaScriptObject.Count > 0 || obj.JsonType == JsonType.Value)
                {
                    foreach (KeyValuePair<string, JToken> pair in javaScriptObject)
                    {
                        ParentModel = obj;
                        obj.Nodes.Add(ConvertToObject(pair.Key, pair.Value));

                    }
                }
            }
            else
            {
                JArray javaScriptArray = jsonObject as JArray;
                if (javaScriptArray != null)
                {
                    for (int i = 0; i < javaScriptArray.Count; i++)
                    {
                        JObject javaScriptObjectChild = javaScriptArray[i] as JObject;
                        foreach (KeyValuePair<string, JToken> pair in javaScriptObjectChild)
                        {
                            ParentModel = obj;
                            obj.Nodes.Add(ConvertToObject(pair.Key, pair.Value));

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 组装Sql
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string GetSQL(ObjModel node, string bot)
        {
            string strSQL = " ";

            if (!node.Leaives)
            {
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    strSQL += " (" + GetSQL(node.Nodes[i], bot) + ")";
                    if (i < node.Nodes.Count - 1)
                    {
                        strSQL += node.Relations;
                    }
                }
            }
            else
            {
                string[] strArr = node.FieldN.Split('.').ToArray();
                //根据对象类型主题获取对象参数的数据类型
                string value = node.Value.ToString();
                string datatype = GetPropertyDataType(strArr[0], strArr[1], bot);
                try
                {
                    if (datatype.ToUpper() == PropertyDataType.Date.ToString().ToUpper())
                    {
                        strSQL = "existsNode(PROPERTY.md,'PropertySet[@name=" + strArr[0] + "]/P[@n=" + strArr[1] + "][text()" + node.Opt + "xs:date(" + node.Value + ")]' passing t2.md )=1 ";
                    }
                    if (datatype.ToUpper() == PropertyDataType.Decimal.ToString().ToUpper())
                    {
                        strSQL = "existsNode(PROPERTY.md,'PropertySet[@name=" + '"' + strArr[0] + '"' + "]/P[@n=" + '"' + strArr[1] + '"' + "][text()" + node.Opt + node.Value + "]')=1 ";
                    }
                    if (datatype.ToUpper() == PropertyDataType.String.ToString().ToUpper())
                    {
                        strSQL = "existsNode(PROPERTY.md,'PropertySet[@name=" + '"' + strArr[0] + '"' + "]/P[@n=" + '"' + strArr[1] + '"' + "][text()" + node.Opt + '"' + node.Value + '"' + "]')=1 ";
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("获取参数数据类型失败", "参数不存在");
                    throw ex;
                }

            }
            return strSQL;
        }
        /// <summary>
        /// 获取参数的数据类型
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="ns"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public string GetPropertyDataType(string ns, string paraName, string bot)
        {
            string dataType = string.Empty;
            StringBuilder strSql = new StringBuilder();
            OracleParameter[] parameters;
            strSql.Append(" SELECT EXTRACTVALUE(VALUE(I), '/P/@t') as datatype");
            strSql.Append(" FROM objtypeproperty X, ");
            strSql.Append(" objecttype t2,");
            strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
            strSql.Append(" WHERE t2.BOT= :BOT  ");
            strSql.Append(" AND X.NS = :NS AND t2.BOTID=X.BOTID ");
            strSql.Append(" AND EXTRACTVALUE(VALUE(I), '/P/@n') = :PARANAME and rownum=1 ");
            parameters = new OracleParameter[]{
                                  new OracleParameter("BOT", OracleDbType.Varchar2,50),
                                  new OracleParameter("NS", OracleDbType.Varchar2,50),
                                  new OracleParameter("PARANAME", OracleDbType.Varchar2,50)
                                                };
            parameters[0].Value = bot.Trim();
            parameters[1].Value = ns.Trim();
            parameters[2].Value = paraName.Trim();
            dataType = OracleDBHelper.OracleHelper.ExecuteQueryText<string>(strSql.ToString(), parameters).FirstOrDefault();
            if (string.IsNullOrEmpty(dataType))
            {
                strSql = new StringBuilder();
                strSql.Append(" SELECT EXTRACTVALUE(VALUE(I), '/P/@t') DataType ");
                strSql.Append(" FROM PROPERTY X,objecttype t2,bo, ");
                strSql.Append(" TABLE(XMLSEQUENCE(EXTRACT(X.MD, '/PropertySet/P'))) I ");
                strSql.Append(" WHERE x.boid=bo.boid and bo.botid=t2.botid ");
                strSql.Append(" AND t2.BOT= :BOT  ");
                strSql.Append(" AND X.NS = :NS ");
                strSql.Append(" AND EXTRACTVALUE(VALUE(I), '/P/@n') = :PARANAME and rownum=1");
                parameters = new OracleParameter[]{
                                  new OracleParameter("BOT", OracleDbType.Varchar2,50),
                                  new OracleParameter("NS", OracleDbType.Varchar2,50),
                                  new OracleParameter("PARANAME", OracleDbType.Varchar2,50)
                                                };
                parameters[0].Value = bot.Trim();
                parameters[1].Value = ns.Trim();
                parameters[2].Value = paraName.Trim();
                dataType = OracleDBHelper.OracleHelper.ExecuteQueryText<string>(strSql.ToString(), parameters).FirstOrDefault();
            }
            return dataType;
        }
    }
}


