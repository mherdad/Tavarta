using System;
using System.Text.RegularExpressions;
using Tavarta.Common.Extentions;

namespace Tavarta.ViewModel.News
{
    public class NewsViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// تیتر عنوان
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// سر تیتر
        /// </summary>
        public string Headline { get; set; }

        public string Image { get; set; }
        public string ViewCount { get; set; }

        /// <summary>
        /// تاریخ انتشار
        /// </summary>
        public virtual DateTime PublishedOn { get; set; }

    

    }
}