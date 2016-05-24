using Microsoft.Web.Mvc;
using PagedList;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts.Category;
using Tavarta.ServiceLayer.Contracts.Posts;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostService _postService;
        private readonly IApplicationUserManager _userManager;
        private readonly ICategoryService _categoryService;

        public PostController(IUnitOfWork unitOfWork, IPostService postService, IApplicationUserManager userManager, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _postService = postService;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        public async Task<ActionResult> Create()
        {
            var viewModel = new AddPostViewModel();
            viewModel.Categorizes = await _categoryService.GetAllAsSelectList();
            return View(viewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(AddPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            viewModel.AuthorId = _userManager.GetCurrentUserId();

            _postService.AddPost(viewModel);

            return RedirectToAction("Create");
        }

        public async Task<ViewResult> List(int? page)
        {
            var pagenumber = (page ?? 1) - 1; 

            var totalCount = 0;
            var posts = await _postService.GetOrderPage( pagenumber, 5);

            totalCount = posts.TotalCount;

            IPagedList<PostViewModel> pageOrders = new StaticPagedList<PostViewModel>(posts.Posts, pagenumber + 1, 5, totalCount);
            return View(pageOrders);

        }

        [AjaxOnly]
        [HttpPost]
        public async Task<ActionResult> ListAjax(UserSearchRequest search)
        {
            var viewModel = await _postService.GetPageList(search,1,10);
            if (viewModel.Posts == null || !viewModel.Posts.Any()) return Content("no-more-info");
            IPagedList<PostViewModel> pageOrders = new StaticPagedList<PostViewModel>(viewModel.Posts,null);
            return PartialView("_ListAjax", pageOrders);
        }
    }
}