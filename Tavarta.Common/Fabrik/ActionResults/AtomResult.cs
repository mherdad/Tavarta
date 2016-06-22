using System.ServiceModel.Syndication;
using Fabrik.Common.Web;

namespace Tavarta.Common.Fabrik.ActionResults
{
    /// <summary>
    /// An ActionResult for returning Atom feeds
    /// </summary>
    public class AtomResult : FeedResult
    {
        public AtomResult(SyndicationFeed feed) 
            : base(new Atom10FeedFormatter(feed), "application/atom+xml")
        {

        }
    }
}
