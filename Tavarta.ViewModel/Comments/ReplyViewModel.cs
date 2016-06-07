using System;

namespace Tavarta.ViewModel.Comments
{
    public class ReplyViewModel
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

        public string CreatorDisplayName { get; set; }

        public Guid? ReplyId { get; set; }
    }
}