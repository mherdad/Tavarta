using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts.Galleryes;
using Tavarta.ViewModel.Gallery;
using Tavarta.ViewModel.News;

namespace Tavarta.Controllers
{
    public class GalleryController :BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGalleryService _galleryService;

        public GalleryController(IUnitOfWork unitOfWork, IGalleryService galleryService)
        {
            _unitOfWork = unitOfWork;
            _galleryService = galleryService;
        }

        public async Task<ViewResult> List(int? page, string category)
        {
            ViewBag.Category = category;
            var pagenumber = (page ?? 1) - 1;
            ViewBag.Category = category;
            var totalCount = 0;
            var news = await _galleryService.GetOrderPage(pagenumber, 8, category);

            totalCount =(int) news.TotalCount;

            IPagedList<GalleryViewModel> pageOrders = new StaticPagedList<GalleryViewModel>(news.PhotoGallery, pagenumber + 1, 8, totalCount);
            return View(pageOrders);

        }
    }
}