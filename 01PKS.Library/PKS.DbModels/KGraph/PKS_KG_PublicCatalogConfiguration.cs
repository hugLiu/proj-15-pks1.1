#pragma warning disable 1591


namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    /// <summary>PKS_KG_PublicCatalogConfiguration</summary>
    public class PKS_KG_PublicCatalogConfiguration : EntityTypeConfiguration<PKS_KG_PublicCatalog>
    {
        public PKS_KG_PublicCatalogConfiguration()
            : this("dbo")
        {
        }

        public PKS_KG_PublicCatalogConfiguration(string schema)
        {
            ToTable("PKS_KGPUBLIC_CATALOG", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName(@"CODE").IsUnicode().IsRequired().HasMaxLength(100);
            Property(x => x.Name).HasColumnName(@"NAME").IsUnicode().IsRequired().HasMaxLength(100);
            Property(x => x.Description).HasColumnName(@"DESCRIPTION").IsUnicode().IsOptional().HasMaxLength(255);
            Property(x => x.LevelNumber).HasColumnName(@"LEVELNUMBER").HasColumnType("int").IsRequired();
            Property(x => x.OrderNumber).HasColumnName(@"ORDERNUMBER").HasColumnType("int").IsRequired();
            Property(x => x.ParentId).HasColumnName(@"PARENTID").HasColumnType("int").IsOptional();
            Property(x => x.CreatedBy).HasColumnName(@"CREATEDBY").IsUnicode().IsOptional().HasMaxLength(50);
            Property(x => x.CreatedDate).HasColumnName(@"CREATEDDATE").HasColumnType("datetime").IsOptional();
            Property(x => x.LastUpdatedBy).HasColumnName(@"LASTUPDATEDBY").IsUnicode().IsOptional().HasMaxLength(50);
            Property(x => x.LastUpdatedDate).HasColumnName(@"LASTUPDATEDDATE").HasColumnType("datetime").IsOptional();
            Property(x => x.ImageURL).HasColumnName(@"IMAGEURL").IsUnicode().IsOptional().HasMaxLength(255);

            // Foreign keys
            HasOptional(a => a.Parent).WithMany(b => b.Children).HasForeignKey(c => c.ParentId).WillCascadeOnDelete(false);
        }
    }

}
