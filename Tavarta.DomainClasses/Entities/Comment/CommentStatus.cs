using System.ComponentModel.DataAnnotations;

namespace Tavarta.DomainClasses.Entities.Comment
{
    public enum CommentStatus
    {
        /* 0 - approved, 1 - pending, 2 - spam, -1 - trash */
        [Display(Name = "تأیید شده")]
        Approved = 0,
        [Display(Name = "در انتظار بررسی")]
        Pending = 1,
        [Display(Name = "جفنگ")]
        Spam = 2,
        [Display(Name = "زباله دان")]
        Trash = -1
    }
}