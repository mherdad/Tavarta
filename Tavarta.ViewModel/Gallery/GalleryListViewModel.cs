using System.Collections.Generic;

namespace Tavarta.ViewModel.Gallery
{
    public class GalleryListViewModel
    {
        public IList<GalleryViewModel> PhotoGallery { get; set; }
        public long TotalCount { get; set; }
    }
}