using System;
using Tavarta.DomainClasses.Entities.Comment;
using Tavarta.DomainClasses.Entities.Users;
using Tavarta.Utility;

namespace Tavarta.DomainClasses.Entities.Common
{
    /// <summary>
    /// Represents a base class for every comment in system
    /// </summary>
    public class BaseComment
    {
        #region Ctor

        /// <summary>
        /// Create one Instance of <see cref="BaseComment"/>
        /// </summary>
        public BaseComment()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// get or set identifier of record
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// gets or sets date of creation
        /// </summary>
        public virtual DateTime CreatedOn { get; set; }

        /// <summary>
        /// gets or sets displayName of this comment's Creator if  he/she is Anonymous
        /// </summary>
        public virtual string CreatorDisplayName { get; set; }

        /// <summary>
        /// gets or sets body of blog post's comment
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// gets or sets body of blog post's comment
        /// </summary>
       // public virtual Rating Rating { get; set; }
        /// <summary>
        /// gets or sets informations of agent
        /// </summary>
        public virtual string UserAgent { get; set; }

        /// <summary>
        /// gets or sets siteUrl of Creator if he/she is Anonymous
        /// </summary>
        public virtual string SiteUrl { get; set; }

        /// <summary>
        /// gets or sets Email of Creator if he/she is anonymous
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// gets or sets status of comment
        /// </summary>
        public virtual CommentStatus Status { get; set; }

        /// <summary>
        /// gets or sets Ip Address of Creator
        /// </summary>
        public virtual string CreatorIp { get; set; }

        /// <summary>
        /// gets or sets datetime that is modified
        /// </summary>
        public virtual DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// gets or sets counter for report this comment
        /// </summary>
        public virtual int ReportsCount { get; set; }

        #endregion Properties

        #region NavigationProperties

        /// <summary>
        /// get or set user that create this record
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// get or set Id of user that create this record
        /// </summary>
        public virtual long? CreatorId { get; set; }

        #endregion NavigationProperties
    }
}