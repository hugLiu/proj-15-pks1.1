using PKS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PKS.DBModels
{
    /// <summary>���ݷ���ʵ��ӳ������</summary>
    public abstract class PKSDbEntityMappingConfiguration : IDbEntityMappingConfiguration
    {
        /// <summary>ӳ��ʵ�弯��</summary>
        public List<object> MapTypes { get; private set; }
        /// <summary>�仯����ʵ�弯��</summary>
        public List<Type> ChangePublishTypes { get; private set; }
        /// <summary>����������</summary>
        private DbModelBuilder ModelBuilder { get; set; }
        /// <summary>��ʼ��ʵ��ӳ��</summary>
        protected EntityTypeConfiguration<TEntity> Entity<TEntity>(bool publishChange = false)
            where TEntity : class
        {
            var entityTypeConfiguration = this.ModelBuilder.Entity<TEntity>();
            this.MapTypes.Add(entityTypeConfiguration);
            if (publishChange) this.ChangePublishTypes.Add(typeof(TEntity));
            return entityTypeConfiguration;
        }
        /// <summary>��ʼ��ʵ��ӳ��</summary>
        protected void Entity<TEntity>(EntityTypeConfiguration<TEntity> entityTypeConfiguration, bool publishChange = false)
            where TEntity : class
        {
            this.ModelBuilder.Configurations.Add(entityTypeConfiguration);
            this.MapTypes.Add(entityTypeConfiguration);
            if (publishChange) this.ChangePublishTypes.Add(typeof(TEntity));
        }
        /// <summary>��ʼ��ʵ��ӳ��</summary>
        public void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.MapTypes = new List<object>();
            this.ChangePublishTypes = new List<Type>();
            this.ModelBuilder = modelBuilder;
            OnModelCreating();
        }

        /// <summary>��ʼ��ʵ��ӳ��</summary>
        protected abstract void OnModelCreating();
    }
}
