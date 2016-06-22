using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Tavarta.Common.Helpers;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.SlideShows;
using Tavarta.ServiceLayer.Contracts.SlideShows;
using Tavarta.Utility;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.SlideShow;

namespace Tavarta.ServiceLayer.EFServiecs.SlideShows
{
    public class SlideShowService :ISlideShowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<SlideShowImage> _slideShow;

        public SlideShowService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _slideShow = unitOfWork.Set<SlideShowImage>();
        }

        public async Task<SlideShowListViewModel> GetPageList()
        {
            var posts = _slideShow.AsNoTracking().OrderBy(x => x.Order).AsQueryable();
            var query = await posts
                .Take(10).ProjectTo<SlideShowItemModel>(_mappingEngine).ToListAsync();

            return new SlideShowListViewModel()
            {
               
                SlideShow = query
            };
        }

        public async Task<SlideShowViewModel> AddSlide(AddSlideShowViewModel viewModel)
        {
            var slide = _mappingEngine.Map<SlideShowImage>(viewModel);
            slide.Id = SequentialGuidGenerator.NewSequentialGuid();
            slide.Image.CleanTags();
            slide.CreatedDate=DateTime.Now;
            _slideShow.Add(slide);

            await _unitOfWork.SaveChangesAsync();
            return await GetSlideShowViewModel(slide.Id);

        }

        public async Task<AddSlideShowViewModel> GetForEditAsync(Guid id)
        {
            var slideShow = await FindByIdAsync(id);

            if (slideShow == null) return null;
            var viewModel = _mappingEngine.Map<AddSlideShowViewModel>(slideShow);

            return viewModel;
        }

        public async Task<SlideShowImage> FindByIdAsync(Guid id)
        {
            var post = await _slideShow.FirstOrDefaultAsync(x => x.Id == id);

            return post;
        }


        public async Task EditSlideShow(AddSlideShowViewModel viewModel)
        {
            //var ff = viewModel.PublishedOn.Date;
            var slideShow = await FindByIdAsync(viewModel.Id);
            _mappingEngine.Map(viewModel, slideShow);

            if (viewModel.Id != Guid.Empty)
            {

                //post.PublishedOn=ff;
                //slideShow. = DateTime.Now;


                await _unitOfWork.SaveChangesAsync();

            }

        }


        public Task<SlideShowViewModel> GetSlideShowViewModel(Guid guid)
        {
            return _slideShow
                .ProjectTo<SlideShowViewModel>(_mappingEngine)
                .FirstOrDefaultAsync(x => x.Id == guid);
        }


        public Task<List<SlideShowItemModel>> GetAll()
        {
            return _slideShow.Select(s => new SlideShowItemModel()
            {
                Id = s.Id,
                Image = s.Image,
                Link = s.Link,
                Order = s.Order,
                Text = s.Text,
                Title = s.Title
            }).ToListAsync();
        }



        public async Task<DeleteSlideShowViewModel> GetDeleteSlideAsync(Guid id)
        {
            var slideShow = _slideShow.AsNoTracking().Where(x => x.Id == id).AsQueryable();
            var viewModel = await _slideShow
                .ProjectTo<DeleteSlideShowViewModel>(_mappingEngine).FirstOrDefaultAsync();
            return viewModel;
        }

        public async Task DeleteSlide(Guid id)
        {
            var slide =await FindByIdAsync(id);
            _slideShow.Remove(slide);
             await _unitOfWork.SaveChangesAsync();
        }


        public Task<SlideShowItemModel> Get(Guid slideShowItemId)
        {
            return _slideShow.Where(s => s.Id == slideShowItemId).Select(s => new SlideShowItemModel()
            {
                Id = s.Id,
                Image = s.Image,
                Link = s.Link,
                Order = s.Order,
                Text = s.Text,
                Title = s.Title
            }).FirstOrDefaultAsync();
        }


        public IList<SlideShowItemModel> GetSliderImage()
        {
            return _slideShow.AsNoTracking().Select(s => new SlideShowItemModel()
            {
                Id = s.Id,
                Image = s.Image,
                Link = s.Link,
                Order = s.Order,
                Text = s.Text,
                Title = s.Title
            }).OrderByDescending(s => s.Order).Cacheable().ToList();
        }

    }
}