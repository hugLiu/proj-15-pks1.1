using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Diagnostics;
using PKS.Data;
using System.Collections.Generic;

namespace PKS.Core.Template
{
    /// <summary>�Զ������ݷ���ʵ��ӳ������</summary>
    public class CustomDbEntityMappingConfiguration : IDbEntityMappingConfiguration
    {
        /// <summary>ӳ��ʵ�弯��</summary>
        public List<object> MapTypes { get; private set; }
        /// <summary>�仯����ʵ�弯��</summary>
        public List<Type> ChangePublishTypes { get; private set; }

        /// <summary>��ʼ��ʵ��ӳ��</summary>
        public virtual void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<GT_Settings>();
            //modelBuilder.Entity<UserProfile>()
            //     .Property(e => e.Email)
            //     .IsUnicode(false);
            // modelBuilder.Entity<webpages_Roles>()
            //     .HasMany(e => e.UserProfile)
            //     .WithMany(e => e.webpages_Roles)
            //     .Map(m => m.ToTable("webpages_UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));
            //modelBuilder.Entity<CRE_BOLayer>()
            //    .HasRequired(e => e.Role)
            //    .WithMany(e => e.);
            //modelBuilder.Entity<CRE_A2Services>();
            //modelBuilder.Entity<CRE_BOFields>();
            throw new NotImplementedException();
        }
    }
}
