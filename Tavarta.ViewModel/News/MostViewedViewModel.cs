using System;

namespace Tavarta.ViewModel.News
{
    public class MostViewedViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// تیتر عنوان
        /// </summary>
        public virtual string Title { get; set; }
        public long ViewCount { get; set; }

        /// <summary>
        /// تاریخ انتشار
        /// </summary>
        public virtual DateTime PublishedOn { get; set; }
    }
}