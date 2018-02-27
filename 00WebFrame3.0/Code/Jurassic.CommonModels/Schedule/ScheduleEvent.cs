using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Schedule
{
    /// <summary>
    /// 日程表的栏目定义
    /// </summary>
    public class ScheduleEvent
    {
        /// <summary>
        /// 表示该日程已读
        /// </summary>
        public const int EventRead = 1;//orginal=66

        /// <summary>
        /// 表示该日程已完成的状态
        /// </summary>
        public const int EventFinished = 2; //orginal=258

        /// <summary>
        /// 表示该日程是只读状态
        /// </summary>
        public const int OptionReadOnly = 1; //orginal=34

        /// <summary>
        /// 该状态位表是不需要在日程表中显示，仅仅是通知一下用户
        /// </summary>
        public const int OptionNotice = 2; //original=6
        const string AlertTypeStr = "无提醒=-100;事件发生时=0;5分钟前=5;15分钟前=15;30分钟前=30;1小时前=60;2小时前=120;1天前=1440;2天前=2880;1周前=10080";

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        [CatalogExt(DataSourceType = ExtDataSourceType.UserDefine)]
        public string EventType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [CatalogExt(DataType = ExtDataType.DateAndTime)]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [CatalogExt(DataType = ExtDataType.DateAndTime)]
        public string EndTime { get; set; }

        /// <summary>
        /// 冗余属性，便于轮询快速判断需要提醒的事件时间，它由StartTime-AlertBefore生成
        /// </summary>
        [CatalogExt(DataType = ExtDataType.DateAndTime, DataSourceType = ExtDataSourceType.Hidden)]
        public string AlertTime { get; set; }

        /// <summary>
        /// 是否是全天事件
        /// </summary>
        [CatalogExt(DataType = ExtDataType.Bool)]
        public string AllDay { get; set; }

        /// <summary>
        /// 提前通知的时间(分钟）,负数表示不通知
        /// </summary>
        [CatalogExt(DataType = ExtDataType.SingleNumber, DataSource = AlertTypeStr)]
        public string AlertBefore { get; set; }

        /// <summary>
        /// 循环类型
        /// </summary>
        [CatalogExt(DataType = ExtDataType.SingleNumber, DataSource = "无=0;日=1;周=2;月=3;年=4")]
        public string LoopType { get; set; }

        /// <summary>
        /// 处理此消息的页面地址
        /// </summary>
        [CatalogExt(DataType = ExtDataType.SingleLineText, DataSourceType = ExtDataSourceType.UserDefine)]
        public string ProcessUrl { get; set; }

        public static ScheduleEvent Root { get; set; }
    }
}
