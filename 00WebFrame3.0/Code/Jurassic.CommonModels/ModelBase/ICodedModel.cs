using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 具有编码属性的实体接口
    /// </summary>
    public interface ICodedModel : IIdModel
    {
        string Code { get; set; }
    }
}
