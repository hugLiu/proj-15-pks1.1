using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Schedule;
using Jurassic.CommonModels.Articles;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 消息发送的公共接口
    /// </summary>
    public interface IMessageSender
    {
        SendResult Send(JMessage msg);
    }

    /// <summary>
    /// 默认的邮件服务发送类
    /// </summary>
    public class MailMessageSender : IMessageSender
    {
        public SendResult Send(JMessage msg)
        {
            using (var resFileService = SiteManager.Get<ResourceFileService>())
            {
                string sendto = String.Join(";", msg.SendToIds.Select(s => AppManager.Instance.UserManager.GetById(s.ToString()))
                    .Where(u => !u.Email.IsEmpty())
                    .Select(u => (u.Name + "<" + u.Email + ">")));
                var fromUser = AppManager.Instance.UserManager.GetById(msg.SenderId.ToString());
                string from = fromUser == null ? null : fromUser.Email;
                string ccto = String.Join(";", msg.CopyToIds.Select(s => AppManager.Instance.UserManager.GetById(s.ToString()))
                    .Where(u => !u.Email.IsEmpty())
                    .Select(u => (u.Name + "<" + u.Email + ">")));
                SMTPMail mail = new SMTPMail(from, sendto, ccto, msg.Title, msg.Content, resFileService.GetFilePath((msg.AttachmentIds ?? new int[0]).ToArray()));
                mail.Send();

                SendResult result = new SendResult
                {
                    FailedReason = mail.ErrorMessage,
                    MessageId = msg.Id,
                    SentTime = DateTime.Now,
                    PlanSendTime = msg.PlanSendTime,
                };
                if (!mail.ErrorMessage.IsEmpty())
                {
                    result.FailedIds = msg.SendToIds.Union(msg.CopyToIds);
                }
                return result;
            }
        }
    }

    /// <summary>
    /// 默认的系统消息发送类
    /// </summary>
    public class SystemMessageSender : IMessageSender
    {
        public SendResult Send(JMessage msg)
        {
            using (var article = SiteManager.Get<ArticleManager>())
            {
                string author = AppManager.Instance.UserManager.GetById(msg.SenderId.ToString()).Name;
                foreach (int id in msg.SendToIds.Union(msg.CopyToIds))
                {
                    var ca = article.CreateByCatalog(ScheduleEvent.Root.Id);
                    ca.Article.Options = ScheduleEvent.OptionNotice;
                    ca.SetExt(ScheduleEvent.Root.AlertBefore, 5);
                    ca.SetExt(ScheduleEvent.Root.StartTime, DateTime.Now);
                    ca.SetExt(ScheduleEvent.Root.AllDay, true);
                    ca.SetExt(ScheduleEvent.Root.ProcessUrl, msg.Url);
                    ca.Article.Title = msg.Title;
                    ca.Article.Text = msg.Content;
                    ca.Article.EditorId = id;
                    ca.Article.Author = author;
                    ca.Article.CreateTime = DateTime.Now;
                    ScheduleManager.AdjustAlertTime(ca);
                    article.Save(ca);
                }
                SendResult result = new SendResult
                {
                    MessageId = msg.Id,
                    SentTime = DateTime.Now,
                    PlanSendTime = msg.PlanSendTime,
                };
                return result;
            }
        }
    }

    /// <summary>
    /// 默认的短消息发送类
    /// </summary>
    public class SMSMessageSender : IMessageSender
    {
        public SendResult Send(JMessage msg)
        {
            return new SendResult();
        }
    }

    /// <summary>
    /// 默认的自定义消息发送器，什么事都不做
    /// </summary>
    public class DefaultCustomMessageSender : IMessageSender
    {
        public SendResult Send(JMessage msg)
        {
            return new SendResult();
        }
    }
}
