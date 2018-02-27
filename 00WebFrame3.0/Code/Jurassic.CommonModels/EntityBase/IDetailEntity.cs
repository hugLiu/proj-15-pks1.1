using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 主从表中子表实体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDetailEntity<T> : IDetailEntity 
    {
        /// <summary>
        /// 父级对象
        /// </summary>
        T Master { get; set; }
    }

    public interface IDetailEntity : IIdEntity
    {
        /// <summary>
        /// 父级对象的ID
        /// </summary>
        int MasterId { get; set; }
    }
}
