using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tavarta.DomainClasses.Entities.Comment;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ViewModel.Comments;
using Tavarta.ViewModel.News;

namespace Tavarta.ServiceLayer.Contracts.Comments
{
    public interface  ICommentsService
    {
        Task<CommentListViewModel> GetOrderPage(int page, int itemsPerPage);
        Task<CommentListViewModel> GetComments(Guid postId);
        Task<CommentViewModel> AddComment(AddCommentsViewModel viewModel);
        Task<AddCommentsViewModel> GetPost(Guid postId);
        Task<Post> FindById(Guid id);
        Task<EditCommentViewModel> EditCommentDetails(Guid id);
        Task EditComment(EditCommentViewModel viewModel);
    }
}