using System;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap;
using StructureMap.Web;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts.Common;
using Tavarta.ServiceLayer.Contracts.Schedulers;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.EFServiecs.Common;
using Tavarta.ServiceLayer.EFServiecs.Schedulers;
using Tavarta.ServiceLayer.EFServiecs.Users;

namespace Tavarta.IocConfig
{
    public static class ProjectObjectFactory
    {
        //#region Fields
        //private static readonly Lazy<Container> ContainerBuilder =
        //  new Lazy<Container>(DefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        //#endregion

        //#region Container
        //public static IContainer Container => ContainerBuilder.Value;

        //#endregion

        //#region DefaultContainer
        //private static Container DefaultContainer()
        //{
        //    var container = new Container(ioc =>
        //     {
        //         ioc.For<Microsoft.AspNet.SignalR.IDependencyResolver>()
        //          .Singleton()
        //          .Add<StructureMapSignalRDependencyResolver>();

        //         ioc.For<IIdentity>().HybridHttpOrThreadLocalScoped()
        //             .Use(
        //                 () =>
        //                     (HttpContext.Current != null && HttpContext.Current.User != null)
        //                         ? HttpContext.Current.User.Identity
        //                         : null);

        //         ioc.For<IUnitOfWork>()
        //               .HybridHttpOrThreadLocalScoped()
        //               .Use<ApplicationDbContext>();

        //         ioc.For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
        //         ioc.For<HttpServerUtilityBase>().Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
        //         ioc.For<HttpRequestBase>().Use(ctx => ctx.GetInstance<HttpContextBase>().Request);

        //         ioc.For<ISessionProvider>().Use<SessionProvider>();
        //         ioc.For<IRemotingFormatter>().Use(a => new BinaryFormatter());
        //       ioc.For<ITempDataProvider>().Use<CookieTempDataProvider>();

        //         ioc.AddRegistry<AspNetIdentityRegistery>();
        //         ioc.AddRegistry<TaskRegistry>();
        //         ioc.AddRegistry<AutoMapperRegistery>();
        //         ioc.AddRegistry<ServiceLayerRegistery>();

        //         ioc.Scan(scanner => scanner.WithDefaultConventions());
        //         ioc.Policies.SetAllProperties(y => y.OfType<HttpContextBase>());

        //     });

        //    //using (var containerLocal = ObjectFactory.Container.GetNestedContainer())
        //    //{
        //    //    foreach (var task in containerLocal.GetAllInstances<IRunAtInit>())
        //    //    {
        //    //        task.Execute();
        //    //    }

        //    //    foreach (var task in container.GetAllInstances<IRunAtStartUp>())
        //    //    {
        //    //        task.Execute();
        //    //    }
        //    //}

        //    ConfigureAutoMapper(container);
        //    return container;
        //}
        //#endregion

        private static readonly Lazy<Container> _containerBuilder =
       new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            var container= new Container(ioc =>
            {
                ioc.For<Microsoft.AspNet.SignalR.IDependencyResolver>()
                    .Singleton()
                    .Add<StructureMapSignalRDependencyResolver>();

                ioc.For<IIdentity>()
                    .Use(
                        () =>
                            (HttpContext.Current != null && HttpContext.Current.User != null)
                                ? HttpContext.Current.User.Identity
                                : null);

                ioc.For<IUnitOfWork>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<ApplicationDbContext>();
                // Remove these 2 lines if you want to use a connection string named connectionString1, defined in the web.config file.
                //.Ctor<string>("connectionString")
                //.Is("Data Source=(local);Initial Catalog=TestDbIdentity;Integrated Security = true");

                ioc.For<ApplicationDbContext>().HybridHttpOrThreadLocalScoped()
                    .Use(context => (ApplicationDbContext) context.GetInstance<IUnitOfWork>());
                ioc.For<DbContext>().HybridHttpOrThreadLocalScoped()
                    .Use(context => (ApplicationDbContext) context.GetInstance<IUnitOfWork>());

                //ioc.For<IUserStore<ApplicationUser, int>>()
                //    .HybridHttpOrThreadLocalScoped()
                //    .Use<CustomUserStore>();

                //ioc.For<IRoleStore<CustomRole, int>>()
                //    .HybridHttpOrThreadLocalScoped()
                //    .Use<RoleStore<CustomRole, int, CustomUserRole>>();

                ioc.For<IAuthenticationManager>()
                    .Use(() => HttpContext.Current.GetOwinContext().Authentication);

                ioc.For<IApplicationSignInManager>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<ApplicationSignInManager>();

                ioc.For<IApplicationRoleManager>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<RoleManager>();

                // map same interface to different concrete classes
                ioc.For<IIdentityMessageService>().Use<SmsService>();
                ioc.For<IIdentityMessageService>().Use<EmailService>();

                ioc.For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                    .Use<ApplicationUserManager>()
                    .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
                    .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
                    .Setter<IIdentityMessageService>(userManager => userManager.SmsService).Is<SmsService>()
                    .Setter<IIdentityMessageService>(userManager => userManager.EmailService).Is<EmailService>();

                ioc.For<ApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                    .Use(context => (ApplicationUserManager) context.GetInstance<IApplicationUserManager>());

                ioc.For<ICustomRoleStore>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<CustomRoleStore>();


                ioc.For<ICustomUserStore>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<CustomUserStore>();

                

                ioc.For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
                ioc.For<HttpServerUtilityBase>().Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
                ioc.For<HttpRequestBase>().Use(ctx => ctx.GetInstance<HttpContextBase>().Request);

                ioc.For<ISessionProvider>().Use<SessionProvider>();
                ioc.For<IRemotingFormatter>().Use(a => new BinaryFormatter());
                ioc.For<ITempDataProvider>().Use<CookieTempDataProvider>();

                ioc.AddRegistry<AspNetIdentityRegistery>();
                ioc.AddRegistry<TaskRegistry>();
                ioc.AddRegistry<AutoMapperRegistery>();
                ioc.AddRegistry<ServiceLayerRegistery>();

                ioc.Scan(scanner => scanner.WithDefaultConventions());
                ioc.Policies.SetAllProperties(y => y.OfType<HttpContextBase>());
                
                //config.For<IDataProtectionProvider>().Use(()=> app.GetDataProtectionProvider()); // In Startup class

                //ioc.For<ICategoryService>().Use<EfCategoryService>();
                //ioc.For<IProductService>().Use<EfProductService>();
            });
            ConfigureAutoMapper(container);
            return container;
        }

        #region ConfigureAutoMapper
        private static void ConfigureAutoMapper(IContainer container)
        {
            var configuration = container.TryGetInstance<IConfiguration>();
            if (configuration == null) return;
            //saying AutoMapper how to resolve services
            configuration.ConstructServicesUsing(container.GetInstance);
            foreach (var profile in container.GetAllInstances<Profile>())
            {
                configuration.AddProfile(profile);
            }
        }
        #endregion
    }
}
