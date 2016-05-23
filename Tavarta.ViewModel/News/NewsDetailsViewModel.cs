using System;

namespace Tavarta.ViewModel.News
{
    public class NewsDetailsViewModel
    {

        /// <summary>
        /// gets or sets the blog pot body
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// تیتر عنوان
        /// </summary>
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
        public virtual string TagNames { get; set; }

        /// <summary>
        /// سر تیتر
        /// </summary>
        public string Headline { get; set; }

        
    }
}