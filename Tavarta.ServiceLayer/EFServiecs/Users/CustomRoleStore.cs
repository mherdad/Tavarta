using System;
using Microsoft.AspNet.Identity;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ServiceLayer.Contracts.Users;

namespace Tavarta.ServiceLayer.EFServiecs.Users
{
    public class CustomRoleStore : ICustomRoleStore
    {
        private readonly IRoleStore<Role, Guid> _roleStore;

        public CustomRoleStore(IRoleStore<Role, Guid> roleStore)
        {
            _roleStore = roleStore;
        }
    }
}
