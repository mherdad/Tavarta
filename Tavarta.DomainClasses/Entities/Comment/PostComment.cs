using System;
using System.Collections.Generic;
using Tavarta.DomainClasses.Entities.Common;
using Tavarta.DomainClasses.Entities.Postes;

namespace Tavarta.DomainClasses.Entities.Comment
{/// <summary>
 /// Represents a blog post's comment
 /// </summary>
    public class PostComment:BaseComment
    {
        #region Ctor
        /// <summary>
        /// Create One Instance for <see cref="PostComment"/>
        /// </summary>
        public PostComment()
        {
            //Rating = new Rating();
            CreatedOn = DateTime.Now;
        }
        #endregion

    public bool IsShow { get; set; }

        #region NavigationProperties
        /// <summary>
        /// gets or sets BlogComment's identifier for Replying and impelemention self referencing
        /// </summary>
        public virtual Guid? ReplyId { get; set; }
        /// <summary>
        /// gets or sets blog's comment for Replying and impelemention self referencing
        /// </summary>
        public virtual PostComment Reply { get; set; }
        /// <summary>
        /// get or set collection of blog's comment for Replying and impelemention self referencing
        /// </summary>
        public virtual ICollection<PostComment> Children { get; set; }
        /// <summary>
        /// gets or sets post that this comment sent to it
        /// </summary>
        public virtual Post Post { get; set; }
        /// <summary>
        /// gets or sets post'Id that this comment sent to it
        /// </summary>
        public virtual Guid PostId { get; set; }
        #endregion
    }
}