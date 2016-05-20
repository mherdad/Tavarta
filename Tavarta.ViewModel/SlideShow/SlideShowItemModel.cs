using System;
using System.ComponentModel.DataAnnotations;

namespace Tavarta.ViewModel.SlideShow
{
    public class SlideShowItemModel
    {
        public Guid Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "نوشته")]
        public string Text { get; set; }
        [Display(Name = "لینک")]
        public string Link { get; set; }
        [Display(Name = "عکس")]
        public string Image { get; set; }
        [Display(Name = "ترتیب")]
        public int Order { get; set; }
    }
}