using System.Web.Routing;

namespace Tavarta.Common.Fabrik
{
    public interface IRouteRegistry
    {
        void RegisterRoutes(RouteCollection routes);
    }
}
