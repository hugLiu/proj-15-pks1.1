using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 非显式依赖关系中子表实体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAttachmentEntity : IIdEntity 
    {
        int BillId { get; set; }

        string ModuleCode { get; set; }
    }
}
