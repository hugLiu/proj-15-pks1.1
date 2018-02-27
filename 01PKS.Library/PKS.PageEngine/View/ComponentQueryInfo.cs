using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.PageEngine.Param;
using PKS.PageEngine.Query;

namespace PKS.PageEngine.View
{
    /// <summary>
    /// 组件查询信息
    /// </summary>
    public class ComponentQueryInfo
    {
        public string ComponentId { get; set; }
        public QueryPlan QueryPlan { get; set; }
        public List<QueryOutputParam> OutputParams { get; set; }
    }
}
