using System.Threading.Tasks;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.ServiceLayer.Contracts.Posts
{
    public interface IPostService
    {
        Task<PostViewModel> AddPost (AddPostViewModel viewModel);
        Task<PostListViewModel> GetPageList(UserSearchRequest search);
    }
}