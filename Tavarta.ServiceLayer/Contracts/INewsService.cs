using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavarta.ViewModel.News;

namespace Tavarta.ServiceLayer.Contracts
{
   public interface INewsService
    {
       Task<NewsDetailsViewModel> GetLastNewsDetailsAsync(Guid? id);
       Task<NewsListViewModel> GetPagedListAsync();
       Task<NewsListViewModel> GetPagedSportNewsAsync(Guid id);
       Task<NewsListViewModel> GetOrderPage(int page, int itemsPerPage, string category);
       Task<List<NewsViewModel>> GetNotesAsync();
       Task<List<CarouselViewModel>> GetCarouselAsync();
       Task<List<LastArticleViewModel>> GetLastArticleAsync();
    }
}
