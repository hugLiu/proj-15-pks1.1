using Jurassic.CommonModels.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery
{
    public class LangTextFormData
    {
        /// <summary>
        /// 多语言控件的ID，默认为master_+ Name的值
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 多语言表单控件的Name属性，当前主语言不带后缀，其他语言的输入框带语言简称后缀，如 Name_en-us
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 本控件绑定的Model对象，如果为空，则取页面本身的Model对象
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// 当前语言下的值
        /// </summary>
        public object StartValue { get; set; }

        /// <summary>
        /// 除了id,value以外的属性
        /// </summary>
        public string Attributes { get; set; }
        
        /// <summary>
        /// 图标类型，国旗或文本， 默认文本
        /// </summary>
        public CultureIconType IconType {get;set;}
    }
}