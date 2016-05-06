using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Nessos.LinqOptimizer.CSharp;
using Tavarta.Common.Extentions;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.ServiceLayer.Security;
using Tavarta.Utility;
using Tavarta.ViewModel.Role;

namespace Tavarta.ServiceLayer.EFServiecs.Users
{
    public class RoleManager : RoleManager<Role, Guid>, IApplicationRoleManager
    {
        #region Fields

        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionService _permissionService;
        private readonly IDbSet<Role> _roles;

        #endregion Fields

        #region Constructor

        public RoleManager(IMappingEngine mappingEngine, IPermissionService permissionService, IUnitOfWork unitOfWork, IRoleStore<Role, Guid> roleStore)
            : base(roleStore)
        {
            _unitOfWork = unitOfWork;
            _roles = _unitOfWork.Set<Role>();
            _permissionService = permissionService;
            _mappingEngine = mappingEngine;
            AutoCommitEnabled = true;
        }

        #endregion Constructor

        #region FindRoleByName

        public Role FindRoleByName(string roleName)
        {
            return this.FindByName(roleName); // RoleManagerExtensions
        }

        #endregion FindRoleByName

        #region CreateRole

        public IdentityResult CreateRole(Role role)
        {
            return this.Create(role); // RoleManagerExtensions
        }

        #endregion CreateRole

        #region GetUsersOfRole

        public IList<UserRole> GetUsersOfRole(string roleName)
        {
            return Roles.Where(role => role.Name == roleName)
                             .SelectMany(role => role.Users)
                             .ToList();
        }

        #endregion GetUsersOfRole

        #region GetApplicationUsersInRole

        public IList<User> GetApplicationUsersInRole(string roleName)
        {
            //var roleUserIdsQuery = from role in Roles
            //                       where role.Name == roleName
            //                       from user in role.Users
            //                       select user.UserId;

            return null; //_userManager.GetUsersWithRoleIds(roleUserIdsQuery.ToArray());
        }

        #endregion GetApplicationUsersInRole

        #region FindUserRoles

        public IList<string> FindUserRoles(Guid userId)
        {
            //    var ff = _unitOfWork.Database.SqlQuery<string>($@"SELECT [Extent1].[Name] AS [Name]
            //FROM[dbo].[Roles] AS[Extent1]
            //       INNER JOIN[dbo].[UserRole] AS[Extent2]
            //         ON[Extent1].[Id] = [Extent2].[RoleId]
            //WHERE[Extent2].[UserId] = '{userId}'").ToList();

            using (var dbContext = new ApplicationDbContext())
            {
                var userRolesQuery =
                  dbContext.Roles.SelectMany(role => role.Users, (role, user) => new { role, user })
                      .Where(@t => @t.user.UserId == userId)
                      .Select(@t => new { @t.role.Name }
                      );

                return userRolesQuery.OrderBy(x => x.Name).Select(a => a.Name).ToList();
            }

              
        }

        public IEnumerable<Guid> FindUserRoleIds(Guid userId)
        {
            var userRolesQuery = from role in Roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return userRolesQuery.Select(a => a.Id).ToList();
        }

        public async Task<IList<Guid>> FindUserRoleIdsAsync(Guid userId)
        {
            var userRolesQuery = from role in Roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return await userRolesQuery.Select(a => a.Id).ToListAsync();
        }

        public async Task<IList<string>> FindUserPermissions(Guid userId)
        {
            var userRolesQuery =
                Roles.SelectMany(role => role.Users, (role, user) => new {role, user})
                    .Where(@t => @t.user.UserId == userId)
                    .Select(@t => new {@t.role.Name, @t.role.Permissions});

            var roles = await userRolesQuery.Cacheable().AsNoTracking().ToListAsync();
            var roleNames = roles.Select(a => a.Name).ToList();
            return
                roleNames.Union(
                    _permissionService.GetUserPermissionsAsList(
                        roles.Select(a => XElement.Parse(a.Permissions)).ToList())).ToList();
        }

        #endregion FindUserRoles

        #region GetRolesForUser

        public string[] GetRolesForUser(Guid userId)
        {
            var roles = FindUserRoles(userId);
            if (roles == null || !roles.Any())
            {
                return new string[] { };
            }

            return roles.ToArray();
        }

        #endregion GetRolesForUser

        #region IsUserInRole

        public bool IsUserInRole(Guid userId, string roleName)
        {
            var userRolesQuery =
                Roles.Where(role => role.Name == roleName)
                    .SelectMany(role => role.Users, (role, user) => new {role, user})
                    .Where(@t => @t.user.UserId == userId)
                    .Select(@t => @t.role);
            var userRole = userRolesQuery.FirstOrDefault();
            return userRole != null;
        }

        #endregion IsUserInRole

        #region GetAllRolesAsync

        public async Task<IList<Role>> GetAllRolesAsync()
        {
            return await Roles.ToListAsync();
        }

        #endregion GetAllRolesAsync

        #region SeedDatabase

        /// <summary>
        /// for instal permissions with roles
        /// </summary>
        public void SeedDatabase()
        {
            var standardRoles = StandardRoles.SystemRolesWithPermissions;

            foreach (var role in
                standardRoles.Select(record => new {record, role = this.FindByName(record.RoleName)})
                    .Where(@t => @t.role == null)
                    .Select(@t => new Role
                    {
                        Name = @t.record.RoleName,
                        IsSystemRole = true,
                        XmlPermissions =
                            _permissionService.GetPermissionsAsXml(@t.record.Permissions.Select(a => a.Name).ToArray())
                    }))
            {
                _roles.Add(role);
            }

            _unitOfWork.SaveChanges();
        }

        #endregion SeedDatabase

        #region DeleteAll

        public Task RemoteAll()
        {
            return Roles.DeleteAsync();
        }

        #endregion DeleteAll

        #region GetList

        public async Task<IEnumerable<RoleViewModel>> GetList()
        {
            return await _roles.Project(_mappingEngine).To<RoleViewModel>().ToListAsync();
        }

        #endregion GetList

        #region AddRole

        public async Task<RoleViewModel> AddRole(AddRoleViewModel viewModel)
        {
            var role = _mappingEngine.Map<Role>(viewModel);
            _roles.Add(role);
            await _unitOfWork.SaveChangesAsync();
            return await GetRole(role.Id);
        }

        #endregion AddRole

        #region GetRoleByPermissions

        public async Task<EditRoleViewModel> GetRoleByPermissionsAsync(Guid id)
        {
            var role = await _roles.FirstOrDefaultAsync(r => r.Id == id);
            var viewModel = _mappingEngine.Map<EditRoleViewModel>(role);
            if (role.Permissions != null)
                viewModel.PermissionNames = _permissionService.GetUserPermissionsAsList(role.XmlPermissions).ToArray();
            return viewModel;
        }

        #endregion GetRoleByPermissions

        #region EditRole

        public async Task<bool> EditRole(EditRoleViewModel viewModel)
        {
            if (viewModel.PermissionNames == null || viewModel.PermissionNames.Length < 1)
                return false;
            var role = await ((DbSet<Role>)_roles).FindAsync(viewModel.Id);
            _mappingEngine.Map(viewModel, role);
            role.XmlPermissions = _permissionService.GetPermissionsAsXml(viewModel.PermissionNames);
            return true;
        }

        #endregion EditRole

        #region AddRange

        public void AddRange(IEnumerable<Role> roles)
        {
            _unitOfWork.AddThisRange(roles);
        }

        #endregion AddRange

        #region AnyAsync

        public Task<bool> AnyAsync()
        {
            return _roles.AnyAsync();
        }

        public bool Any()
        {
            return _roles.Any();
        }

        #endregion AnyAsync

        #region AutoCommitEnabled

        public bool AutoCommitEnabled { get; set; }

        #endregion AutoCommitEnabled

        #region CheckForExisByName

        public bool CheckForExisByName(string name, Guid? id)
        {
            var roles = _roles.Select(a => new {a.Id, a.Name }).ToList();
            return id == null ? roles.Any(a => a.Name.GetFriendlyPersianName() == name.GetFriendlyPersianName())
                : roles.Any(a => id.Value != a.Id && a.Name.GetFriendlyPersianName() == name.GetFriendlyPersianName());
        }

        #endregion CheckForExisByName

        #region GetPageList

        public async Task<RoleListViewModel> GetPageList(RoleSearchRequest request)
        {
            var roles = _roles.AsNoTracking().AsQueryable();

            if (request.Term.HasValue())
                roles = roles.Where(a => a.Name.Contains(request.Term));

            var selectedTitles = roles.ProjectTo<RoleViewModel>(_mappingEngine);

            var query = await selectedTitles
                .OrderBy(a => a.Name)
                .Skip((request.PageIndex - 1) * 5)
                .Take(5)
                .ToListAsync();

            query.ForEach(a =>
            {
                if (a.Permissions != null)
                    a.PermissionsList = _permissionService.GetDescriptions(a.XmlPermission);
            });
            return new RoleListViewModel { SearchRequest = request, Roles = query };
        }

        #endregion GetPageList

        #region RemoveById

        public Task RemoveById(Guid id)
        {
            return _roles.Where(a => a.Id == id).DeleteAsync();
        }

        #endregion RemoveById

        #region CheckRoleIsSystemRoleAsync

        public Task<bool> CheckRoleIsSystemRoleAsync(Guid id)
        {
            return _roles.AnyAsync(a => a.Id == id && a.IsSystemRole);
        }

        #endregion CheckRoleIsSystemRoleAsync

        #region GetAllAsSelectList

        public async Task<IEnumerable<SelectListItem>> GetAllAsSelectList()
        {
            var roles =
                await _roles.AsNoTracking().Project(_mappingEngine).To<SelectListItem>().Cacheable().ToListAsync();
            return roles;
        }

        #endregion GetAllAsSelectList

        #region IsInDb

        public Task<bool> IsInDb(Guid id)
        {
            return _roles.AnyAsync(a => a.Id == id);
        }

        #endregion IsInDb

        #region Fill

        public void FillForEdit(EditRoleViewModel viewModel)
        {
            var permissions = AssignableToRolePermissions.GetAsSelectListItems();

            var selectListItems = permissions as IList<SelectListItem> ?? permissions.ToList();
            if (viewModel.PermissionNames != null)
            {
                selectListItems.ToList().ForEach(
                    a => a.Selected = viewModel.PermissionNames.Any(s => s == a.Value));
            }

            viewModel.Permissions = selectListItems;
        }

        #endregion Fill

        #region GetRole

        public async Task<RoleViewModel> GetRole(Guid id)
        {
            var role = await _roles.FirstAsync(r => r.Id == id);
            var viewModel = _mappingEngine.Map<RoleViewModel>(role);
            if (viewModel.Permissions != null)
                viewModel.PermissionsList = _permissionService.GetDescriptions(viewModel.XmlPermission);
            return viewModel;
        }

        #endregion GetRole
    }
}