using System.ComponentModel.DataAnnotations;

namespace Tavarta.DomainClasses.Entities.Postes
{
    public enum LinkBackStatus
    {
        [Display(Name = "غیرفعال")]
        Disable,
        [Display(Name = "فعال")]
        Enable,
        [Display(Name = "لینک‌ها داخلی")]
        JustInternal,
        [Display(Name = "لینک‌ها خارجی")]
        JustExternal
    }
}