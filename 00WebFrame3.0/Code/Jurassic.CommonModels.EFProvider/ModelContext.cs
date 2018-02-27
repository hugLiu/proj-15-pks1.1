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
    /// һ���������Ĭ�ϱ�ṹ�����������ģ������û���ɫ�����ţ���ͻ��������ݹ����
    /// </summary>
    public class ModelContext : DbContext, IDisposable
    {
        /// <summary>
        /// ctor, Ĭ����config�ļ�����ΪDefaultConnection�����Ӵ��������µ�������
        /// </summary>
        public ModelContext()
            : base("DefaultConnection")
        {

            //this.Configuration.LazyLoadingEnabled = false;
        }

        //��ʼ���Ż�EF����ʱ�����ٶ�
        static ModelContext()
        {
            Database.SetInitializer<ModelContext>(null);
            using (var dbcontext = SiteManager.Get<ModelContext>())
            {
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
                //�Գ����ж��������DbContext��һ�����������
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

            //���ݶ����Ա� wang 2016/11/3
            modelBuilder.Entity<Sys_DataLanguage>();
            //����Ȩ�ޱ� wang 2016/11/3
            modelBuilder.Entity<Sys_DataRule>();

            #region ��֯�ṹ����
            modelBuilder.Entity<DepartmentModel>().HasKey(e => e.Id);

            modelBuilder.Entity<DepPostModel>().HasKey(e => e.Id);

            modelBuilder.Entity<DepUserModel>().HasKey(e => e.Id);

            modelBuilder.Entity<PostModel>().HasKey(e => e.Id);

            #endregion

            #region WebApi��Ȩģ�����
            //������Ȩ��ȫ����
            modelBuilder.Entity<AuthToken>()
            .HasKey(e => e.ToKeyId)
            .ToTable("API_AUTH_TOKEN");

            //������Ȩ���ݽڵ�
            modelBuilder.Entity<DataNodeInfo>()
            .HasKey(e => e.DataID)
            .ToTable("API_DATA_NODE_INFO");

            //����ڵ����
            modelBuilder.Entity<ServiceInfo>()
            .HasKey(e => e.ServiceID)
            .ToTable("API_SERVICE_INFO");

            //������Ȩ��ϵ��
            modelBuilder.Entity<DataRelation>()
            .HasKey(e => e.RID)
            .ToTable("API_DATA_RELATION");

            //������Ȩ��ϵ��
            modelBuilder.Entity<ServiceRelation>()
            .HasKey(e => e.SID)
            .ToTable("API_SERVICE_RELATION");
            #endregion

        }
    }
}
