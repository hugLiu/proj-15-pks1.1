using System.Data.Entity.ModelConfiguration.Conventions;
using Jurassic.Com.Tools;
using Ninject;

namespace Jurassic.CommonModels.EFProvider
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Jurassic.AppCenter.Logs;
    using Jurassic.CommonModels.Articles;
    //using Jurassic.CommonModels.Organization;
    using Jurassic.AppCenter;
    using Jurassic.CommonModels.ServerAuth;
    using Jurassic.CommonModels.Organization;
    using Jurassic.CommonModels.EntityBase;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Collections.Generic;

    /// <summary>
    /// 一个包含框架默认表结构的数据上下文，包括用户角色，部门，组和基本的内容管理表
    /// </summary>
    public class ModelContext : DbContext, IDisposable
    {
        /// <summary>
        /// ctor, 默认以config文件中名为DefaultConnection的连接串来创建新的上下文
        /// </summary>
        public ModelContext()
            : base("DefaultConnection")
        {

            //this.Configuration.LazyLoadingEnabled = false;
        }

        //初始化优化EF启动时加载速度
        static ModelContext()
        {
            Database.SetInitializer<ModelContext>(null);
            using (var dbcontext = SiteManager.Get<ModelContext>())
            {
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
                //对程序中定义的所有DbContext逐一进行这个操作
            }
        }

        [Inject]
        public string Schema { get; set; }
        public virtual DbSet<Base_CatalogArticle> CatalogArticles { get; set; }
        public virtual DbSet<Base_ArticleRelation> ArticleRelations { get; set; }
        public virtual DbSet<Base_ArticleExt> ArticleExts { get; set; }
        public virtual DbSet<Base_Catalog> Catalogs { get; set; }
        public virtual DbSet<Base_CatalogExt> CatlogExts { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<MemberShip> MemberShips { get; set; }

        public virtual DbSet<AuthToken> AuthTokens { get; set; }
        public virtual DbSet<DataNodeInfo> DataNodeInfos { get; set; }
        public virtual DbSet<ServiceInfo> ServiceInfos { get; set; }
        public virtual DbSet<DataRelation> DataRelations { get; set; }
        public virtual DbSet<ServiceRelation> ServiceRelations { get; set; }

        public virtual DbSet<DepartmentModel> DepartmentModels { get; set; }
        public virtual DbSet<DepPostModel> DepPostModels { get; set; }
        public virtual DbSet<DepUserModel> DepUserModels { get; set; }
        public virtual DbSet<PostModel> PostModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (!Schema.IsEmpty())
            {
                var schema = modelBuilder.HasDefaultSchema(Schema);
            }

            //modelBuilder.Conventions.Add(new DateTime2Convention());

            modelBuilder.Entity<Base_Article>()
                .Property(e => e.UrlTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Base_Article>()
                .HasMany(e => e.Exts)
                .WithRequired(e => e.Article)
                .HasForeignKey(e => e.ArticleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Base_Article>()
                .HasMany(e => e.Targets)
                .WithRequired(e => e.Source)
                .HasForeignKey(e => e.SourceId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<JLogInfo>()
            //    .HasKey(log => log.Id)
            //    .ToTable("SYS_LOG");
            modelBuilder.Entity<Base_ArticleText>()
                .HasKey(txt => txt.Id)
                .ToTable("BASE_ARTICLETEXT");

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<JLogInfo>()
            .HasKey(log => log.Id)
            .ToTable("SYS_LOG");

            //数据多语言表 wang 2016/11/3
            modelBuilder.Entity<Sys_DataLanguage>();
            //数据权限表 wang 2016/11/3
            modelBuilder.Entity<Sys_DataRule>();

            #region 组织结构对象
            modelBuilder.Entity<DepartmentModel>().HasKey(e => e.Id);

            modelBuilder.Entity<DepPostModel>().HasKey(e => e.Id);

            modelBuilder.Entity<DepUserModel>().HasKey(e => e.Id);

            modelBuilder.Entity<PostModel>().HasKey(e => e.Id);

            #endregion

            #region WebApi授权模块对象
            //服务授权安全令牌
            modelBuilder.Entity<AuthToken>()
            .HasKey(e => e.ToKeyId)
            .ToTable("API_AUTH_TOKEN");

            //服务授权数据节点
            modelBuilder.Entity<DataNodeInfo>()
            .HasKey(e => e.DataID)
            .ToTable("API_DATA_NODE_INFO");

            //服务节点对象
            modelBuilder.Entity<ServiceInfo>()
            .HasKey(e => e.ServiceID)
            .ToTable("API_SERVICE_INFO");

            //数据授权关系表
            modelBuilder.Entity<DataRelation>()
            .HasKey(e => e.RID)
            .ToTable("API_DATA_RELATION");

            //服务授权关系表
            modelBuilder.Entity<ServiceRelation>()
            .HasKey(e => e.SID)
            .ToTable("API_SERVICE_RELATION");
            #endregion

        }
    }
}
