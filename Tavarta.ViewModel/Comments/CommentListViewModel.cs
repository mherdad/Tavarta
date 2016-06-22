using System;
using System.Collections.Generic;
using Tavarta.DomainClasses.Entities.Comment;

namespace Tavarta.ViewModel.Comments
{
    public class CommentListViewModel
    {
        public IList<CommentViewModel> Comments { get; set; }
        public int TotalCount { get; set; }
        //public IList<CommentViewModel> Comments1 { get; set; }
        //public IList<CommentViewModel> Comments2 { get; set; }
        //public AddCommentsViewModel AddComments { get; set; }
        public Guid PostId { get; set; }
    }
}