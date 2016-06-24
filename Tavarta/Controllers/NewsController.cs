using PagedList;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavarta.Common.Extentions;
using Tavarta.Common.Fabrik.ActionResults;
using Tavarta.Common.Filters;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts;
using Tavarta.ServiceLayer.Contracts.Category;
using Tavarta.ViewModel.News;


namespace Tavarta.Controllers
{
    
  [RedirectToCanonicalUrl(true,true)]
    public class NewsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService; 
        public NewsController(INewsService newsService, IUnitOfWork unitOfWork, ICategoryService categoryService)
        {

            _newsService = newsService;
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        
        public virtual async Task<ActionResult> Index()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");

            //viewModel.Sports = await _newsService.GetPagedSportNewsAsync(Guid.Parse("7ab8bc23-091a-b846-8662-39d7ccf5af64"));
            return View(viewModel);
        }

        public async Task<ActionResult> List(int? page, string category)
        {
            if (_categoryService.FindByName(category) == null)
                return RedirectToAction("NotFound", "Error");

            var pagenumber = (page ?? 1) - 1;
            ViewBag.Category = category;
            var totalCount = 0;
            var news = await _newsService.GetOrderPage(pagenumber, 5, category);

            totalCount = news.TotalCount;

            IPagedList<NewsViewModel> pageOrders = new StaticPagedList<NewsViewModel>(news.News, pagenumber + 1, 5, totalCount);
            return View(pageOrders);
        }

        public virtual async Task<ActionResult> LastNewsAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LastNewsAjax", viewModel);
        }

        public async Task<ActionResult> SportListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_SportListAjax", viewModel);
        }

        public async Task<ActionResult> EnvironmentListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_EnvironmentListAjax", viewModel);
        }

        public async Task<ActionResult> HealthEventsListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_HealthEventsListAjax", viewModel);
        }

        public async Task<ActionResult> LiteraryListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LiteraryListAjax", viewModel);
        }

        public async Task<ActionResult> PhotoGalleryListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_PhotoGalleryListAjax");
        }

        public async Task<ActionResult> NotesListAjax()
        {
            var viewModel = await _newsService.GetNotesAsync();
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_NotesListAjax", viewModel);
        }

        public async Task<ActionResult> MostViewedListAjax()
        {
            var viewModel = await _newsService.GetPagedListAsync();
            if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_MostViewedListAjax", viewModel);
        }

        public async Task<ActionResult> CarouselListAjax()
        {
            var viewModel = await _newsService.GetCarouselAsync();
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_CarouselListAjax", viewModel);
        }

        public ActionResult SurveyListAjax()
        {
            return null;
        }

        public async Task<ActionResult> LatestArticlesAjax()
        {
            var viewModel = await _newsService.GetLastArticleAsync();
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return PartialView("_LatestArticlesAjax", viewModel);
        }

        [HttpGet]
        [NoBrowserCache]
        [CompressFilter]
        //[OutputCache(Location = OutputCacheLocation.ServerAndClient,CacheProfile = "",VaryByParam = "id",Duration = 10,NoStore = false)]
        public async Task<ActionResult> Details(string title,Guid? id)
        {

            var viewModel = await _newsService.GetLastNewsDetailsAsync(id);
            if (viewModel == null)
                return RedirectToAction("NotFound", "Error");
            //return HttpNotFound();

            string realTitle =viewModel.Title.ToSeoUrl();
            string urlTitle = (title ?? "").Trim().ToLower();

            if (realTitle != urlTitle)
            {
                string url = "/news/" + viewModel.Id + "/" + realTitle;
                return new PermanentRedirectResult(url);
            }




            ViewBag.Title = viewModel.Title;
            ViewBag.MetaDescription= viewModel.MetaDescription;
            ViewBag.MetaKeywords= viewModel.MetaKeywords;
            //if (viewModel.News == null || !viewModel.News.Any()) return Content("no-more-info");
            return View(viewModel);
        }

    
    }





    


}