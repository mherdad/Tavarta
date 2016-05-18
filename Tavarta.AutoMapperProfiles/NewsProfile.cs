using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ViewModel.News;

namespace Tavarta.AutoMapperProfiles
{
    public class NewsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Post, NewsViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Post, NewsDetailsViewModel>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}