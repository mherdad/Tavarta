using System.Web.Mvc;
using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ViewModel.Posts;

namespace Tavarta.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Post, PostViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Category, SelectListItem>()
               .ForMember(d => d.Text, m => m.MapFrom(s => s.Name))
               .ForMember(d => d.Value, m => m.MapFrom(s => s.Id)).IgnoreAllNonExisting();
        }

        public override string ProfileName => GetType().Name;
    }
}