using System.Threading.Tasks;
using System.Web.Mvc;
using Tavarta.Common.Controller;
using Tavarta.ServiceLayer.Contracts.Comments;
using Tavarta.ServiceLayer.Contracts.Posts;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.EFServiecs.Users;

namespace Tavarta.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController:BaseController
    {
        private readonly ICommentsService _commentsService;
        private readonly IApplicationUserManager _userManager;
        private readonly IPostService _postService;

        public HomeController(ICommentsService commentsService,IApplicationUserManager userManager, IPostService postService)
        {
            _commentsService = commentsService;
            _userManager = userManager;
            _postService = postService;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.TotalComments = await _commentsService.TotalComments();
            ViewBag.TotalUsers = await _userManager.TotalUsers();
            ViewBag.TotalPosts = await _postService.TotalPosts();
            return View();
        }
         
    }
}