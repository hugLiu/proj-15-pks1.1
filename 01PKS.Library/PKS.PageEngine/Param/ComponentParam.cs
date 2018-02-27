namespace PKS.PageEngine.Param
{
    /// <summary>
    /// 组件系统参数
    /// </summary>
    public class ComponentParam: VParam
    {
        /// <summary>
        /// 对应FragmentTypeId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 对应Es标签【用来定义Es查询输出参数】
        /// </summary>
        public string Metadata { get; set; }
    }
}
