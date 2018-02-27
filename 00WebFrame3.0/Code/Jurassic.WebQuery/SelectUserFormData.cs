using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 专用于用户选择界面的选项
    /// </summary>
    public class SelectUserFormData
    {
        /// <summary>
        /// 允许多选
        /// </summary>
        public bool MultiSelect { get; set; }

        /// <summary>
        /// 标识符，用于标识该控件
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 作为表单元素时的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 起始值，如果是单选，则是单个用户ID，如果是多选，则是，号分隔的ID串
        /// </summary>
        public string StartValue { get; set; }

        /// <summary>
        /// 除了id,value以外的属性
        /// </summary>
        public string Attributes { get; set; }

        /// <summary>
        /// 扩展的分部视图，如果有多个，用逗号分隔
        /// </summary>
        public string ExtensionViews { get; set; }
    }
}