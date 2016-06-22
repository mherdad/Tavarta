using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Tavarta.Common.Helpers;
using Tavarta.Utility;

namespace Tavarta.Common.Extentions
{
    public static class SeoExtensions
    {
        #region Fields
        private const string SeparatorTitle = " - ";
        private const int MaxLenghtTitle = 60;
        private const int MaxLenghtSlug = 45;
        private const int MaxLenghtDescription = 170;
        #endregion

        #region Social Snippet
        public static string GenerateSocialSnippet()
        {
            return null;
        }
        #endregion

        public static string UrlFriendly(this string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }


        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }




        //public static string GenerateSeoUrl(this string title)
        //{

            //    string phrase = string.Format("{0}-{1}", Id, Name);

            //    string str = RemoveAccent(phrase).ToLower();
            //    // invalid chars           
            //    str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            //    // convert multiple spaces into one space   
            //    str = Regex.Replace(str, @"\s+", " ").Trim();
            //    // cut and trim 
            //    str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            //    str = Regex.Replace(str, @"\s", "-"); // hyphens   
            //    return str;
            //}

            //private string RemoveAccent(string text)
            //{
            //    byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            //    return System.Text.Encoding.ASCII.GetString(bytes);
            //}




            #region Rich Snippet

        public static string AuthorGooglePlusLink(string googlePlusId, string virtualImageUrl, int imageWidth, int imageHeight)
        {
            var url = "https://plus.google.com/" + googlePlusId + "?rel=author";
            var div =
                $" <a itemprop=\"social\" rel=\"author\" href=\"{url}\">" +
                $"<img src=\"{virtualImageUrl}\" width=\"{imageWidth}\" height=\"{imageHeight}\" alt=\"g+\" /></a>";
            return div;
        }
        public static string GenerateRichSnippetForRating(string personName, string personWritedItemsUrl, string itemTitle, int ratersCount, int ratingValue, string itemtype = "Product")
        {
            itemtype = $"http://schema.org/{itemtype}";

            var div = $"<div itemscope itemtype=\"{itemtype}\" class=\"aggregateRating\">" +
                      $"<span itemprop=\"name\">{itemTitle}</span>" +
                      "<div itemprop=\"aggregateRating\" itemscope itemtype=\"http://schema.org/AggregateRating\">" +
                      $"Rated <span itemprop=\"ratingValue\">{ratingValue}</span>/5 based on <span itemprop=\"reviewCount\">{ratersCount}</span>" +
                      "readers reviews</div></div>";
            if (!string.IsNullOrEmpty(personName))
                div +=
                    "<div itemscope itemtype=\"http://schema.org/Person\" class=\"aggregateRating\">" +
                    $"<span itemprop=\"name\">{personName}</span> More About Author" +
                    $" <a href=\"{personWritedItemsUrl}\" itemprop=\"url\">اطلاعات بیشتر در مورد نویسنده</a></div>";

            return div;
        }
        #endregion

        #region MetaTag
        private const string FaviconPath = "~/Content/favicon/favicon.ico";

        public static string GenerateMetaTag(string title, string description, string canonicalUrl, string googlePlusUrl, bool allowIndexPage, bool allowCache,
            bool allowFollowLinks, string author = "", string lastmodified = "", string expires = "never",
            string applicationName = "web app", string language = "fa",
            CacheControlType cacheControlType = CacheControlType.Private, bool allowTranslate = true)
        {
            title = title.Substring(0, title.Length <= MaxLenghtTitle ? title.Length : MaxLenghtTitle).Trim();
            description =
                description.Substring(0,
                    description.Length <= MaxLenghtDescription ? description.Length : MaxLenghtDescription).Trim();

            var meta = "";
            meta += string.Format("<meta charset=\"utf-8\"/>\n");
            meta += $"<title>{title}</title>\n";

            meta +=
                string.Format(
                    "<meta content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\" name=\"viewport\">");

            if (!string.IsNullOrEmpty(googlePlusUrl))
                meta += $"<link rel=\"author\" href=\"{googlePlusUrl}\"/>\n";
            meta += $"<link rel=\"canonical\" href=\"{canonicalUrl}\"/>\n";
            meta += $"<link rel=\"shortcut icon\" href=\"{FaviconPath}\" type=\"image/x-icon\" />\n";
            meta += $"<meta name=\"application-name\" content=\"{applicationName}\" />\n";
            meta += string.Format("<meta name=\"msapplication-config\" content=\"/browserconfig.xml\" />\n");
            meta += $"<meta http-equiv=\"content-language\" content=\"{language}\"/>\n";
            if (allowTranslate)
                meta += string.Format("<meta name=\"google\" content=\"notranslate\" />\n");

            meta += $"<meta name=\"description\" content=\"{description}\"/>\n";
            meta +=
                $"<meta http-equiv=\"Cache-control\" content=\"{EnumHelper.GetEnumDescription(typeof (CacheControlType), cacheControlType.ToString())}\"/>\n";

            meta +=
                $"<meta name=\"robots\" content=\"{(allowIndexPage ? "index" : "noindex")}, {(allowFollowLinks ? "follow" : "nofollow")}, {(allowCache ? "archive" : "noarchive")}\" />\n";
            meta += $"<meta name=\"expires\" content=\"{expires}\"/>\n";

            if (!string.IsNullOrEmpty(lastmodified))
                meta += $"<meta name=\"last-modified\" content=\"{lastmodified}\"/>\n";

            if (!string.IsNullOrEmpty(author))
                meta += $"<meta name=\"author\" content=\"{author}\"/>\n";

            //------------------------------------Google & Bing Doesn't Use Meta Keywords ...
            //meta += string.Format("<meta name=\"keywords\" content=\"{0}\"/>\n", keywords);

            return meta;
        }

        #endregion

        #region Slug
        public static string GenerateSlug(this string title)
        {
            var slug = RemoveAccent(title).ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9-\u0600-\u06FF]", "-");
            slug = Regex.Replace(slug, @"\s+", "-").Trim();
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Substring(0, slug.Length <= MaxLenghtSlug ? slug.Length : MaxLenghtSlug).Trim();

            return slug.RemoveDiacritics();
        }
        private static string RemoveAccent(this string text)
        {
            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region Title
        public static string ResolveTitleForUrl(this HtmlHelper htmlHelper, string title)
        {
            return string.IsNullOrEmpty(title)
                ? string.Empty
                : Regex.Replace(Regex.Replace(title, "[^\\w]", "-"), "[-]{2,}", "-");
        }

        public static string ResolveTitleForUrl(string title)
        {
            return string.IsNullOrEmpty(title)
                ? string.Empty
                : Regex.Replace(Regex.Replace(title, "[^\\w]", "-"), "[-]{2,}", "-");
        }


        public static string GeneratePageTitle(params string[] crumbs)
        {
            var title = "";

            for (var i = 0; i < crumbs.Length; i++)
            {
                title += $"{crumbs[i]}{((i < crumbs.Length - 1) ? SeparatorTitle : string.Empty)}";
            }

            title = title.Substring(0, title.Length <= MaxLenghtTitle ? title.Length : MaxLenghtTitle).Trim();

            return title.RemoveDiacritics();
        }

      
        #endregion

        #region GeneratePageDescription
          public static string GeneratePageDescription(string description)
        {
            return
                description.Substring(0,
                    description.Length <= MaxLenghtDescription ? description.Length : MaxLenghtDescription).Trim();
        }
        #endregion

        #region Enum
        public enum CacheControlType
        {
            [Description("public")]
            Public,
            [Description("private")]
            Private,
            [Description("no-cache")]
            Nocache,
            [Description("no-store")]
            Nostore
        }
        #endregion

        #region GetQueryString of referrer search engine
        public static string GetKeywords(string urlReferrer)
        {
            string searchQuery;
            var url = new Uri(urlReferrer);
            var query = HttpUtility.ParseQueryString(urlReferrer);
            switch (url.Host)
            {
                case "google":
                case "daum":
                case "msn":
                case "bing":
                case "ask":
                case "altavista":
                case "alltheweb":
                case "live":
                case "najdi":
                case "aol":
                case "seznam":
                case "search":
                case "szukacz":
                case "pchome":
                case "kvasir":
                case "sesam":
                case "ozu":
                case "mynet":
                case "ekolay":
                    searchQuery = query["q"];
                    break;
                case "naver":
                case "netscape":
                case "mama":
                case "mamma":
                case "terra":
                case "cnn":
                    searchQuery = query["query"];
                    break;
                case "virgilio":
                case "alice":
                    searchQuery = query["qs"];
                    break;
                case "yahoo":
                    searchQuery = query["p"];
                    break;
                case "onet":
                    searchQuery = query["qt"];
                    break;
                case "eniro":
                    searchQuery = query["search_word"];
                    break;
                case "about":
                    searchQuery = query["terms"];
                    break;
                case "voila":
                    searchQuery = query["rdata"];
                    break;
                case "baidu":
                    searchQuery = query["wd"];
                    break;
                case "yandex":
                    searchQuery = query["text"];
                    break;
                case "szukaj":
                    searchQuery = query["wp"];
                    break;
                case "yam":
                    searchQuery = query["k"];
                    break;
                case "rambler":
                    searchQuery = query["words"];
                    break;
                default:
                    searchQuery = query["q"];
                    break;
            }
            return searchQuery;
        }
        #endregion

        #region  SeoUrls
        public static string ToSeoUrl(this string url)
        {
            // make the url lowercase
            var encodedUrl = (url ?? "").ToLower();

            // replace & with and
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // remove characters
            encodedUrl = encodedUrl.Replace("'", "");

            // remove invalid characters
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9-\u0600-\u06FF]", "-");

            // remove duplicates
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // trim leading & trailing characters
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }
        #endregion

    }
}