using System.Collections.Generic;
using Ninject;
using PKS.Data;

namespace PKS.Core
{
    /// <summary>仓储数据访问组件加载器配置</summary>
    public class RepositoryLoaderConfig : LoaderConfig, IDbContextConfig
    {
        /// <summary>构造函数</summary>
        public RepositoryLoaderConfig()
        {
            this.Name = "PKS.Repository";
        }
        /// <summary>是否发布变化</summary>
        public bool PublishChange { get; set; }
        /// <summary>连接名称</summary>
        public string ConnectionName { get; set; } = "DefaultConnection";
        /// <summary>实体映射配置集合</summary>
        public List<IDbEntityMappingConfiguration> MappingConfigs { get; } = new List<IDbEntityMappingConfiguration>();
    }
}