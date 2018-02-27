using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Messages
{
    /// <summary>
    /// 消息的处理类,排队处理消息 
    /// </summary>
    public class MessageProcesser : TimerProcesser
    {
        MessageRouter _router;
        public MessageProcesser(MessageRouter router)
        {
            _router = router;
        }

        public void Register(SendChannel channel, IMessageSender sender)
        {
            _router.Register(channel, sender);
        }

        public override void Process(object item)
        {
            JMessage msg = item as JMessage;
            if (msg == null)
            {
                throw new ArgumentException("'item' must be typeof MessaageInfo");
            }

            if (msg.Id == 0)
            {
                msg.Id = SaveGetId(msg);
            }

            ///如果计划发送时间早于当前时间，或为空，则立即发送
            if (msg.Id > 0 && msg.PlanSendTime <= DateTime.Now)
            {
                _router.Send(msg);
            }
        }
        /// <summary>
        /// 将信息实体和待发送到的信息存到数据表中
        /// 转换规则（Base_Article)：
        /// CreateTime = 计划发送时间
        /// Clicks = 发送通道
        /// UrlTitle = 点击消息后转到的Url
        /// EditorId = 要发送到的人
        /// 
        /// 转换规则（Base_ArticleRelation)
        /// RelationType =  是发送还是抄送
        /// 
        /// 转换规则 （Base_CatalogArticle)
        /// Ord = 优先级
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        int SaveGetId(JMessage msg)
        {
            using (var article = SiteManager.Get<ArticleManager>())
            {
                Base_CatalogArticle ca = article.CreateByCatalog(MessageRoot.Root.Id);

                ca.Article.Title = msg.Title;
                ca.Article.Text = msg.Content;
                ca.Article.EditorId = msg.SenderId;
                ca.Article.CreateTime = msg.CreateTime;
                ca.Article.State = SendResultType.WaitForSend;
                ca.Article.UrlTitle = msg.Url;
                ca.Ord = (int)msg.Priority;
                ca.Article.Clicks = (int)msg.Channel;
                if (msg.SendToIds.IsEmpty() && msg.CopyToIds.IsEmpty())
                {
                    return 0;
                }
                foreach (SendChannel channel in Enum.GetValues(typeof(SendChannel)))
                {
                    if ((msg.Channel & channel) == channel)
                    {
                        foreach (var uid in msg.SendToIds)
                        {
                            Base_Article a = new Base_Article
                            {
                                EditorId = uid,
                                Title = ca.Article.Title,
                                CreateTime = msg.PlanSendTime,
                                State = SendResultType.WaitForSend
                            };
                            Base_ArticleRelation ra = new Base_ArticleRelation
                            {
                                Source = ca.Article,
                                Target = a,
                                RelationType = SendType.Send
                            };

                            ca.Article.Targets.Add(ra);
                        }

                        foreach (var uid in msg.CopyToIds)
                        {
                            Base_Article a = new Base_Article
                            {
                                EditorId = uid,
                                Title = ca.Article.Title,
                                CreateTime = msg.PlanSendTime,
                                State = SendResultType.WaitForSend
                            };
                            Base_ArticleRelation ra = new Base_ArticleRelation
                            {
                                Source = ca.Article,
                                Target = a,
                                RelationType = SendType.Copy
                            };

                            ca.Article.Targets.Add(ra);
                        }

                        foreach (var attId in msg.AttachmentIds)
                        {
                            Base_ArticleRelation ra = new Base_ArticleRelation
                            {
                                RelationType = ArticleRelationType.Attachment,
                                Source = ca.Article,
                                TargetId = attId,

                            };
                            ca.Article.Targets.Add(ra);
                        }
                    }
                }

                article.Save(ca);
                return ca.Id;
            }
        }

        protected override System.Collections.IEnumerable LoadData()
        {
            var timeNow = DateTime.Now;
            var timeOut = DateTime.Now.AddDays(-1);
            using (var article = SiteManager.Get<ArticleManager>())
            {
                var list = article.GetAllAtCatalog(MessageRoot.Root.Id)
                     .Where(ca => ca.Article.State >= SendResultType.WaitForSend
                         && ca.Article.CreateTime < timeNow
                         && ca.Article.CreateTime > timeOut)
                         .Select(ca => new JMessage
                         {
                             Id = ca.Id,
                             Title = ca.Article.Title,
                             Content = ca.Article.ArticleText.Text,
                             SendToIds = ca.Article.Targets.Where(a => (a.Target.State >= SendResultType.WaitForSend) && a.RelationType == SendType.Send).Select(a => a.Target.EditorId),
                             CopyToIds = ca.Article.Targets.Where(a => (a.Target.State >= SendResultType.WaitForSend) && a.RelationType == SendType.Copy).Select(a => a.Target.EditorId),
                             AttachmentIds = ca.Article.Targets.Where(a => a.RelationType == ArticleRelationType.Attachment).Select(a => a.TargetId),

                             Channel = (SendChannel)ca.Article.Clicks,
                             Priority = (PriorityType)ca.Ord,
                             PlanSendTime = ca.Article.CreateTime,
                             SenderId = ca.Article.EditorId,
                             Url = ca.Article.UrlTitle
                         });
                return list.ToList();
            }
        }
    }
}
