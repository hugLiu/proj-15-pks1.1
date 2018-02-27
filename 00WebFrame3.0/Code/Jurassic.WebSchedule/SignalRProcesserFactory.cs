using Jurassic.AppCenter;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Schedule;
using Jurassic.Com.Tools;

namespace Jurassic.WebSchedule
{
    /// <summary>
    /// 基于SingalR的处理器工厂
    /// </summary>
    public class SignalRProcesserFactory : ProcesserFactory
    {
        private readonly static Lazy<SignalRProcesserFactory> _instance =
            new Lazy<SignalRProcesserFactory>(() =>
            new SignalRProcesserFactory(GlobalHost.ConnectionManager.GetHubContext<ProcesserHub>().Clients));

        internal IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        /// <summary>
        /// 基于SignalR处理器工厂的单例
        /// </summary>
        public new static SignalRProcesserFactory Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private SignalRProcesserFactory(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }
        public dynamic Group(string grpName)
        {
            return Instance.Clients.Group(grpName);
        }


        protected override void AfterRegister(ProcesserBase processer)
        {
            processer.ProgressChanged += processer_ProgressChanged;
        }

        void processer_ProgressChanged(object sender, EventArgs e)
        {
            Clients.Group("Admin").reportProgress(sender);
        }
    }

    class ScheduleAlertProcesser : ProcesserBase
    {
        public ScheduleAlertProcesser()
        {
            timer.Start();
            timer.Elapsed += timer_Elapsed;
        }

        Timer timer = new Timer(30000);

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var schedule = SiteManager.Get<ScheduleManager>();
            var alertIds = schedule.GetUserIdsToAlert().Except(AppManager.Instance.UserManager.GetAll()
                .Where(u=>!u.IsOnline).Select(u=>u.Id.ToInt()));
            SiteManager.Kernel.Release(schedule.Article);
            schedule.Article.Dispose();

            foreach (var id in alertIds)
            {
                SignalRProcesserFactory.Instance.Add("UserAlert", id);
            }
        }

        public override void Process(object item)
        {
            SignalRProcesserFactory.Instance.Group(item.ToString()).Alert();
        }
    }

}