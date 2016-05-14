using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tavarta.ViewModel.User;

namespace Tavarta.ViewModel.Account
{
    public class RegisterViewModel
    {
        /// <summary>
        /// آی دی 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>

        /// <summary>
        /// کلمه عبور
        /// </summary>
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
            [StringLength(50, ErrorMessage = "کلمه عبور نباید کمتر از ۵ حرف و بیتشر از ۵۰ حرف باشد", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [DisplayName("کلمه عبور")]
            public string Password { get; set; }

            /// <summary>
            /// تکرار کلمه عبور
            /// </summary>
            [Required(ErrorMessage = "لطفا تکرار کلمه عبور را وارد کنید")]
            [DataType(DataType.Password)]
            [DisplayName("تکرار کلمه عبور")]
            [Compare("Password", ErrorMessage = "کلمات عبور باهم مطابقت ندارند")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// نام کاربری
            /// </summary>
            [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
            [DisplayName("نام کاربری")]
            [StringLength(256, ErrorMessage = "کلمه عبور نباید کمتر از ۵ حرف و بیتشر از ۲۵۶ حرف باشد", MinimumLength = 5
                )]
            [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "لطفا فقط از حروف انگلیسی و اعدد استفاده کنید")]
            public string UserName { get; set; }

            /// <summary>
            /// نام کاربر
            /// </summary>
            [DisplayName("نام نمایشی")]
            [StringLength(255, ErrorMessage = "نام نمایشی نباید کمتر از 5 حرف و بیتشر از 25۵ حرف باشد",
                MinimumLength = 5)]
            [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF,۰-۹\s]*$",
                ErrorMessage = "لطفا فقط ازاعداد و حروف  فارسی استفاده کنید")]
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
