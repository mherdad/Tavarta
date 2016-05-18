using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
        public virtual async Task<ActionResult> Index()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");

            return View(viewModel);
        }

        public ActionResult List()
        {
            return View();
        }

        public virtual async Task<ActionResult> LastNewsAjax()
        {
           // var viewModel = await _newsService.GetPagedListAsync();
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LastNewsAjax");
        }

        public ActionResult SportListAjax()
        {
            return null;
        }

        public ActionResult EnvironmentListAjax()
        {
            return null;
        }

        public ActionResult HealthEventsListAjax()
        {
            return null;
        }

        public ActionResult LiteraryListAjax()
        {
            return null;
        }

        public ActionResult PhotoGalleryListAjax()
        {
            return null;
        }

        public ActionResult NotesListAjax()
        {
            return null;
        }

        public ActionResult MostViewedListAjax()
        {
            return null;
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