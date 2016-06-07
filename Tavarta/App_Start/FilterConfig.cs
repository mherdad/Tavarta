using System.IO;
using System.IO.Compression;
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
            filters.Add(new PageViewAttribute());

           // filters.Add(new ForceWww("http://localhost:4269/"));
            filters.Add(new CompressFilter());
            
            filters.Add(new WhitespaceFilterAttribute());
            //filters.Add(new GZipFilter(new GZipStream(new BufferedStream(Stream.Null), CompressionMode.Compress )));
           // filters.Add(new ETagAttribute());
           // filters.Add(new ContentSecurityPolicyFilterAttribute());
          

        }
    }
}
