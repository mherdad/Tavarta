using System;
using System.ComponentModel.DataAnnotations;

namespace Tavarta.ViewModel.Comments
{
    public class AddCommentsViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>

            [Display (Name = "نام")]
        public string Title { get; set; }
        /// <summary>
        /// ایمیل
        /// </summary>

        [Display(Name = "ایمیل",Prompt = "ایمیل")]
        public string Email { get; set; }
        /// <summary>
        /// نظر
        /// </summary>
        [Display(Name = "نظر", Prompt = "نظر")]
        public string Body { get; set; }

        public Guid PostId { get; set; }


    }
}