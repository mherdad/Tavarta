using System.ComponentModel.DataAnnotations;

namespace Tavarta.DomainClasses.Entities.Common
{
    public enum AuditAction
    {
        /// <summary>
        /// درج رکود
        /// </summary>
        [Display(Name = "درج")]
        Create,
        /// <summary>
        /// ویرایش
        /// </summary>
        [Display(Name = "ویرایش")]
        Update,
        /// <summary>
        /// حذف فیزیکی
        /// </summary>
        [Display(Name = "حذف فیزیکی")]
        Delete,
        /// <summary>
        /// حذف نرم
        /// </summary>
        [Display(Name = "حذف نرم")]
        SoftDelete,
    }
}