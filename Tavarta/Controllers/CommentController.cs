using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.ServiceLayer.Contracts.Comments;
using Tavarta.ViewModel.Comments;

namespace Tavarta.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentsService _commentsService;

        public CommentController(ICommentsService commentsService, IUnitOfWork unitOfWork)
        {
            _commentsService = commentsService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Showcomment()
        {
            //var viewModel = await _commentsService.GetComments();
            return PartialView();
        }

        [HttpGet]
        public async Task<ActionResult> PostComments(Guid postId)
        {
            var viewModel = await _commentsService.GetComments(postId);
            return PartialView(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> PostComments(AddCommentsViewModel viewModel ,Guid? postId)
        {
            var comment = await _commentsService.AddComment(viewModel, viewModel.Id);
            return RedirectToAction("Details", "News", new {viewModel.Id});
        }


        [HttpGet]
        public async Task<ActionResult> AddPostComments(Guid postId)
        {
            ViewBag.PostId = postId;
            return PartialView("_AddPostComments");
        }

        [HttpPost]
        public async Task<ActionResult> AddPostComments(AddCommentsViewModel viewModel,Guid postId)
        {
            var comment = await _commentsService.AddComment(viewModel,postId);
            return RedirectToAction("Details", "News", new {viewModel.Id});
            return PartialView("_AddPostComments");
        }

        public ActionResult InsertComment(Guid parentid)
        {
            //if (parentid != 0)
            //{
            //    return PartialView(new PostComment()
            //    {
            //        ReplyId = parentid
            //    });
            //}
            return PartialView();
        }

        public ActionResult Reply()
        {
            return View("_Reply");
        }

        public ActionResult Temp()
        {
            return PartialView();
        }
    }
}