using System.Web.Mvc;
using Tavarta.Common.Controller;

namespace Tavarta.Areas.Admin.Controllers
{
    public class HomeController:BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
         
    }
}