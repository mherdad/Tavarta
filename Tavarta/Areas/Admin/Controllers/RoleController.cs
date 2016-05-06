using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Web.Mvc;
using MvcSiteMapProvider;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ViewModel.Role;

namespace Tavarta.Areas.Admin.Controllers
{
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
    }
}