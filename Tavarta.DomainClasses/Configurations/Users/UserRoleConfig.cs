using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Users;

namespace Tavarta.DomainClasses.Configurations.Users
{
    /// <summary>
    /// نشان دهنده مپینک کلاس کاربر-گروه کاربری
    /// </summary>
    public class UserRoleConfig : EntityTypeConfiguration<UserRole>
    {
        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        public UserRoleConfig()
        {
            HasKey(r => new { r.UserId, r.RoleId });
            ToTable(nameof(UserRole));
        }
    }
}
