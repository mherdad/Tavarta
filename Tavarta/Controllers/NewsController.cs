using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tavarta.Common.Filters;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts;

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
        public ActionResult SportListAjax()
        {
            return PartialView("_SportListAjax");
        }
        [PageView]
        public ActionResult EnvironmentListAjax()
        {
            return PartialView("_EnvironmentListAjax");
        }
        [PageView]
        public ActionResult HealthEventsListAjax()
        {
            return PartialView("_HealthEventsListAjax");
        }
        [PageView]
        public ActionResult LiteraryListAjax()
        {
            return PartialView("_LiteraryListAjax");
        }

        public ActionResult PhotoGalleryListAjax()
        {
            return null;
        }
        [PageView]
        public ActionResult NotesListAjax()
        {
            return PartialView("_NotesListAjax");
        }

        public ActionResult MostViewedListAjax()
        {
            return PartialView("_MostViewedListAjax");
        }

        public ActionResult CarouselListAjax()
        {
            return PartialView("_CarouselListAjax");
        }

        public ActionResult SurveyListAjax()
        {
            return null;
        }

        public ActionResult LatestArticles()
        {
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid? id)
        {
            var viewModel = await _newsService.GetLastNewsDetailsAsync(id);
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return View(viewModel);
        }
    }
}