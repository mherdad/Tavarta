using System;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Tavarta.Common.Filters
{
    public class UseAmpImageAttribute : ActionFilterAttribute
    {
        private HtmlTextWriter _htmlTextWriter;
        private StringWriter _stringWriter;
        private StringBuilder _stringBuilder;
        private HttpWriter _output;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stringBuilder = new StringBuilder();
            _stringWriter = new StringWriter(_stringBuilder);
            _htmlTextWriter = new HtmlTextWriter(_stringWriter);
            _output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;
            filterContext.RequestContext.HttpContext.Response.Output = _htmlTextWriter;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = _stringBuilder.ToString();

            // Change images to Amp-specific images
            response = UpdateAmpImages(response);

            // For AMP pages, change Script content to a link.
            response = ReplaceWithLink("script", response);

            // For AMP pages, change iFrame content to a link.
            response = ReplaceWithLink("iframe", response);

            _output.Write(response);
        }

        private string ReplaceWithLink(string tag, string response)
        {
            var doc = GetHtmlDocument(response);
            var elements = doc.DocumentNode.Descendants(tag);
            foreach (var htmlNode in elements)
            {
                if (htmlNode.Attributes["data-link"] == null) continue;

                var dataLink = htmlNode.Attributes["data-link"].Value;
                var paragraph = doc.CreateElement("p");

                var text = String.Format("[Embedded Link] {0}", dataLink);

                var anchor = doc.CreateElement("a");
                anchor.InnerHtml = text;
                anchor.Attributes.Add("href", dataLink);
                anchor.Attributes.Add("title", text);
                paragraph.InnerHtml = anchor.OuterHtml;

                var original = htmlNode.OuterHtml;
                var replacement = paragraph.OuterHtml;

                response = response.Replace(original, replacement);
            }

            return response;
        }

        private string UpdateAmpImages(string response)
        {
            // Use HtmlAgilityPack (install-package HtmlAgilityPack)
            var doc = GetHtmlDocument(response);
            var imageList = doc.DocumentNode.Descendants("img");

            const string ampImage = "amp-img";

            if (!imageList.Any()) return response;

            if (!HtmlNode.ElementsFlags.ContainsKey("amp-img"))
            {
                HtmlNode.ElementsFlags.Add("amp-img", HtmlElementFlag.Closed);
            }

            foreach (var imgTag in imageList)
            {
                var original = imgTag.OuterHtml;
                var replacement = imgTag.Clone();
                replacement.Name = ampImage;
                replacement.Attributes.Remove("caption");
                response = response.Replace(original, replacement.OuterHtml);
            }

            return response;
        }

        private HtmlDocument GetHtmlDocument(string htmlContent)
        {
            var doc = new HtmlDocument
            {
                OptionOutputAsXml = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            doc.LoadHtml(htmlContent);

            return doc;
        }
    }
}