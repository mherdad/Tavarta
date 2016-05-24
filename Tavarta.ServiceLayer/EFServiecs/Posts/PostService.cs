using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EFSecondLevelCache;
using EFSecondLevelCache.Contracts;
using EntityFramework.Extensions;
using Tavarta.Common.Extentions;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ServiceLayer.Contracts.Category;
using Tavarta.ServiceLayer.Contracts.Posts;
using Tavarta.ServiceLayer.Contracts.Users;
using Tavarta.Utility;
using Tavarta.ViewModel.Posts;
using Tavarta.ViewModel.User;

namespace Tavarta.ServiceLayer.EFServiecs.Posts
{
    public class PostService : IPostService
    {
        #region Fileds

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IApplicationUserManager _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IDbSet<Post> _posts;

        #endregion Fileds

        #region ctor

        public PostService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine, IApplicationUserManager userManager, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _userManager = userManager;
            _categoryService = categoryService;
            _posts = _unitOfWork.Set<Post>();
        }

        #endregion ctor

        #region AddPost

        public async Task<PostViewModel> AddPost(AddPostViewModel viewModel)
        {
            var post = _mappingEngine.Map<Post>(viewModel);

            // viewModel.Categorizes.ToList().ForEach(a => a.Selected = post.CategoryId.ToString() == a.Value);
            post.CategoryId = viewModel.CategoryId;
            post.Id = SequentialGuidGenerator.NewSequentialGuid();
            //var post = new Post();
            //post.Body = "hgfdsdhgfd";
            //post.Title = "hghfds";
            //post.TagNames = "fghjk";
            //post.Headline = "dfghjk";
            post.PublishedOn = DateTime.Now;
            //post.Category.Id=viewModel.Categorizes.;
            post.LinkBackStatus = LinkBackStatus.Disable;
            post.DaysCountForSupportComment = 12;
            post.ShowWithRss = true;
            post.Agent = "ali";
            post.AllowCommentForAnonymous = true;
            post.ApprovedCommentsCount = 1;
            post.CanonicalUrl = "asfds";
            post.FocusKeyword = "gf";
            post.IsDeleted = false;
            post.MetaTitle = "gfd";
            post.ModifiedOn = DateTime.Now;
            post.SlugUrl = "fddf";

            _posts.Add(post);

            await _unitOfWork.SaveChangesAsync();
            return await GetPostViewModel(post.Id);
        }

        public Task<PostListViewModel> GetPageList(UserSearchRequest search, int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<PostViewModel> GetPostViewModel(Guid guid)
        {
            return _posts
                .ProjectTo<PostViewModel>(_mappingEngine)
                .FirstOrDefaultAsync(x => x.Id == guid);
        }

        #endregion AddPost

        public async Task<PostListViewModel> GetPageList(UserSearchRequest search)
        {
            //var posts = _posts.AsNoTracking().OrderBy(x => x.PublishedOn).AsQueryable();
            //var query = await posts
            //    .Skip((search.PageIndex - 1) * 10).Take(10).ProjectTo<PostViewModel>(_mappingEngine).ToListAsync();

            var posts = _posts.AsNoTracking().OrderBy(x => x.PublishedOn).AsQueryable();
            int total;
            var query = await posts
              .ProjectTo<PostViewModel>(_mappingEngine).ToListAsync();

            return new PostListViewModel
            {
                SearchRequest = search,
                Posts = query
            };
        }

        public async Task<PostListViewModel> GetOrderPage(int page, int itemsPerPage )
        {
            var resultsToSkip = page *itemsPerPage;
            var query1 = await _posts.OrderByDescending(x=>x.PublishedOn)
                .ToPagedQuery(itemsPerPage,page)          
          .ProjectTo<PostViewModel>(_mappingEngine).ToListAsync();

           //todo Cache checked with time
          var  totalCount = _posts.Select(x=>x.Id).Count();//return the number of pages

            return new PostListViewModel
            {
                Posts = query1,TotalCount = totalCount
            };
        }

        public async Task<PostListViewModel> GetPaged(UserSearchRequest search, int pageNumber, int pageSize)
        {
            var posts = _posts.AsNoTracking().OrderBy(x => x.PublishedOn).AsQueryable();
            var query = await posts
                .ProjectTo<PostViewModel>(_mappingEngine).ToListAsync();

            //var posts = _posts.AsNoTracking().OrderBy(x => x.PublishedOn).AsQueryable();
            //int total;
            //var query = await posts
            //  .PagedResult(pageNumber, pageSize, x => x.PublishedOn, false, out total).ProjectTo<PostViewModel>(_mappingEngine).ToListAsync();

            return new PostListViewModel
            {
                SearchRequest = search,
                Posts = query
            };
        }


        #region GetForEditAsync
        public async Task<AddPostViewModel> GetForEditAsync(Guid id)
        {

            var post = await FindByIdAsync(id);
                
            if (post == null) return null;
            var viewModel = _mappingEngine.Map<AddPostViewModel>(post);
           
            return viewModel;
        }

        #endregion

        public async Task<Post> FindByIdAsync(Guid id)
        {
            var post = await _posts.FirstOrDefaultAsync(x => x.Id == id);

            return post;
        }


        public async Task EditUser(AddPostViewModel viewModel )
        {
            //var ff = viewModel.PublishedOn.Date;
            var post = await FindByIdAsync(viewModel.Id);
            _mappingEngine.Map(viewModel, post);

            if (viewModel.CategoryId != Guid.Empty)
            {

                //post.PublishedOn=ff;
                post.ModifiedOn=DateTime.Now;
                

                await _unitOfWork.SaveChangesAsync();
              
            }

        }



    }
}