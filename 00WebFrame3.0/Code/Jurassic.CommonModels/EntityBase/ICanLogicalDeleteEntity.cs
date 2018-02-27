using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 定义可供逻辑删除的接口
    /// </summary>
    public interface ICanLogicalDeleteEntity
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        bool IsDeleted
        {
            get;
            set;
        }
    }
}
