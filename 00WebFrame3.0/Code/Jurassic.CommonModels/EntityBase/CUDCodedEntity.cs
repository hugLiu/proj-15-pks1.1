using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 记录了编号和增删改信息的实体信息基类
    /// </summary>
    public abstract class CUDCodedEntity : CUDEntity, ICodedEntity
    {
        /// <summary>
        /// 表示单据的编号
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
    }
}
