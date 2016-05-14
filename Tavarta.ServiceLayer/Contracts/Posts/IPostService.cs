using System.Threading.Tasks;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.ServiceLayer.Contracts.Posts
{
    public interface IPostService
    {
        Task<PostViewModel> Create (AddPostViewModel viewModel);
        Task<PostListViewModel> GetPageList(UserSearchRequest search);
    }
}