using System;
using System.ComponentModel.DataAnnotations;

namespace Tavarta.ViewModel.User
{
    public class UserViewModel
    {
        /// <summary>
        /// آی دی 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// قفل شده؟
        /// </summary>
        public bool IsBanned { get; set; }
        /// <summary>
        /// اکانت سیستمی است؟
        /// </summary>
        public bool IsSystemAccount { get; set; }
        /// <summary>
        /// نام /نام خانوادگی
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// ایمیل
        /// </summary>
        [Required(ErrorMessage = "فیلد خالی است")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "ایمیل را بدرستی وارد نمائید")]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
