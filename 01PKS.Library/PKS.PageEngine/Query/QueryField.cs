namespace PKS.PageEngine.Query
{
    public class QueryField
    {
        public string FieldName { get; set; }
        public FieldQueryType FieldQueryType { get; set; }
        public OperationType OperationType { get; set; }
        /// <summary>
        /// �����ϵ�����ֵ
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// ʵ��ֵ
        /// </summary>
        public object FieldValue { get; set; }
    }
}