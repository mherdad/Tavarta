using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Tavarta.Common
{
    public class TwoLevelViewCache : IViewLocationCache
    {
        private static readonly object SKey = new object();
        private readonly IViewLocationCache _cache;
        public TwoLevelViewCache(IViewLocationCache cache)
        {
            _cache = cache;
        }
        private static IDictionary<string, string> GetRequestCache(HttpContextBase httpContext)
        {
            var d = httpContext.Items[SKey] as IDictionary<string, string>;
            if (d != null) return d;
            d = new Dictionary<string, string>();
            httpContext.Items[SKey] = d;
            return d;
        }
        public string GetViewLocation(HttpContextBase httpContext, string key)
        {
            var d = GetRequestCache(httpContext);
            string location;
            if (d.TryGetValue(key, out location)) return location;
            location = _cache.GetViewLocation(httpContext, key);
            d[key] = location;
            return location;
        }
        public void InsertViewLocation(HttpContextBase httpContext, string key, string virtualPath)
        {
            _cache.InsertViewLocation(httpContext, key, virtualPath);
        }
    }
}