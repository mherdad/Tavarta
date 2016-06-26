using System.Collections.Generic;

namespace Tavarta.ViewModel.News
{
    public class ListMostViewedViewModel
    {
        public IList<MostViewedViewModel> MostViewedWeek { get; set; }
        public IList<MostViewedViewModel> MostViewedMonth { get; set; }
        public IList<MostViewedViewModel> MostViewedYear { get; set; }
    }
}