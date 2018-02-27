using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Jurassic.CommonModels.EntityBase
{
    /// <summary>
    /// 实现多语言属性的接口
    /// </summary>
    public interface IMultiLanguage
    {
        /// <summary>
        /// 该对象中所有多语言的属性文本集合
        /// </summary>
        [ForeignKey("BillId")] 
        //以上的ForeignKey标记在接口中无效，必须在实现它的类中重复定义
        ICollection<Sys_DataLanguage> LangTexts { get; set; }
    }
}
