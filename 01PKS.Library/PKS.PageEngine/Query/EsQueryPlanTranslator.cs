using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.PageEngine.Param;

namespace PKS.PageEngine.Query
{
    public class EsQueryPlanTranslator:IQueryPlanTranslator
    {
        /// <summary>
        /// 翻译查询参数
        /// </summary>
        /// <param name="queryPlan">输入参数</param>
        /// <param name="outputParams">输出参数</param>
        /// <returns></returns>
        public string Translate(QueryPlan queryPlan,List<QueryOutputParam> outputParams)
        {
            if (queryPlan == null || queryPlan.Fields == null|| queryPlan.Fields.Count==0)
                return null;
            var fields = queryPlan.Fields;
            var queryOrders = queryPlan.QueryOrders;
            string cFormat = "{{{0}\"query\":{{\"bool\":{{{1}}}}}{2}{3}}}";


            //查询输出参数
            StringBuilder outputParamBuilder = new StringBuilder();
            if (outputParams != null && outputParams.Count > 0)
            {
                var includeFields = string.Join(",", outputParams.Select(item => "\"" + item.Metadata + "\""));
                outputParamBuilder.Append("\"_source\":[" + includeFields + "]");
                outputParamBuilder.Append(",");
            }

            StringBuilder mustBuilder = new StringBuilder();
            if (fields != null && fields.Count > 0)
            {
                //todo 需要获取ES标签的数据类型
                mustBuilder.Append("\"must\":[");
                for (var i = 0; i < fields.Count; i++)
                {
                    if (fields[i].FieldValue == null)
                        continue;
                    var queryValue = Convert.ToString(fields[i].FieldValue);
                    if (string.IsNullOrWhiteSpace(queryValue))
                        continue;
                    var dataType = fields[i].DataType;
                    var keyword = fields[i].FieldName+".keyword";
                    var keyOperator = string.Empty;
                    var keyData = string.Empty;
                    var operatorType = fields[i].OperationType;
                
                    if (operatorType == OperationType.Equal)
                    {
                        keyOperator = "term";
                        keyData = GetParamValue(dataType, fields[i].FieldValue);
                    }
                    if (operatorType == OperationType.Between)
                    {
                        //todo 可能为日期类型，若为日期类型，值要进行相应转换
                        keyOperator = "range";
                        var queryParamArray= queryValue.Split(new char[] {';'});
                        var minValue = GetParamValue(dataType,queryParamArray[0]);
                        var maxValue = queryParamArray.Length>1? GetParamValue(dataType, queryParamArray[1]):null;
                        keyData = "{\"gte\":" + minValue +
                                      (string.IsNullOrWhiteSpace(maxValue) ? "" : (",\"lte\":" + maxValue)) + "}";
                    }
                    if (operatorType == OperationType.In)
                    {
                        keyOperator = "terms";
                     
                        var queryParamArray = queryValue.Split(new char[] { ';','；' },StringSplitOptions.RemoveEmptyEntries);
                        var quotParams = queryParamArray.Select(item => GetParamValue(dataType, item));
                        var arrayValues=string.Join(",", quotParams);
                        keyData = "[" + arrayValues + "]";
                    }
                    if (operatorType == OperationType.Contains)
                    {
                        keyOperator = "match_phrase";
                        keyword = fields[i].FieldName;//不需要加keyword
                        keyData = GetParamValue(dataType, fields[i].FieldValue);
                    }
                    if (operatorType == OperationType.GreaterThan)
                    {
                        keyOperator = "range";
                        var value = GetParamValue(dataType, fields[i].FieldValue);
                        keyData = "{\"gt\":" + value + "}";
                    }
                    if (operatorType == OperationType.GreaterThanEqual)
                    {
                        keyOperator = "range";
                        var value = GetParamValue(dataType, fields[i].FieldValue);
                        keyData = "{\"gte\":" + value + "}";
                    }
                    if (operatorType == OperationType.LessThan)
                    {
                        keyOperator = "range";
                        var value = GetParamValue(dataType, fields[i].FieldValue);
                        keyData = "{\"lt\":" + value + "}";
                    }
                    if (operatorType == OperationType.LessThanEqual)
                    {
                        keyOperator = "range";
                        var value = GetParamValue(dataType, fields[i].FieldValue);
                        keyData = "{\"lte\":" + value + "}";
                    }
                    mustBuilder.AppendFormat("{{\"{0}\":{{\"{1}\":{2}}}}}", keyOperator,keyword,keyData);
                    if (i != fields.Count - 1)
                    {
                        mustBuilder.Append(",");
                    }
                }
                mustBuilder.Append("]");
            }

            StringBuilder orderBuilder = new StringBuilder();
            if (queryOrders != null && queryOrders.Count > 0)
            {
                if (fields != null && fields.Count > 0)
                    orderBuilder.Append(",");
                orderBuilder.Append("\"sort\":[");
                for (var i = 0; i < queryOrders.Count; i++)
                {
                    orderBuilder.AppendFormat("{{\"{0}\":{{\"order\":{1}}}", queryOrders[i].FieldName+".keyword", "\"" + queryOrders[i].OrderDirection + "\"}");
                    if (i != queryOrders.Count - 1)
                    {
                        orderBuilder.Append(",");
                    }
                }
                orderBuilder.Append("]");
            }
            StringBuilder topCountBuilder = new StringBuilder();
            topCountBuilder.Append(",");
            topCountBuilder.Append("\"size\":" + queryPlan.TopCount);

            return string.Format(cFormat, outputParamBuilder,mustBuilder, orderBuilder, topCountBuilder);
        }

        private string GetParamValue(string dataType,object value)
        {
            return (dataType == null || dataType.ToLower() == "string")
                              ? ("\"" + Convert.ToString(value) + "\"")
                              : Convert.ToString(value);
        }
    }
}
