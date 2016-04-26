using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ViewModel.User;

namespace Tavarta.AutoMapperProfiles
{
  public   class UserProfile :Profile
    {
        protected override void Configure()
        {
            CreateMap<User, UserViewModel>()

                .IgnoreAllNonExisting();

            CreateMap<AddUserViewModel, User>()

                .IgnoreAllNonExisting();

            CreateMap<EditUserViewModel, User>()
                .ForMember(d => d.Roles, m => m.Ignore())
                 .IgnoreAllNonExisting();

            CreateMap<User, EditUserViewModel>().ForMember(d => d.Roles, m => m.Ignore()).IgnoreAllNonExisting();

            CreateMap<User, SelectListItem>()
               .ForMember(d => d.Text, m => m.MapFrom(s => s.UserName))
               .ForMember(d => d.Value, m => m.MapFrom(s => s.Id)).IgnoreAllNonExisting();
        }

        public override string ProfileName
        {
            get { return GetType().Name; }
        }
    }
}
