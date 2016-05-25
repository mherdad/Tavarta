using System.Collections.Generic;

namespace Tavarta.ViewModel.News
{
    public class NewsListViewModel
    {
        public IList<NewsViewModel> News { get; set; }
        public IList<NewsViewModel> Sports { get; set; }

        public IList<NewsViewModel> Environment { get; set; }
        public IList<NewsViewModel> HealthEvents { get; set; }
        public IList<NewsViewModel> Literary { get; set; }
        public IList<NewsViewModel> PhotoGallery { get; set; }

        public IList<NewsViewModel> Notes { get; set; }
        public IList<NewsViewModel> MostViewed { get; set; }
        public IList<NewsViewModel> Surveys { get; set; }

        public IList<CarouselViewModel> Carousel { get; set; }
        public IList<LastArticleViewModel> LastArticle { get; set; }
        public int TotalCount { get; set; }
    }
}