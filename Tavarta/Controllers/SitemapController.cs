using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tavarta.Common.Fabrik;
using Tavarta.Common.Fabrik.ActionResults;
using Tavarta.Common.Fabrik.Others;
using Tavarta.Common.Fabrik.SEO;

namespace Tavarta.Controllers
{
    public class SitemapController : Controller
    {
       
        public ActionResult Index()
        {
            var sitemapItems = new List<SitemapItem>
            {
                new SitemapItem(Url.QualifiedAction("index", "news"), changeFrequency: SitemapChangeFrequency.Always,
                    priority: 1.0),
                new SitemapItem(Url.QualifiedAction("about", "news"), lastModified: DateTime.Now),
                new SitemapItem(Url.QualifiedAction("LastNewsAjax", "news"), lastModified: DateTime.Now),
                new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority), "/news/list"))
            };

            return new SitemapResult(sitemapItems);


            //Sitemap sm = new Sitemap();
            //sm.Add(new Location()
            //{
            //    Url = string.Format("http://www.TechnoDesign.ir/Articles/{0}/{1}", 1, "SEO-in-ASP.NET-MVC"),
            //    LastModified = DateTime.UtcNow,
            //    Priority = 0.5D
            //});
            //return new XmlResult(sm);
        }


        
        public ContentResult RobotsText()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("allow: /error/foo");
            stringBuilder.Append("sitemap: ");
            if (Request.Url != null)
            {
               

                var routeUrl = Url.RouteUrl("GetSitemapXml", null, Request.Url.Scheme);
                if (routeUrl != null)
                    stringBuilder.AppendLine(routeUrl.TrimEnd('/'));
             
            }

            return Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }


    }

    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Sitemap
    {
        private ArrayList map;

        public Sitemap()
        {
            map = new ArrayList();
        }

        [XmlElement("url")]
        public Location[] Locations
        {
            get
            {
                Location[] items = new Location[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                Location[] items = (Location[])value;
                map.Clear();
                foreach (Location item in items)
                    map.Add(item);
            }
        }

        public int Add(Location item)
        {
            return map.Add(item);
        }
    }

    public class Location
    {
        public enum eChangeFrequency
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        [XmlElement("lastmod")]
        public DateTime? LastModified { get; set; }
        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

        [XmlElement("priority")]
        public double? Priority { get; set; }
        public bool ShouldSerializePriority() { return Priority.HasValue; }
    }

    public class XmlResult : ActionResult
    {
        private object objectToSerialize;

        public XmlResult(object objectToSerialize)
        {
            this.objectToSerialize = objectToSerialize;
        }

        public object ObjectToSerialize
        {
            get { return this.objectToSerialize; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (this.objectToSerialize != null)
            {
                context.HttpContext.Response.Clear();
                var xs = new System.Xml.Serialization.XmlSerializer(this.objectToSerialize.GetType());
                context.HttpContext.Response.ContentType = "text/xml";
                xs.Serialize(context.HttpContext.Response.Output, this.objectToSerialize);
            }
        }
    }





}