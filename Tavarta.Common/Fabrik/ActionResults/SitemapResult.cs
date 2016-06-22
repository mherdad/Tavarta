using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Fabrik.Common.Web;
using Tavarta.Common.Fabrik.SEO;

namespace Tavarta.Common.Fabrik.ActionResults
{
    /// <summary>
    /// Generates an XML sitemap from a collection of <see cref="ISitemapItem"/>
    /// </summary>
    public class SitemapResult : ActionResult
    {
        private readonly IEnumerable<ISitemapItem> _items;
        private readonly ISitemapGenerator _generator;

        public SitemapResult(IEnumerable<ISitemapItem> items) : this(items, new SitemapGenerator())
        {
        }

        public SitemapResult(IEnumerable<ISitemapItem> items, ISitemapGenerator generator)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (generator == null) throw new ArgumentNullException(nameof(generator));
            
            

            _items = items;
            _generator = generator;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                var sitemap = _generator.GenerateSiteMap(_items);

                sitemap.WriteTo(writer);
            }
        }
    }
}
