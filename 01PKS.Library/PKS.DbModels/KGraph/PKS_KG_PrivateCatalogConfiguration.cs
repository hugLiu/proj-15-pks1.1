#pragma warning disable 1591


namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>PKS_KGPRIVATE_CATALOG</summary>
    public class PKS_KG_PrivateCatalogConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PKS_KG_PrivateCatalog>
    {
        public PKS_KG_PrivateCatalogConfiguration()
            : this("dbo")
        {
        }

        public PKS_KG_PrivateCatalogConfiguration(string schema)
        {
            ToTable("PKS_KGPRIVATE_CATALOG", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
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
            HasOptional(a => a.Parent).WithMany(b => b.Children).HasForeignKey(c => c.ParentId).WillCascadeOnDelete(false); // FK_PKS_KGPRIVATE_CATALOG_PKS_KGPRIVATE_CATALOG
        }
    }

}
