using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.CommonModels.Articles;
using Jurassic.Com.Tools;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 消息的总的发送类，根据消息发送通道的定义
    /// 分别发送并回写状态
    /// </summary>
    public class MessageRouter
    {
        Dictionary<SendChannel, IMessageSender> sendersDict
            = new Dictionary<SendChannel, IMessageSender>();

        public MessageRouter()
        {
            sendersDict[SendChannel.Email] = new MailMessageSender();
            sendersDict[SendChannel.SMS] = new SMSMessageSender();
            sendersDict[SendChannel.System] = new SystemMessageSender();
            sendersDict[SendChannel.Custom] = new DefaultCustomMessageSender();
        }

        public void Register(SendChannel channel, IMessageSender sender)
        {
            sendersDict[channel] = sender;
        }

        public void Send(JMessage msg)
        {
            foreach (SendChannel channel in Enum.GetValues(typeof(SendChannel)))
            {
                if ((msg.Channel & channel) == channel)
                {
                    SendResult result = sendersDict[channel].Send(msg);
                    WriteBack(result);
                }
            }
        }

        private void WriteBack(SendResult result)
        {
            using (var article = SiteManager.Get<ArticleManager>())
            {
                var ca = article.GetById(result.MessageId);

                foreach (var ta in ca.Article.Targets)
                {
                    ta.Target.State = SendResultType.Success;
                    ta.Target.EditTime = DateTime.Now;
                    ca.Article.TargetArticles.Add(ta.Target);
                }

                //暂时取消发送失败后重复发送，因为多通道发送时导致成功的通道消息重复
                //if (result.FailedIds.IsEmpty())
                //{
                ca.Article.State = SendResultType.Success;
                //}
                //else
                //{

                //    ca.Article.State = SendResultType.FailedWaitForSend;
                //    foreach (var ta in ca.Article.Targets.Where(ta => result.FailedIds.Contains(ta.Target.EditorId)))
                //    {
                //        ta.Target.State = SendResultType.FailedWaitForSend;
                //        ta.Target.EditTime = DateTime.Now;
                //        ta.Target.Abstract = result.FailedReason;
                //    }

                //}
                //ca.Article.TargetArticles = ca.Article.Targets.Select(ta => ta.Target).ToList();
                ca.Article.EditTime = DateTime.Now;

                article.Save(ca);
            }
        }
    }
}
