using Jurassic.AppCenter;
using Jurassic.CommonModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebSchedule
{
    /// <summary>
    /// 用于后台处理消息队列的signalr集线器类
    /// </summary>
    public class ProcesserHub : Hub
    {
        /// <summary>
        /// 获取所有后台处理器的列表
        /// </summary>
        /// <returns>所有后台处理器的列表</returns>
        public IList<ProcesserBase> GetAllProcessers()
        {
            return SignalRProcesserFactory.Instance.GetAllProcessers();   
        }

        /// <summary>
        /// 添加一个用于消息通信的组
        /// </summary>
        /// <param name="groupId">组的ID</param>
        public void AddGroup(String groupId)
        {
            Groups.Add(Context.ConnectionId, groupId);
        }
    }
}