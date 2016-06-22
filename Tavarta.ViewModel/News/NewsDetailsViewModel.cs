using System;
using Tavarta.Common.Fabrik;

namespace Tavarta.ViewModel.News
{
    public class NewsDetailsViewModel 
    {
        public Guid Id { get; set; }

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

        public string Image { get; set; }



        /// <summary>
        /// gets or sets meta title for seo
        /// </summary>
        public  string MetaTitle { get; set; }

        /// <summary>
        /// gets or sets meta keywords for seo
        /// </summary>
        public  string MetaKeywords { get; set; }

        /// <summary>
        /// gets or sets meta description of the content
        /// </summary>
        public  string MetaDescription { get; set; }



    }
}