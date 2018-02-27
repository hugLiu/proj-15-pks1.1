using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 标识操作类型的枚举
    /// </summary>
    [Flags]
    public enum OperationType
    {
        /// <summary>
        /// 无（默认值）
        /// </summary>
        None = 0,

        /// <summary>
        /// 查看
        /// </summary>
        View = 1,

        /// <summary>
        /// 新增
        /// </summary>
        Insert = 2,

        /// <summary>
        /// 修改
        /// </summary>
        Update = 4,

        /// <summary>
        /// 删除
        /// </summary>
        Delete = 8
    }
}
