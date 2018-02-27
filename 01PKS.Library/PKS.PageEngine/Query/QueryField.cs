namespace PKS.PageEngine.Query
{
    public class QueryField
    {
        public string FieldName { get; set; }
        public FieldQueryType FieldQueryType { get; set; }
        public OperationType OperationType { get; set; }
        /// <summary>
        /// 界面上的配置值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public object FieldValue { get; set; }
    }
}