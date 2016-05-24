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
            post.ViewCount += 1;
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
            var photoGallery = await GetPhotoGalleryAsync();

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
                LastArticle = lastArticle,
                PhotoGallery = photoGallery
            };
        }

        private async Task<List<NewsViewModel>> GetNewsAsync()
        {
            var ostan = "استان بوشهر";
            var iran = "ایران و جهان";
            var news = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).AsQueryable();
            var query = await news.Where(x=>x.Category.Name !=ostan && x.Category.Name !=iran)
                .Take(10).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query;
        }

        private async Task<List<NewsViewModel>> GetSportAsync()
        {
            var sportName = "ورزشی";
            var sport =
                _news.AsNoTracking().Include(x=>x.Category).OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == sportName).AsQueryable();
            var query1 = await sport
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query1;
        }

        private async Task<List<NewsViewModel>> GetPhotoGalleryAsync()
        {
            var gallery = "گالری تصاویر";
            var sport =
                _news.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == gallery).AsQueryable();
            var query1 = await sport
                .Take(7).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
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
            var environmentName = "محیط زیست";
            var environment =
                _news.AsNoTracking()
                .OrderByDescending(x => x.PublishedOn)
                .Where(x => x.Category.Name == environmentName ).AsQueryable();
            var query2 = await environment
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query2;
        }

        private async Task<List<NewsViewModel>> GetHealthEventAsync()
        {
            var healthName = "سلامت";
            var eventsName = "حوادث";
            var healthEvent =
                _news.AsNoTracking()
                    .OrderByDescending(x => x.PublishedOn)
                    .Where(x => x.Category.Name == healthName || x.Category.Name == eventsName)
                    .AsQueryable();
            var query3 = await healthEvent
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query3;
        }

        private async Task<List<NewsViewModel>> GetLiteraryAsync()
        {
            var literaryName = "ادبی";
            var literary =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == literaryName).AsQueryable();
            var query4 = await literary
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query4;
        }

        private async Task<List<NewsViewModel>> GetNotesAsync()
        {
            var noteName = "یادداشت ها";
            var notes =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == noteName).AsQueryable();
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
               .Take(30) .ProjectTo<LastArticleViewModel>(_mappingEngine).ToListAsync();
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