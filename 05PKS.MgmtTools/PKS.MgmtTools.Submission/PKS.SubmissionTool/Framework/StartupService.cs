using Jurassic.AppCenter;
using Jurassic.AppCenter.Logs;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
//using Jurassic.Log4;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace PKS.SubmissionTool
{
    public class StartupService : IStartupService
    {
        private WorkItem _rootWorkItem;

        [InjectionConstructor]
        public StartupService([ServiceDependency] WorkItem rootWorkItem)
        {
            _rootWorkItem = rootWorkItem;
        }

        public void AddServices()
        {
            #region add用户服务
            //var jLogManager = new JLogManager();
            //_rootWorkItem.Services.Add<IJLogManager>(jLogManager);
            //LogHelper.Init(jLogManager, "default");
            _rootWorkItem.Services.AddNew<LoginService, ILoginService>();
            AppManager.Instance.StateProvider = new AppStateProvider();
            #endregion
        }
    }
}