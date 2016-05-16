using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.ViewModel.Posts;

namespace Tavarta.AutoMapperProfiles
{
    public class PostProfile:Profile
    {
        protected override void Configure()
        {
            CreateMap<AddPostViewModel, Post>()
                .IgnoreAllNonExisting();
            //CreateMap<User, PostViewModel>()
            //    .IgnoreAllNonExisting();
            CreateMap<Post, AddPostViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<Post, PostViewModel>()
                
                .IgnoreAllNonExisting();
        }

        public override string ProfileName => GetType().Name;
    }
}