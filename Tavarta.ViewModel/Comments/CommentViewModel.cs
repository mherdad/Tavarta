using System;

namespace Tavarta.ViewModel.Comments
{
    public class CommentViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// ایمیل
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// نظر
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// نام
        /// </summary>

        public bool IsShow { get; set; }

        public string CreatorDisplayName { get; set; }

        public Guid? ReplyId { get; set; }

        public  DateTime CreatedOn { get; set; }
    }
}