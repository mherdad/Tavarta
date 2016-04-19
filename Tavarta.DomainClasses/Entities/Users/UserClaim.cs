using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Tavarta.DomainClasses.Entities.Users
{
    /// <summary>
    /// ادعای های کاربر سیستم مبنی بر دسترسی ها
    /// </summary>
    public class UserClaim : IdentityUserClaim<Guid>
    {
    }
}
