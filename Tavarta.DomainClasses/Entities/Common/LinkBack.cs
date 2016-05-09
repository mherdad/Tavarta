using System;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.Utility;

namespace Tavarta.DomainClasses.Entities.Common
{
    /// <summary>
    /// Represents link for implemention linkback
    /// </summary>
    public class LinkBack
    {
        #region Ctor
        /// <summary>
        /// create one instance of <see cref="LinkBack"/>
        /// </summary>
        public LinkBack()
        {
            CreatedOn = DateTime.Now;
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }
        #endregion

        #region Properties
        /// <summary>
        /// gets or sets link's Id
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// gets or sets text for show Link
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// gets or sets link's address
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// gets or set value indicating whether this link is internal o external
        /// </summary>
        public virtual LinkBackType Type { get; set; }
        /// <summary>
        /// gets or sets date that this record is added
        /// </summary>
        public virtual DateTime CreatedOn { get; set; }
        #endregion

        #region NavigationProperties
        /// <summary>
        /// gets or sets Post that associated
        /// </summary>
        public virtual Post Post { get; set; }
        /// <summary>
        /// gets or sets id of Post that associated
        /// </summary>
        public virtual long PostId { get; set; }
        #endregion
    }
}