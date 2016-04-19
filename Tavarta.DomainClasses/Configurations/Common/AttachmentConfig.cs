using System.Data.Entity.ModelConfiguration;
using Tavarta.DomainClasses.Entities.Common;

namespace Tavarta.DomainClasses.Configurations.Common
{
    /// <summary>
    /// مشخص کننده کلاس مپینگ مربوط به فایل های ضمیمه
    /// </summary>
    public class AttachmentConfig : EntityTypeConfiguration<Attachment>
    {
        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        public AttachmentConfig()
        {
            
        }
    }
}
