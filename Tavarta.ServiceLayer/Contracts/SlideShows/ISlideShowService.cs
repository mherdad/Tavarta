using System;
using System.Threading.Tasks;
using Tavarta.DomainClasses.Entities.SlideShows;
using Tavarta.ViewModel.SlideShow;

namespace Tavarta.ServiceLayer.Contracts.SlideShows
{
    public interface ISlideShowService
    {
        Task<SlideShowListViewModel> GetPageList();
        Task<SlideShowViewModel> AddSlide(AddSlideShowViewModel viewModel);
        Task<AddSlideShowViewModel> GetForEditAsync(Guid id);
        Task<SlideShowImage> FindByIdAsync(Guid id);
        Task EditSlideShow(AddSlideShowViewModel viewModel);
        Task<DeleteSlideShowViewModel> GetDeleteSlideAsync(Guid id);
        Task DeleteSlide(Guid id);
    }
}