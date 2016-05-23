using System;
using Tavarta.DomainClasses.Entities.Postes;

namespace Tavarta.ViewModel.News
{
    public class LastArticleViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// تیتر عنوان
        /// </summary>
        public virtual string Title { get; set; }

        public virtual DateTime PublishedOn { get; set; }

        public Category Category { get; set; }

        public Guid CategoryId { get; set; }
    }
}