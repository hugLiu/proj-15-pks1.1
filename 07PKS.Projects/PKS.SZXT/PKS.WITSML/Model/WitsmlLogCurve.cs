using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PKS.WITSML.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class WitsmlLogCurve : WitsmlObject
    {
        private static String WITSML_TYPE = "logCurveInfo";
        public string UidLog { get; set; }

        public String Mnemonic { get; set; }         //别名，mnemonic或者mnemAlias
        public String Unit { get; set; }             //单位，unit
        public String NullValue { get; set; }        //空值，nullValue
        public int ColumnIndex { get; set; }         //列索引，从0开始，实际数据从1开始，columnIndex
        public Type TypeLogData { get; set; }        //类型，typeLogData，见过：double、date time
        public string CurveDescription { get; set; } //描述，curveDescription
        public DateTime MinDateTimeIndex { get; set; } //起始时间
        public DateTime MaxDateTimeIndex { get; set; } //结束时间
        public double StartIndex { get; set; }       //起始深度，startIndex
        public double EndIndex { get; set; }         //结束深度，endIndex
        //public string ClassWitsml { get; set; }      //类描述？？？

        public static WitsmlLogCurve Instance(XElement node)
        {
            WitsmlLogCurve logcurve = new WitsmlLogCurve();
            logcurve.Mnemonic = Common.GetElement<string>(node, "mnemonic", "");
            logcurve.Unit = Common.GetElement<string>(node, "unit", "");
            logcurve.NullValue = Common.GetElement<string>(node, "nullValue", "");

            double index = Common.GetElement<double>(node, "columnIndex", -1);
            logcurve.ColumnIndex = (int)index - 1;

            string witsmlType = Common.GetElement<string>(node, "typeLogData", "double");
            logcurve.TypeLogData = ConvertType(witsmlType);

            logcurve.CurveDescription = Common.GetElement<string>(node, "curveDescription", "");
            logcurve.MinDateTimeIndex = Common.GetElement<DateTime>(node, "minDateTimeIndex", DateTime.MinValue);
            logcurve.MaxDateTimeIndex = Common.GetElement<DateTime>(node, "maxDateTimeIndex", DateTime.MinValue);
            logcurve.StartIndex = Common.GetElement<double>(node, "startIndex", double.NaN);
            logcurve.EndIndex = Common.GetElement<double>(node, "endIndex", double.NaN);

            return logcurve;
        }

        /// <summary>
        /// witsml基本类型转换为.NET的类型，目前只支持时间和值
        /// </summary>
        private static Type ConvertType(string witsmlType)
        {
            if (string.Equals("date time" ,witsmlType, StringComparison.OrdinalIgnoreCase) || (witsmlType.IndexOf("time") >= 0))
            {
                return typeof(DateTime);
            }
            else if (string.Equals("double", witsmlType, StringComparison.OrdinalIgnoreCase))
            {
                return typeof(double);
            }
            else if (string.Equals("string", witsmlType, StringComparison.OrdinalIgnoreCase))
            {
                return typeof(string);
            }
            throw new NotSupportedException("type:" + witsmlType + " not supported");

        }


    }
}
