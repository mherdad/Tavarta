using System.ComponentModel;

namespace Tavarta.ViewModel.Common
{
    public abstract class BaseIsDelete
    {
        /// <summary>
        /// آیا رکورد به صورت منطقی حذف شده است
        /// </summary>
        [DisplayName("حذف منطقی")]
        public bool IsDeleted { get; set; }
    }
}
