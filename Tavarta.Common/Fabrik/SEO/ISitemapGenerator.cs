using System.Collections.Generic;
using System.Xml.Linq;
using Fabrik.Common.Web;

namespace Tavarta.Common.Fabrik.SEO
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
