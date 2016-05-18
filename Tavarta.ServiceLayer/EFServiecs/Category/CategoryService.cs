using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ServiceLayer.Contracts.Category;
using Tavarta.ViewModel.Posts;


namespace Tavarta.ServiceLayer.EFServiecs.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<DomainClasses.Entities.Postes.Category> _categories;
        private readonly IDbSet<Post> _posts;

        public CategoryService(IUnitOfWork unitOfWork ,IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _categories = _unitOfWork.Set<DomainClasses.Entities.Postes.Category>();
        }



        public async Task<IEnumerable<SelectListItem>> GetAllAsSelectList()
        {

            var categories =
               await _categories.AsNoTracking().Project(_mappingEngine).To<SelectListItem>().Cacheable().ToListAsync();
            return categories;
        }
    }
}