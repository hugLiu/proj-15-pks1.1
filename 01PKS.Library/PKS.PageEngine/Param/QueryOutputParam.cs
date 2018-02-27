using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.PageEngine.Param
{
    /// <summary>
    /// 查询输出参数
    /// </summary>
    public class QueryOutputParam : VParam
    {  /// <summary>
       /// 对应Es标签【用来定义Es查询输出参数】
       /// </summary>
        public string Metadata { get; set; }
    }
}
