using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using Tavarta.Common.Controller;
using Tavarta.Common.Fabrik.Others;
using Tavarta.Common.Sitemap;

namespace Tavarta.Controllers
{
    [Authorize]
    public class SiteController : BaseController
    {
        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

        public ActionResult AllowCookies(string ReturnUrl)
        {
            CookieConsent.SetCookieConsent(Response, true);
            return RedirectToAction(ReturnUrl);
        }

        public ActionResult NoCookies(string ReturnUrl)
        {
            CookieConsent.SetCookieConsent(Response, false);
            // if we got an ajax submit, just return 200 OK, else redirect back
            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            else
                return RedirectToAction(ReturnUrl);
        }


        [OutputCache(Duration = 60*60*24*365, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult FacebookChannel()
        {
            return View();
        }

        [OutputCache(Duration = 60*60*24, Location = System.Web.UI.OutputCacheLocation.Any)]
        public FileContentResult RobotsText()
        {
            var content = new StringBuilder("User-agent: *" + Environment.NewLine);

            if (string.Equals(ConfigurationManager.AppSettings["SiteStatus"], "live",
                StringComparison.InvariantCultureIgnoreCase))
            {
                content.Append("Disallow: ").Append("/Account" + Environment.NewLine);
                content.Append("Disallow: ").Append("/Error" + Environment.NewLine);
                content.Append("Disallow: ").Append("/signalr" + Environment.NewLine);

                // exclude content pages with NoSearch set to "true"
                //var items = Query(new GetSeoContentPages(noSearch: true));
                //foreach (var item in items)
                //{
                //    content.Append("Disallow: ")
                //        .Append(Url.Action("ContentPage", "Page", new {area = "", slug = item.Slug}))
                //        .Append(Environment.NewLine);
                //}
                content.Append("Sitemap: ")
                    .Append("https://")
                    .Append(ConfigurationManager.AppSettings["HostName"])
                    .Append("/sitemap.xml" + Environment.NewLine);

            }
            else
            {
                // disallow indexing for test and dev servers
                content.Append("Disallow: /" + Environment.NewLine);
            }


            return File(
                Encoding.UTF8.GetBytes(content.ToString()),
                "text/plain");
        }

        [NonAction]
        private IEnumerable<SitemapNode> GetSitemapNodes()
        {
            List<SitemapNode> nodes = new List<SitemapNode>();

            nodes.Add(new SitemapNode(this.ControllerContext.RequestContext,
                new {area = "", controller = "Home", action = "Index"})
            {
                Frequency = SitemapFrequency.Always,
                Priority = 0.8
            });

            //var items = Query(new GetSeoContentPages(false));
            //foreach (var item in items)
            //{
            //    nodes.Add(new SitemapNode(this.ControllerContext.RequestContext,
            //        new {area = "", controller = "Page", action = "ContentPage", id = item.Slug})
            //    {
            //        Frequency = SitemapFrequency.Yearly,
            //        Priority = 0.5,
            //        LastModified = item.Modified
            //    });
            //}

            return nodes;
        }

        [NonAction]
        private string GetSitemapXml()
        {
            XElement root;
            XNamespace xmlns = SitemapsNamespace;

            var nodes = GetSitemapNodes();

            root = new XElement(xmlns + "urlset");


            foreach (var node in nodes)
            {
                root.Add(
                    new XElement(xmlns + "url",
                        new XElement(xmlns + "loc", Uri.EscapeUriString(node.Url)),
                        node.Priority == null
                            ? null
                            : new XElement(xmlns + "priority",
                                node.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)),
                        node.LastModified == null
                            ? null
                            : new XElement(xmlns + "lastmod",
                                node.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                        node.Frequency == null
                            ? null
                            : new XElement(xmlns + "changefreq", node.Frequency.Value.ToString().ToLowerInvariant())
                        ));
            }

            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms, Encoding.UTF8))
                {
                    root.Save(writer);
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }


        [HttpGet]
        [OutputCache(Duration = 24*60*60, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult SitemapXml()
        {
            Trace.WriteLine("sitemap.xml was requested. User Agent: " + Request.Headers.Get("User-Agent"));

            var content = GetSitemapXml();
            return Content(content, "application/xml", Encoding.UTF8);
        }

        public ActionResult Google(string id)
        {
            if (ConfigurationManager.AppSettings["GoogleId"] == id)
                return View(model: id);
            else
                return new HttpNotFoundResult();
        }
    }


    public class SitemapNode
    {
        public string Url { get; set; }
        public DateTime? LastModified { get; set; }
        public SitemapFrequency? Frequency { get; set; }
        public double? Priority { get; set; }


        public SitemapNode(string url)
        {
            Url = url;
            Priority = null;
            Frequency = null;
            LastModified = null;
        }

        public SitemapNode(RequestContext request, object routeValues)
        {
            Url = GetUrl(request, new RouteValueDictionary(routeValues));
            Priority = null;
            Frequency = null;
            LastModified = null;
        }

        private string GetUrl(RequestContext request, RouteValueDictionary values)
        {
            var routes = RouteTable.Routes;
            var data = routes.GetVirtualPathForArea(request, values);

            if (data == null)
            {
                return null;
            }

            var baseUrl = request.HttpContext.Request.Url;
            var relativeUrl = data.VirtualPath;

            return request.HttpContext != null &&
                   (request.HttpContext.Request != null && baseUrl != null)
                ? new Uri(baseUrl, relativeUrl).AbsoluteUri
                : null;
        }



    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }


    /// <summary>
    /// ASP.NET MVC FilterAttribute for implementing european cookie-law
    /// </summary>
    public class CookieConsentAttribute : ActionFilterAttribute
    {
        public const string CONSENT_COOKIE_NAME = "CookieConsent";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            viewBag.AskCookieConsent = true;
            viewBag.HasCookieConsent = false;

            var request = filterContext.HttpContext.Request;

            // Check if the user has a consent cookie
            var consentCookie = request.Cookies[CONSENT_COOKIE_NAME];
            if (consentCookie == null)
            {
                // No consent cookie. We first check the Do Not Track header value, this can have the value "0" or "1"
                string dnt = request.Headers.Get("DNT");

                // If we receive a DNT header, we accept its value and do not ask the user anymore
                if (!String.IsNullOrEmpty(dnt))
                {
                    viewBag.AskCookieConsent = false;
                    if (dnt == "0")
                    {
                        viewBag.HasCookieConsent = true;
                    }
                }
                else
                {
                    if (IsSearchCrawler(request.Headers.Get("User-Agent")))
                    {
                        // don't ask consent from search engines, also don't set cookies
                        viewBag.AskCookieConsent = false;
                    }
                    else
                    {
                        // first request on the site and no DNT header. 
                        consentCookie = new HttpCookie(CONSENT_COOKIE_NAME);
                        consentCookie.Value = "asked";
                        filterContext.HttpContext.Response.Cookies.Add(consentCookie);
                    }
                }
            }
            else
            {
                // we received a consent cookie
                viewBag.AskCookieConsent = false;
                if (consentCookie.Value == "asked")
                {
                    // consent is implicitly given
                    consentCookie.Value = "true";
                    consentCookie.Expires = DateTime.UtcNow.AddYears(1);
                    filterContext.HttpContext.Response.Cookies.Set(consentCookie);
                    viewBag.HasCookieConsent = true;
                }
                else if (consentCookie.Value == "true")
                {
                    viewBag.HasCookieConsent = true;
                }
                else
                {
                    // assume consent denied
                    viewBag.HasCookieConsent = false;
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private bool IsSearchCrawler(string userAgent)
        {
            if (!userAgent.IsNullOrEmpty())
            {
                string[] crawlers = new string[]
                {
                    "Baiduspider",
                    "Googlebot",
                    "YandexBot",
                    "YandexImages",
                    "bingbot",
                    "msnbot",
                    "Vagabondo",
                    "SeznamBot",
                    "ia_archiver",
                    "AcoonBot",
                    "Yahoo! Slurp",
                    "AhrefsBot"
                };
                foreach (string crawler in crawlers)
                    if (userAgent.Contains(crawler))
                        return true;
            }
            return false;
        }
    }


    /// <summary>
    /// Helper class for easy/typesafe getting the cookie consent status
    /// </summary>
    public static class CookieConsent
    {
        public static void SetCookieConsent(HttpResponseBase response, bool consent)
        {
            var consentCookie = new HttpCookie(CookieConsentAttribute.CONSENT_COOKIE_NAME);
            consentCookie.Value = consent ? "true" : "false";
            consentCookie.Expires = DateTime.UtcNow.AddYears(1);
            response.Cookies.Set(consentCookie);
        }

        public static bool AskCookieConsent(ViewContext context)
        {
            return context.ViewBag.AskCookieConsent ?? false;
        }

        public static bool HasCookieConsent(ViewContext context)
        {
            return context.ViewBag.HasCookieConsent ?? false;
        }
    }


    /// <summary>
    /// allow to disable implicit consent after 1st popup
    /// using HttpContext.Current.Item and private class instead of ViewBag
    /// simpler methods (do not need Response) - and changed to property
    /// </summary>

    public class CookieConsentFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Some government relax their interpretation of the law somewhat:
        /// After the first page with the message, clicking anything other than the cookie refusal link may be interpreted as implicitly allowing cookies. 
        /// </summary>
        public bool ImplicitlyAllowCookies { get; set; }

        public CookieConsentFilter()
        {
            ImplicitlyAllowCookies = true;
        }

        private const string cookieConsentCookieName = "cookieConsent";
        private const string cookieConsentContextName = "cookieConsentInfo";

        private class CookieConsentInfo
        {
            public bool NeedToAskConsent { get; set; }
            public bool HasConsent { get; set; }

            public CookieConsentInfo()
            {
                NeedToAskConsent = true;
                HasConsent = false;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var consentInfo = new CookieConsentInfo();

            var request = filterContext.HttpContext.Request;

            // Check if the user has a consent cookie
            var consentCookie = request.Cookies[cookieConsentCookieName];

            if (consentCookie == null)
            {
                // No consent cookie. We first check the Do Not Track header value, this can have the value "0" or "1"
                string dnt = request.Headers.Get("DNT");

                // If we receive a DNT header, we accept its value (0 = give consent, 1 = deny) and do not ask the user anymore...
                if (!String.IsNullOrEmpty(dnt))
                {
                    consentInfo.NeedToAskConsent = false;

                    if (dnt == "0")
                    {
                        consentInfo.HasConsent = true;
                    }
                }
                else
                {
                    if (IsSearchCrawler(request.Headers.Get("User-Agent")))
                    {
                        // don't ask consent from search engines, also don't set cookies
                        consentInfo.NeedToAskConsent = false;
                    }
                    else
                    {
                        // first request on the site and no DNT header (we use session cookie, which is allowed by EU cookie law). 
                        consentCookie = new HttpCookie(cookieConsentCookieName);
                        consentCookie.Value = "asked";

                        filterContext.HttpContext.Response.Cookies.Add(consentCookie);
                    }
                }
            }
            else
            {
                // we received a consent cookie
                consentInfo.NeedToAskConsent = false;

                if (ImplicitlyAllowCookies && consentCookie.Value == "asked")
                {
                    // consent is implicitly given & stored
                    consentCookie.Value = "true";
                    consentCookie.Expires = DateTime.UtcNow.AddYears(1);
                    filterContext.HttpContext.Response.Cookies.Set(consentCookie);

                    consentInfo.HasConsent = true;
                }
                else if (consentCookie.Value == "true")
                {
                    consentInfo.HasConsent = true;
                }
                else
                {
                    // assume consent denied
                    consentInfo.HasConsent = false;
                }
            }

            HttpContext.Current.Items[cookieConsentContextName] = consentInfo;

            base.OnActionExecuting(filterContext);
        }

        private bool IsSearchCrawler(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                string[] crawlers = new string[]
                {
                    "Baiduspider",
                    "Googlebot",
                    "YandexBot",
                    "YandexImages",
                    "bingbot",
                    "msnbot",
                    "Vagabondo",
                    "SeznamBot",
                    "ia_archiver",
                    "AcoonBot",
                    "Yahoo! Slurp",
                    "AhrefsBot"
                };
                foreach (string crawler in crawlers)
                    if (userAgent.Contains(crawler))
                        return true;
            }
            return false;
        }

        public static void SetCookieConsent(bool consent)
        {
            var consentCookie = new HttpCookie(CookieConsentFilter.cookieConsentCookieName);
            consentCookie.Value = consent ? "true" : "false";
            consentCookie.Expires = DateTime.UtcNow.AddYears(1);
            HttpContext.Current.Response.Cookies.Set(consentCookie);
        }

        public static bool NeedToAskCookieConsent
        {
            get
            {
                return
                    (HttpContext.Current.Items[cookieConsentContextName] as CookieConsentInfo ?? new CookieConsentInfo())
                        .NeedToAskConsent;
            }
        }

        public static bool HasConsent
        {
            get
            {
                return
                    (HttpContext.Current.Items[cookieConsentContextName] as CookieConsentInfo ?? new CookieConsentInfo())
                        .HasConsent;
            }
        }



    }


  

 



}