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
    public class ModelBase
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// 状态值枚举
        /// </summary>
        [Column("STATE")]
        public ArticleState State { get; set; }
    }
}
