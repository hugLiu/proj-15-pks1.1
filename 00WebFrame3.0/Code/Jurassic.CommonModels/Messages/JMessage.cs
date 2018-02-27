using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 信息发送的实体类，包括信息本身和发送的用户
    /// </summary>
    public class JMessage
    {
        public JMessage()
        {
            SendToIds = new List<int>();
            CopyToIds = new List<int>();
            AttachmentIds = new List<int>();
        }
        /// <summary>
        /// 消息的数据库记录ID, 在初始化后应该保持默认值0
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 信息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 发送人的ID
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// 要发送到的用户ID列表
        /// </summary>
        public IEnumerable<int> SendToIds { get; set; }

        /// <summary>
        /// 抄送的用户ID列表
        /// </summary>
        public IEnumerable<int> CopyToIds { get; set; }

        /// <summary>
        /// 计划发送时间
        /// </summary>
        public DateTime PlanSendTime { get; set; }

        /// <summary>
        /// 发送通道（短信、邮件、系统通知等，可以组合)
        /// </summary>
        public SendChannel Channel { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority { get; set; }

        /// <summary>
        /// 附件的ID列表
        /// </summary>
        public IEnumerable<int> AttachmentIds { get; set; }

        /// <summary>
        /// 点击消息转到的URL
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 发送结果
    /// </summary>
    public class SendResult
    {
        /// <summary>
        /// 消息的数据库ID
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 发送失败的用户ID列表
        /// </summary>
        public IEnumerable<int> FailedIds { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailedReason { get; set; }

        /// <summary>
        /// 计划发送时间
        /// </summary>
        public DateTime PlanSendTime { get; set; }

        /// <summary>
        /// 实际发送时间
        /// </summary>
        public DateTime SentTime { get; set; }

        /// <summary>
        /// 发送状态 0-发送成功  2-等待发送 4-发送失败 6-发送失败后等待再次发送
        /// </summary>
        public int State { get; set; }
    }
}
