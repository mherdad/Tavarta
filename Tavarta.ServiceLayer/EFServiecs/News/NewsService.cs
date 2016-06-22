using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using EFSecondLevelCache.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.DomainClasses.Entities.SlideShows;
using Tavarta.ServiceLayer.Contracts;
using Tavarta.Utility;
using Tavarta.ViewModel.News;
using System.Net;

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


        private Post  FindById(Guid? id)
        {
            var ff = _news.Find(id);
            if (ff == null)
                return null;
            return new Post();

        }


        /// <summary>
        /// جزییات آخرین خبر
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>NewsDetailsViewModel</returns>
        public async Task<NewsDetailsViewModel> GetLastNewsDetailsAsync(Guid? id)
        {
            //_mappingEngine.Map<Post>(viewModel);
            if (FindById(id) == null)
                return null;


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



        public async Task<NewsListViewModel> GetPagedListAsync2()
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






        public async Task<NewsListViewModel> GetOrderPage(int page, int itemsPerPage, string category)
        {
            List<NewsViewModel> query;
            int totalCount = 0;
            switch (category)
            {
                case "اخبار":
                    totalCount = _news.Select(x => x.Id).Count();//return the number of pages
                    query = await _news.OrderByDescending(x => x.PublishedOn)
                        .ToPagedQuery(itemsPerPage, page)
                        .ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();
                    break;
                case "سلامت و حوادث":
                    query = await _news.OrderByDescending(x => x.PublishedOn).Where(x=>x.Category.Name=="سلامت" || x.Category.Name=="حوادث")
                        .ToPagedQuery(itemsPerPage, page)
                        .ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();
                    totalCount = _news.Where(x => x.Category.Name == "سلامت" || x.Category.Name == "حوادث").Select(x => x.Id).Count();//return the number of pages
                    break;
                default:
                    query = await _news.OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == category)
                        .ToPagedQuery(itemsPerPage, page)
                        .ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();
                    totalCount = _news.Where(x => x.Category.Name == category).Select(x => x.Id).Count();//return the number of pages
                    break;
            }

            
            //todo Cache checked with time
            //var totalCount = _news.Where(x=>x.Category.Name==category).Select(x => x.Id).Count();//return the number of pages

            return new NewsListViewModel()
            {
                News = query,
                TotalCount = totalCount
            };
        }

        private async Task<List<NewsViewModel>> GetNewsAsync()
        {
            var ostan = "استان ";
            var iran = "ایران و جهان";
            var news = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).AsQueryable();
            var query = await news.Where(x => x.Category.Name != ostan && x.Category.Name != iran)
                .Take(10).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return query;
        }

        private async Task<List<NewsViewModel>> GetSportAsync()
        {
            var sportName = "ورزشی";
            var sport =
                _news.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == sportName).AsQueryable();
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
                .Where(x => x.Category.Name == environmentName).AsQueryable();
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
            var lastArticle = _news.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.PublishedOn).AsQueryable();
            var query6 = await lastArticle
               .Take(30).ProjectTo<LastArticleViewModel>(_mappingEngine).ToListAsync();
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