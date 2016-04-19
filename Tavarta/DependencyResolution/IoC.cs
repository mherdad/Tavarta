// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
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


using System;
using System.Threading;
using System.Web;
using StructureMap;
using Tavarta.IocConfig;

namespace Tavarta.DependencyResolution {
    public static class IoC {

        private static readonly Lazy<Container> _containerBuilder =
           new Lazy<Container>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }
        
            
            
            
        public static Container Initialize()
        {
            var container = new Container(ioc =>
            {
                ioc.AddRegistry<AspNetIdentityRegistery>();
                ioc.AddRegistry<TaskRegistry>();
                ioc.AddRegistry<AutoMapperRegistery>();
                ioc.AddRegistry<ServiceLayerRegistery>();
                ioc.Scan(scanner => scanner.WithDefaultConventions());
                ioc.Policies.SetAllProperties(y => y.OfType<HttpContextBase>());
            });
            return container;
            
           
        }
    }
}