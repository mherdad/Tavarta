using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using StructureMap;
using StructureMap.Web;

using Tavarta.IocConfig;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.EFServiecs.Users;


namespace Tavarta
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }

        private static void ConfigureAuth(IAppBuilder appBuilder)
        {
            const int twoWeeks = 14;
            const int oneWeeks = 7;

            ProjectObjectFactory.Container.Configure(config => config.For<IDataProtectionProvider>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => appBuilder.GetDataProtectionProvider()));
            
           appBuilder.CreatePerOwinContext(
                () => ProjectObjectFactory.Container.GetInstance<ApplicationUserManager>());

            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromDays(oneWeeks),
                SlidingExpiration = true,
                CookieName = "tavarta",
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity =
                           ProjectObjectFactory.Container.GetInstance<IApplicationUserManager>().OnValidateIdentity()
                }
            });

            ProjectObjectFactory.Container.GetInstance<IApplicationRoleManager>()
           .SeedDatabase();

            ProjectObjectFactory.Container.GetInstance<IApplicationUserManager>()
               .SeedDatabase();

        }
    }
}
