using System;
using System.Web;
using System.Web.Mvc;
using Tavarta.Common.Referrer;

namespace Tavarta.Common.Extentions
{
    /// <summary>
    /// <see cref="HtmlHelper"/> extension methods.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a string containing the referrer meta tags. <see cref="ReferrerMode"/> for more information.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="referrerMode">The type of referrer allowed to be sent.</param>
        /// <returns>The referrer meta tag.</returns>
        public static IHtmlString ReferrerMeta(this HtmlHelper htmlHelper, ReferrerMode referrerMode)
        {
            if (referrerMode == ReferrerMode.NoneWhenDowngrade)
            {
                return null;
            }

            return new MvcHtmlString("<meta name=\"referrer\" content=\"" + referrerMode.ToLowercaseString() + "\">");
        }


        public static MvcHtmlString BootstrapPager(this HtmlHelper helper, int currentPageIndex, Func<int, string> action, int totalItems, int pageSize = 10, int numberOfLinks = 5)
 
        {
 
            if (totalItems <= 0)
 
            {
 
                return MvcHtmlString.Empty;

            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var lastPageNumber = (int)Math.Ceiling((double)currentPageIndex / numberOfLinks) * numberOfLinks;

            var firstPageNumber = lastPageNumber - (numberOfLinks - 1);

            var hasPreviousPage = currentPageIndex > 1;

            var hasNextPage = currentPageIndex < totalPages;

            if (lastPageNumber > totalPages)

            {

                lastPageNumber = totalPages;

            }

            var ul = new TagBuilder("ul");

            ul.AddCssClass("pagination");
 
            ul.InnerHtml += AddLink(1, action, currentPageIndex == 1, "disabled", "<<", "First Page");
 
            ul.InnerHtml += AddLink(currentPageIndex - 1, action, !hasPreviousPage, "disabled", "<", "Previous Page");
 
            for (int i = firstPageNumber; i <= lastPageNumber; i++)
 
            {
 
                ul.InnerHtml += AddLink(i, action, i == currentPageIndex, "active", i.ToString(), i.ToString());
 
            }
 
            ul.InnerHtml += AddLink(currentPageIndex + 1, action, !hasNextPage, "disabled", ">", "Next Page");
 
            ul.InnerHtml += AddLink(totalPages, action, currentPageIndex == totalPages, "disabled", ">>", "Last Page");
 
            return MvcHtmlString.Create(ul.ToString());
 
        }
  
 
  
        private static TagBuilder AddLink(int index, Func<int, string> action, bool condition, string classToAdd, string linkText, string tooltip)
  
        {
  
            var li = new TagBuilder("li");
 
            li.MergeAttribute("title", tooltip);
  
            if (condition)
 
            {
 
                li.AddCssClass(classToAdd);
 
            }
 
            var a = new TagBuilder("a");
 
            a.MergeAttribute("href", !condition? action(index) : "javascript:");
 
            a.SetInnerText(linkText);
 
            li.InnerHtml = a.ToString();
 
            return li;
  
        }
 
    }





    }

