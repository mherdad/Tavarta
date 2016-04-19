using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.EFServiecs.Users;

namespace Tavarta.ServiceLayer.EFServiecs.Common
{
    public class ApplicationSignInManager : SignInManager<User, Guid>, IApplicationSignInManager
    {

        #region Fields
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        #endregion

        #region Constructor

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }
        #endregion

       
    }
}
