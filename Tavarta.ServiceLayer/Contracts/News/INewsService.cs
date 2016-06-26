using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tavarta.ViewModel.News;

namespace Tavarta.ServiceLayer.Contracts.News
{
    public interface  INewsService
    {
        /// <summary>
        /// جزییات آخرین خبر
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>NewsDetailsViewModel</returns>
        Task<NewsDetailsViewModel> GetLastNewsDetailsAsync(Guid? id);

        /// <summary>
        /// لیست از اخبار
        /// </summary>
        /// <returns>NewsListViewModel</returns>
        Task<NewsListViewModel> GetPagedListAsync();

        Task<NewsListViewModel> GetOrderPage(int page, int itemsPerPage, string category);
        Task<ListMostViewedViewModel> GetMostViewedAsync();
        Task<List<NewsViewModel>> GetNotesAsync();
        Task<List<CarouselViewModel>> GetCarouselAsync();
        Task<List<LastArticleViewModel>> GetLastArticleAsync();
    }
}