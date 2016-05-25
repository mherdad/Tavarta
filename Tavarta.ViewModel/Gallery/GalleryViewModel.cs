using System;

namespace Tavarta.ViewModel.Gallery
{
    public class GalleryViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// تیتر عنوان
        /// </summary>
        public virtual string Title { get; set; }

        public string Image { get; set; }

    }
}