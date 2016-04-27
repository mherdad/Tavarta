using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StructureMap;
using StructureMap.Web.Pipeline;

using Tavarta.IocConfig;
using Tavarta.Scheduler;
using Tavarta.ServiceLayer.Contracts.Common;

namespace Tavarta
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //public IContainer Container
        //{
        //    get { return (IContainer) HttpContext.Current.Items["_Container"]; }
        //    set { HttpContext.Current.Items["_Container"] = value; }
        //}

        #region Application_Start
        protected void Application_Start()
        {
            //ScheduledTasksRegistry.Init();
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            try
            {
                AreaRegistration.RegisterAllAreas();
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                RoutingConfig.RegisterRoutes(RouteTable.Routes);
            
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ApplicationStart.Config();
                

            }
            catch
            {
                HttpRuntime.UnloadAppDomain(); // سبب ری استارت برنامه و آغاز مجدد آن با درخواست بعدی می‌شود
                throw;
            }

        }
        #endregion

        protected void Application_End()
        {
            ScheduledTasksRegistry.End();
            // This method needs a ping service to keep it alive.
            ScheduledTasksRegistry.WakeUp(ConfigurationManager.AppSettings["SiteRootUrl"]);
        }

        #region Application_EndRequest
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {


                foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunAfterEachRequest>())
                {
                    task.Execute();
                }
                HttpContextLifecycle.DisposeAndClearAll();
            }
            catch (Exception)
            {
                HttpContextLifecycle.DisposeAndClearAll();
            }

        }
        #endregion

        #region Application_BeginRequest
        private void Application_BeginRequest(object sender, EventArgs e)
        {
            //Container = ObjectFactory.Container.GetNestedContainer();
            foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }

        }
        #endregion

        #region ShouldIgnoreRequest
        private bool ShouldIgnoreRequest()
        {
            string[] reservedPath =
              {
                    "/__browserLink",
                "/img",
                "/fonts",
                "/Scripts",
                "/Content",
                "/Uploads",
                "/Images"
              };

            var rawUrl = Context.Request.RawUrl;
            if (reservedPath.Any(path => rawUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~'))
                      .Any(bundlePath => rawUrl.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Private
        private void SetPermissions(IEnumerable<string> permissions)
        {
            Context.User =
                new GenericPrincipal(Context.User.Identity, permissions.ToArray());
        }
        #endregion


        //protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        //{
        //    if (ShouldIgnoreRequest()) return;

        //    var context = DependencyResolver.Current.GetService<HttpContextBase>();

        //    var principalService = IoC.Container.GetInstance<context>();

        //    // Set the HttpContext's User to our IPrincipal
        //    context.User = principalService.GetCurrent();
        //}

        //protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        //{
        //    var app = sender as HttpApplication;
        //    if (app == null || app.Context == null)
        //        return;
        //    app.Context.Response.Headers.Remove("Server");
        //}

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (ShouldIgnoreRequest()) return;

            if (Context.User == null)
                return;
        }

        #region Application_Error
        protected void Application_Error()
        {
            foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunOnError>())
            {
                task.Execute();
            }
        }
        #endregion

    }
}

