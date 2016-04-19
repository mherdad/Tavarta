using System;
using System.ComponentModel;
using Tavarta.ViewModel.Common;

namespace Tavarta.ViewModel.User
{
    public class UserSearchRequest : BaseSearchRequest
    {
        public string UserName { get; set; }
        /// <summary>
        /// گروه  کاربری
        /// </summary>
        [DisplayName("گروه کاربری")]
        public Guid? RoleId { get; set; }

    }

    public static class UserSortBy
    {
        public const string UserName = "UserName";
        public const string LastLogingDate = "LastLogingDate";
    }
}
