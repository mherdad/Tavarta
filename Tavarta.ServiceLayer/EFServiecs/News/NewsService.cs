using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ServiceLayer.Contracts;
using Tavarta.ViewModel.News;

namespace Tavarta.ServiceLayer.EFServiecs.News
{
    /// <summary>
    /// سرویس اخبار
    /// </summary>
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Post> _news;

#region Ctor
        public NewsService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _news = _unitOfWork.Set<Post>();
        }
        #endregion Ctor


        #region GetLastNewsDetails
        /// <summary>
        /// جزییات آخرین خبر
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>NewsDetailsViewModel</returns>
        public async Task<NewsDetailsViewModel> GetLastNewsDetailsAsync(Guid? id)
        {
            var viewModel = await _news.Where(x => x.Id == id)
                .ProjectTo<NewsDetailsViewModel>(_mappingEngine)
                .FirstOrDefaultAsync();
            return viewModel;
        }
        #endregion GetLastNewsDetails


        #region  GetPagedList
        /// <summary>
        /// لیست از اخبار
        /// </summary>
        /// <returns>NewsListViewModel</returns>
        public async Task<NewsListViewModel> GetPagedListAsync()
        {
            var news = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).AsQueryable();
            var query = await news
                .Take(10).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();
            return new NewsListViewModel
            {
                News = query
            };
        }

        #endregion  GetPagedList
    }
}