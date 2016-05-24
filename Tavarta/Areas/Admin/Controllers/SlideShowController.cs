using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Tavarta.Common.Json;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.SlideShows;
using Tavarta.ViewModel.Posts;
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



        [HttpGet]
        // [AjaxOnly]
        [Activity(Description = "ویرایش پست")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [ActionName("Edit")]
        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _slideShowService.GetForEditAsync(id.Value);
            
            if (viewModel == null) return HttpNotFound();

            return View("Create", viewModel);
        }


        [HttpPost]
        //[AjaxOnly]
        // [Route("Edit/{id}")]
        //[CheckReferrer]

        public virtual async Task<ActionResult> Edit(AddSlideShowViewModel viewModel)
        {

            var post = _slideShowService.FindByIdAsync(viewModel.Id);

            if (post == null) return HttpNotFound();
           

            await _slideShowService.EditSlideShow(viewModel);
            return RedirectToAction("List");
            return new JsonNetResult
            {
                Data =
                new
                {
                    success = true,
                    View = View("_UserItem")
                }
            };
        }

    }
}