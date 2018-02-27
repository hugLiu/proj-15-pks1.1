using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 记录了增改信息的实体接口
    /// </summary>
    public interface ICUEntity : IIdEntity
    {
        int CreaterId { get; set; }

        DateTime CreateTime { get; set; }

        int UpdaterId { get; set; }

        DateTime UpdateTime { get; set; }

    }
}
