using System.Threading.Tasks;
using Tavarta.ViewModel.Gallery;

namespace Tavarta.ServiceLayer.Contracts.Galleryes
{
    public interface IGalleryService
    {
        Task<GalleryListViewModel> GetOrderPage(int page, int itemsPerPage, string category);
    }
}