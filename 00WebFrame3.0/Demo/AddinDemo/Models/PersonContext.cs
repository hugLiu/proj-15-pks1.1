namespace AddinDemo.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Jurassic.CommonModels.EFProvider;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class PersonContext : ModelContext
    {
        public virtual DbSet<EduHistory> EduHistory { get; set; }
        public virtual DbSet<Honor> Honor { get; set; }
        public virtual DbSet<Person> Person { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>()
                .HasMany(e => e.EduHistorys)
                .WithRequired(e => e.Master)
                .HasForeignKey(e => e.MasterId)
                
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>().ToTable("Supplier");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Stock>().ToTable("Stock");

        }
    }
}
