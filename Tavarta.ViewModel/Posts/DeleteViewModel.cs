using System;
using System.ComponentModel.DataAnnotations;

namespace Tavarta.ViewModel.Posts
{
    public class DeleteViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public  Guid Id { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }
        /// <summary>
        /// تیتر عنوان
        /// </summary>

        [Display(Name = "عنوان")]
        public  string Title { get; set; }

        [Display(Name = "تگ")]
        public virtual string TagNames { get; set; }

        /// <summary>
        /// سر تیتر
        /// </summary>

        [Display(Name = "سر تیتر")]
        public string Headline { get; set; }


        [Display(Name = " نوشته")]
        public string Body { set; get; }

        [Display(Name = "گروه ")]
        public string Name { get; set; }
        

    }
}