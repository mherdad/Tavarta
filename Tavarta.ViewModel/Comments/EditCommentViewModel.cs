using System;
using System.ComponentModel.DataAnnotations;

namespace Tavarta.ViewModel.Comments
{
    public class EditCommentViewModel
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
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        /// <summary>
        /// نظر
        /// </summary>
        [Display(Name = "نظر")]
        public string Body { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        [Display(Name = "نام و نام خانوادگی")]
        public string CreatorDisplayName { get; set; }
    }
}