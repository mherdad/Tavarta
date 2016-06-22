using System.Web.Mvc;
using System.Web.WebPages;

namespace Tavarta.Common.Fabrik
{
    /// <summary>
    /// Exposes the MVC HtmlHelper to Razor helpers instead of the crappy System.Web.WebPages HtmlHelper
    /// </summary>
    public class HelperPage : System.Web.WebPages.HelperPage
    {
        public new static HtmlHelper Html
        {
            get { return ((WebViewPage)WebPageContext.Current.Page).Html; }
        }

        public static UrlHelper Url
        {
            get { return ((WebViewPage)WebPageContext.Current.Page).Url; }
        }

        public static ViewDataDictionary ViewData
        {
            get { return ((WebViewPage)WebPageContext.Current.Page).ViewData; }
        }
    }
}