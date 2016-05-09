using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavarta.DomainClasses.Entities.Polling
{
    public class PollOption
    {
        #region Properties
        /// <summary>
        /// gets or sets identifier of this polloption
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// gets or sets Title of this polloption
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// gets or sets count of votes
        /// </summary>
        public virtual long VotesCount { get; set; }
        /// <summary>
        /// gets or sets Description of this Option for more details
        /// </summary>
        public virtual string Description { get; set; }
        #endregion

        #region NavigationProperties
        /// <summary>
        /// gets or sets the poll that assosiated with this Polloption
        /// </summary>
        public virtual Poll Poll { get; set; }
        /// <summary>
        /// gets or sets the id of poll that assosiated with this Polloption
        /// </summary>
        public virtual long PollId { get; set; }
        #endregion
    }
}
