using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ServiceLayer.Contracts.Users;

namespace Tavarta.ServiceLayer.EFServiecs.Users
{
    public class CustomUserStore : UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>, ICustomUserStore
    {
        public CustomUserStore(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

    }
}
