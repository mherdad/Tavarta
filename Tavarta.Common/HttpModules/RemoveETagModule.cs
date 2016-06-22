using System;
using System.Web;

namespace Tavarta.Common.HttpModules
{
    public class RemoveETagModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("ETag");
        }
    }
}