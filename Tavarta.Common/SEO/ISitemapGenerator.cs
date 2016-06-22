using System.Collections.Generic;
using System.Xml.Linq;

namespace Tavarta.Common.SEO
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}