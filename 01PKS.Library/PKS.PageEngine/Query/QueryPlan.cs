using System.Collections.Generic;
using System.Text;

namespace PKS.PageEngine.Query
{
    /// <summary>
    /// 查询计划[即组件查询输入参数]
    /// </summary>
    public class QueryPlan
    {
        public QueryPlan()
        {
            Fields=new List<QueryField>();
        }
        /// <summary>
        /// 查询标签
        /// </summary>
        public List<QueryField> Fields { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public List<QueryOrder> QueryOrders { get; set; }
        /// <summary>
        /// 取的条数
        /// </summary>
        public int TopCount { get; set; }
    }
}
