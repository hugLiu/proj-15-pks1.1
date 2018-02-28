using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PKS.WITSML.Model
{
    public class WitsmlLog : WitsmlObject
    {
        private static String WITSML_TYPE = "log";
        public string UidWellbore { get; set; }
        public string UidWell { get; set; }
        public string indexCurve;
        public string direction;
        public DateTime startTime;
        public DateTime endTime;
        public List<WitsmlLogCurve> logCurves;
        public List<string> datas;
        public static WitsmlLog Instance(XElement xElementLog)
        {
            var uidWell = Common.GetAttribute<string>(xElementLog, "uidWell", "");
            var uidWellbore = Common.GetAttribute<string>(xElementLog, "uidWellbore", "");
            var uid = Common.GetAttribute<string>(xElementLog, "uid", "");
            var name = Common.GetElement<string>(xElementLog, "name", "");

            var indexCurve = Common.GetElement<string>(xElementLog, "indexCurve", "");                   //返回：dTime或者md
            var direction = Common.GetElement<string>(xElementLog, "direction", "increasing");     //排序，只见过增顺
            var startTime = Common.GetElement<DateTime>(xElementLog, "startTime", DateTime.MinValue);     //数据开始时间
            var endTime = Common.GetElement<DateTime>(xElementLog, "endTime", DateTime.MinValue);     //数据结束时间

            List<WitsmlLogCurve> logcurves = GetCurves(xElementLog);

            List<string> datas = GetDatas(xElementLog);

            var obj = new WitsmlLog
            {
                Uid = uid,
                UidWellbore = uidWellbore,
                UidWell = uidWell,
                Name = name,
                indexCurve = indexCurve,
                direction = direction,
                startTime = startTime,
                endTime = endTime,
                logCurves = logcurves,
                datas = datas
            };
            return obj;
        }
        private static List<WitsmlLogCurve> GetCurves(XElement xElementLog)
        {
            List<WitsmlLogCurve> logCurves = new List<WitsmlLogCurve>();
            var nameSpace = xElementLog.Name.Namespace;
            var curveElements = xElementLog.Elements(nameSpace + "logCurveInfo");
            foreach (var ele in curveElements)
            {
                var logcurve = WitsmlLogCurve.Instance(ele);
                logCurves.Add(logcurve);
            }
            return logCurves;
        }

        private static List<string> GetDatas(XElement xElementLog)
        {
            List<string> logDatas = new List<string>();
            var nameSpace = xElementLog.Name.Namespace;
            var ldElements = xElementLog.Element(nameSpace + "logData");
            var dataElements = ldElements.Elements(nameSpace + "data");

            foreach (var ele in dataElements)
            {
                var data = ele.Value;
                logDatas.Add(data);
            }
            return logDatas;
        }
        public static string QueryString(string uidWell = "", string uidWellbore = "", string uid = "")
        {
            System.Text.StringBuilder strB = new System.Text.StringBuilder();
            strB.AppendLine("<logs xmlns=\"http://www.witsml.org/schemas/131\">");
            strB.AppendLine("<log uidWell=\"{0}\" uidWellbore=\"{1}\" uid=\"{2}\">");
            strB.AppendLine("<name />");
            strB.AppendLine("<objectGrowing />");

            strB.AppendLine("<runNumber />");

            strB.AppendLine("<indexType />");

            strB.AppendLine("<indexCurve>{3}</indexCurve>");
            strB.AppendLine("<startDateTimeIndex />");
            strB.AppendLine("<endDateTimeIndex />");
            strB.AppendLine("<stepIncrement />");
            strB.AppendLine("<direction />");
            strB.AppendLine("<startIndex uom=\"m\" />");
            strB.AppendLine("<endIndex uom=\"m\" />");
            //strB.AppendLine("<dataRowCount />");
            //strB.AppendLine("<serviceCompany />");
            //strB.AppendLine("<nullValue />");
            //strB.AppendLine("<logParam />");
            //strB.AppendLine("<bhaRunNumber />");
            //strB.AppendLine("<pass />");
            //strB.AppendLine("<creationDate />");
            //strB.AppendLine("<description />");
            //strB.AppendLine("<commonData>");
            //strB.AppendLine(" <sourceName />");
            //strB.AppendLine(" <dTimCreation />");
            //strB.AppendLine(" <dTimLastChange />");
            //strB.AppendLine(" <itemState />");
            //strB.AppendLine(" <comments />");
            //strB.AppendLine("</commonData>");
            strB.AppendLine("<logCurveInfo uid=\"\">");
            strB.AppendLine("   <mnemonic />");
            strB.AppendLine("   <classWitsml />");
            strB.AppendLine("   <unit />");
            strB.AppendLine("   <mnemAlias />");
            strB.AppendLine("   <nullValue />");
            strB.AppendLine("   <minDateTimeIndex />");
            strB.AppendLine("   <maxDateTimeIndex />");
            strB.AppendLine("   <columnIndex />");
            strB.AppendLine("   <curveDescription />");
            strB.AppendLine("   <sensorOffset />");
            strB.AppendLine("   <dataSource />");
            strB.AppendLine("   <traceState />");
            strB.AppendLine("   <typeLogData />");
            //strB.AppendLine("<alternateIndex />");
            //strB.AppendLine("<wellDatum />");
            //strB.AppendLine("<minIndex />");
            //strB.AppendLine("<maxIndex />");
            //strB.AppendLine(" <densData />");
            //strB.AppendLine(" <traceOrigin />");
            //strB.AppendLine(" <axisDefinition />");
            strB.AppendLine("</logCurveInfo>");
            strB.AppendLine("<logData>");
            //strB.AppendLine("<mnemonicList />");
            //strB.AppendLine("<unitList />");
            strB.AppendLine("<data />");
            strB.AppendLine("</logData>");
            strB.AppendLine(" </log>");
            strB.AppendLine("</logs>");

            var indexCurve = "md";    //实际用md作过滤，因为需要的都是深度索引的数据
            var str = string.Format(strB.ToString(), uidWell, uidWellbore, uid, indexCurve);
            return str;

        }



    }
}
