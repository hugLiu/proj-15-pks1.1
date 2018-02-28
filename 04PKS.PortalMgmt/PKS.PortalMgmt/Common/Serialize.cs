using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PKS.PortalMgmt.Common
{
    public static class Serialize
    {
        /// <summary>
        /// 序列化数据为Json数据格式. (NewTown版本)
        /// </summary>
        /// <param name="value">被序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object value)
        {
            JsonConvter jc = new JsonConvter();
            return jc.ToJson(value);
        }

        public static List<T> JSONStringToList<T>(this string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        /// <summary>
        /// 将Json数据转为对象   （NewTown版本)
        /// </summary>
        /// <typeparam name="T">目标对象</typeparam>
        /// <param name="jsonText">json数据字符串</param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonText)
        {
            if (String.IsNullOrEmpty(jsonText)) return default(T);
            JsonConvter jc = new JsonConvter();
            return jc.JsonTo<T>(jsonText);
        }

        /// <summary>
        /// 将datatable转换为json  
        /// </summary>
        /// <param name="dtb">Dt</param>
        /// <returns>JSON字符串</returns>
        public static string TableToJson(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (DataRow dr in dtb.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dtb.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);

            }
            //序列化  
            return jss.Serialize(dic);
        }

        /// <summary>
        /// 将datatable转换为json  
        /// </summary>
        /// <param name="dt">Dt</param>
        /// <param name="total">记录总数</param>
        /// <returns>JSON字符串</returns>
        public static string TableToJson(DataTable dt, int total)
        {
            System.Text.StringBuilder JsonString = new System.Text.StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("{ ");
                JsonString.Append("\"total\":" + total + ",");
                JsonString.Append("\"T_Data\":[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }


    }

    internal class JsonConvter
    {
        internal JsonConvter() { }
        internal T JsonTo<T>(string jsonText)
        {
            if (String.IsNullOrEmpty(jsonText)) return default(T);
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
            json.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            json.Converters.Add(timeConverter);

            StringReader sr = new StringReader(jsonText);
            T result = default(T);
            using (Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr))
            {
                result = (T)json.Deserialize(reader, typeof(T));
            }
            return result;
        }

        internal string ToJson(object value)
        {
            IsoDateTimeConverter dateConverter = new IsoDateTimeConverter();
            dateConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            string output = JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, dateConverter);
            return output;

        }
    }
}