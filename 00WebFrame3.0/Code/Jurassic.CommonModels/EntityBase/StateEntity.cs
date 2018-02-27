using Jurassic.AppCenter;
using Jurassic.CommonModels.EntityBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Articles
{
    /// <summary>
    /// 所有具有ID和状态值的实体类的基类
    /// </summary>
    public class StateEntity : IId<int>, ICanLogicalDeleteEntity
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// 状态值枚举,可能随流程而变化
        /// </summary>
        [Column("STATE")]
        public int State { get; set; }

        /// <summary>
        /// 额外的附加选项，一般不随流程而变化
        /// </summary>
        [Column("OPTIONS")]
        public int Options { get; set; }

        [Column("ISDELETED")]
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
