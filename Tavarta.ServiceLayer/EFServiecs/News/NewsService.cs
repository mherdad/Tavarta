using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using EFSecondLevelCache.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Extensions;
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
        private readonly EFCachePolicy _expirationTimeCachePolicy;

        #region Ctor

        public NewsService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _news = _unitOfWork.Set<Post>();
            _slideShow = _unitOfWork.Set<SlideShowImage>();
            _expirationTimeCachePolicy = new EFCachePolicy { AbsoluteExpiration = DateTime.Now.Add(new TimeSpan(0, 0, 20)) };
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
            //_mappingEngine.Map<Post>(viewModel);

            await UpdateViewCountAsync(id);

            var viewModel = await _news.Where(x => x.Id == id)
                .ProjectTo<NewsDetailsViewModel>(_mappingEngine)
                .Cacheable(_expirationTimeCachePolicy)
                .FirstOrDefaultAsync();

            return viewModel;
        }

        #endregion GetLastNewsDetails

        public Task UpdateViewCountAsync(Guid? id)
        {   
                   

            var post = _news.FirstOrDefault(x => x.Id == id);

            if (post == null) throw new ArgumentNullException(nameof(post));
            post.ViewCount = post.ViewCount + 1;
            return _unitOfWork.SaveChangesAsync();
        }

        #region GetPagedList

        /// <summary>
        /// لیست از اخبار
        /// </summary>
        /// <returns>NewsListViewModel</returns>
        public async Task<NewsListViewModel> GetPagedListAsync()
        {
            var news = await GetNewsAsync();

            var sports = await GetSportAsync();

            var environment = await GetEnvironmentAsync();

            var healthEvents = await GetHealthEventAsync();

            var mostViewed = await GetMostViewedAsync();

            var literary = await GetLiteraryAsync();

            var notes = await GetNotesAsync();

            var carousel = await GetCarouselAsync();
            var lastArticle = await GetLastArticleAsync();

            return new NewsListViewModel
            {
                News = news,
                Sports = sports,
                Environment = environment,
                HealthEvents = healthEvents,
                Literary = literary,
                Notes = notes,
                Carousel = carousel,
                MostViewed = mostViewed,
                LastArticle = lastArticle
            };
        }

        private async Task<List<NewsViewModel>> GetNewsAsync()
        {
            var news = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).AsQueryable();
            var query = await news.Where(x=>x.Category.Name !="استان بوشهر" && x.Category.Name !="ایران و جهان")
                .Take(10).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query;
        }

        private async Task<List<NewsViewModel>> GetSportAsync()
        {
            var sportId = Guid.Parse("7ab8bc23-091a-b846-8662-39d7ccf5af64");
            var sport =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == sportId).AsQueryable();
            var query1 = await sport
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query1;
        }

        private async Task<List<NewsViewModel>> GetMostViewedAsync()
        {
            var sport =
                _news.AsNoTracking().OrderByDescending(x => x.ViewCount).AsQueryable();
            var query1 = await sport
                .Take(5).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query1;
        }

        private async Task<List<NewsViewModel>> GetEnvironmentAsync()
        {
            var environmentId = Guid.Parse("f6114793-82fa-8bce-f38a-39d7d04bbb79");
            var environment =
                _news.AsNoTracking()
                .OrderByDescending(x => x.PublishedOn)
                .Where(x => x.CategoryId == environmentId).AsQueryable();
            var query2 = await environment
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query2;
        }

        private async Task<List<NewsViewModel>> GetHealthEventAsync()
        {
            var healthId = Guid.Parse("107e473a-7bc4-43e4-6738-39d7d262fc00");
            var eventsId = Guid.Parse("7ab8bc23-091a-b846-8662-39d7bcf5cf64");
            var healthEvent =
                _news.AsNoTracking()
                    .OrderByDescending(x => x.PublishedOn)
                    .Where(x => x.CategoryId == healthId || x.CategoryId == eventsId)
                    .AsQueryable();
            var query3 = await healthEvent
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query3;
        }

        private async Task<List<NewsViewModel>> GetLiteraryAsync()
        {
            var literaryId = Guid.Parse("551f25b0-3746-0df1-5b55-39d7cd719f28");
            var literary =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == literaryId).AsQueryable();
            var query4 = await literary
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query4;
        }

        private async Task<List<NewsViewModel>> GetNotesAsync()
        {
            var noteId = Guid.Parse("31af9ba5-cf59-25ec-826a-39d7d73cf170");
            var notes =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == noteId).AsQueryable();
            var query5 = await notes
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query5;
        }

        private async Task<List<CarouselViewModel>> GetCarouselAsync()
        {
            var slideShow = _slideShow.AsNoTracking().OrderByDescending(x => x.Order).AsQueryable();
            var query6 = await slideShow
                .Take(6).ProjectTo<CarouselViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query6;
        }

        private async Task<List<LastArticleViewModel>> GetLastArticleAsync()
        {
            var lastArticle = _news.AsNoTracking().Include(x=>x.Category).OrderByDescending(x => x.PublishedOn).AsQueryable();
            var query6 = await lastArticle
                .ProjectTo<LastArticleViewModel>(_mappingEngine).ToListAsync();
            return query6;
        }



        #endregion GetPagedList

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