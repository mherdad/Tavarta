using System;
using System.ComponentModel.DataAnnotations;

namespace Tavarta.ViewModel.Posts
{
    public class AddPostViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// gets or sets the blog pot body
        /// </summary>
        [Display(Name = "نوشته")]
        public virtual string Body { get; set; }

        /// <summary>
        /// تیتر عنوان
        /// </summary>

            [Display(Name = "عنوان")]
        public virtual string Title { get; set; }

        /// <summary>
        /// تاریخ انتشار
        /// </summary>
        public virtual DateTime PublishedOn { get; set; }

        /// <summary>
        /// تعداد بازیدید
        /// </summary>
        public virtual long ViewCount { get; set; }


        /// <summary>
        /// gets or sets name of tags seperated by comma that assosiated with this content fo increase performance
        /// </summary>

            [Display(Name = "تگ")]
        public virtual string TagNames { get; set; }


        /// <summary>
        /// سر تیتر
        /// </summary>

            [Display(Name = "سر تیتر")]
        public string HeadTitle { get; set; }
    }
}