﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Web.Mvc;
using MvcSiteMapProvider;
using Tavarta.Common.Extentions;
using Tavarta.Common.Json;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ViewModel.Role;

namespace Tavarta.Areas.Admin.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationRoleManager _roleManager;

        #endregion

        #region Const

        public RoleController(IUnitOfWork unitOfWork, IApplicationRoleManager roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        #endregion

        #region ListAjax , List
        [HttpGet]
        [Activity(Description = "مشاده گروه های کاربری")]
        [MvcSiteMapNode(ParentKey = "User_List", Title = "مدیریت گروه های کاربری")]
        public virtual async Task<ActionResult> List()
        {
            var roles = await _roleManager.GetPageList(new RoleSearchRequest());
            return View(roles);
        }

        //[CheckReferrer]
        [AjaxOnly]
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> ListAjax(RoleSearchRequest request)
        {
            var viewModel = await _roleManager.GetPageList(request);
            if (viewModel.Roles == null || !viewModel.Roles.Any()) return Content("no-more-info");
            return PartialView("_ListAjax", viewModel);
        }
        #endregion


        #region Create
        [HttpGet]
        [AjaxOnly]
        public virtual ActionResult Create()
        {
            return PartialView("_Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        //[CheckReferrer]
        [Activity(Description = "درج گروه کاربری")]
        public virtual async Task<ActionResult> Create(AddRoleViewModel viewModel)
        {
            if (_roleManager.CheckForExisByName(viewModel.Name, null))
                this.AddErrors("Name", "این گروه  قبلا در سیستم ثبت شده است");

            if (!ModelState.IsValid)
            {
                return new JsonNetResult
                {
                    Data =
                        new
                        {
                            success = false,
                            View = this.RenderPartialViewToString("_Create", viewModel)
                        }
                };
            }
            var newRole = await _roleManager.AddRole(viewModel);
            return new JsonNetResult
            {
                Data =
                    new
                    {
                        success = true,
                        View = this.RenderPartialViewToString("_RoleItem", newRole)
                    }
            };
        }

        #endregion


        #region Edit
        [HttpGet]
        [AjaxOnly]
        [Activity(Description = " ویرایش گروه کاربری")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _roleManager.GetRoleByPermissionsAsync(id.Value);
            if (viewModel == null)
                return HttpNotFound();
            if (viewModel.IsSystemRole)
                return Content("system");
            _roleManager.FillForEdit(viewModel);
            return PartialView("_Edit", viewModel);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        //[CheckReferrer]
        public virtual async Task<ActionResult> Edit(EditRoleViewModel viewModel)
        {
            if (await _roleManager.CheckRoleIsSystemRoleAsync(viewModel.Id))
                return Content("system");

            if (_roleManager.CheckForExisByName(viewModel.Name, viewModel.Id))
                this.AddErrors("Name", "این گروه  قبلا در سیستم ثبت شده است");

            if (!ModelState.IsValid)
            {
                _roleManager.FillForEdit(viewModel);
                return new JsonNetResult
                {
                    Data =
                        new
                        {
                            success = false,
                            View = this.RenderPartialViewToString("_Edit", viewModel)
                        }
                };
            }

            if (!await _roleManager.IsInDb(viewModel.Id))
                return HttpNotFound();

            if (!await _roleManager.EditRole(viewModel))
            {
                this.AddErrors("Name", "برای گروه کاربری مورد نظر ، دسترسی تعیین کنید");
                _roleManager.FillForEdit(viewModel);
                return new JsonNetResult
                {
                    Data =
                        new
                        {
                            success = false,
                            View = this.RenderPartialViewToString("_Edit", viewModel)
                        }
                };
            }
            await _unitOfWork.SaveAllChangesAsync();

            var editedRole = await _roleManager.GetRole(viewModel.Id);
            return new JsonNetResult
            {
                Data =
                    new
                    {
                        success = true,
                        View = this.RenderPartialViewToString("_RoleItem", editedRole)
                    }
            };

        }

        #endregion

        #region CancelEdit
        [HttpPost]
        [AjaxOnly]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual async Task<ActionResult> CancelEdit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _roleManager.GetRole(id.Value);
            if (viewModel == null) return HttpNotFound();
            return PartialView("_RoleItem", viewModel);
        }
        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CheckReferrer]
        [AjaxOnly]
        [Activity(Description = " حذف گروه کاربری")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (await _roleManager.CheckRoleIsSystemRoleAsync(id.Value))
            {
                return Content("system");
            }
            await _roleManager.RemoveById(id.Value);
            return Content("ok");
        }

        #endregion

        #region RemoteValidation

        [HttpPost]
        [AjaxOnly]
        // [CheckReferrer]
        [OutputCache(NoStore = true, Duration = 0)]
        public virtual JsonResult RoleNameExist(string name, Guid? id)
        {
            return _roleManager.CheckForExisByName(name, id) ? Json(false) : Json(true);
        }

        #endregion

    }
}