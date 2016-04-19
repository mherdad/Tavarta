namespace Tavarta.Common.Filters
{
    public class SignalrAuthorizeAttribute : Microsoft.AspNet.SignalR.AuthorizeAttribute
    {
        public SignalrAuthorizeAttribute(params string[] permissions)
            : base()
        {
            Roles = string.Join(",", permissions);
        }

    }
}
