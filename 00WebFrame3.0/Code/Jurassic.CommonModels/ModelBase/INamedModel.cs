using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 具有名称属性的实体接口
    /// </summary>
    public interface INamedModel : IIdModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
    }
}
