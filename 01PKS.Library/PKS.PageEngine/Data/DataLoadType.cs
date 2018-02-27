using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.PageEngine.Data
{
    /// <summary>
    /// 数据加载模式
    /// </summary>
    public enum DataLoadType
    {
        /// <summary>
        /// 一个接着一个
        /// </summary>
        OneByOne,
        /// <summary>
        /// 一次请求所有数据
        /// </summary>
        AllAtOnce
    }
}
