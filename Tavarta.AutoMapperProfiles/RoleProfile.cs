using System.Web.Mvc;
using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ViewModel.Role;

namespace Tavarta.AutoMapperProfiles
{
    public class RoleProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Role, RoleViewModel>().ForMember(d=>d.PermissionsList,m=>m.Ignore())
                .ForMember(d=>d.XmlPermission,m=>m.Ignore()).IgnoreAllNonExisting();

            CreateMap<AddRoleViewModel, Role>()
                .IgnoreAllNonExisting();

            CreateMap<RoleViewModel, Role>().IgnoreAllNonExisting();

            CreateMap<EditRoleViewModel, Role>()
                .ForMember(d => d.IsSystemRole, m => m.Ignore())
               .IgnoreAllNonExisting();

            CreateMap<Role, EditRoleViewModel>().ForMember(d=>d.Permissions,m=>m.Ignore()).IgnoreAllNonExisting();
            CreateMap<Role, SelectListItem>()
                .ForMember(d => d.Text, m => m.MapFrom(s => s.Name))
                .ForMember(d => d.Value, m => m.MapFrom(s => s.Id)).IgnoreAllNonExisting();
        }

        public override string ProfileName => this.GetType().Name;
    }
}
