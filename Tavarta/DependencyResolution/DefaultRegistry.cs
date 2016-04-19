// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.IocConfig;

namespace Tavarta.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            //this.Forward<IViewEngine,RazorViewEngine>();

            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));

            //For<IViewEngine>().Use(() => new RazorViewEngine());


            For<IIdentity>().HybridHttpOrThreadLocalScoped()
                   .Use(
                       () =>
                           (HttpContext.Current != null && HttpContext.Current.User != null)
                               ? HttpContext.Current.User.Identity
                               : null);

            For<IUnitOfWork>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<ApplicationDbContext>();

            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            For<HttpServerUtilityBase>().Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
           For<HttpRequestBase>().Use(ctx => ctx.GetInstance<HttpContextBase>().Request);

           For<ISessionProvider>().Use<SessionProvider>();
            For<IRemotingFormatter>().Use(a => new BinaryFormatter());
            For<ITempDataProvider>().Use<CookieTempDataProvider>();

          

          
        }

        #endregion
    }
}