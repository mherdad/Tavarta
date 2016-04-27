using System.Web.Mvc;

namespace Tavarta.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Admin_default",
            //    "admin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
                 "Admin_default",
                 "Admin/{controller}/{action}/{id}",
                    new { action = "Index", id = UrlParameter.Optional },
                    new { controller = "Account|Admin|User" },
                 new[] { "Tavarta.Areas.Admin.Controllers" }
  );
        }
    }
}