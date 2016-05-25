using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using Tavarta.Common.Filters;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts;
using Tavarta.Utility;
using Tavarta.ViewModel.News;
using Tavarta.ViewModel.Posts;

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
            

            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            
           //viewModel.Sports = await _newsService.GetPagedSportNewsAsync(Guid.Parse("7ab8bc23-091a-b846-8662-39d7ccf5af64"));
            return View(viewModel);
        }


        public async Task<ViewResult> List(int? page,string category)
        {
            var pagenumber = (page ?? 1) - 1;
            ViewBag.Category = category;
            var totalCount = 0;
            var news = await _newsService.GetOrderPage(pagenumber, 5,category);

            totalCount = news.TotalCount;

            IPagedList<NewsViewModel> pageOrders = new StaticPagedList<NewsViewModel>(news.News, pagenumber + 1, 5, totalCount);
            return View(pageOrders);

        }



        [PageView]
        public virtual async Task<ActionResult> LastNewsAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LastNewsAjax",viewModel);
        }
        [PageView]
        public async Task< ActionResult> SportListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_SportListAjax",viewModel);
        }
        [PageView]
        public async Task< ActionResult> EnvironmentListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_EnvironmentListAjax",viewModel);
        }
        [PageView]
        public async Task<ActionResult> HealthEventsListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_HealthEventsListAjax",viewModel);
        }
        [PageView]
        public async Task< ActionResult> LiteraryListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LiteraryListAjax",viewModel);
        }

        public async Task<ActionResult> PhotoGalleryListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_PhotoGalleryListAjax");
        }
        [PageView]
        public async Task< ActionResult>  NotesListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_NotesListAjax",viewModel);
        }

        public async Task <ActionResult> MostViewedListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_MostViewedListAjax",viewModel);
        }

        public async Task<ActionResult> CarouselListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_CarouselListAjax",viewModel);
        }

        public ActionResult SurveyListAjax()
        {
            return null;
        }

        public async Task<ActionResult> LatestArticlesAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LatestArticlesAjax",viewModel);
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