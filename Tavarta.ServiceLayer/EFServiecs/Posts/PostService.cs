using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ServiceLayer.Contracts.Posts;
using Tavarta.ServiceLayer.Contracts.Users;
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
        private readonly IDbSet<Post> _posts;

        #endregion Fileds

        #region ctor

        public PostService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine, IApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _userManager = userManager;
            _posts = _unitOfWork.Set<Post>();
        }

        #endregion ctor

        #region AddPost

        public async Task<PostViewModel> AddPost(AddPostViewModel viewModel)
        {
            var post = _mappingEngine.Map<Post>(viewModel);
            //var post = new Post();
            //post.Body = "hgfdsdhgfd";
            //post.Title = "hghfds";
            //post.TagNames = "fghjk";
            //post.HeadTitle = "dfghjk";
            post.PublishedOn=DateTime.Now;
            post.Category=new Category {Name="temp"};
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
            post.ModifiedOn=DateTime.Now;
            post.SlugUrl = "fddf";
            

            _posts.Add(post);
            
            await _unitOfWork.SaveChangesAsync();
            return await GetPostViewModel(post.Id);
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
            var posts = _posts.AsNoTracking().OrderBy(x => x.PublishedOn).AsQueryable();
            var query = await posts
                .Skip((search.PageIndex - 1) * 10).Take(10).ProjectTo<PostViewModel>(_mappingEngine).ToListAsync();

            return new PostListViewModel
            {
                SearchRequest = search,
                Posts = query
            };
        }
    }
}