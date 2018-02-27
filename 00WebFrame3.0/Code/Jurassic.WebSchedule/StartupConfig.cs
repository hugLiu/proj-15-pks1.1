using Owin;
using Microsoft.Owin;
using Jurassic.AppCenter;
using Jurassic.CommonModels;
using Jurassic.WebFrame;

namespace Jurassic.WebSchedule
{
    /// <summary>
    /// 在应用程序启动时初始化
    /// </summary>
    public class StartupConfig : IStartupConfig
    {
        public void Config(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            SignalRProcesserFactory.Instance.Register("Broadcast", new BroadcastProcesser());
            SignalRProcesserFactory.Instance.Start("Broadcast");
            SignalRProcesserFactory.Instance.Register("UserAlert", SiteManager.Get<ScheduleAlertProcesser>());
            SignalRProcesserFactory.Instance.Start("UserAlert");
        }
    }
}