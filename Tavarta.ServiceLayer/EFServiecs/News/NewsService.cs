using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.DomainClasses.Entities.SlideShows;
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
        private readonly IDbSet<SlideShowImage> _slideShow;
        

#region Ctor
        public NewsService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _news = _unitOfWork.Set<Post>();
            _slideShow = _unitOfWork.Set<SlideShowImage>();
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

            var sportId = Guid.Parse("7ab8bc23-091a-b846-8662-39d7ccf5af64");
            var sport = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId ==sportId).AsQueryable();
            var query1 = await sport
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();

            var environmentId = Guid.Parse("f6114793-82fa-8bce-f38a-39d7d04bbb79");
            var environment = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == environmentId).AsQueryable();
            var query2 = await environment
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();

            var healthId = Guid.Parse("107e473a-7bc4-43e4-6738-39d7d262fc00");
            var eventsId = Guid.Parse("7ab8bc23-091a-b846-8662-39d7bcf5cf64");
            var healthEvent = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == healthId || x.CategoryId== eventsId).AsQueryable();
            var query3 = await healthEvent
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();



            var literaryId = Guid.Parse("551f25b0-3746-0df1-5b55-39d7cd719f28");
            var literary = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == literaryId ).AsQueryable();
            var query4 = await literary
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();

            var noteId = Guid.Parse("31af9ba5-cf59-25ec-826a-39d7d73cf170");
            var notes = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == noteId).AsQueryable();
            var query5 = await notes
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();

            
            var slideShow = _slideShow.AsNoTracking().OrderByDescending(x => x.Order).AsQueryable();
            var query6 = await slideShow
                .Take(6).ProjectTo<CarouselViewModel>(_mappingEngine).ToListAsync();


            return new NewsListViewModel
            {
                News = query,
                Sports = query1,
                Environment = query2,
                HealthEvents = query3,
                Literary = query4,
                Notes = query5,
                Carousel = query6
            };
        }

        #endregion  GetPagedList


        public async Task<NewsListViewModel> GetPagedSportNewsAsync(Guid id)
        {
            var sport = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == id).AsQueryable();
            var query = await sport
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();
            return new NewsListViewModel
            {
                Sports = query
            };
        }

    }
}