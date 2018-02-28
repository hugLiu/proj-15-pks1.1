using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PKS.WITSML.Model;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace PKS.WITSML
{
    public class DataAccess
    {
        //录井气测参数列映射关系
        static Dictionary<string, string> COLUMNS_MUDLOG = new Dictionary<string, string>{
            {"dTim","DateTime"},
            {"DRTM","LAGDepth"},
            {"MTHA","C1"}, 
            {"ETHA","C2"}, 
            {"PRPA","C3"}, 
            {"IBTA","IC4"}, 
            {"NBTA","NC4"}, 
            {"IPNA","IC5"}, 
            {"NPNA","NC5"}, 
            {"TOTG" ,"TotalGas"}                  
        };

        //随钻测井情况参数列映射关系
        static Dictionary<string, string> COLUMNS_LWD = new Dictionary<string, string>{
            {"dTim","DateTime"}, 
            {"DMEA","DMEA"}, 
            {"MG2C","MG1C"},     //未找到MG1C，用MG2C替代，描述是Gamma Ray 2(borehole corr)
            {"RT","MR1C"},       //RT的描述是：Resis 1(borehole corr)
            {"ILD","MR2C"},      //ILD的描述是：Resis 2(borehole corr)，有MR2C列，描述却不对 
            {"MR3C","MR3C"}, 
            {"MR4C","MR4C"} 
        };

        /// <summary>
        /// 获取witsml数据
        /// </summary>
        /// <typeparam name="T">WitsmlObject</typeparam>
        /// <param name="idTree">从外层到内层的Id列表,Id不确定时用“”代替</param>
        /// <returns></returns>
        public static List<T> Get<T>(String[] idTree) where T : WitsmlObject
        {
            var witsmlType = typeof(T);
            var queryStringMethod = witsmlType.GetMethod("QueryString", BindingFlags.Public | BindingFlags.Static);
            var queryString = (string)queryStringMethod.Invoke(null, idTree);
            var typeIn = GetWitsmlType(witsmlType);
            ServiceAccessor svr = new ServiceAccessor();
            string xmlOut, msg;
            var statusCode = svr.WMLS_GetFromStore(typeIn, queryString, out xmlOut, out msg);
            if (statusCode != 1 || string.IsNullOrEmpty(xmlOut))
            {
                return new List<T>();
            }
            MethodInfo instanceMethod = witsmlType.GetMethod("Instance", BindingFlags.Public | BindingFlags.Static);
            XDocument document = XDocument.Load(new StringReader(xmlOut));
            var root = document.Root;
            var nameSpace = root.Name.Namespace;
            var rt = root.Elements(nameSpace + typeIn)
                .Select(node => (WitsmlObject)instanceMethod.Invoke(null, new object[] { node }))
                .Where(i => i.Uid != "").Select(o => o as T).ToList();
            return rt;
        }

        /// <summary>
        /// 获得所有井的uid列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllWells()
        {
            var wellBores = DataAccess.Get<WitsmlWellbore>(new string[] { "", "" });//LH28-4-1
            return wellBores.Select(well => well.UidWell).ToList();
        }

        /// <summary>
        /// 获取所有随钻测井情况数据
        /// </summary>
        /// <param name="uidWell"></param>
        /// <param name="lastDateTime">该井上次最后数据时间</param>
        /// <returns></returns>
        public static List<LWD> GetLWDDatas(string uidWell, DateTime lastDateTime)
        {
            var logs = DataAccess.Get<WitsmlLog>(new string[] { uidWell, uidWell, "" });
            List<LWD> lwdLogs = new List<LWD>();
            foreach (var log in logs)
            {
                //时间属性有效并且不符合请求时间范围,立即退出
                if (log.endTime > DateTime.MinValue && log.endTime <= lastDateTime) { continue; }
                var curves = log.logCurves;
                var indexDic = COLUMNS_LWD.Keys
                    .Select(col => new { colName = col, index = FindColumnIndex(curves, col) })
                    .ToDictionary(obj => obj.colName, obj => obj.index);

                //有一列找不到就退出，这里可以适当修改使得可以找到部分数据
                //if (indexDic.Any(index => index.Value.ColumnIndex < 0)) { continue; }

                var count = indexDic.Count(index => index.Value.ColumnIndex > 0);
                if (count < 3) { continue; }

                var type = typeof(LWD);
                for (var i = log.datas.Count - 1; i > 0; i--)
                {
                    var instance = new LWD { UidWell = log.UidWell, UidLog = log.Uid };
                    var dataArr = log.datas[i].Split(new string[] { ",", "，" }, StringSplitOptions.None);
                    foreach (var dic in COLUMNS_LWD)
                    {
                        var curveInfo = indexDic[dic.Key];      //元信息
                        var nullValue = curveInfo.NullValue;    //数据空值字符
                        var dataType = curveInfo.TypeLogData;   //数据类型
                        var index = curveInfo.ColumnIndex;      //数据在行中的索引
                        if (index < 0) { continue; }            //为测试试允许个别列为空
                        var valueStr = dataArr[index];
                        var field = type.GetProperty(dic.Value);
                        var value = Common.ConvertToType(dataType, valueStr, nullValue);
                        field.SetValue(instance, value, null);
                    }

                    if (instance.DateTime > lastDateTime)
                    {
                        lwdLogs.Add(instance);
                    }
                    else { break; }    //数据是按照时间逆序的，找到不符合条件的数据就停止解析
                }
            }
            return lwdLogs;
        }

        /// <summary>
        /// 获取录井气测参数数据
        /// </summary>
        /// <param name="uidWell"></param>
        /// <param name="lastDateTime">该井上次最后数据时间</param>
        /// <returns></returns>
        public static List<MudLog> GetMudLogDatas(string uidWell, DateTime lastDateTime)
        {
            var logs = DataAccess.Get<WitsmlLog>(new string[] { uidWell, uidWell, "" });
            List<MudLog> mudLogs = new List<MudLog>();
            foreach (var log in logs)
            {
                //时间属性有效并且不符合请求时间范围,立即退出
                if (log.endTime > DateTime.MinValue && log.endTime <= lastDateTime) { continue; }
                var curves = log.logCurves;
                var indexDic = COLUMNS_MUDLOG.Keys
                    .Select(col => new { colName = col, index = FindColumnIndex(curves, col) })
                    .ToDictionary(obj => obj.colName, obj => obj.index);

                //有一列找不到就退出，这里可以适当修改使得可以找到部分数据
                if (indexDic.Any(index => index.Value.ColumnIndex < 0)) { continue; }

                var type = typeof(MudLog);
                for (var i = log.datas.Count - 1; i > 0; i--)
                {
                    var instance = new MudLog { UidWell = log.UidWell, UidLog = log.Uid };
                    var dataArr = log.datas[i].Split(new string[] { ",", "，" }, StringSplitOptions.None);
                    foreach (var dic in COLUMNS_MUDLOG)
                    {
                        var curveInfo = indexDic[dic.Key];      //元信息
                        var nullValue = curveInfo.NullValue;    //数据空值字符
                        var dataType = curveInfo.TypeLogData;   //数据类型
                        var index = curveInfo.ColumnIndex;      //数据在行中的索引
                        var valueStr = dataArr[index];
                        var field = type.GetProperty(dic.Value);
                        var value = Common.ConvertToType(dataType, valueStr, nullValue);
                        field.SetValue(instance, value, null);
                    }

                    if (instance.DateTime > lastDateTime)
                    {
                        if (instance.Check())
                            mudLogs.Add(instance);
                    }
                    else { break; }    //数据是按照时间逆序的，找到不符合条件的数据就停止解析
                }
            }
            return mudLogs;
        }

        private static WitsmlLogCurve FindColumnIndex(List<WitsmlLogCurve> curves, string columnName)
        {
            if (curves.Any(c => c.Mnemonic == columnName))
            {
                return curves.Find(c => c.Mnemonic == columnName);
            }
            return new WitsmlLogCurve { ColumnIndex = -1 };
        }

        private static String GetWitsmlType(Type type)
        {
            try
            {
                var field = type.GetField("WITSML_TYPE", BindingFlags.Static | BindingFlags.NonPublic);
                return (String)field.GetValue(null);
            }
            catch
            {
                throw new NotSupportedException(type.ToString() + "类型中无法访问WITSML_TYPE字段");
            }
        }

        private static string Formater(string xml)
        {
            try
            {
                XDocument xd = XDocument.Load(new StringReader(xml));
                return xd.Document.ToString();
            }
            catch
            {
                throw new System.Xml.XmlException("xml格式不对");
            }
        }



    }
}
