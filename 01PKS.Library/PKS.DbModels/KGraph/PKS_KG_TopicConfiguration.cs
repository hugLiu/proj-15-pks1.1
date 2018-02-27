#pragma warning disable 1591


namespace PKS.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>PKS_KG_PublicTopicConfiguration</summary>
    public class PKS_KG_TopicConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PKS_KG_Topic>
    {
        public PKS_KG_TopicConfiguration()
            : this("dbo")
        {
        }

        public PKS_KG_TopicConfiguration(string schema)
        {
            ToTable("PKS_KG_TOPICS", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PrivateCatalogId).HasColumnName(@"PRIVATECATALOGID").HasColumnType("int").IsOptional();
            Property(x => x.PublicCatalogId).HasColumnName(@"PUBLICCATALOGID").HasColumnType("int").IsOptional();
            Property(x => x.Title).HasColumnName(@"TITLE").IsUnicode().IsRequired().HasMaxLength(255);
            Property(x => x.LinkUrl).HasColumnName(@"LINKURL").IsUnicode().IsRequired().HasMaxLength(255);
            Property(x => x.Contents).HasColumnName(@"CONTENTS").IsUnicode().IsOptional().HasMaxLength(4000);
            Property(x => x.CreatedBy).HasColumnName(@"CREATEDBY").IsUnicode().IsRequired().HasMaxLength(50);
            Property(x => x.CreatedDate).HasColumnName(@"CREATEDDATE").HasColumnType("datetime").IsRequired();
            Property(x => x.LastUpdatedBy).HasColumnName(@"LASTUPDATEDBY").IsUnicode().IsOptional().HasMaxLength(50);
            Property(x => x.LastUpdatedDate).HasColumnName(@"LASTUPDATEDDATE").HasColumnType("datetime").IsOptional();

            // Foreign keys
            HasOptional(a => a.PrivateCatalog).WithMany(b => b.Topics).HasForeignKey(c => c.PrivateCatalogId).WillCascadeOnDelete(false);
            HasOptional(a => a.PublicCatalog).WithMany(b => b.Topics).HasForeignKey(c => c.PublicCatalogId).WillCascadeOnDelete(false);
        }
    }

}
