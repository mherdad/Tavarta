using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ViewModel.Gallery;

namespace Tavarta.AutoMapperProfiles
{
    public class GalleryProfile :Profile
    {
        protected override void Configure()
        {
            CreateMap<Post, GalleryViewModel>()
                .IgnoreAllNonExisting();
        }

        public override string ProfileName => GetType().Name;
    }
}