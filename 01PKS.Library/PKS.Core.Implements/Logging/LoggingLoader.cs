using System.Diagnostics;
using Common.Logging;
using Common.Logging.NLog;
using Ninject;
using NLog;

namespace PKS.Core
{
    /// <summary>日志加载器</summary>
    public class LoggingLoader : IServiceLoader<LoggingLoaderConfig>
    {
        /// <summary>初始化</summary>
        public void Initialize(IKernel kernel, LoggingLoaderConfig config)
        {
            var commonLogger = Common.Logging.LogManager.GetLogger(config.Name);
            kernel.Bind<ILog>().ToConstant(commonLogger);
            var nlogLogger = NLog.LogManager.GetLogger(config.Name);
            kernel.Bind<ILogger>().ToConstant(nlogLogger);
        }

        /// <summary>加载间接DLL</summary>
        private void LoadIndirectAssemblies()
        {
            Debug.WriteLine(typeof(Common.Logging.LogManager));
            Debug.WriteLine(typeof(NLog.LogManager));
            Debug.WriteLine(typeof(NLogLogger));
        }
    }
}