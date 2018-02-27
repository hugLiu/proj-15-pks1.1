using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 具有编码属性的实体接口
    /// </summary>
    public interface ICodedEntity : IIdEntity
    {
        /// <summary>
        /// 表示单据的编码
        /// </summary>
        string Code { get; set; }
    }
}
