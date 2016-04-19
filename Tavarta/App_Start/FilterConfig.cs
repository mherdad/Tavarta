using System.Web.Mvc;
using Tavarta.Common.Filters;

namespace Tavarta
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

            //// logg action errors
            //filters.Add(new ElmahHandledErrorLoggerFilter());
            ////  logg xss attacks
            //filters.Add(new ElmahRequestValidationErrorFilter());
            
            filters.Add(new RemoveServerHeaderFilterAttribute());

            //filters.Add(new ForceWww("http://localhost:25890/"));
            
        }
    }
}
