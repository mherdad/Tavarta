using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts.Posts;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.Areas.Admin.Controllers
{
    public class PostController:BaseController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostService _postService;
        private readonly IApplicationUserManager _userManager;

        public PostController(IUnitOfWork unitOfWork, IPostService postService,IApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _postService = postService;
            _userManager = userManager;
        }
       
        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(AddPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            viewModel.AuthorId = _userManager.GetCurrentUserId();
            _postService.AddPost(viewModel);

           return  RedirectToAction("Create");
        }


        public async Task<ViewResult> List()
        {
            var viewModel = await _postService.GetPageList(
                new UserSearchRequest());
            return View(viewModel);
        }
        [AjaxOnly]
        [HttpPost]
        public async Task<ActionResult> ListAjax(UserSearchRequest search)
        {
            var viewModel = await _postService.GetPageList(search);
            if (viewModel.Posts == null || !viewModel.Posts.Any()) return Content("no-more-info");
            return PartialView("_ListAjax", viewModel);

        }

    }
}