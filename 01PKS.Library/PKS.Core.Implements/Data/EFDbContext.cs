using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus;
using Ninject.Infrastructure.Language;
using PKS.Utils;

namespace PKS.Data
{
    /// <summary>EF数据访问</summary>
    public class EFDbContext : DbContext, IDbContext
    {
        /// <summary>静态构造函数</summary>
        static EFDbContext()
        {
            //关闭初始化器
            Database.SetInitializer<EFDbContext>(null);
            s_ChangePublishTypes = new Dictionary<Type, int>();
        }
        /// <summary>构造函数</summary>
        public EFDbContext(IDbContextConfig config, IEventBus eventBus) : base("name=" + config.ConnectionName)
        {
            this.Config = config;
            this.EventBus = eventBus;
        }
        /// <summary>数据访问配置接口</summary>
        public IDbContextConfig Config { get; }
        /// <summary>事件总线</summary>
        protected IEventBus EventBus { get; set; }
        /// <summary>变化发布实体集合</summary>
        protected static Dictionary<Type, int> s_ChangePublishTypes { get; private set; }
        /// <summary>初始化实体映射</summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            var settings = ConfigurationManager.ConnectionStrings[this.Config.ConnectionName];
            bool isOracleClient = settings.IsOracleClient();
            if (isOracleClient)
            {
                var schema = settings.GetOracleSchema();
                modelBuilder.HasDefaultSchema(schema);
            }
            foreach (var mappingConfig in this.Config.MappingConfigs)
            {
                mappingConfig.OnModelCreating(modelBuilder);
                if (this.Config.PublishChange)
                {
                    foreach (var type in mappingConfig.ChangePublishTypes)
                    {
                        s_ChangePublishTypes[type] = 1;
                    }
                }
                if (!isOracleClient) continue;
                foreach (var mapping in mappingConfig.MapTypes)
                {
                    dynamic mapping2 = mapping;
                    ConfigureEntityType(mappingConfig, mapping2);
                }
            }
            if (isOracleClient)
            {
                modelBuilder.Properties().Configure(ConfigureProperty);
            }
        }

        /// <summary>配置实体类型</summary>
        private void ConfigureEntityType<TEntity>(IDbEntityMappingConfiguration config, EntityTypeConfiguration<TEntity> mapping)
            where TEntity : class
        {
            var entityType = typeof(TEntity);
            var tableAttribute = entityType.GetAttributes<TableAttribute>().FirstOrDefault();
            if (tableAttribute == null)
            {
                mapping.ToTable(entityType.Name.ToUpperInvariant());
            }
        }
        /// <summary>配置属性类型</summary>
        private void ConfigureProperty(ConventionPrimitivePropertyConfiguration propertyConfiguration)
        {
            if (!propertyConfiguration.ClrPropertyInfo.HasAttribute<ColumnAttribute>())
            {
                propertyConfiguration.HasColumnName(propertyConfiguration.ClrPropertyInfo.Name.ToUpperInvariant());
            }
        }

        /// <summary>保存变化</summary>
        public override int SaveChanges()
        {
            if (this.Config.PublishChange)
            {
                var changedList = BuildChangedList();
                var result = base.SaveChanges();
                PublishChangedList(changedList);
                return result;
            }
            return base.SaveChanges();
        }
        /// <summary>保存变化</summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (this.Config.PublishChange)
            {
                var changedList = BuildChangedList();
                var result = await base.SaveChangesAsync(cancellationToken);
                PublishChangedList(changedList);
                return result;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>生成变化实体集合</summary>
        private List<Tuple<Type, EntityState, object>> BuildChangedList()
        {
            var entries = base.ChangeTracker.Entries();
            var changedList = new List<Tuple<Type, EntityState, object>>();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        var entityType = entry.Entity.GetType();
                        if (s_ChangePublishTypes.ContainsKey(entityType))
                        {
                            changedList.Add(new Tuple<Type, EntityState, object>(entityType, entry.State, entry.Entity));
                        }
                        else if (s_ChangePublishTypes.ContainsKey(entityType.BaseType))
                        {
                            changedList.Add(new Tuple<Type, EntityState, object>(entityType.BaseType, entry.State, entry.Entity));
                        }
                        break;
                }
            }
            return changedList;
        }
        /// <summary>发布变化实体集合</summary>
        private void PublishChangedList(List<Tuple<Type, EntityState, object>> changedList)
        {
            if (changedList.Count == 0) return;
            var groupChangedList = changedList.GroupBy(e => e.Item1).ToArray();
            var entityChangedEventArgsBaseType = typeof(EntityChangedEventArgs<>);
            foreach (var group in groupChangedList)
            {
                var entityChangedEventArgsType = entityChangedEventArgsBaseType.MakeGenericType(group.Key);
                var e = entityChangedEventArgsType.CreateInstance<IEntityChangedEventArgs>();
                foreach (var tuple in group)
                {
                    e.Add(tuple.Item2, tuple.Item3);
                }
                this.EventBus.Post(e, TimeSpan.Zero);
            }
        }
    }
}