using System.Web.Mvc;
using Tavarta.Common.Filters;

namespace Tavarta.Controllers
{
    public class WeatherController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}