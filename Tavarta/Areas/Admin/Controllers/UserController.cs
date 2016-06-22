﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Web.Mvc;
using MvcSiteMapProvider;
using Tavarta.Common.Controller;
using Tavarta.Common.Extentions;
using Tavarta.Common.Json;
using Tavarta.Controllers;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.Utility;
using Tavarta.ViewModel.User;

namespace Tavarta.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController:BaseController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;

        #endregion

        #region Constructor

        public UserController(IUnitOfWork unitOfWork, IPermissionService permissionService, IApplicationRoleManager roleManager,
            IApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionService = permissionService;
        }

       


        #region List,ListAjax
        [HttpGet]
        [Activity(Description = "مشاهده کاربران")]
        [MvcSiteMapNode(ParentKey = "Home_Index", Title = "مدیریت کاربران", Key = "User_List")]
        public virtual async Task<ActionResult> List()
        {
            var viewModel = await _userManager.GetPageList(
                new UserSearchRequest());
            viewModel.Roles = await _roleManager.GetAllAsSelectList();
            return View(viewModel);
        }

        //[CheckReferrer]
        [AjaxOnly]
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> ListAjax(UserSearchRequest search)
        {
            var viewModel = await _userManager.GetPageList(search);
            if (viewModel.Users == null || !viewModel.Users.Any()) return Content("no-more-info");
           return PartialView("_ListAjax", viewModel);
           
        }
        #endregion

        #region Edit
        
        [HttpGet]
       // [AjaxOnly]
        [Activity(Description = "ویرایش کاربر")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _userManager.GetForEditAsync(id.Value);
            if (viewModel == null) return HttpNotFound();

            return PartialView("_Edit", viewModel);
        }

        [HttpPost]
        //[AjaxOnly]
       // [Route("Edit/{id}")]
        //[CheckReferrer]

        public virtual async Task<ActionResult> Edit(EditUserViewModel viewModel)
        {
            #region Validation
            if (_userManager.CheckUserNameExist(viewModel.UserName, viewModel.Id))
                this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");
            if (viewModel.Password.IsNotEmpty() && !viewModel.Password.IsSafePasword())
                this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");

            #endregion

            //if (!ModelState.IsValid)
            //{
            //    await _userManager.FillForEdit(viewModel);
            //    return new JsonNetResult
            //    {
            //        Data =
            //            new
            //            {
            //                success = false,
            //                View = View("Edit", viewModel)
            //            }
            //    };
            //}
            var dbUser = await _userManager.FindByIdAsync(viewModel.Id);
            if (dbUser == null) return HttpNotFound();
            var userViewModel = await _userManager.EditUser(viewModel);
            return RedirectToAction("List");
                return new JsonNetResult
            {
                Data =
                    new
                    {
                        success = true,
                        View = View("_UserItem", userViewModel)
                    }
            };
        }

        #endregion

        #region CancelEdit
        [HttpPost]
        [AjaxOnly]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> CancelEdit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _userManager.GetUserViewModel(id.Value);
            if (viewModel == null) return HttpNotFound();
            return PartialView("_UserItem", viewModel);
        }
        #endregion

        #endregion
        [HttpGet]
        [AllowAnonymous]
        public  ActionResult Create()
        {
            return PartialView("_Create");
        }

        [AllowAnonymous]

        [HttpPost]
        [Activity(Description = "درج کاربر جدید")]
        public async Task<ActionResult> Create(AddUserViewModel viewModel)
        {
            #region Validation
            if (_userManager.CheckUserNameExist(viewModel.UserName, null))
                this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");
            if (!viewModel.Password.IsSafePasword())
                this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");

            #endregion

            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction("Create",viewModel);
            //    return new JsonNetResult
            //    {
            //        Data =
            //            new
            //            {
            //                success = false,
            //                View = this.RenderPartialViewToString("_Create", viewModel)
            //            }
            //    };
            //}
            var newUser =
            await _userManager.AddUser(viewModel);
            return RedirectToAction("List");
            return new JsonNetResult
            {
                Data =
                    new
                    {
                        success = true,
                        View = this.RenderPartialViewToString("UserItem", newUser)
                    }
            };

        }


        #region Ban

        [HttpPost]
        [AjaxOnly]
        //[CheckReferrer]
        [Activity(Description = "مسدود کردن حساب کاربر")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> BanUser(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (await _userManager.IsSystemUser(id.Value))
            {
                return Content("system");
            }
            var userViewModel = await _userManager.Ban(id.Value, true);
            return PartialView("_UserItem", userViewModel);

        }

        [HttpPost]
        [AjaxOnly]
        //[CheckReferrer]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [Activity(Description = "مسدود کردن حساب کاربر")]
        public virtual async Task<ActionResult> EnableUser(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userViewModel = await _userManager.Ban(id.Value, false);
            return PartialView("_UserItem", userViewModel);

        }
        #endregion




    }

}