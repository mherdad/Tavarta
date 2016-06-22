using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.SlideShows;
using Tavarta.ViewModel.SlideShow;

namespace Tavarta.AutoMapperProfiles
{
    public class SlideShowProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<SlideShowImage, SlideShowListViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<SlideShowImage, SlideShowViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<SlideShowImage, AddSlideShowViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<SlideShowImage, DeleteSlideShowViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<AddSlideShowViewModel, SlideShowImage>()
               .IgnoreAllNonExisting();


            CreateMap<SlideShowImage, SlideShowItemModel>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}