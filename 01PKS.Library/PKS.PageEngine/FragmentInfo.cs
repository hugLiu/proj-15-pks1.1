using System.Collections.Generic;
using PKS.PageEngine.Param;
using PKS.PageEngine.Query;

namespace PKS.PageEngine
{
    /// <summary>
    /// 知识片段组件信息
    /// </summary>
    public class FragmentInfo
    {
        public string Id { get; set; }

        //public string KTemplateCatalogueId { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// 查询输入参数
        /// </summary>
        public QueryPlan QueryPlan { get; set; }
        /// <summary>
        /// 查询输出参数
        /// </summary>
        public List<QueryOutputParam> QueryOutputParams { get; set; }
        /// <summary>
        /// 组件参数[Json格式]
        /// </summary>
        public string StrComponentParameters { get; set; }
        /// <summary>
        /// 组件参数[StrComponentParameters和数据库组件参数的合并]
        /// </summary>
        public List<ComponentParam> ComponentParameters { get; set; }
        /// <summary>
        /// Html文本
        /// </summary>
        public string Htmltext { get; set; }

        public int FragmentTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlaceHolderId { get; set; }

        /// <summary>
        /// 知识片段类型
        /// </summary>
        public string FragmentTypeCode { get; set; }

        /// <summary>
        /// 知识片段名称
        /// </summary>
        public string FragmentTypeName { get; set; }

        /// <summary>
        /// 知识片段对应Vue标签
        /// </summary>
        public string FragmentVueTag { get; set; }

        /// <summary>
        /// 知识片段是否为文本模板
        /// </summary>
        public bool FragmentHasTextTemplate { get; set; }
    }
}
