using System.Web.Mvc;
using Tavarta.Common.Controller;

namespace Tavarta.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController:BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
         
    }
}