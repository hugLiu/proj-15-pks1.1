using System;
using PKS.Core;

namespace PKS.SubmissionTool
{
    /// <summary>管理服务启动器</summary>
    public class ModuleBootstrapper : Bootstrapper
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
            //config.ExternalName = null;
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
        /// <summary>记录异常</summary>
        public static void Error(object instance, string method, Exception ex)
        {
            var message = $"{instance.GetType().Name}.{method}:";
            Error(message, ex);
        }
        #endregion
    }
}
