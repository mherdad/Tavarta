using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Users;

namespace Tavarta.DomainClasses.Configurations.Users
{
    /// <summary>
    /// 
    /// </summary>
   public class UserLoginConfig:EntityTypeConfiguration<UserLogin>
   {
       /// <summary>
       /// 
       /// </summary>
       public UserLoginConfig()
       {
           HasKey(l => new {l.LoginProvider, l.ProviderKey, l.UserId});
               ToTable("UserLogins");

       }
    }
}
