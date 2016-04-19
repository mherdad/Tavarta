using StructureMap;
using StructureMap.Configuration.DSL;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.EFServiecs.Users;


namespace Tavarta.IocConfig
{
    public class ServiceLayerRegistery : Registry
    {
        public ServiceLayerRegistery()
        {
            Policies.SetAllProperties(y =>
            {
                y.OfType<IActivityLogService>();
            });
            Scan(scanner =>
            {
                scanner.WithDefaultConventions();
                scanner.AssemblyContainingType<ApplicationUserManager>();

            });
        }
    }
}
