using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.CommonModels.Schedule
{
    /// <summary>
    /// 配合左侧菜单项显示徽章数据的类
    /// </summary>
    public class MenuExtInfo
    {
        /// <summary>
        /// 菜单项的ID(在AppFuntions.json文件中）
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 徽章中显示的文字 
        /// </summary>
        public string BadgeText { get; set; }

        ///// <summary>
        ///// 显示的样式
        ///// </summary>
        //public string Style { get; set; }
    }
}