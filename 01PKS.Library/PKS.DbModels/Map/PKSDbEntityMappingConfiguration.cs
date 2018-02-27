using PKS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PKS.DBModels
{
    /// <summary>数据访问实体映射配置</summary>
    public abstract class PKSDbEntityMappingConfiguration : IDbEntityMappingConfiguration
    {
        /// <summary>映射实体集合</summary>
        public List<object> MapTypes { get; private set; }
        /// <summary>变化发布实体集合</summary>
        public List<Type> ChangePublishTypes { get; private set; }
        /// <summary>配置生成器</summary>
        private DbModelBuilder ModelBuilder { get; set; }
        /// <summary>初始化实体映射</summary>
        protected EntityTypeConfiguration<TEntity> Entity<TEntity>(bool publishChange = false)
            where TEntity : class
        {
            var entityTypeConfiguration = this.ModelBuilder.Entity<TEntity>();
            this.MapTypes.Add(entityTypeConfiguration);
            if (publishChange) this.ChangePublishTypes.Add(typeof(TEntity));
            return entityTypeConfiguration;
        }
        /// <summary>初始化实体映射</summary>
        protected void Entity<TEntity>(EntityTypeConfiguration<TEntity> entityTypeConfiguration, bool publishChange = false)
            where TEntity : class
        {
            this.ModelBuilder.Configurations.Add(entityTypeConfiguration);
            this.MapTypes.Add(entityTypeConfiguration);
            if (publishChange) this.ChangePublishTypes.Add(typeof(TEntity));
        }
        /// <summary>初始化实体映射</summary>
        public void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.MapTypes = new List<object>();
            this.ChangePublishTypes = new List<Type>();
            this.ModelBuilder = modelBuilder;
            OnModelCreating();
        }

        /// <summary>初始化实体映射</summary>
        protected abstract void OnModelCreating();
    }
}
