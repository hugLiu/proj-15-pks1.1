using Jurassic.AppCenter;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 数据库层面的多语言表
    /// </summary>
    [Table("Sys_DataLanguage")]
    public class Sys_DataLanguage : IIdNameBase<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 实体类的唯一标识ID
        /// </summary>
        public int BillId { get; set; }

        /// <summary>
        /// 实体类中的属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对应Language的文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 实体类的类型名称
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 语言简称的小写，如zh-cn, en-us等等
        /// </summary>
        public string Language { get; set; }
    }
}
