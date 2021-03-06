﻿using System;
using System.Collections.Generic;
using Tavarta.DomainClasses.Entities.Comment;
using Tavarta.DomainClasses.Entities.Common;
using Tavarta.DomainClasses.Entities.Users;

namespace Tavarta.DomainClasses.Entities.Postes
{
    /// <summary>
    /// Represents a  post
    /// </summary>
    public class Post : BaseContent
    {
        #region Ctor

        /// <summary>
        /// Create one Instance of <see cref="Post"/>
        /// </summary>
        public Post()
        {
            //Rating = new Rating();
            PublishedOn = DateTime.Now;
        }

        #endregion Ctor

        #region Properties

        public string Image { get; set; }

        /// <summary>
        /// gets or sets Status of LinkBack Notifications
        /// </summary>
        public virtual LinkBackStatus LinkBackStatus { get; set; }

        #endregion Properties

        #region NavigationProperties

        /// <summary>
        /// get or set  blog post's Reviews
        /// </summary>
        public virtual ICollection<PostComment> Comments { get; set; }

        /// <summary>
        /// get or set collection of links that reference to this blog post
        /// </summary>
        public virtual ICollection<LinkBack> LinkBacks { get; set; }

        /// <summary>
        /// get or set Collection of Users that Contribute on this post
        /// </summary>
        public virtual ICollection<User> Contributors { get; set; }

        /// <summary>
        /// get or set of category that Contribute on this post
        /// </summary>

        
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }

        #endregion NavigationProperties
    }
}