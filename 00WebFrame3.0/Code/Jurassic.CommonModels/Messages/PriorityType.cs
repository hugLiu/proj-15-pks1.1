using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 优先级
    /// </summary>
    [Flags]
    public enum PriorityType
    {
        /// <summary>
        /// 一般
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 重要
        /// </summary>
        Important = 1,

        /// <summary>
        /// 紧急
        /// </summary>
        Urgent = 2,

        /// <summary>
        /// 重要且紧急
        /// </summary>
        ImportantUrgent = 3
    }

    /// <summary>
    /// 消息的发送通道
    /// </summary>
    [Flags]
    public enum SendChannel
    {
        /// <summary>
        /// 系统内部
        /// </summary>
        System = 1,

        /// <summary>
        /// 邮件通知
        /// </summary>
        Email = 2,

        /// <summary>
        /// 短信通知
        /// </summary>
        SMS = 4,

        /// <summary>
        /// 自定义通道
        /// </summary>
        Custom = 8
    }

    /// <summary>
    /// 发送或抄送的关系定义
    /// </summary>
    public static class SendType
    {
        /// <summary>
        /// 发送的关系
        /// </summary>
        public const int Send = 0;

        /// <summary>
        /// 抄送的关系
        /// </summary>
        public const int Copy = 1;
    }

    /// <summary>
    /// 消息发送的结果
    /// </summary>
    public static class SendResultType
    {
        /// <summary>
        /// 等待发送
        /// </summary>
        public const int WaitForSend = 2; //0x010

        /// <summary>
        /// 发送成功
        /// </summary>
        public const int Success = 0; //0x000

        /// <summary>
        /// 发送失败
        /// </summary>
        public const int Failed = 4; //0x110

        /// <summary>
        /// 发送失改后等待再次发送
        /// </summary>
        public const int FailedWaitForSend = Failed + WaitForSend;
    }
}
