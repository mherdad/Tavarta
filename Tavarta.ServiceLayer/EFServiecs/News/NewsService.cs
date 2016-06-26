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
using Tavarta.ServiceLayer.Contracts.News;

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
            var news = _news.Find(id);
            if (news == null)
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

            //var mostViewed = await GetMostViewedAsync();

            var literary = await GetLiteraryAsync();

            //var notes = await GetNotesAsync();

            var carousel = await GetCarouselAsync();
            //var lastArticle = await GetLastArticleAsync();
            var photoGallery = await GetPhotoGalleryAsync();

            return new NewsListViewModel
            {
                News = news,
                Sports = sports,
                Environment = environment,
                HealthEvents = healthEvents,
                Literary = literary,
                //Notes = notes,
                Carousel = carousel,
                //MostViewed = mostViewed,
                //LastArticle = lastArticle,
                PhotoGallery = photoGallery
            };
        }



        public async Task<NewsListViewModel> GetPagedListAsync2()
        {
            var news = await GetNewsAsync();

            var sports = await GetSportAsync();

            var environment = await GetEnvironmentAsync();

            var healthEvents = await GetHealthEventAsync();

            //var mostViewed = await GetMostViewedAsync();

            var literary = await GetLiteraryAsync();

            //var notes = await GetNotesAsync();

            //var carousel = await GetCarouselAsync();
            //var lastArticle = await GetLastArticleAsync();
            var photoGallery = await GetPhotoGalleryAsync();

            return new NewsListViewModel
            {
                News = news,
                Sports = sports,
                Environment = environment,
                HealthEvents = healthEvents,
                Literary = literary,
                //Notes = notes,
                //Carousel = carousel,
                //MostViewed = mostViewed,
                //LastArticle = lastArticle,
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
                        .ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
                    break;
                case "سلامت و حوادث":
                    query = await _news.OrderByDescending(x => x.PublishedOn).Where(x=>x.Category.Name=="سلامت" || x.Category.Name=="حوادث")
                        .ToPagedQuery(itemsPerPage, page)
                        .ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
                    totalCount = _news.Where(x => x.Category.Name == "سلامت" || x.Category.Name == "حوادث").Select(x => x.Id).Count();//return the number of pages
                    break;
                default:
                    query = await _news.OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == category)
                        .ToPagedQuery(itemsPerPage, page)
                        .ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
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
            var photoGalleryModel = await sport
                .Take(7).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return photoGalleryModel;
        }

        public async Task<ListMostViewedViewModel> GetMostViewedAsync()
        {
            var newsWeek = await GetByWeek();
            var newsMonth = await GetByMonth();
            var newsYear = await GetByYear();
            return new ListMostViewedViewModel
            {
                MostViewedMonth = newsMonth,
                MostViewedWeek = newsWeek,
                MostViewedYear = newsYear
            };
        }


        private async Task<List<MostViewedViewModel>> GetByWeek()
        {
            var week = DateTime.Now.AddDays(-7);
            var expirationQuery = new EFCachePolicy() { AbsoluteExpiration = DateTime.Now.Add(new TimeSpan(1,0,0)) };
            var news =
                _news.AsNoTracking().OrderByDescending(x => x.ViewCount).Where(x => x.PublishedOn >= week).AsQueryable();
            var mostViewedModel = await news
                .Take(5).ProjectTo<MostViewedViewModel>(_mappingEngine).Cacheable().ToListAsync();
            return mostViewedModel;
        }

        private async Task<List<MostViewedViewModel>> GetByMonth()
        {
            var month = DateTime.Now.AddDays(-31);
            var expirationQuery = new EFCachePolicy() { AbsoluteExpiration = DateTime.Now.Add(new TimeSpan(10,0,0)) };
            var news =
                _news.AsNoTracking().OrderByDescending(x => x.ViewCount).Where(x => x.PublishedOn >= month).Cacheable().AsQueryable();
            var mostViewedModel = await news
                .Take(5).ProjectTo<MostViewedViewModel>(_mappingEngine).ToListAsync();
            return mostViewedModel;
        }

        private async Task<List<MostViewedViewModel>> GetByYear()
        {
            var year = DateTime.Now.AddDays(-365);
            var expirationQuery = new EFCachePolicy() { AbsoluteExpiration = DateTime.Now.Add(new TimeSpan(10,0,0)) };
            var news =
                _news.AsNoTracking().OrderByDescending(x => x.ViewCount).Where(x => x.PublishedOn >= year).Cacheable().AsQueryable();
            var mostViewedModel = await news
                .Take(5).ProjectTo<MostViewedViewModel>(_mappingEngine).ToListAsync();
            return mostViewedModel;
        }


        private async Task<List<NewsViewModel>> GetEnvironmentAsync()
        {
            var environmentName = "محیط زیست";
            var environment =
                _news.AsNoTracking()
                .OrderByDescending(x => x.PublishedOn)
                .Where(x => x.Category.Name == environmentName).AsQueryable();
            var environmentViewModel = await environment
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return environmentViewModel;
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
            var healthEventViewModel = await healthEvent
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return healthEventViewModel;
        }

        private async Task<List<NewsViewModel>> GetLiteraryAsync()
        {
            var literaryName = "ادبی";
            var literary =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == literaryName).AsQueryable();
            var literaryViewModel = await literary
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return literaryViewModel;
        }

        public async Task<List<NewsViewModel>> GetNotesAsync()
        {
            var noteName = "یادداشت ها";
            var notes =
                _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.Category.Name == noteName).AsQueryable();
            var notesViewModel = await notes
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return notesViewModel;
        }

        public async Task<List<CarouselViewModel>> GetCarouselAsync()
        {
            var slideShow = _slideShow.AsNoTracking().OrderByDescending(x => x.Order).AsQueryable();
            var slideViewModel = await slideShow
                .Take(6).ProjectTo<CarouselViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return slideViewModel;
        }

        public async Task<List<LastArticleViewModel>> GetLastArticleAsync()
        {
            var lastArticle =
                 _news.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.PublishedOn).AsQueryable();
            //.GetViewModelAsync(new LastArticleViewModel(),30,_expirationTimeCachePolicy,_mappingEngine);
            //var t= await GetViewModel(30, new LastArticleViewModel(), lastArticle, _expirationTimeCachePolicy);
            var lastArticleViewModels = await lastArticle
               .Take(30).ProjectTo<LastArticleViewModel>(_mappingEngine).Cacheable(_expirationTimeCachePolicy).ToListAsync();
            return lastArticleViewModels;
        }

        //public async Task<List<T>> GetViewModel<T, TE>(int count, T viewModel, IQueryable<TE> query, EFCachePolicy expirationTimePolicy) where TE : class 
        //{
        //    var listViewModel = await query
        //        .Take(count).ProjectTo<T>(_mappingEngine).Cacheable(expirationTimePolicy).ToListAsync();
        //    return listViewModel;
        //}

        #endregion GetPagedList

        public async Task<NewsListViewModel> GetPagedSportNewsAsync(Guid id)
        {
            var sport = _news.AsNoTracking().OrderByDescending(x => x.PublishedOn).Where(x => x.CategoryId == id).AsQueryable();
            var sportViewModel = await sport
                .Take(4).ProjectTo<NewsViewModel>(_mappingEngine).ToListAsync();
            return new NewsListViewModel
            {
                Sports = sportViewModel
            };
        }
    }

    public static class AutoMapperHelper
    {
        public static async Task<List<TE>> GetViewModelAsync<T,TE>(this IQueryable<T> query,TE viewModel, int count, EFCachePolicy cachePolicy,IMappingEngine mapEngine)
        {
            var listViewModel = await query
               .Take(count).ProjectTo<TE>(mapEngine).Cacheable(cachePolicy).ToListAsync();
            return listViewModel;
        }
        public static async Task<List<TE>> GetViewModelAsync<T,TE>(this IQueryable<T> query,TE viewModel, int count, IMappingEngine mapEngine)
        {
            var listViewModel = await query
               .Take(count).ProjectTo<TE>(mapEngine).ToListAsync();
            return listViewModel;
        }
    }

}