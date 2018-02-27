using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 具有名称属性的实体接口
    /// </summary>
    public interface INamedEntity : IIdEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
    }
}
