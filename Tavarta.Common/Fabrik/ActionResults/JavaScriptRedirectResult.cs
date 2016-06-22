using System;
using System.Web.Mvc;
using Tavarta.Common.Fabrik.Others;

namespace Tavarta.Common.Fabrik.ActionResults
{
    /// <summary>
    /// An ActionResult for performing redirects via JavaScript
    /// </summary>
    public class JavaScriptRedirectResult : JavaScriptResult
    {
        private const string RedirectScriptFormat = "window.location = '{0}';";
        
        public JavaScriptRedirectResult(string redirectUrl)
        {
            if (redirectUrl == null) throw new ArgumentNullException(nameof(redirectUrl));
            
            Script = RedirectScriptFormat.FormatWith(redirectUrl);
        }
    }
}
