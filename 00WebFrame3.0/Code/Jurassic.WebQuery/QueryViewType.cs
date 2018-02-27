using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 筛选视图的呈现方式
    /// </summary>
    public enum QueryViewType
    {
        /// <summary>
        /// 平面列表表达式
        /// </summary>
        Plain =1,

        /// <summary>
        /// 树型条件表达式
        /// </summary>
        Tree =2,

        /// <summary>
        /// 列表和树形的结合
        /// </summary>
        PlainAndTree = 3
    }
}