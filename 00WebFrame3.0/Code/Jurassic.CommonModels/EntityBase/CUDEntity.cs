using Jurassic.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 记录了增删改信息的实体基类
    /// </summary>
    public class CUDEntity : CUEntity, ICanLogicalDeleteEntity
    {
        public bool IsDeleted
        {
            get;
            set;
        }
    }
}
