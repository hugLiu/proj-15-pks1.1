using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebSchedule
{
    /// <summary>
    /// 向全体发送广播消息的消息处理器
    /// </summary>
    public class BroadcastProcesser : ProcesserBase
    {
        public override void Process(object item)
        {
            SignalRProcesserFactory.Instance.Clients.All.Broadcast(item);
        }
    }
}