using AutoMapper;
using Tavarta.AutoMapperProfiles.Extentions;
using Tavarta.DomainClasses.Entities.Comment;
using Tavarta.ViewModel.Comments;

namespace Tavarta.AutoMapperProfiles
{
    public class CommentProfile :Profile
    {
        protected override void Configure()
        {
            CreateMap<PostComment, CommentViewModel>()
                .IgnoreAllNonExisting();
            CreateMap<PostComment, AddCommentsViewModel>()
                .IgnoreAllNonExisting();
            CreateMap<AddCommentsViewModel, PostComment>()
               .IgnoreAllNonExisting();
            CreateMap<PostComment, EditCommentViewModel>()
                .IgnoreAllNonExisting();

            CreateMap<PostComment, DeleteCommentViewModel>()
                .IgnoreAllNonExisting();
            CreateMap<EditCommentViewModel, PostComment>()
                .IgnoreAllNonExisting();
        }

        public override string ProfileName => GetType().Name;
    }
}