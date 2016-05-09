using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavarta.DomainClasses.Entities.Common;

namespace Tavarta.DomainClasses.Entities.Announcements
{/// <summary>
 /// Represents Announcement For Announcement Section
 /// </summary>
    public class Announcement:BaseContent
    {
        #region Properties
        /// <summary>
        /// gets or sets Date that this Announcement will Expire
        /// </summary>
        public virtual DateTime? ExpireOn { get; set; }
        /// <summary>
        /// indicate this accouncement is approved by admin if announcementSetting.Moderate==true
        /// </summary>
        public virtual bool IsApproved { get; set; }
        #endregion

        #region NavigationProperties
        /// <summary>
        /// get or set Collection of Comments for this Announcement
        /// </summary>
        //public virtual ICollection<AnnouncementComment> Comments { get; set; }

        #endregion
    }
}
