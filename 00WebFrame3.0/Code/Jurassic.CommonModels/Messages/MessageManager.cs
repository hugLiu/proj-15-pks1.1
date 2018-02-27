using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 消息的管理类
    /// </summary>
    public class MessageManager
    {
        MessageProcesser _processer;
        static MessageManager()
        {
        }

        /// <summary>
        /// ctor,由IOC容器自动调用
        /// </summary>
        /// <param name="processer"></param>
        public MessageManager(MessageProcesser processer)
        {
            _processer = processer;
        }

        /// <summary>
        /// 注册指定通道的消息发送类
        /// </summary>
        /// <param name="channel">通道枚举</param>
        /// <param name="sender">执行发送的对象</param>
        public void Register(SendChannel channel, IMessageSender sender)
        {
            _processer.Register(channel, sender);
        }

        /// <summary>
        /// 发送消息 
        /// </summary>
        /// <param name="msg">消息实体对象</param>
        public void Send(JMessage msg)
        {
            _processer.Add(msg);
        }

        /// <summary>
        /// 根据消息 标题 通道和发送目标发送消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="channel"></param>
        /// <param name="sendtoIds"></param>
        public void Send(string title, SendChannel channel, params int[] sendtoIds)
        {
            JMessage msg = new JMessage
            {
                Title = title,
                SendToIds = sendtoIds,
                Channel = channel,
                SenderId = AppManager.Instance.GetCurrentUserId().ToInt()
            };

            _processer.Add(msg);
        }

        /// <summary>
        /// 根据消息 标题 内容 通道和发送目标发送消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="channel"></param>
        /// <param name="sendtoIds"></param>
        public void Send(string title, string content, SendChannel channel, params int[] sendtoIds)
        {
            JMessage msg = new JMessage
            {
                Title = title,
                Content = content,
                SendToIds = sendtoIds,
                Channel = channel,
                SenderId = AppManager.Instance.GetCurrentUserId().ToInt()
            };

            _processer.Add(msg);
        }

        /// <summary>
        /// 向指定部门发送广播消息
        /// </summary>
        /// <param name="msg">消息实体</param>
        /// <param name="deptIds">部门ID的集合</param>
        /// <returns></returns>
        public int BroadCast(JMessage msg, params int[] deptIds)
        {
            return msg.Id;
        }

        /// <summary>
        /// 获取指定消息发送的结果
        /// </summary>
        /// <param name="msgId">消息的数据库ID</param>
        /// <returns>发送结果</returns>
        public SendResult GetSendResults(int msgId)
        {
            using (var article = SiteManager.Get<ArticleManager>())
            {
                var ca = article.GetById(msgId);
                if (ca == null)
                {
                    return null;
                }
                SendResult result = new SendResult
                {
                    FailedIds = ca.Article.Targets.Where(ta => ta.Target.State >= SendResultType.Failed)
                    .Select(ta => ta.Target.EditorId),
                    FailedReason = ca.Article.Abstract,
                    MessageId = ca.Id,
                    PlanSendTime = ca.Article.CreateTime,
                    SentTime = ca.Article.EditTime,
                    State = ca.Article.State
                };
                return result;
            }
        }

    }
}
