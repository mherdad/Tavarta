using System;
using System.Data.Entity;
using System.Web;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.EFServiecs.Common;
using Tavarta.ServiceLayer.EFServiecs.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap.Configuration.DSL;
using StructureMap.Web;

namespace Tavarta.IocConfig
{
    public class AspNetIdentityRegistery : Registry
    {
        public AspNetIdentityRegistery()
        {
            For<ApplicationDbContext>().HybridHttpOrThreadLocalScoped()
                               .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());
            For<DbContext>().HybridHttpOrThreadLocalScoped()
                 .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());

            For<IUserStore<User, Guid>>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>>();


            For<IRoleStore<Role, Guid>>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<RoleStore<Role, Guid, UserRole>>();
            
            For<IAuthenticationManager>()
                 .Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<IApplicationSignInManager>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<ApplicationSignInManager>();

            For<IApplicationRoleManager>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use<RoleManager>();

            For<IIdentityMessageService>().Use<SmsService>();
            For<IIdentityMessageService>().Use<EmailService>();

            For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped()
               .Use<ApplicationUserManager>()
               .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
               .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
               .Setter<IIdentityMessageService>(userManager => userManager.SmsService).Is<SmsService>()
               .Setter<IIdentityMessageService>(userManager => userManager.EmailService).Is<EmailService>();

            For<ApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                 .Use(context => (ApplicationUserManager)context.GetInstance<IApplicationUserManager>());

            For<ICustomRoleStore>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<CustomRoleStore>();

            For<ICustomUserStore>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<CustomUserStore>();
        }
    }

}
