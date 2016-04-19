using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Users;
using System.Data.Entity;

namespace Tavarta.DomainClasses.Configurations.Users
{
    /// <summary>
    /// نشان دهنده  مپینگ کلاس کاربر
    /// </summary>
    public class UserConfig : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        public UserConfig()
        {
            ToTable(nameof(Users));
            HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            Property(u => u.LastIp).IsOptional().HasMaxLength(20);
            Property(u => u.RowVersion).IsRowVersion();
            Property(u => u.DisplayName).IsRequired().HasMaxLength(50);
            Property(u => u.PhoneNumber).IsOptional().HasMaxLength(20);
            Property(u => u.DirectPermissions).HasColumnType("xml");
            Ignore(u => u.XmlDirectPermissions);
            Ignore(u => u.ConnectionIds);

            //.IndexingExtensions.HasIndex(User.,"ddfdhfghj",is
            //System.Data.Entity.IndexingExtensions.HasIndex("IX_Customers_Name", // Provide the index name.
            //        e => e.))
            //IndexingExtensions.HasIndex("sdsdsds",Property(e=>e.IsSystemAccount))
            Property(u => u.UserName)
                 .IsRequired()
                 .HasMaxLength(256)
                 .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_UserName") { IsUnique = true }));

            Property(u => u.Email)
                .IsOptional()
                .HasMaxLength(256);
        }
    }
}
