using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Comment;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.ServiceLayer.Contracts.Comments;
using Tavarta.Utility;
using Tavarta.ViewModel.Comments;

namespace Tavarta.ServiceLayer.EFServiecs.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<PostComment> _comments;
        private readonly IDbSet<Post> _posts;

        public CommentsService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _comments = _unitOfWork.Set<PostComment>();
            _posts = _unitOfWork.Set<Post>();
        }

        public async Task<CommentListViewModel> GetOrderPage(int page, int itemsPerPage)
        {
            var resultsToSkip = page * itemsPerPage;
            var query = await _comments.Join(_posts, comment => comment.PostId, post => post.Id, (comment, post) => new CommentViewModel
            {
                Id = comment.Id,

                Body = comment.Body,
                CreatorDisplayName = comment.CreatorDisplayName,
                Email = comment.Email,
                Title = post.Title,
                CreatedOn = comment.CreatedOn
            }).OrderByDescending(x => x.CreatedOn).ToPagedQuery(itemsPerPage, page).ToListAsync();

            //  var query1 = await _comments.Include(x=>x.Post).OrderByDescending(x => x.CreatedOn)
            //      .ToPagedQuery(itemsPerPage, page)
            //.ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();

            //todo Cache checked with time
            var totalCount = _comments.Select(x => x.Id).Count();//return the number of pages

            return new CommentListViewModel()
            {
                Comments = query,
                TotalCount = totalCount
            };
        }

        public async Task<EditCommentViewModel> EditCommentDetails(Guid id)
        {
            var comment = await _comments.ProjectTo<EditCommentViewModel>(_mappingEngine).FirstOrDefaultAsync(x=>x.Id==id);
            return comment;

        }

        public Task EditComment(EditCommentViewModel viewModel)
        {
            var comment = _comments.Find(viewModel.Id);
            _mappingEngine.Map(viewModel, comment);
            return _unitOfWork.SaveChangesAsync();
        }

        public async Task<CommentListViewModel> GetComments(Guid postId)
        {
            var postComments = await _comments.Where(c => c.ReplyId == null && c.PostId == postId).ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();
            //var tt = await _comments.Where(x => x.ReplyId != null).ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();

            return new CommentListViewModel
            {
                Comments = postComments,
                PostId = postId,
                TotalCount = postComments.Count
            };
        }

        public async Task<CommentViewModel> AddComment(AddCommentsViewModel viewModel)
        {
            var comment = _mappingEngine.Map<PostComment>(viewModel);

            // viewModel.Categorizes.ToList().ForEach(a => a.Selected = post.CategoryId.ToString() == a.Value);

            comment.Id = SequentialGuidGenerator.NewSequentialGuid();
            comment.PostId = viewModel.PostId;
            comment.CreatedOn = DateTime.Now;
            comment.Email = viewModel.Email;
            comment.Body = viewModel.Body;
            comment.CreatorDisplayName = viewModel.Title;

            _comments.Add(comment);

            await _unitOfWork.SaveChangesAsync();
            return await GetCommentViewModel(comment.Id);
        }

        public async Task<Post> FindById(Guid id)
        {
            var post = _posts.Find(id);
            return post;
        }

        public async Task<AddCommentsViewModel> GetPost(Guid postId)
        {
            //var postComments = await _comments.Include(x => x.Post).Where(c => c.ReplyId == null && c.PostId == postId).ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();

            return new AddCommentsViewModel
            {
                Body = "",
                Email = "",
                Title = "",

                PostId = postId
            };
        }

        public Task<CommentViewModel> GetCommentViewModel(Guid guid)
        {
            return _comments
                .ProjectTo<CommentViewModel>(_mappingEngine)
                .FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}