using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Tavarta.Common.Filters
{
    //http://stackoverflow.com/questions/6055415/adding-id-and-title-to-url-slugs-in-asp-net-mvc
    public class SeoFriendlyRoute : Route
    {
        public SeoFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("id"))
                    routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
            }

            return routeData;
        }

        private object GetIdValue(object id)
        {
            if (id != null)
            {
                string idValue = id.ToString();

                var regex = new Regex(@"^(?<id>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["id"].Value;
                }
            }

            return id;
        }
    }
}