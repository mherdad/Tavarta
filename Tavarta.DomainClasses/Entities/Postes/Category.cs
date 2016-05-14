using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tavarta.Utility;

namespace Tavarta.DomainClasses.Entities.Postes
{
    public class Category
    {
        public Category()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }
        /// <summary>
        /// get or set identifier of record
        /// </summary>
        
        public Guid Id { get; set; }


        /// <summary>
        /// sets or gets Category's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get or set Collection of posts that Contribute on this category
        /// </summary>
        public ICollection<Post> Posts { get; set; }
    }
}