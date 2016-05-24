using System;

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
    }
}