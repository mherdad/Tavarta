﻿using PagedList;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Web.Mvc;
using Tavarta.Common.Controller;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.Comments;
using Tavarta.ViewModel.Comments;

namespace Tavarta.Areas.Admin.Controllers
{
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentsService _commentsService;

        public CommentsController(IUnitOfWork unitOfWork, ICommentsService commentsService)
        {
            _unitOfWork = unitOfWork;
            _commentsService = commentsService;
        }

        public async Task<ViewResult> List(int? page)
        {
            var pagenumber = (page ?? 1) - 1;

            var totalCount = 0;
            var posts = await _commentsService.GetOrderPage(pagenumber, 5);

            totalCount = posts.TotalCount;

            IPagedList<CommentViewModel> pageOrders = new StaticPagedList<CommentViewModel>(posts.Comments, pagenumber + 1, 5, totalCount);
            return View(pageOrders);
        }

        public ActionResult ListAjax()
        {
            return PartialView("_ListAjax");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var viewModel = await _commentsService.EditCommentDetails(id);
            return View(viewModel);
        }

        public ActionResult Edit(EditCommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            _commentsService.EditComment(viewModel);
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var viewModel = await _commentsService.DeleteCommentDetails(id);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(DeleteCommentViewModel viewModel)
        {
            await _commentsService.DeleteComment(viewModel);
            return RedirectToAction("List");
        }


        [HttpPost]
        [AjaxOnly]
        //[CheckReferrer]
        //[Activity(Description = "مسدود کردن حساب کاربر")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> DisableComment(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //if (await _commentsService.IsShow(id.Value))
            //{
            //    return Content("system");
            //}
            var commentViewModel = await _commentsService.Disable(id.Value, true);
            return PartialView("_CommentItem", commentViewModel);

        }

        [HttpPost]
        [AjaxOnly]
        //[CheckReferrer]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        //[Activity(Description = "مسدود کردن حساب کاربر")]
        public virtual async Task<ActionResult> EnableComment(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var commentViewModel = await _commentsService.Disable(id.Value, false);
            return PartialView("_CommentItem", commentViewModel);

        }


    }
}