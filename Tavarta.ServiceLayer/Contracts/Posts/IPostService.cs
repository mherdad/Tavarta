using System;
using System.Threading.Tasks;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.ServiceLayer.Contracts.Posts
{
    public interface IPostService
    {
        Task<PostViewModel> AddPost (AddPostViewModel viewModel);

       
        Task<PostListViewModel> GetPageList(UserSearchRequest search, int page, int itemsPerPage);
        Task<PostListViewModel> GetOrderPage(int page, int itemsPerPage );
        Task<AddPostViewModel> GetForEditAsync(Guid id);
        Task<Post> FindByIdAsync(Guid viewModel);
        Task EditUser(AddPostViewModel viewModel );
        Task<DeleteViewModel> GetDeletePostAsync(Guid id);
        Task DeletePost(Guid id);
        Task<int> TotalPosts();
    }
}