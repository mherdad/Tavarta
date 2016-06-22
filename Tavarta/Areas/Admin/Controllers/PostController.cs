using Microsoft.Web.Mvc;
using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.Category;
using Tavarta.ServiceLayer.Contracts.Posts;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.Areas.Admin.Controllers
{
    [Authorize]
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

            return RedirectToAction("List");
        }

        public async Task<ViewResult> List(int? page)
        {
            var pagenumber = (page ?? 1) - 1;

            var totalCount = 0;
            var posts = await _postService.GetOrderPage(pagenumber, 5);

            totalCount = posts.TotalCount;

            IPagedList<PostViewModel> pageOrders = new StaticPagedList<PostViewModel>(posts.Posts, pagenumber + 1, 5, totalCount);
            return View(pageOrders);
        }

        [AjaxOnly]
        [HttpPost]
        public async Task<ActionResult> ListAjax(UserSearchRequest search)
        {
            var viewModel = await _postService.GetPageList(search, 1, 10);
            if (viewModel.Posts == null || !viewModel.Posts.Any()) return Content("no-more-info");
            IPagedList<PostViewModel> pageOrders = new StaticPagedList<PostViewModel>(viewModel.Posts, null);
            return PartialView("_ListAjax", pageOrders);
        }

        [HttpGet]
        // [AjaxOnly]
        [Activity(Description = "ویرایش پست")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [ActionName("Edit")]
        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _postService.GetForEditAsync(id.Value);
            viewModel.Categorizes = await _categoryService.GetAllAsSelectList();
            if (viewModel == null) return HttpNotFound();

            return View("Create", viewModel);
        }

        [HttpPost]
        //[AjaxOnly]
        // [Route("Edit/{id}")]
        //[CheckReferrer]

        public virtual async Task<ActionResult> Edit(AddPostViewModel viewModel)
        {
            var post = _postService.FindByIdAsync(viewModel.Id);

            if (post == null) return HttpNotFound();
            viewModel.AuthorId = _userManager.GetCurrentUserId();

            await _postService.EditUser(viewModel);
            return RedirectToAction("List");
            //return new JsonNetResult
            //{
            //    Data =
            //    new
            //    {
            //        success = true,
            //        View = View("_UserItem")
            //    }
            //};
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var post = await _postService.GetDeletePostAsync(id);
            if (post == null) return HttpNotFound();

            return View(post);
        }

        [HttpPost]
        public ActionResult Delete(DeleteViewModel viewModel)
        {
            _postService.DeletePost(viewModel.Id);
            return RedirectToAction("List");
        }
    }
}