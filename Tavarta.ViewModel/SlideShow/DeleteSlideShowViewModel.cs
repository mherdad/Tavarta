using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tavarta.ViewModel.SlideShow
{
    public class DeleteSlideShowViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "نوشته")]
        public string Text { get; set; }
        [Display(Name = "لینک")]
        public string Link { get; set; }
        [AllowHtml]
        [Display(Name = "عکس")]
        public string Image { get; set; }
        [Display(Name = "ترتیب")]
        public int Order { get; set; }
    }
}