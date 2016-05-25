using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ServiceLayer.Contracts.Galleryes;
using Tavarta.Utility;
using Tavarta.ViewModel.Gallery;
using Tavarta.ViewModel.News;

namespace Tavarta.ServiceLayer.EFServiecs.Galleryes
{
    public class GalleryService : IGalleryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Post> _gallery;

        public GalleryService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _gallery = _unitOfWork.Set<Post>();
        }

        public async Task<GalleryListViewModel> GetOrderPage(int page, int itemsPerPage, string category)
        {
            
            List<GalleryViewModel> query;
            query = await _gallery.OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == "گالری تصاویر")
                .ToPagedQuery(itemsPerPage, page)
                .ProjectTo<GalleryViewModel>(_mappingEngine).ToListAsync();

            //todo Cache checked with time
            var totalCount =
                _gallery.Include(x => x.Category)
                    .Where(x => x.Category.Name == "گالری تصاویر")
                    .Select(x => x.Id)
                    .Count(); //return the number of pages

            return new GalleryListViewModel
            {
                PhotoGallery = query,
                TotalCount = totalCount
            };
        }
    }
}