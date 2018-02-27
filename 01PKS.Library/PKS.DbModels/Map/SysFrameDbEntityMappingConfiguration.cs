using PKS.DbModels;
using PKS.DbModels.OilWiki;
using PKS.DbModels.Portal;
using PKS.DbModels.PortalMgmt;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKS.DBModels
{
    /// <summary>SysFrame数据访问实体映射配置</summary>
    public class SysFrameDbEntityMappingConfiguration : PKSDbEntityMappingConfiguration
    {
        /// <summary>初始化实体映射</summary>
        protected override void OnModelCreating()
        {
            Entity<UserAuthSessions>();
            Entity<USERPROFILE>();
            Entity<WEBPAGES_MEMBERSHIP>();
            Entity<WEBPAGES_OAUTHMEMBERSHIP>();
            Entity<WEBPAGES_ROLES>();
            Entity<WEBPAGES_USERSINROLES>();
            Entity<VI_USERINFO>();

            Entity<PKS_Code>();
            Entity<PKS_CodeType>()
                .HasMany(e => e.PKS_Code)
                .WithRequired(e => e.PKS_CodeType)
                .HasForeignKey(e => e.CodeTypeId)
                .WillCascadeOnDelete(false);

            //var mapping_PKS_Log = Entity<PKS_Log>();
            //mapping_PKS_Log.HasKey(p => p.Id)
            //    .Property(p => p.Id)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //mapping_PKS_Log.Property(p => p.Request).IsUnicode(true);
            //mapping_PKS_Log.Property(p => p.Message).IsUnicode(true);
            //mapping_PKS_Log.Property(p => p.Source).IsUnicode(true);
            //mapping_PKS_Log.Property(p => p.Exception).IsUnicode(true).HasColumnType("NCLOB");

            var mapping_PKS_SUBSYSTEM = Entity<PKS_SUBSYSTEM>(true);
            mapping_PKS_SUBSYSTEM.HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            mapping_PKS_SUBSYSTEM.HasMany(p => p.Permissions)
                .WithRequired()
                .HasForeignKey(t => t.SubSystemId);

            var pt_map = Entity<PKS_PERMISSION_TYPE>();
            pt_map.HasMany(t => t.Permissions)
                  .WithRequired()
                  .HasForeignKey(p => p.PermissionTypeId);

            var p_map = Entity<PKS_PERMISSION>(true);
            p_map.HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            p_map.HasMany(p => p.RolePermissions)
                .WithRequired(r => r.Permission)
                .HasForeignKey(r => r.PermissionId);

            Entity<PKS_ROLE_PERMISSION>(true)
                .HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            var map_PKS_Remark = Entity<PKS_Remark>();
            map_PKS_Remark.HasMany(t => t.Thumbups).WithRequired().HasForeignKey(p => p.RemarkId);

            Entity<PKS_Remark_Thumbup>();

            Entity<PKS_SearchHistory>();

            #region 用户行为
            var map_PKS_FavoriteCatelog = Entity<PKS_FAVORITECATALOG>();
            map_PKS_FavoriteCatelog.HasMany(t => t.PKS_USERBEHAVIOR)
                .WithRequired(t => t.PKS_FAVORITECATALOG)
                .HasForeignKey(t => t.FAVORITECATALOGID)
                .WillCascadeOnDelete(false);

            map_PKS_FavoriteCatelog.HasMany(t => t.PKS_FAVORITECATALOG1)
                .WithRequired(t => t.PKS_FAVORITECATALOG2)
                .HasForeignKey(t => t.PARENTID)
                .WillCascadeOnDelete(false);



            Entity<PKS_USERBEHAVIOR>();

            #endregion

            #region 数据权限新增
            var meta = Entity<Models.MetadataDefinition>(true);
            meta.HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            meta.HasMany(m => m.Items)
                .WithRequired()
                .HasForeignKey(i => i.MetaId);

            var metaItem = Entity<Models.MetadataValueItem>(true);
            metaItem.HasKey(p => p.Id)
                    .Property(p => p.Id)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            var map_role_meta = Entity<PKS_ROLE_METADATAPERMISSION>(true);
            map_role_meta.HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            map_role_meta.HasMany(m => m.MetadataItemPermissioin)
                .WithRequired()
                .HasForeignKey(i => i.RoleMetaId);

            var map_role_metaItem = Entity<PKS_ROLE_METADATAITEMPERMISSION>(true);
            map_role_metaItem.HasKey(p => p.Id)
               .Property(p => p.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            #endregion 

            #region 语义库

            var map_SD_CCTerm = Entity<SD_CCTerm>();
            map_SD_CCTerm.Property(e => e.CCCode)
                .IsUnicode(false);
            map_SD_CCTerm.Property(e => e.LangCode)
                .IsUnicode(false);
            map_SD_CCTerm.HasMany(e => e.SD_Semantics)
                .WithRequired(e => e.SD_CCTerm)
                .HasForeignKey(e => e.FTermClassId)
                .WillCascadeOnDelete(false);
            map_SD_CCTerm.HasMany(e => e.SD_Semantics1)
                .WithRequired(e => e.SD_CCTerm1)
                .HasForeignKey(e => e.LTermClassId)
                .WillCascadeOnDelete(false);
            map_SD_CCTerm.HasMany(e => e.SD_TermKeyword)
                .WithRequired(e => e.SD_CCTerm)
                .WillCascadeOnDelete(false);
            map_SD_CCTerm.HasMany(e => e.SD_TermTranslation)
                .WithRequired(e => e.SD_CCTerm)
                .WillCascadeOnDelete(false);

            var map_SD_ConceptClass = Entity<SD_ConceptClass>();
            map_SD_ConceptClass.Property(e => e.CCCode)
                .IsUnicode(false);

            var map_SD_Semantics = Entity<SD_Semantics>();
            map_SD_Semantics.Property(e => e.SR)
                .IsUnicode(false);

            var map_SD_SemanticsType = Entity<SD_SemanticsType>();
            map_SD_SemanticsType.Property(e => e.SR)
                .IsUnicode(false);
            map_SD_SemanticsType.Property(e => e.CCCode1)
                .IsUnicode(false);
            map_SD_SemanticsType.Property(e => e.CCCode2)
                .IsUnicode(false);
            map_SD_SemanticsType.HasMany(e => e.SD_Semantics)
                .WithRequired(e => e.SD_SemanticsType)
                .WillCascadeOnDelete(false);

            Entity<SD_TermKeyword>();

            var map_SD_TermTranslation = Entity<SD_TermTranslation>();
            map_SD_TermTranslation.Property(e => e.LangCode)
                .IsUnicode(false);

            var map_SD_TermSource = Entity<SD_TermSource>();
            map_SD_TermSource.Property(e => e.CCCode)
                .IsUnicode(false);

            Entity<Dict_CommonView>();
            Entity<Dict_ProfessionalView>();

            var map_SMT_BFTreeView = Entity<SMT_BFTreeView>();
            map_SMT_BFTreeView.Property(e => e.SR)
                .IsUnicode(false);

            var map_SMT_BOTTreeView = Entity<SMT_BOTTreeView>();
            map_SMT_BOTTreeView.Property(e => e.SR)
                .IsUnicode(false);

            var map_SMT_GNTreeView = Entity<SMT_GNTreeView>();
            map_SMT_GNTreeView.Property(e => e.SR)
                .IsUnicode(false);

            Entity<SMT_PTContextView>();
            #endregion

            #region 百科配置

            Entity<PKS_KFRAGMENT>()
                .Property(e => e.PLACEHOLDERID)
                .IsUnicode(false);

            var map_PKS_KFRAGMENT_TYPE = Entity<PKS_KFRAGMENT_TYPE>();
            map_PKS_KFRAGMENT_TYPE.Property(e => e.CODE)
                .IsUnicode(false);
            map_PKS_KFRAGMENT_TYPE.HasMany(e => e.PKS_KFRAGMENT)
                .WithRequired(e => e.PKS_KFRAGMENT_TYPE)
                .HasForeignKey(e => e.KFRAGMENTTYPEID)
                .WillCascadeOnDelete(false);
            map_PKS_KFRAGMENT_TYPE.HasMany(e => e.PKS_KFRAGMENT_TYPE_PARAMETER)
                .WithRequired(e => e.PKS_KFRAGMENT_TYPE)
                .HasForeignKey(e => e.KFRAGMENTTYPEID)
                .WillCascadeOnDelete(false);

            var map_PKS_KTEMPLATE = Entity<PKS_KTEMPLATE>();
            map_PKS_KTEMPLATE.HasMany(e => e.PKS_KTEMPLATE_CATALOGUE)
                .WithRequired(e => e.PKS_KTEMPLATE)
                .HasForeignKey(e => e.KTEMPLATEID)
                .WillCascadeOnDelete(false);
            map_PKS_KTEMPLATE.HasMany(e => e.PKS_KTEMPLATE_INSTANCE)
                .WithRequired(e => e.PKS_KTEMPLATE)
                .HasForeignKey(e => e.KTEMPLATEID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KTEMPLATE_CATALOGUE>()
                .HasMany(e => e.PKS_KFRAGMENT)
                .WithRequired(e => e.PKS_KTEMPLATE_CATALOGUE)
                .HasForeignKey(e => e.KTEMPLATECATALOGUEID)
                .WillCascadeOnDelete(false);

            var map_PKS_KTEMPLATE_CATEGORY = Entity<PKS_KTEMPLATE_CATEGORY>();
            map_PKS_KTEMPLATE_CATEGORY.HasMany(e => e.PKS_KTEMPLATE)
                .WithRequired(e => e.PKS_KTEMPLATE_CATEGORY)
                .HasForeignKey(e => e.KTEMPLATECATEGORYID)
                .WillCascadeOnDelete(false);
            map_PKS_KTEMPLATE_CATEGORY.HasMany(e => e.PKS_KTEMPLATE_CATEGORY_PARAMETER)
                .WithRequired(e => e.PKS_KTEMPLATE_CATEGORY)
                .HasForeignKey(e => e.KTEMPLATECATEGORYID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KTEMPLATE_PARAMETER>()
                .HasMany(e => e.PKS_KTEMPLATE_CATEGORY_PARAMETER)
                .WithRequired(e => e.PKS_KTEMPLATE_PARAMETER)
                .HasForeignKey(e => e.KTEMPLATEPARAMETERID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KTEMPLATE_URL>()
                .HasMany(e => e.PKS_KTEMPLATE_CATEGORY)
                .WithRequired(e => e.PKS_KTEMPLATE_URL)
                .HasForeignKey(e => e.KTEMPLATEURLID)
                .WillCascadeOnDelete(false);


            #endregion

            #region 石油百科

            Entity<PKS_OILWIKI_DOMAIN>()
               .HasMany(e => e.PKS_OILWIKI_CATALOG)
               .WithRequired(e => e.PKS_OILWIKI_DOMAIN)
               .HasForeignKey(e => e.DOMAINID)
               .WillCascadeOnDelete(false);

            Entity<PKS_OILWIKI_CATALOG>()
                .HasMany(e => e.PKS_OILWIKI_ENTRY)
                .WithRequired(e => e.PKS_OILWIKI_CATALOG)
                .HasForeignKey(e => e.CATALOGID)
                .WillCascadeOnDelete(false);

            var entry_map = Entity<PKS_OILWIKI_ENTRY>();
            entry_map.HasMany(e => e.PKS_OILWIKI_ALIASENTRY)
                .WithRequired(e => e.PKS_OILWIKI_ENTRY)
                .HasForeignKey(e => e.ENTRYID)
                .WillCascadeOnDelete(false);
            //entry_map.HasMany(e => e.PKS_OILWIKI_RELATEDENTRY)
            //    .WithRequired(e => e.PKS_OILWIKI_ENTRY)
            //    .HasForeignKey(e => e.ENTRYID)
            //    .WillCascadeOnDelete(false);
            entry_map.HasMany(e => e.PKS_OILWIKI_RELATEDENTRY)
               .WithRequired(e => e.PKS_OILWIKI_ENTRY1)
               .HasForeignKey(e => e.RELATEDENTRYID)
               .WillCascadeOnDelete(false);

            Entity<PKS_OILWIKI_ALIASENTRY>();

            Entity<PKS_OILWIKI_RELATEDENTRY>();

            #endregion

            #region 知识图谱
            Entity(new PKS_KG_PublicCatalogConfiguration(), true);
            Entity(new PKS_KG_PrivateCatalogConfiguration());
            Entity(new PKS_KG_TopicConfiguration());
            #endregion

            #region 知识案例

            var map_Case_Category = Entity<PKS_KCASE_CATEGORY>();
            map_Case_Category.HasMany(e => e.PKS_KCASE_CATEGORY1)
                .WithOptional(e => e.PKS_KCASE_CATEGORY2)
                .HasForeignKey(e => e.PARENTID);
            map_Case_Category.HasMany(e => e.PKS_KCASE_THEME)
                .WithRequired(e => e.PKS_KCASE_CATEGORY)
                .HasForeignKey(e => e.KCASECATEGORYID)
                .WillCascadeOnDelete(false);

            var map_Case_Instance = Entity<PKS_KCASE_INSTANCE>();
            map_Case_Instance.HasMany(e => e.PKS_KCASE_INSTANCE_CHART)
                .WithRequired(e => e.PKS_KCASE_INSTANCE)
                .HasForeignKey(e => e.KCASEINSTANCEID)
                .WillCascadeOnDelete(false);
            map_Case_Instance.HasMany(e => e.PKS_KCASE_INSTANCE_PARAMETER)
                .WithRequired(e => e.PKS_KCASE_INSTANCE)
                .HasForeignKey(e => e.KCASEINSTANCEID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KCASE_INSTANCE_CHART>();

            Entity<PKS_KCASE_INSTANCE_PARAMETER>();

            var map_Case_Parameter_Category = Entity<PKS_KCASE_PARAMETER_CATEGORY>();
            map_Case_Parameter_Category.HasMany(e => e.PKS_KCASE_PARAMETER_CATEGORY1)
                .WithOptional(e => e.PKS_KCASE_PARAMETER_CATEGORY2)
                .HasForeignKey(e => e.PARENTID);
            map_Case_Parameter_Category.HasMany(e => e.PKS_KCASE_THEME_PARAMETER)
                .WithRequired(e => e.PKS_KCASE_PARAMETER_CATEGORY)
                .HasForeignKey(e => e.KCASEPARAMETERCATEGORYID)
                .WillCascadeOnDelete(false);

            var map_Case_Theme = Entity<PKS_KCASE_THEME>();
            map_Case_Theme.HasMany(e => e.PKS_KCASE_INSTANCE)
                .WithRequired(e => e.PKS_KCASE_THEME)
                .HasForeignKey(e => e.KCASETHEMEID)
                .WillCascadeOnDelete(false);
            map_Case_Theme.HasMany(e => e.PKS_KCASE_PARAMETER_CATEGORY)
                .WithRequired(e => e.PKS_KCASE_THEME)
                .HasForeignKey(e => e.KCASETHEMEID)
                .WillCascadeOnDelete(false);
            map_Case_Theme.HasMany(e => e.PKS_KCASE_THEME_CHART)
                .WithRequired(e => e.PKS_KCASE_THEME)
                .HasForeignKey(e => e.KCASETHEMEID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KCASE_THEME_CHART>()
                .HasMany(e => e.PKS_KCASE_INSTANCE_CHART)
                .WithRequired(e => e.PKS_KCASE_THEME_CHART)
                .HasForeignKey(e => e.KCASETHEMECHARTID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KCASE_THEME_PARAMETER>()
                .HasMany(e => e.PKS_KCASE_INSTANCE_PARAMETER)
                .WithRequired(e => e.PKS_KCASE_THEME_PARAMETER)
                .HasForeignKey(e => e.KCASETHEMEPARAMETERID)
                .WillCascadeOnDelete(false);

            #endregion

            #region 标准规范

            Entity<PKS_STANDARD_EXTERNAL>();

            #endregion

            #region 勘探首页

            var map_Home_Module = Entity<PKS_KHOME_MODULE>();
            map_Home_Module.HasMany(e => e.PKS_KHOME_MODULE_QUERY)
                .WithRequired(e => e.PKS_KHOME_MODULE)
                .HasForeignKey(e => e.KHOMEMODULEID)
                .WillCascadeOnDelete(false);
            map_Home_Module.HasMany(e => e.PKS_KHOME_POST_MODULE)
                .WithRequired(e => e.PKS_KHOME_MODULE)
                .HasForeignKey(e => e.KHOMEMODULEID);

            Entity<PKS_KHOME_MODULE_CATEGORY>()
                .HasMany(e => e.PKS_KHOME_MODULE)
                .WithRequired(e => e.PKS_KHOME_MODULE_CATEGORY)
                .HasForeignKey(e => e.KHOMEMODULECATEGORYID)
                .WillCascadeOnDelete(false);

            Entity<PKS_KHOME_MODULE_QUERY>()
                .HasMany(e => e.PKS_KHOME_POST_MODULE_FILTER)
                .WithOptional(e => e.PKS_KHOME_MODULE_QUERY)
                .HasForeignKey(e => e.KHOMEMODULEQUERYID);


            var map_Home_Post_Module = Entity<PKS_KHOME_POST_MODULE>();
            map_Home_Post_Module.HasMany(e => e.PKS_KHOME_POST_MODULE_FILTER)
                .WithRequired(e => e.PKS_KHOME_POST_MODULE)
                .HasForeignKey(e => e.KHOMEPOSTMODULEID)
                .WillCascadeOnDelete(false);
            map_Home_Post_Module.HasMany(e => e.PKS_KHOME_USER_MODULE)
                .WithOptional(e => e.PKS_KHOME_POST_MODULE)
                .HasForeignKey(e => e.KHOMEPOSTMODULEID);

            Entity<PKS_KHOME_POST_MODULE_FILTER>();
            Entity<PKS_KHOME_USER_MODULE>();

            #endregion

            Entity<PKS_ROLE_MAP>().HasRequired(rm => rm.Role);
            Entity<PKS_PORTAL_EXTERN_LINK>(true);
            Entity<PKS_PORTAL_LINKEDIN_TEXT>(true);
        }
    }
}
