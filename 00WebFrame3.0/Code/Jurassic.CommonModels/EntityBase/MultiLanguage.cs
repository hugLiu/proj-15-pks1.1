using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 含多语言字段的表继承的基类
    /// </summary>
    public abstract class MultiLanguage : IMultiLanguage 
    {
        /// <summary>
        /// 该对象中所有多语言的属性文本集合
        /// </summary>
        [ForeignKey("BillId")]
        public ICollection<Sys_DataLanguage> LangTexts { get; set; }
    }
}
