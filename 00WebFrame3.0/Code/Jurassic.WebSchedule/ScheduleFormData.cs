using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebSchedule
{
    /// <summary>
    /// 用于初始化日程表控件的表单数据对象
    /// </summary>
    public class ScheduleFormData
    {
        /// <summary>
        /// 承载日程表的HTML对象的ID，默认为"schedule"
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// 基准日期，默认为当前日期
        /// </summary>
        public DateTime DefaultDate { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// 使用默认值创建一个日程表初始化数据对象
        /// </summary>
        public ScheduleFormData()
        {
            DefaultDate = DateTime.Now;
            ElementId = "schedule";
        }
    }
}