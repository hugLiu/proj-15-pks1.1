using System;
using System.ServiceModel;
using Common.Logging;
using PKS.Core;

namespace PKS.MgmtServices.Core
{
    /// <summary>管理服务启动器</summary>
    public class MgmtServiceBootstrapper : Bootstrapper
    {
        #region 初始化
        /// <summary>设置异常配置</summary>
        public override void SetExceptionConfig(ExceptionLoaderConfig config)
        {
            config.Enable = false;
        }
        /// <summary>设置缓存配置</summary>
        public override void SetCacheConfig(CacheLoaderConfig config)
        {
            config.Enable = false;
        }
        /// <summary>设置仓储数据访问配置</summary>
        public override void SetRepositoryConfig(RepositoryLoaderConfig config)
        {
            config.Enable = false;
        }
        /// <summary>设置对象映射配置</summary>
        public override void SetObjectMapperConfig(ObjectMapperLoaderConfig config)
        {
            config.Enable = false;
        }
        #endregion

        /// <summary>服务宿主</summary>
        private ServiceHost ServiceHost { get; set; }
        /// <summary>启动</summary>
        public void Start()
        {
            var service = Get<IMgmtService>();
            this.ServiceHost = new ServiceHost(service);
            this.ServiceHost.Open();
            this.ServiceHost.Faulted += ServiceHost_Faulted;
            this.ServiceHost.Closed += ServiceHost_Closed;
        }
        /// <summary>处理出错事件</summary>
        private void ServiceHost_Faulted(object sender, EventArgs e)
        {
            Get<ILog>().Info("MgmtService:Faulted");
        }
        /// <summary>处理关闭事件</summary>
        private void ServiceHost_Closed(object sender, EventArgs e)
        {
            Get<ILog>().Info("MgmtService:Closed");
        }
        /// <summary>停止</summary>
        public void Stop()
        {
            TryExecute(this.ServiceHost.Close);
            this.ServiceHost = null;
        }
    }
}
