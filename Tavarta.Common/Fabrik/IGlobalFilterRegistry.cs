using System.Web.Mvc;

namespace Tavarta.Common.Fabrik
{
    public interface IGlobalFilterRegistry
    {
        void RegisterGlobalFilters(GlobalFilterCollection filters);
    }
}
