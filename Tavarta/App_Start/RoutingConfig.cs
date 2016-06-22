using System.Web.Mvc;
using System.Web.Routing;

namespace Tavarta
{
    public static class RoutingConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            #region IgnoreRoutes

            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.ico");
            routes.IgnoreRoute("{resource}.png");
            routes.IgnoreRoute("{resource}.jpg");
            routes.IgnoreRoute("{resource}.gif");
            routes.IgnoreRoute("{resource}.txt");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            #endregion IgnoreRoutes

            routes.LowercaseUrls = true;
            routes.AppendTrailingSlash = true;
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "newsD",
                "news/{id}/{title}",
                new {controller = "news", action = "Details", id = "", productName = ""},
                new[] {$"{typeof(RoutingConfig).Namespace}.Controllers"}
                );


            // AreaRegistration.RegisterAllAreas();


            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                {
                    controller = "news",
                    action = "index",
                    //controller = MVC.Home.Name,
                    //action = MVC.Home.ActionNames.Index,
                    id = UrlParameter.Optional
                },
                new[] {$"{typeof(RoutingConfig).Namespace}.Controllers"}
                );

            routes.MapRoute(
                "GetSitemapXml", // Route name
                "sitemap.xml", // URL with parameters
                new {controller = "Sitemap", action = "index", name = UrlParameter.Optional, area = ""}
                // Parameter defaults
                );

            routes.MapRoute(
                "GetRobots", // Route name
                "robots.txt", // URL with parameters
                new {controller = "Sitemap", action = "RobotsText", name = UrlParameter.Optional, area = ""}
                // Parameter defaults
                );

            routes.MapRoute(
                "Error500", // Route name
                "error/500", // URL with parameters
                new {controller = "Error", action = "error", name = UrlParameter.Optional, area = ""}
                // Parameter defaults
                );

            routes.MapRoute(
                "Error400", // Route name
                "error/404", // URL with parameters
                new {controller = "Error", action = "NotFound", name = UrlParameter.Optional, area = ""}
                // Parameter defaults
                );

           
        }
    }
}