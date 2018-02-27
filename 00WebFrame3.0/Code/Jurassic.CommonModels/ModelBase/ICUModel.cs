using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 记录了增删改信息的实体接口
    /// </summary>
    public interface ICUModel
    {
        string CreaterName { get; set; }

        DateTime CreateTime { get; set; }

        string UpdaterName { get; set; }

        DateTime UpdateTime { get; set; }
    }
}
