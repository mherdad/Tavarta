using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tavarta.DataLayer.Context;
using Tavarta.DomainClasses.Entities.Comment;
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

        public CommentsService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _comments = _unitOfWork.Set<PostComment>();
        }

        public async Task<CommentListViewModel> GetOrderPage(int page, int itemsPerPage)
        {
            var resultsToSkip = page * itemsPerPage;
            var query1 = await _comments.OrderByDescending(x => x.CreatedOn)
                .ToPagedQuery(itemsPerPage, page)
          .ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();

            //todo Cache checked with time
            var totalCount = _comments.Select(x => x.Id).Count();//return the number of pages

            return new CommentListViewModel()
            {
                Comments = query1,
                TotalCount = totalCount
            };
        }

        public async Task<CommentListViewModel> GetComments(Guid postId)
        {

            var postComments = await _comments.Where(c => c.ReplyId == null && c.PostId== postId).ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();
            //var tt = await _comments.Where(x => x.ReplyId != null).ProjectTo<CommentViewModel>(_mappingEngine).ToListAsync();
            return new CommentListViewModel
            {
                Comments = postComments,
                PostId = postId
            };
            
        }


        public async Task<CommentViewModel> AddComment(AddCommentsViewModel viewModel, Guid postId)
        {
            var comment = _mappingEngine.Map<PostComment>(viewModel);

            // viewModel.Categorizes.ToList().ForEach(a => a.Selected = post.CategoryId.ToString() == a.Value);
            
            comment.Id = SequentialGuidGenerator.NewSequentialGuid();
            comment.PostId = postId;
            comment.CreatedOn=DateTime.Now;
            comment.Email = viewModel.Email;
            comment.Body = viewModel.Body;
            comment.CreatorDisplayName = viewModel.Title;
            
         

            _comments.Add(comment);

            await _unitOfWork.SaveChangesAsync();
            return await GetCommentViewModel(comment.Id);
        }

        public Task<CommentViewModel> GetCommentViewModel(Guid guid)
        {
            return _comments
                .ProjectTo<CommentViewModel>(_mappingEngine)
                .FirstOrDefaultAsync(x => x.Id == guid);
        }


    }
}