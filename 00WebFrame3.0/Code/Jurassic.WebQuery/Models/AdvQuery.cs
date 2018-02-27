using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic.CommonModels.Articles;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 保存高级查询条件表达式的栏目信息
    /// </summary>
    public class AdvQuery
    {
        /// <summary>
        /// 生成的高级查询栏目静态对象
        /// </summary>
        public static AdvQuery Query { get; set; }
        /// <summary>
        /// 栏目ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表达式对应的模型名称
        /// </summary>
        public string ModelName { get; set; }
    }

    /// <summary>
    /// 用户建立高级查询时，表达式树的单个结点信息
    /// </summary>
    public class AdvQueryNode
    {
        /// <summary>
        /// 唯一标识，兼排序位
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级标识
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 字段名或AND /OR
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Expression的前台展示字符串
        /// </summary>
        public string ExpressionText { get; set; }

        /// <summary>
        /// 结点类型， AND/OR （Operator） 或 表达式(Expr)两种
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 运算符
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 比较值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 前台显示用，如果是枚举值，则和Value配对
        /// </summary>
        public string ValueText { get; set; }
    }

    /// <summary>
    /// 在高级查询页，返回用户保存过的查询列表中的每一列表项的实体
    /// </summary>
    public class AdvQueryItem
    {
        /// <summary>
        /// 表达式ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 表达式名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 查询表达式对应的ODT模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 组成表达式树的结点列表
        /// </summary>
        public List<AdvQueryNode> Nodes { get; set; }

    }

}