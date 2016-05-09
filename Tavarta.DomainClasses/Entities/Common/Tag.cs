using System;
using System.Collections.Generic;
using Tavarta.DomainClasses.Entities.Postes;
using Tavarta.Utility;

namespace Tavarta.DomainClasses.Entities.Common
{
    public class Tag
    {
        #region Ctor
        /// <summary>
        /// Create one instance of <see cref="Tag"/>
        /// </summary>
        public Tag()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }
        #endregion

        #region Properties
        /// <summary>
        /// sets or gets Tag's identifier
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// sets or gets Tag's name
        /// </summary>
        public virtual string Name { get; set; }
        #endregion

        #region NavigationProperties
        /// <summary>
        /// sets or gets Tag's posts
        /// </summary>
        public virtual ICollection<Post> BlogPosts { get; set; }
        #endregion
    }
}