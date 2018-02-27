using Jurassic.AppCenter.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebSchedule
{
    /// <summary>
    /// Schedule模块的多语言关键字
    /// </summary>
    public class SStr:IStartupStr
    {
        /// <summary>
        /// 新建日程
        /// </summary>
        public static string NewSchedule
        {
            get { return ResHelper.GetStr("NewSchedule", "新日程"); }
        }
        /// <summary>
        /// 编辑日程
        /// </summary>
        public static string EditSchedule
        {
            get { return ResHelper.GetStr("EditSchedule", "编辑日程"); }
        }
        /// <summary>
        /// 不能修改只读的日程
        /// </summary>
        public static string CantChangeReadonlySchedule
        {
            get { return ResHelper.GetStr("CantChangeReadonlySchedule"); }
        }
        /// <summary>
        /// 不能删除只读的消息
        /// </summary>
        public static string CantDeleteReadonlySchedule
        {
            get { return ResHelper.GetStr("CantDeleteReadonlySchedule"); }
        }
        /// <summary>
        /// 所有消息已被清除
        /// </summary>
        public static string AllMessageCleared
        {
            get { return ResHelper.GetStr("AllMessageCleared"); }
        }
        /// <summary>
        /// 所有消息已读
        /// </summary>
        public static string AllMessageRead
        {
            get { return ResHelper.GetStr("AllMessageRead"); }
        }
        /// <summary>
        /// 消息管理
        /// </summary>
        public static string MessageManager
        {
            get { return ResHelper.GetStr("MessageManager"); }
        }

        /// <summary>
        /// 全部已读
        /// </summary>
        public static string ReadAll
        {
            get { return ResHelper.GetStr("ReadAll"); }
        }

        /// <summary>
        /// 全部清除
        /// </summary>
        public static string ClearAll
        {
            get { return ResHelper.GetStr("ClearAll"); }
        }

        /// <summary>
        /// 没有消息
        /// </summary>
        public static string NoMessage
        {
            get { return ResHelper.GetStr("NoMessage"); }
        }
        /// <summary>
        /// 日程
        /// </summary>
        public static string Schedule
        {
            get { return ResHelper.GetStr("Schedule"); }
        }
        /// <summary>
        /// 已读
        /// </summary>
        public static string AlreadyRead
        {
            get { return ResHelper.GetStr("AlreadyRead"); }
        }

        /// <summary>
        /// 确定清除所有消息
        /// </summary>
        public static string ConfirmClearAllMessages
        {
            get { return ResHelper.GetStr("ConfirmClearAllMessages"); }
        }
    }
}