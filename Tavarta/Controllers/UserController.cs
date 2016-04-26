using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Tavarta.Common.Controller;
using Tavarta.Common.Extentions;
using Tavarta.DataLayer.Context;
using Tavarta.Filters;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.Utility;
using Tavarta.ViewModel.User;

namespace Tavarta.Controllers
{
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

        #endregion
        [HttpGet]
        [AllowAnonymous]
        public  ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Activity(Description = "درج کاربر جدید")]
        public async Task<ActionResult> Create(AddUserViewModel viewModel)
        {
            if(_userManager.CheckUserNameExist(viewModel.UserName,null))
                this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");
            if (!viewModel.Password.IsSafePasword())
                this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");

            if (!ModelState.IsValid)
                return View (viewModel);

            var newUser =await  _userManager.AddUser(viewModel);

            return this.RedirectToAction<HomeController>(action => action.Index());

        }
    }
}