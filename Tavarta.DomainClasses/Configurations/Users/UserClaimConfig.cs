using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Users;

namespace Tavarta.DomainClasses.Configurations.Users
{
    /// <summary>
    /// نشان دهنده مپینگ کلاس ادعا های کاربر
    /// </summary>
    public class UserClaimConfig : EntityTypeConfiguration<UserClaim>
    {
        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        public UserClaimConfig()
        {
            ToTable("UserClaims");
        }
    }
}
