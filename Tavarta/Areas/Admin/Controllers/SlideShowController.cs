using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts.SlideShows;
using Tavarta.ViewModel.SlideShow;

namespace Tavarta.Areas.Admin.Controllers
{
    public class SlideShowController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlideShowService _slideShowService;

        public SlideShowController(IUnitOfWork unitOfWork, ISlideShowService slideShowService)
        {
            _unitOfWork = unitOfWork;
            _slideShowService = slideShowService;
        }

        public virtual async Task<ActionResult> List()
        {
            var viewModel = await _slideShowService.GetPageList();

            return View(viewModel);
        }

        public virtual async Task<ActionResult> ListAjax()
        {
            var viewModel = await _slideShowService.GetPageList();
            if (viewModel.SlideShow == null || !viewModel.SlideShow.Any()) return Content("no-more-info");
            return PartialView("_ListAjax", viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new AddSlideShowViewModel());
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(AddSlideShowViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            _slideShowService.AddSlide(viewModel);
            return RedirectToAction("List");

        }

    }
}