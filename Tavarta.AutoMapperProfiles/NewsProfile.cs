using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.DomainClasses.Entities.SlideShows;
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
            CreateMap<Post, LastArticleViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<SlideShowImage, CarouselViewModel>()
                .IgnoreAllNonExisting();
        }

        public override string ProfileName => GetType().Name;
    }
}