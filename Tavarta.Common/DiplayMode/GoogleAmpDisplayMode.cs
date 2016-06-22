using System.Web.WebPages;

namespace Tavarta.Common.DiplayMode
{
    public class GoogleAmpDisplayMode : DefaultDisplayMode
    {
        public GoogleAmpDisplayMode() : base("amp") // for filename.amp.cshtml files.
        {
            ContextCondition = context => context.Request.RawUrl.Contains("?amp");
        }

    }
}