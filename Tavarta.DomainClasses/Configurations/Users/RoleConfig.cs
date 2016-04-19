using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Users;

namespace Tavarta.DomainClasses.Configurations.Users
{
    /// <summary>
    /// نشان دهنده مپینگ کلاس گروه کاربری
    /// </summary>
    public class RoleConfig : EntityTypeConfiguration<Role>
    {
        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        public RoleConfig()
        {
            ToTable("Roles");
            Property(r => r.RowVersion).IsRowVersion();
            Ignore(r => r.XmlPermissions);
            Property(r => r.Name)
                 .IsRequired()
                 .HasMaxLength(50)
                 .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_RoleName",1) { IsUnique = true }));
            //Property(e=>e.Permissions)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_RoleName",2) { IsUnique = true }));
            //Property(e => e.RowVersion)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_RoleName", 3) { IsUnique = true }));
            //Property(e => e.IsBanned)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_RoleName", 4) { IsUnique = true }));
            //Property(e => e.IsSystemRole)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_RoleName", 5) { IsUnique = true }));

            Property(r => r.RowVersion).IsRowVersion();
            Property(r => r.Permissions).HasColumnType("xml");
            HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }
    }
}
