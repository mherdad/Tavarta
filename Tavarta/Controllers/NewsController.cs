using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Tavarta.Common.Filters;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts;
using Tavarta.Utility;

namespace Tavarta.Controllers
{
    public class NewsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService, IUnitOfWork unitOfWork)
        {
            _newsService = newsService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [PageView]
        public virtual async Task<ActionResult> Index()
        {
            var Page = new PageViewAttribute.PageViewValue();
            ViewBag.te = Page.Value;

            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            
           //viewModel.Sports = await _newsService.GetPagedSportNewsAsync(Guid.Parse("7ab8bc23-091a-b846-8662-39d7ccf5af64"));
            return View(viewModel);
        }

        public ActionResult List()
        {
            return View();
        }

        [PageView]
        public virtual async Task<ActionResult> LastNewsAjax()
        {
           // var viewModel = await _newsService.GetPagedListAsync();
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LastNewsAjax");
        }
        [PageView]
        public async Task< ActionResult> SportListAjax()
        {
            return PartialView("_SportListAjax");
        }
        [PageView]
        public async Task< ActionResult> EnvironmentListAjax()
        {
            return PartialView("_EnvironmentListAjax");
        }
        [PageView]
        public async Task<PartialViewResult> HealthEventsListAjax()
        {
            return PartialView("_HealthEventsListAjax");
        }
        [PageView]
        public async Task< ActionResult> LiteraryListAjax()
        {
            return PartialView("_LiteraryListAjax");
        }

        public ActionResult PhotoGalleryListAjax()
        {
            return PartialView("_PhotoGalleryListAjax");
        }
        [PageView]
        public async Task< ActionResult>  NotesListAjax()
        {
            return PartialView("_NotesListAjax");
        }

        public async Task <ActionResult> MostViewedListAjax()
        {
            return PartialView("_MostViewedListAjax");
        }

        public async Task<ActionResult> CarouselListAjax()
        {
            return PartialView("_CarouselListAjax");
        }

        public ActionResult SurveyListAjax()
        {
            return null;
        }

        public ActionResult LatestArticlesAjax()
        {
            return PartialView("_LatestArticlesAjax");
        }

        [HttpGet]
       [NoBrowserCache]
        //[OutputCache(Location = OutputCacheLocation.ServerAndClient,CacheProfile = "",VaryByParam = "id",Duration = 10,NoStore = false)]
        public async Task<ActionResult> Details(Guid? id)
        {
            var viewModel = await _newsService.GetLastNewsDetailsAsync(id);
           
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return View(viewModel);
        }
    }
}