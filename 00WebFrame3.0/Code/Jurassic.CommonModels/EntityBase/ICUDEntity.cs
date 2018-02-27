using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 记录了增删改信息的实体接口
    /// </summary>
    public interface ICUDEntity : ICUEntity, ICanLogicalDeleteEntity
    {
    }
}
