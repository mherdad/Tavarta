using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Tavarta.Common.Filters
{
    public class PageViewAttribute : ActionFilterAttribute
    {
        private static readonly TimeSpan PageViewDumpToDatabaseTimeSpan = new TimeSpan(0, 0, 10);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var calledMethod =
                $"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName} -> {filterContext.ActionDescriptor.ActionName}";

            var cacheKey = $"PV-{calledMethod}";

            var cachedResult = HttpRuntime.Cache[cacheKey];

            if (cachedResult == null)
            {
                HttpRuntime.Cache.Insert(cacheKey, new PageViewValue(), null, DateTime.Now.Add(PageViewDumpToDatabaseTimeSpan), Cache.NoSlidingExpiration, CacheItemPriority.Default,
                                      OnRemove);
            }
            else
            {
                var currentValue = (PageViewValue)cachedResult;

                currentValue.Value++;
            }
        }

        private static void OnRemove(string key, object value, CacheItemRemovedReason reason)
        {
            if (!key.StartsWith("PV-"))
            {
                return;
            }

            // write out the value to the database
        }

        // Used to get around weird cache behavior with value types
        public class PageViewValue
        {
            public PageViewValue()
            {
                Value = 1;
            }

            public int Value { get; set; }
        }

    }
}