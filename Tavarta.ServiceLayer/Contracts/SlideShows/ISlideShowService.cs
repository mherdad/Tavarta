using System.Threading.Tasks;
using Tavarta.ViewModel.SlideShow;

namespace Tavarta.ServiceLayer.Contracts.SlideShows
{
    public interface ISlideShowService
    {
        Task<SlideShowListViewModel> GetPageList();
        Task<SlideShowViewModel> AddSlide(AddSlideShowViewModel viewModel);
    }
}