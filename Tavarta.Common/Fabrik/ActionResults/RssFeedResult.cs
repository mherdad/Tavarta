using System.ServiceModel.Syndication;

namespace Tavarta.Common.Fabrik.ActionResults
{
    /// <summary>
    /// An ActionResult for returning RSS feeds.
    /// </summary>
    public class RssFeedResult : FeedResult
    {
        public RssFeedResult(SyndicationFeed feed)
            : base(new Rss20FeedFormatter(feed), "application/rss+xml")
        {
            
        }
    }
}
