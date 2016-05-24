using System.Collections.Generic;
using System.Web.Mvc;
using Tavarta.ViewModel.User;

namespace Tavarta.ViewModel.Posts
{
    public class PostListViewModel
    {
        public IList<PostViewModel> Posts { get; set; }

        /// <summary>
        /// درخواست
        /// </summary>
        public UserSearchRequest SearchRequest { get; set; }

        /// <summary>
        /// لیست گروه ها برای لیست آبشاری
        /// </summary>
        public IEnumerable<SelectListItem> Category { get; set; }

        public int TotalCount { get; set; }

    }
}