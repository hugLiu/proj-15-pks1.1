using Jurassic.CommonModels;
using Jurassic.CommonModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Jurassic.WebSchedule.ApiControllers
{
    public class MessageController : ApiController
    {
        /// <summary>
        /// 接收外部系统发过来的消息
        /// </summary>
        /// <param name="msg">消息实体</param>
        /// <returns>发送结果</returns>
        [HttpPost]
        public SendResult Send(JMessage msg)
        {
            SiteManager.Message.Send(msg);
            return SiteManager.Message.GetSendResults(msg.Id);
        }
    }
}
